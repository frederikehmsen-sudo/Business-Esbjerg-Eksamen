using Service.DTOs;

namespace Service.Implementation;

public interface IEmployeeService
{
    List<EmployeeResponse> GetAll(bool includeFormer);
    EmployeeResponse GetById(Guid id);
    EmployeeResponse Create(CreateEmployeeRequest request);
    EmployeeResponse Update(Guid id, UpdateEmployeeRequest request);
    void EndEmployment(Guid id, DateOnly? endDate);
    void ResetPassword(Guid id, ResetPasswordRequest request);
}