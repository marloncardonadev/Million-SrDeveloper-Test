using FluentAssertions;
using Million.RealEstate.Backend.Application.Properties.Commands.AddPropertyImage;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Entities;
using Million.RealEstate.Backend.Domain.Interfaces;
using Moq;

namespace Million.RealEstate.Backend.Tests;

[TestFixture]
public class AddPropertyImageCommandHandlerTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock = null!;
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private AddPropertyImageCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new AddPropertyImageCommandHandler(
            _propertyRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Test]
    public async Task Handle_Should_ThrowDomainException_WhenPropertyNotFound()
    {
        // Arrange
        var propertyId = 1;
        var command = new AddPropertyImageCommand(propertyId, "path/to/image.jpg");

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
    public async Task Handle_Should_AddImage_WhenPropertyExists()
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

        typeof(Property).GetProperty("Id")!.SetValue(property, propertyId);

        var filePath = "path/to/image.jpg";
        var command = new AddPropertyImageCommand(propertyId, filePath);

        _propertyRepositoryMock
            .Setup(x => x.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(property);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var imageId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        imageId.Should().NotBe(1);
        property.Images.Should().ContainSingle(i => i.File == filePath);

        _propertyRepositoryMock.Verify(
            x => x.Update(property),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
