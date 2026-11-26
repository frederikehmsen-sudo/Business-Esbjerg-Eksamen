using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public interface IWorkScheduleRepository
{
    List<WorkSchedule> GetForEmployee(Guid employeeId);
    WorkSchedule? GetLatest(Guid employeeId);
    void AddAndClosePrevious(WorkSchedule? previous, WorkSchedule next);
}