using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class HolidayRequest
{
    [PrimaryKey] public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public HolidayAccount Account { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    public int WorkingDays { get; set; }
    public decimal Hours { get; set; }

    public HolidayRequestStatus Status { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}