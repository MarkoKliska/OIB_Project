using Autoservice.Application.Authentication;
using Autoservice.Application.DTOs.User.Login;
using Autoservice.Application.Features.Invoice.CompleteService;
using Autoservice.Application.Features.User.Login;
using Autoservice.Application.Interfaces;
using Autoservice.Domain.Entities;
using Autoservice.Domain.Repositories;
using Autoservice.Infrastructure.Services;
using Moq;
using NUnit.Framework;

namespace Autoservice.Tests;

// ============================================================
// MODEL TESTS 1 — User
// ============================================================

[TestFixture]
public class UserModelTests
{
    [Test]
    public void FullName_ShouldCombineFirstAndLastName()
    {
        // Arrange
        var user = new User { FirstName = "Petar", LastName = "Petrovic" };

        // Act & Assert
        Assert.That(user.FullName, Is.EqualTo("Petar Petrovic"));
    }

    [Test]
    public void User_RoleManager_ShouldBeSetCorrectly()
    {
        // Arrange
        var user = new User { Role = UserRole.Manager };

        // Act & Assert
        Assert.That(user.Role, Is.EqualTo(UserRole.Manager));
    }

    [Test]
    public void User_IssuedInvoices_ShouldBeEmptyCollectionByDefault()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.That(user.IssuedInvoices, Is.Not.Null);
        Assert.That(user.IssuedInvoices, Is.Empty);
    }

    [Test]
    public void User_Id_ShouldBeSetExplicitly()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var user = new User { Id = id };

        // Assert
        Assert.That(user.Id, Is.EqualTo(id));
    }
}

// ============================================================
// MODEL TESTS 2 — Vehicle
// ============================================================

[TestFixture]
public class VehicleModelTests
{
    [Test]
    public void Vehicle_IsServiced_ShouldBeFalseByDefault()
    {
        // Arrange & Act
        var vehicle = new Vehicle();

        // Assert
        Assert.That(vehicle.IsServiced, Is.False);
    }

    [Test]
    public void Vehicle_Type_ShouldSupportAllEnumValues()
    {
        // Arrange & Act
        var passenger = new Vehicle { Type = VehicleType.Passenger };
        var truck = new Vehicle { Type = VehicleType.Truck };
        var motorcycle = new Vehicle { Type = VehicleType.Motorcycle };

        // Assert
        Assert.That(passenger.Type, Is.EqualTo(VehicleType.Passenger));
        Assert.That(truck.Type, Is.EqualTo(VehicleType.Truck));
        Assert.That(motorcycle.Type, Is.EqualTo(VehicleType.Motorcycle));
    }

    [Test]
    public void Vehicle_EstimatedPrice_ShouldStoreDecimalValue()
    {
        // Arrange & Act
        var vehicle = new Vehicle { EstimatedPrice = 12500.50m };

        // Assert
        Assert.That(vehicle.EstimatedPrice, Is.EqualTo(12500.50m));
    }

    [Test]
    public void Vehicle_LicensePlate_ShouldStoreValue()
    {
        // Arrange & Act
        var vehicle = new Vehicle { LicensePlate = "NS-001-AA" };

        // Assert
        Assert.That(vehicle.LicensePlate, Is.EqualTo("NS-001-AA"));
    }
}

// ============================================================
// MODEL TESTS 3 — ServiceInvoice
// ============================================================

[TestFixture]
public class ServiceInvoiceModelTests
{
    [Test]
    public void ServiceInvoice_TotalAmount_ShouldStoreAssignedValue()
    {
        // Arrange & Act
        var invoice = new ServiceInvoice { TotalAmount = 8500m };

        // Assert
        Assert.That(invoice.TotalAmount, Is.EqualTo(8500m));
    }

    [Test]
    public void ServiceInvoice_IssuedAt_ShouldBeWithinExpectedRange()
    {
        // Arrange
        var before = DateTime.UtcNow;

        // Act
        var invoice = new ServiceInvoice { IssuedAt = DateTime.UtcNow };
        var after = DateTime.UtcNow;

        // Assert
        Assert.That(invoice.IssuedAt, Is.InRange(before, after));
    }

    [Test]
    public void ServiceInvoice_MechanicName_ShouldStoreValue()
    {
        // Arrange & Act
        var invoice = new ServiceInvoice { MechanicName = "Petar Petrovic" };

        // Assert
        Assert.That(invoice.MechanicName, Is.EqualTo("Petar Petrovic"));
    }

    [Test]
    public void ServiceInvoice_VehicleIdAndMechanicId_ShouldBeSet()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var mechanicId = Guid.NewGuid();

        // Act
        var invoice = new ServiceInvoice { VehicleId = vehicleId, MechanicId = mechanicId };

        // Assert
        Assert.That(invoice.VehicleId, Is.EqualTo(vehicleId));
        Assert.That(invoice.MechanicId, Is.EqualTo(mechanicId));
    }
}

// ============================================================
// SERVICE TESTS 1 — BillingServiceFactory
// ============================================================

[TestFixture]
public class BillingServiceFactoryTests
{
    private Mock<ITimeProvider> _timeProviderMock = default!;
    private BillingServiceFactory _sut = default!;

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<ITimeProvider>();
        _sut = new BillingServiceFactory(
            new MorningBillingService(),
            new AfternoonBillingService(),
            _timeProviderMock.Object);
    }

    [Test]
    public void GetForCurrentShift_At9AM_ShouldReturnMorningBillingService()
    {
        // Arrange
        _timeProviderMock.Setup(t => t.Now)
            .Returns(new DateTime(2025, 1, 1, 9, 0, 0));

        // Act
        var billing = _sut.GetForCurrentShift();

        // Assert
        Assert.That(billing, Is.InstanceOf<MorningBillingService>());
    }

    [Test]
    public void GetForCurrentShift_At1159AM_ShouldReturnMorningBillingService()
    {
        // Arrange - last minute of first shift
        _timeProviderMock.Setup(t => t.Now)
            .Returns(new DateTime(2025, 1, 1, 11, 59, 0));

        // Act
        var billing = _sut.GetForCurrentShift();

        // Assert
        Assert.That(billing, Is.InstanceOf<MorningBillingService>());
    }

    [Test]
    public void GetForCurrentShift_At12PM_ShouldReturnAfternoonBillingService()
    {
        // Arrange - start of second shift
        _timeProviderMock.Setup(t => t.Now)
            .Returns(new DateTime(2025, 1, 1, 12, 0, 0));

        // Act
        var billing = _sut.GetForCurrentShift();

        // Assert
        Assert.That(billing, Is.InstanceOf<AfternoonBillingService>());
    }

    [Test]
    public void GetForCurrentShift_OutsideWorkingHours_ShouldReturnAfternoonBillingService()
    {
        // Arrange - 20:00, outside both shifts
        _timeProviderMock.Setup(t => t.Now)
            .Returns(new DateTime(2025, 1, 1, 20, 0, 0));

        // Act
        var billing = _sut.GetForCurrentShift();

        // Assert
        Assert.That(billing, Is.InstanceOf<AfternoonBillingService>());
    }

    [Test]
    public void MorningBilling_ShouldApply15PercentDiscount()
    {
        // Arrange
        var morning = new MorningBillingService();

        // Act
        var result = morning.CalculateFinalAmount(10000m);

        // Assert
        Assert.That(result, Is.EqualTo(8500m));
    }

    [Test]
    public void AfternoonBilling_ShouldApply10PercentTax()
    {
        // Arrange
        var afternoon = new AfternoonBillingService();

        // Act
        var result = afternoon.CalculateFinalAmount(10000m);

        // Assert
        Assert.That(result, Is.EqualTo(11000m));
    }
}

// ============================================================
// SERVICE TESTS 2 — LoginCommandHandler
// ============================================================

[TestFixture]
public class LoginCommandHandlerTests
{
    private Mock<IUserRepository> _userRepoMock = default!;
    private Mock<IJwtTokenService> _jwtMock = default!;
    private Mock<IEventLogger> _loggerMock = default!;
    private LoginCommandHandler _sut = default!;

    [SetUp]
    public void SetUp()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _jwtMock = new Mock<IJwtTokenService>();
        _loggerMock = new Mock<IEventLogger>();

        _sut = new LoginCommandHandler(
            _userRepoMock.Object,
            _jwtMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ValidCredentials_ShouldReturnSuccessWithToken()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "mechanic1",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
            FirstName = "Petar",
            LastName = "Petrovic",
            Role = UserRole.Mechanic
        };

        _userRepoMock
            .Setup(r => r.GetByUsernameAsync("mechanic1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _jwtMock
            .Setup(j => j.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserRole>(), It.IsAny<string>()))
            .Returns("test-jwt-token");

        // Act
        var result = await _sut.Handle(
            new LoginCommand(new LoginRequestDto { Username = "mechanic1", Password = "pass123" }),
            CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value!.Token, Is.EqualTo("test-jwt-token"));
        Assert.That(result.Value.Role, Is.EqualTo("Mechanic"));
        Assert.That(result.Value.FullName, Is.EqualTo("Petar Petrovic"));
    }

    [Test]
    public async Task Handle_UserNotFound_ShouldReturnFailure()
    {
        // Arrange
        _userRepoMock
            .Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _sut.Handle(
            new LoginCommand(new LoginRequestDto { Username = "nobody", Password = "pass" }),
            CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Does.Contain("Invalid"));
    }

    [Test]
    public async Task Handle_WrongPassword_ShouldReturnFailure()
    {
        // Arrange
        var user = new User
        {
            Username = "mechanic1",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct-pass")
        };

        _userRepoMock
            .Setup(r => r.GetByUsernameAsync("mechanic1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _sut.Handle(
            new LoginCommand(new LoginRequestDto { Username = "mechanic1", Password = "wrong-pass" }),
            CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Does.Contain("Invalid"));
    }

    [Test]
    public async Task Handle_FailedLogin_ShouldLogWarning()
    {
        // Arrange
        _userRepoMock
            .Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        await _sut.Handle(
            new LoginCommand(new LoginRequestDto { Username = "nobody", Password = "x" }),
            CancellationToken.None);

        // Assert
        _loggerMock.Verify(l => l.Warning(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Handle_SuccessfulLogin_ShouldLogAttemptInfo()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "mechanic1",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
            FirstName = "Petar",
            LastName = "Petrovic",
            Role = UserRole.Mechanic
        };

        _userRepoMock
            .Setup(r => r.GetByUsernameAsync("mechanic1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _jwtMock
            .Setup(j => j.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserRole>(), It.IsAny<string>()))
            .Returns("token");

        // Act
        await _sut.Handle(
            new LoginCommand(new LoginRequestDto { Username = "mechanic1", Password = "pass123" }),
            CancellationToken.None);

        // Assert - Info logged at least once (attempt log always happens)
        _loggerMock.Verify(l => l.Info(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public async Task Handle_SuccessfulLogin_ShouldNotLogWarning()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "mechanic1",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
            FirstName = "Petar",
            LastName = "Petrovic",
            Role = UserRole.Mechanic
        };

        _userRepoMock
            .Setup(r => r.GetByUsernameAsync("mechanic1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _jwtMock
            .Setup(j => j.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserRole>(), It.IsAny<string>()))
            .Returns("token");

        // Act
        await _sut.Handle(
            new LoginCommand(new LoginRequestDto { Username = "mechanic1", Password = "pass123" }),
            CancellationToken.None);

        // Assert - on success, Warning should never be called
        _loggerMock.Verify(l => l.Warning(It.IsAny<string>()), Times.Never);
    }
}

// ============================================================
// SERVICE TESTS 3 — CompleteServiceCommandHandler
// ============================================================

[TestFixture]
public class CompleteServiceCommandHandlerTests
{
    private Mock<IVehicleRepository> _vehicleRepoMock = default!;
    private Mock<IServiceInvoiceRepository> _invoiceRepoMock = default!;
    private Mock<IUnitOfWork> _uowMock = default!;
    private Mock<IBillingService> _billingMock = default!;
    private Mock<IEventLogger> _loggerMock = default!;
    private CompleteServiceCommandHandler _sut = default!;

    [SetUp]
    public void SetUp()
    {
        _vehicleRepoMock = new Mock<IVehicleRepository>();
        _invoiceRepoMock = new Mock<IServiceInvoiceRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _billingMock = new Mock<IBillingService>();
        _loggerMock = new Mock<IEventLogger>();

        _sut = new CompleteServiceCommandHandler(
            _vehicleRepoMock.Object,
            _invoiceRepoMock.Object,
            _uowMock.Object,
            _billingMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ValidVehicle_ShouldIssueInvoiceAndMarkVehicleAsServiced()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var mechanicId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            Id = vehicleId,
            LicensePlate = "NS-001-AA",
            Brand = "Volkswagen",
            Model = "Golf",
            EstimatedPrice = 10000m,
            IsServiced = false
        };

        _vehicleRepoMock
            .Setup(r => r.GetByIdAsync(vehicleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);
        _billingMock
            .Setup(b => b.CalculateFinalAmount(10000m))
            .Returns(8500m);

        // Act
        var result = await _sut.Handle(
            new CompleteServiceCommand(vehicleId, mechanicId, "Petar Petrovic"),
            CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value!.TotalAmount, Is.EqualTo(8500m));
        Assert.That(result.Value.MechanicName, Is.EqualTo("Petar Petrovic"));
        Assert.That(vehicle.IsServiced, Is.True);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_VehicleNotFound_ShouldReturnFailureAndNotSave()
    {
        // Arrange
        _vehicleRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.Handle(
            new CompleteServiceCommand(Guid.NewGuid(), Guid.NewGuid(), "Petar"),
            CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Does.Contain("not found"));
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Handle_AlreadyServicedVehicle_ShouldReturnFailureAndNotSave()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var vehicle = new Vehicle { Id = vehicleId, LicensePlate = "NS-001-AA", IsServiced = true };

        _vehicleRepoMock
            .Setup(r => r.GetByIdAsync(vehicleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _sut.Handle(
            new CompleteServiceCommand(vehicleId, Guid.NewGuid(), "Petar"),
            CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Does.Contain("already been serviced"));
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Handle_ValidVehicle_BillingServiceShouldBeCalledWithEstimatedPrice()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            Id = vehicleId,
            LicensePlate = "BG-123-XY",
            EstimatedPrice = 5000m,
            IsServiced = false
        };

        _vehicleRepoMock
            .Setup(r => r.GetByIdAsync(vehicleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);
        _billingMock
            .Setup(b => b.CalculateFinalAmount(5000m))
            .Returns(5500m);

        // Act
        await _sut.Handle(
            new CompleteServiceCommand(vehicleId, Guid.NewGuid(), "Nikola"),
            CancellationToken.None);

        // Assert - billing must be called exactly once with the vehicle's estimated price
        _billingMock.Verify(b => b.CalculateFinalAmount(5000m), Times.Once);
    }

    [Test]
    public async Task Handle_ValidVehicle_ShouldLogSuccessInfo()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            Id = vehicleId,
            LicensePlate = "NS-555-BB",
            EstimatedPrice = 12000m,
            IsServiced = false
        };

        _vehicleRepoMock
            .Setup(r => r.GetByIdAsync(vehicleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);
        _billingMock
            .Setup(b => b.CalculateFinalAmount(It.IsAny<decimal>()))
            .Returns(13200m);

        // Act
        await _sut.Handle(
            new CompleteServiceCommand(vehicleId, Guid.NewGuid(), "Nikola"),
            CancellationToken.None);

        // Assert
        _loggerMock.Verify(l => l.Info(It.IsAny<string>()), Times.AtLeastOnce);
    }
}