using LinqToDB.Mapping;

namespace Infrastructure.Entities;


public class HolidayRequestEvent
{
    [PrimaryKey] public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RequestId { get; set; }
    public HolidayRequestStatus Status { get; set; }
    public Guid ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Note { get; set; }
}