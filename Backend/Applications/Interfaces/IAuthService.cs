

namespace InsurenceManagementSystemWebApi.Applications.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
        Task<OperationResult<CustomerRegisterResponseDto>> RegisterAsync(CustomerRegisterRequestDto dto);
        OperationResult<string> GetRoleService();
    }
}
