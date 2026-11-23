using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class Employee
{
    [PrimaryKey] public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public EmploymentType EmploymentType { get; set; }
}