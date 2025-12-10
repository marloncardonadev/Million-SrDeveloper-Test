using FluentAssertions;
using Million.RealEstate.Backend.Application.Properties.Commands.ChangePropertyPrice;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Entities;
using Million.RealEstate.Backend.Domain.Interfaces;
using Moq;

namespace Million.RealEstate.Backend.Tests;

[TestFixture]
public class ChangePropertyPriceCommandHandlerTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock = null!;
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private ChangePropertyPriceCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new ChangePropertyPriceCommandHandler(
            _propertyRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Test]
    public async Task Handle_Should_ThrowDomainException_WhenPropertyNotFound()
    {
        // Arrange
        var propertyId = 1;
        var command = new ChangePropertyPriceCommand(propertyId, 200000);

        _propertyRepositoryMock
            .Setup(x => x.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"Property {propertyId} not found.");

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task Handle_Should_UpdatePrice_WhenPropertyExists()
    {
        // Arrange
        var propertyId = 1;
        var property = new Property(
            name: "House 1",
            address: "123 Street",
            price: 100000,
            codeInternal: "CODE-001",
            year: 2020,
            ownerId: 1
        );

        // hack: set Id to match propertyId (si no tienes set; puedes ignorar esta igualdad)
        typeof(Property).GetProperty("Id")!.SetValue(property, propertyId);

        var newPrice = 200000;
        var command = new ChangePropertyPriceCommand(propertyId, newPrice);

        _propertyRepositoryMock
            .Setup(x => x.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(property);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        property.Price.Should().Be(newPrice);

        _propertyRepositoryMock.Verify(
            x => x.Update(property),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
