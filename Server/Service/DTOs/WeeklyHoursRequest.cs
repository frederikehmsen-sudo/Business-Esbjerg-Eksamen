namespace Service.DTOs;

public class WeeklyHoursRequest
{
    public TimeOnly? MondayFrom { get; set; }
    public TimeOnly? MondayTo { get; set; }
    public TimeOnly? TuesdayFrom { get; set; }
    public TimeOnly? TuesdayTo { get; set; }
    public TimeOnly? WednesdayFrom { get; set; }
    public TimeOnly? WednesdayTo { get; set; }
    public TimeOnly? ThursdayFrom { get; set; }
    public TimeOnly? ThursdayTo { get; set; }
    public TimeOnly? FridayFrom { get; set; }
    public TimeOnly? FridayTo { get; set; }
}

public class CreateWorkScheduleRequest : WeeklyHoursRequest
{
    public DateOnly ValidFrom { get; set; }
}