using Infrastructure.Entities;
using Service.DTOs;

namespace Service.Implementation;

public interface IHolidayRuleSetService
{
    HolidayRuleSetResponse Create(Role actorRole, Guid actorId, CreateHolidayRuleSetRequest request);
    List<HolidayRuleSetResponse> GetHistory();
    HolidayRuleSetResponse GetInForce(DateOnly date);
    List<CreateHolidayRuleSetRequest> GetPresets();
}