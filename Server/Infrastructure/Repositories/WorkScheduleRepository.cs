using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace Infrastructure.Repositories;

public class WorkScheduleRepository(AppDatabase db) : IWorkScheduleRepository
{

    public List<WorkSchedule> GetForEmployee(Guid employeeId) =>
        db.WorkSchedules
            .Where(s => s.EmployeeId == employeeId)
            .OrderBy(s => s.ValidFrom)
            .ToList();

   
    public WorkSchedule? GetLatest(Guid employeeId) =>
        db.WorkSchedules
            .Where(s => s.EmployeeId == employeeId)
            .OrderByDescending(s => s.ValidFrom)
            .FirstOrDefault();
    
    public void AddAndClosePrevious(WorkSchedule? previous, WorkSchedule next)
    {
        using var tr = db.BeginTransaction();
        if (previous is not null)
            db.WorkSchedules
                .Where(s => s.Id == previous.Id)
                .Set(s => s.ValidTo, previous.ValidTo)
                .Update();
        db.Insert(next);
        tr.Commit();
    }
}