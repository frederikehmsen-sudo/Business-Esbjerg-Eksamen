using Infrastructure.Entities;
using Infrastructure.Repositories;
using NSubstitute;
using Service.DTOs;
using Service.Implementation;
using Service.Security;

namespace Tests.UnitTests;

public class EmployeeServiceTests
{
    private static readonly DateOnly Today = new(2026, 11, 26);

    private readonly IEmployeeRepository _employeeRepository = Substitute.For<IEmployeeRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IWorkScheduleRepository _scheduleRepository = Substitute.For<IWorkScheduleRepository>();
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        var passwordHasher = Substitute.For<IPasswordHasher>();
        passwordHasher.HashAndSaltPassword(Arg.Any<string>()).Returns("hashed");

        _service = new EmployeeService(
            _employeeRepository,
            _userRepository,
            _scheduleRepository,
            passwordHasher,
            new FixedTimeProvider(Today));
    }

    [Fact]
    public void Create_ValidRequest_CreatesEmployeeLoginAndFirstSchedule()
    {
        // Arrange
        var request = CreateRequest("  Anna.Jensen@BusinessEsbjerg.com ");

        // Act
        var result = _service.Create(request);

        // Assert
        Assert.Equal("anna.jensen@businessesbjerg.com", result.Email);
        _employeeRepository.Received(1).Create(
            Arg.Is<Employee>(e => e.Email == "anna.jensen@businessesbjerg.com"),
            Arg.Is<User>(u => u.Role == Role.Employee && u.Username == "anna.jensen@businessesbjerg.com"),
            Arg.Is<WorkSchedule>(s => s.ValidFrom == request.StartDate && s.ValidTo == null));
    }

    [Theory]
    [InlineData("anna@gmail.com")]
    [InlineData("@businessesbjerg.com")]
    public void Create_EmailNotOnCompanyDomain_Throws(string email)
    {
        Assert.Throws<ArgumentException>(() => _service.Create(CreateRequest(email)));
        _employeeRepository.DidNotReceive().Create(Arg.Any<Employee>(), Arg.Any<User>(), Arg.Any<WorkSchedule>());
    }

    [Fact]
    public void EndEmployment_ClosesCurrentScheduleOnEndDate()
    {
        // Arrange
        var (employee, user) = ExistingEmployee();
        var current = new WorkSchedule { EmployeeId = employee.Id, ValidFrom = employee.StartDate };
        _scheduleRepository.GetLatest(employee.Id).Returns(current);
        var endDate = new DateOnly(2026, 12, 31);

        // Act
        _service.EndEmployment(employee.Id, endDate);

        // Assert
        Assert.Equal(endDate, employee.EndDate);
        Assert.Equal(endDate, current.ValidTo);
        _employeeRepository.Received(1).EndEmployment(employee, user, current);
    }

    [Fact]
    public void EndEmployment_AlreadyEnded_Throws()
    {
        var (employee, _) = ExistingEmployee();
        employee.EndDate = new DateOnly(2026, 6, 30);

        Assert.Throws<InvalidOperationException>(() => _service.EndEmployment(employee.Id, Today));
        _employeeRepository.DidNotReceive()
            .EndEmployment(Arg.Any<Employee>(), Arg.Any<User>(), Arg.Any<WorkSchedule?>());
    }

    // Helpers

    private (Employee, User) ExistingEmployee()
    {
        var user = new User { Role = Role.Employee };
        var employee = new Employee { UserId = user.Id, StartDate = new DateOnly(2025, 1, 1) };
        _employeeRepository.GetById(employee.Id).Returns(employee);
        _userRepository.GetById(user.Id).Returns(user);
        return (employee, user);
    }

    private static CreateEmployeeRequest CreateRequest(string email) => new()
    {
        Name = "Anna Jensen",
        Email = email,
        StartDate = new DateOnly(2026, 1, 1),
        EmploymentType = EmploymentType.FullTime,
        InitialPassword = "password123",
        WorkSchedule = new WeeklyHoursRequest
        {
            MondayFrom = new TimeOnly(8, 0),
            MondayTo = new TimeOnly(16, 0)
        }
    };
}