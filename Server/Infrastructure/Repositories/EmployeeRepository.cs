using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace Infrastructure.Repositories;

public class EmployeeRepository(AppDatabase db) : IEmployeeRepository
{
   
    public void Create(Employee employee, User user, WorkSchedule firstSchedule)
    {
        using var tr = db.BeginTransaction();
        db.Insert(user);
        employee.UserId = user.Id;
        db.Insert(employee);
        firstSchedule.EmployeeId = employee.Id;
        db.Insert(firstSchedule);
        tr.Commit();
    }

    public Employee? GetById(Guid id) =>
        db.Employees.FirstOrDefault(e => e.Id == id);

    public Employee? GetByUserId(Guid userId) =>
        db.Employees.FirstOrDefault(e => e.UserId == userId);

    public List<Employee> GetAll(bool includeFormer)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var query = db.Employees.AsQueryable();

        if (!includeFormer)
            query = query.Where(e => e.EndDate == null || e.EndDate > today);

        return query.OrderBy(e => e.Name).ToList();
    }

    
    public bool EmailExists(string email)
    {
        var lower = email.ToLower();
        return db.Employees.Any(e => e.Email.ToLower() == lower)
            || db.Users.Any(u => u.Username.ToLower() == lower);
    }

    public void Update(Employee employee, User user)
    {
        using var tr = db.BeginTransaction();
        db.Update(employee);
        db.Update(user);
        tr.Commit();
    }

    public void EndEmployment(Employee employee, User user, WorkSchedule? scheduleToClose)
    {
        using var tr = db.BeginTransaction();
        db.Update(employee);
        db.Update(user);
        if (scheduleToClose is not null)
            db.WorkSchedules
                .Where(s => s.Id == scheduleToClose.Id)
                .Set(s => s.ValidTo, scheduleToClose.ValidTo)
                .Update();
        tr.Commit();
    }
}