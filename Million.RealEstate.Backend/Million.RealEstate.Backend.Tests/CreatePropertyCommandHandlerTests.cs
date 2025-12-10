using FluentAssertions;
using Million.RealEstate.Backend.Application.Properties.Commands.CreateProperty;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Entities;
using Million.RealEstate.Backend.Domain.Interfaces;
using Moq;

namespace Million.RealEstate.Backend.Tests;

[TestFixture]
public class CreatePropertyCommandHandlerTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock = null!;
    private Mock<IOwnerRepository> _ownerRepositoryMock = null!;
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private CreatePropertyCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _ownerRepositoryMock = new Mock<IOwnerRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreatePropertyCommandHandler(
            _propertyRepositoryMock.Object,
            _ownerRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Test]
    public async Task Handle_Should_ThrowDomainException_WhenOwnerDoesNotExist()
    {
        // Arrange
        var command = new CreatePropertyCommand(
            Name: "House 1",
            Address: "123 Street",
            Price: 100000,
            CodeInternal: "CODE-001",
            Year: 2020,
            OwnerId: 1
        );

        _ownerRepositoryMock
            .Setup(x => x.GetByIdAsync(command.OwnerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Owner?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"Owner {command.OwnerId} not found.");

        _propertyRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task Handle_Should_ThrowDomainException_WhenCodeInternalAlreadyExists()
    {
        // Arrange
        var ownerId = 1;
        var command = new CreatePropertyCommand(
            Name: "House 1",
            Address: "123 Street",
            Price: 100000,
            CodeInternal: "CODE-001",
            Year: 2020,
            OwnerId: ownerId
        );

        _ownerRepositoryMock
            .Setup(x => x.GetByIdAsync(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Owner("Owner 1", "Address", DateTime.UtcNow.AddYears(-30)));

        _propertyRepositoryMock
            .Setup(x => x.ExistsCodeInternalAsync(command.CodeInternal, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("CodeInternal already exists.");

        _propertyRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task Handle_Should_CreateProperty_WhenDataIsValid()
    {
        // Arrange
        var ownerId = 1;
        var command = new CreatePropertyCommand(
            Name: "House 1",
            Address: "123 Street",
            Price: 100000,
            CodeInternal: "CODE-001",
            Year: 2020,
            OwnerId: ownerId
        );

        _ownerRepositoryMock
            .Setup(x => x.GetByIdAsync(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Owner("Owner 1", "Address", DateTime.UtcNow.AddYears(-30)));

        _propertyRepositoryMock
            .Setup(x => x.ExistsCodeInternalAsync(command.CodeInternal, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _propertyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(1);

        _propertyRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Property>(p =>
                p.Name == command.Name &&
                p.Address == command.Address &&
                p.Price == command.Price &&
                p.CodeInternal == command.CodeInternal &&
                p.Year == command.Year &&
                p.OwnerId == command.OwnerId),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
