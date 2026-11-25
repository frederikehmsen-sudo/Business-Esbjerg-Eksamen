using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class CalendarDayOff
{
    [PrimaryKey] public Guid Id { get; set; } = Guid.NewGuid();
    public DateOnly Date { get; set; }
    public string Name { get; set; } = string.Empty;
    public DayOffType Type {get;set;}
}