using Infrastructure.Entities;
using Infrastructure.Repositories;
using NSubstitute;
using Service.DTOs;
using Service.Implementation;
using Service.Presets;

namespace Tests.UnitTests;

public class HolidayRuleSetServiceTests
{
    private static readonly DateOnly Today = new(2026, 11, 26);

    private readonly IHolidayRuleSetRepository _repository = Substitute.For<IHolidayRuleSetRepository>();
    private readonly HolidayRuleSetService _service;

    public HolidayRuleSetServiceTests()
    {
        _service = new HolidayRuleSetService(_repository, new FixedTimeProvider(Today));
    }

    [Fact]
    public void Create_ValidPresetByAdmin_SavesRuleSetWithCreator()
    {
        var adminId = Guid.NewGuid();
        var request = Preset(Today.AddMonths(1));

        var result = _service.Create(Role.Admin, adminId, request);

        Assert.Equal(adminId, result.CreatedBy);
        Assert.Equal(Today.AddMonths(1), result.EffectiveFrom);
        _repository.Received(1).Add(Arg.Is<HolidayRuleSet>(r => r.Week6Enabled && r.CreatedBy == adminId));
    }

    [Theory]
    [InlineData(Role.Employee)]
    [InlineData(Role.Accountant)]
    public void Create_NotAdmin_ThrowsUnauthorized(Role role)
    {
        var request = Preset(Today.AddMonths(1));

        Assert.Throws<UnauthorizedAccessException>(() => _service.Create(role, Guid.NewGuid(), request));
        _repository.DidNotReceive().Add(Arg.Any<HolidayRuleSet>());
    }

    [Fact]
    public void Create_EffectiveFromInPast_Throws()
    {
        var request = Preset(Today.AddDays(-1));

        Assert.Throws<ArgumentException>(() => _service.Create(Role.Admin, Guid.NewGuid(), request));
        _repository.DidNotReceive().Add(Arg.Any<HolidayRuleSet>());
    }

    private static CreateHolidayRuleSetRequest Preset(DateOnly effectiveFrom)
    {
        var preset = HolidayRuleSetPresets.FerielovenWith6thWeek();
        preset.EffectiveFrom = effectiveFrom;
        return preset;
    }
}