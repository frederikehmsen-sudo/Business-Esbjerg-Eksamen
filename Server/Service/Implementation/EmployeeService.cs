using Infrastructure.Entities;
using Infrastructure.Repositories;
using Service.DTOs;
using Service.Security;

namespace Service.Implementation;

public class EmployeeService(
    IEmployeeRepository employeeRepository,
    IUserRepository userRepository,
    IWorkScheduleRepository workScheduleRepository,
    IPasswordHasher passwordHasher,
    TimeProvider timeProvider) : IEmployeeService
{
    private const string EmailDomain = "@businessesbjerg.com";

    private DateOnly Today => DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);

    public List<EmployeeResponse> GetAll(bool includeFormer)
    {
        var usersById = userRepository.GetAll().ToDictionary(u => u.Id);

        return employeeRepository.GetAll(includeFormer, Today)
            .Where(e => usersById.ContainsKey(e.UserId))
            .Select(e => ToResponse(e, usersById[e.UserId]))
            .ToList();
    }

    public EmployeeResponse GetById(Guid id)
    {
        var employee = GetEmployee(id);
        return ToResponse(employee, GetUser(employee));
    }

    public EmployeeResponse Create(CreateEmployeeRequest request)
    {
        var name = RequireName(request.Name);
        var email = NormalizeEmail(request.Email);
        RequireEmploymentType(request.EmploymentType);
        RequirePassword(request.InitialPassword);

        if (request.StartDate == default)
            throw new ArgumentException("Employment start date is required.");

        if (request.WorkSchedule is null)
            throw new ArgumentException("An initial work schedule is required.");

        WorkScheduleMapper.Validate(request.WorkSchedule);

        if (employeeRepository.EmailExists(email))
            throw new InvalidOperationException("Email already exists.");

        var user = new User
        {
            Username = email,
            Password = passwordHasher.HashAndSaltPassword(request.InitialPassword),
            Role = Role.Employee,
            IsActive = true
        };

        var employee = new Employee
        {
            Name = name,
            Email = email,
            StartDate = request.StartDate,
            EmploymentType = request.EmploymentType
        };

        // The first schedule always starts on the employment start date.
        var firstSchedule = WorkScheduleMapper.ToEntity(request.WorkSchedule, request.StartDate);

        employeeRepository.Create(employee, user, firstSchedule);

        return ToResponse(employee, user);
    }

    public EmployeeResponse Update(Guid id, UpdateEmployeeRequest request)
    {
        var employee = GetEmployee(id);
        var user = GetUser(employee);

        var name = RequireName(request.Name);
        var email = NormalizeEmail(request.Email);
        RequireEmploymentType(request.EmploymentType);

        var emailChanged = !string.Equals(email, employee.Email, StringComparison.OrdinalIgnoreCase);
        if (emailChanged && employeeRepository.EmailExists(email))
            throw new InvalidOperationException("Email already exists.");

        employee.Name = name;
        employee.Email = email;
        employee.EmploymentType = request.EmploymentType;
        user.Username = email; 

        employeeRepository.Update(employee, user);

        return ToResponse(employee, user);
    }

    public void EndEmployment(Guid id, DateOnly? endDate)
    {
        var employee = GetEmployee(id);
        var user = GetUser(employee);

        if (employee.EndDate is not null)
            throw new InvalidOperationException("Employment has already ended.");

        var end = endDate ?? Today;
        if (end < employee.StartDate)
            throw new ArgumentException("End date cannot be before the employment start date.");

        var current = workScheduleRepository.GetLatest(employee.Id);
        if (current is not null && current.ValidFrom > end)
            throw new ArgumentException(
                $"End date cannot be before the latest work schedule starts ({current.ValidFrom:yyyy-MM-dd}).");

        if (current is not null)
            current.ValidTo = end;

        employee.EndDate = end;

        employeeRepository.EndEmployment(employee, user, current);
    }

    public void ResetPassword(Guid id, ResetPasswordRequest request)
    {
        var employee = GetEmployee(id);
        var user = GetUser(employee);

        RequirePassword(request.NewPassword);

        user.Password = passwordHasher.HashAndSaltPassword(request.NewPassword);
        userRepository.UpdateUser(user);
    }

    // helpers

    private Employee GetEmployee(Guid id) =>
        employeeRepository.GetById(id) ?? throw new KeyNotFoundException("Employee not found.");

    private User GetUser(Employee employee) =>
        userRepository.GetById(employee.UserId) ?? throw new KeyNotFoundException("Login for employee not found.");

    private static string RequireName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");
        return name.Trim();
    }

    private static string NormalizeEmail(string email)
    {
        var normalized = (email ?? string.Empty).Trim().ToLowerInvariant();

        if (!normalized.EndsWith(EmailDomain) || normalized.Length == EmailDomain.Length)
            throw new ArgumentException($"Email must end with {EmailDomain}.");

        return normalized;
    }

    private static void RequireEmploymentType(EmploymentType type)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentException("Employment type must be FullTime or PartTime.");
    }

    private static void RequirePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters.");
    }

    private EmployeeResponse ToResponse(Employee e, User user) =>
        new(e.Id, e.Name, e.Email, e.StartDate, e.EndDate, e.EmploymentType.ToString(),
            IsActive: user.IsActive && (e.EndDate is null || e.EndDate >= Today));
}