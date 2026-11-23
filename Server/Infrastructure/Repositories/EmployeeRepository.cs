using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace Infrastructure.Repositories;

public class EmployeeRepository(UserDatabase db) : IEmployeeRepository
{
    // transaction login and employee
    public void Create(Employee employee, User user)
    {
        using var tr = db.BeginTransaction();
        db.Insert(user);
        employee.UserId = user.Id;
        db.Insert(employee);
        tr.Commit();
    }

    public Employee? GetById(Guid id) =>
        db.Employees.FirstOrDefault(e => e.Id == id);

    public List<Employee> GetAll(bool includeFormer)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var query = db.Employees.AsQueryable();

        if (!includeFormer)
            query = query.Where(e => e.EndDate == null || e.EndDate > today);

        return query.OrderBy(e => e.Name).ToList();
    }

    public bool EmailExists(string email) =>
        db.Employees.Any(e => e.Email.ToLower() == email.ToLower());

    public void Update(Employee employee) =>
        db.Update(employee);
}