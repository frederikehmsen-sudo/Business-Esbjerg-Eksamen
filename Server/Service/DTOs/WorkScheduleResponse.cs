namespace Service.DTOs;

public record WorkScheduleDayResponse(string Day, TimeOnly? From, TimeOnly? To, decimal Hours);

public record WorkScheduleResponse(
    Guid Id, DateOnly ValidFrom, DateOnly? ValidTo,
    List<WorkScheduleDayResponse> Days, decimal WeeklyHours);