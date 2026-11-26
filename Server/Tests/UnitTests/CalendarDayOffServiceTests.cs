using Infrastructure.Entities;
using Infrastructure.Repositories;
using NSubstitute;
using Service.DTOs;
using Service.Implementation;

namespace Tests.UnitTests;

public class CalendarDayOffServiceTests
{
    private static readonly DateOnly Today = new(2026, 11, 26);

    private readonly ICalendarDayOffRepository _repository = Substitute.For<ICalendarDayOffRepository>();
    private readonly CalendarDayOffService _service;

    public CalendarDayOffServiceTests()
    {
        _service = new CalendarDayOffService(_repository, new FixedTimeProvider(Today));
    }

    [Theory]
    [InlineData(Role.Employee)]
    [InlineData(Role.Accountant)]
    public void Create_NotAdmin_ThrowsUnauthorized(Role role)
    {
        var request = Request(Today.AddDays(10));

        Assert.Throws<UnauthorizedAccessException>(() => _service.Create(role, request));
        _repository.DidNotReceive().Add(Arg.Any<CalendarDayOff>());
    }

    [Fact]
    public void Create_DateInPast_Throws()
    {
        var request = Request(Today.AddDays(-1));

        Assert.Throws<InvalidOperationException>(() => _service.Create(Role.Admin, request));
        _repository.DidNotReceive().Add(Arg.Any<CalendarDayOff>());
    }

    [Fact]
    public void GenerateDanishHolidays_SkipsPastDaysAndExistingDays()
    {
        // Arrange
        _repository.ExistsOnDate(new DateOnly(2026, 12, 25)).Returns(true);

        // Act
        var created = _service.GenerateDanishHolidays(Role.Admin, 2026);

        // Assert
        Assert.Equal(
            new[] { new DateOnly(2026, 12, 24), new DateOnly(2026, 12, 26), new DateOnly(2026, 12, 31) },
            created.Select(d => d.Date));
        _repository.Received(3).Add(Arg.Any<CalendarDayOff>());
    }

    private static CreateCalendarDayOffRequest Request(DateOnly date) =>
        new() { Date = date, Name = "Firmadag", Type = DayOffType.CompanyDayOff };
}