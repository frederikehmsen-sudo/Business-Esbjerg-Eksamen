using Infrastructure.Entities;
using Service.DTOs;

namespace Service.Implementation;

public interface IWorkScheduleService
{
    List<WorkScheduleResponse> GetForEmployee(Role actorRole, Guid actorId, Guid employeeId);
    List<WorkScheduleResponse> GetMine(Guid actorId);
    WorkScheduleResponse Add(Role actorRole, Guid employeeId, CreateWorkScheduleRequest request);
}