using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class CalendarDayOff
{
    [PrimaryKey] public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string Name { get; set; }
    public DayOffType Type {get;set;}
}