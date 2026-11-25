using Infrastructure.Entities;
using Infrastructure.Repositories;
using Service.DTOs;

namespace Service.Implementation;

public class WorkScheduleService(
    IWorkScheduleRepository workScheduleRepository,
    IEmployeeRepository employeeRepository) : IWorkScheduleService
{
    public List<WorkScheduleResponse> GetForEmployee(Role actorRole, Guid actorId, Guid employeeId)
    {
        var employee = GetEmployee(employeeId);
        
        if (actorRole == Role.Employee && employee.UserId != actorId)
            throw new UnauthorizedAccessException("You can only see your own work schedule.");

        return History(employee.Id);
    }

    public List<WorkScheduleResponse> GetMine(Guid actorId)
    {
        var employee = employeeRepository.GetByUserId(actorId)
                       ?? throw new KeyNotFoundException("No employee profile for this user.");

        return History(employee.Id);
    }

    public WorkScheduleResponse Add(Role actorRole, Guid employeeId, CreateWorkScheduleRequest request)
    {
        if (actorRole is not (Role.Admin or Role.Accountant))
            throw new UnauthorizedAccessException("Only Admin or Accountant can change work schedules.");

        WorkScheduleMapper.Validate(request);

        var employee = GetEmployee(employeeId);

        if (employee.EndDate is not null)
            throw new InvalidOperationException("Employment has ended. No new schedules can be added.");

        if (request.ValidFrom < employee.StartDate)
            throw new ArgumentException("ValidFrom cannot be before the employment start date.");

        var current = workScheduleRepository.GetLatest(employee.Id);

        if (current is null && request.ValidFrom != employee.StartDate)
            throw new ArgumentException("The first schedule must start on the employment start date.");

        if (current is not null && request.ValidFrom <= current.ValidFrom)
            throw new ArgumentException(
                $"ValidFrom must be after the current schedule's ValidFrom ({current.ValidFrom:yyyy-MM-dd}).");
        
        if (current is not null)
            current.ValidTo = request.ValidFrom.AddDays(-1);

        var next = WorkScheduleMapper.ToEntity(request, request.ValidFrom, employee.Id);
        workScheduleRepository.AddAndClosePrevious(current, next);

        return WorkScheduleMapper.ToResponse(next);
    }

    private Employee GetEmployee(Guid id) =>
        employeeRepository.GetById(id) ?? throw new KeyNotFoundException("Employee not found.");

    private List<WorkScheduleResponse> History(Guid employeeId) =>
        workScheduleRepository.GetForEmployee(employeeId)
            .Select(WorkScheduleMapper.ToResponse)
            .ToList();
}