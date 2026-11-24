using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public interface IEmployeeRepository
{
    void Create(Employee employee, User user, WorkSchedule firstSchedule);
    Employee? GetById(Guid id);
    Employee? GetByUserId(Guid userId);
    List<Employee> GetAll(bool includeFormer);
    bool EmailExists(string email);
    void Update(Employee employee, User user);
    void EndEmployment(Employee employee, User user, WorkSchedule? scheduleToClose);
}