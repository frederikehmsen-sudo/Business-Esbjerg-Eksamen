using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public interface IEmployeeRepository
{
    void Create(Employee employee, User user);
    Employee? GetById(Guid id);
    List<Employee> GetAll(bool includeFormer);
    bool EmailExists(string email);
    void Update(Employee employee);
}