

namespace InsurenceManagementSystemWebApi.Applications.Interfaces
{
    public interface IAdminService
    {
        public Task<OperationResult<PagedResult<CustomerProfileResponseDto>>> GetAllCustomersAsync(int page, int size);
        public Task<OperationResult<PagedResult<AgentProfileResponseDto>>> GetAllAgentsAsync(int page, int size);
        public Task<OperationResult<PagedResult<AvailablePolicyResponseDto>>> GetAllAvailablePoliciesAsync(int page, int size);
        public Task<OperationResult<bool>> AddAvailablePolicyAsync(AvailablePolicyRequestDto dto);
        public Task<OperationResult<bool>> UpdateAvailablePolicyAsync(AvailablePolicyRequestDto dto, int policyId);
        public Task<OperationResult<bool>> DeleteAvailablePolicyAsync(int id);
        public Task<OperationResult<PagedResult<PolicyRequestStatusResponseDto>>> GetAllPolicyRequestsAsync(int page, int size);
        public Task<OperationResult<bool>> ApprovePolicyRequestAsync(AssignAgentRequestDto dto,int requestId);
        public Task<OperationResult<bool>> RejectPolicyRequestAsync(int requestId);
        public Task<OperationResult<PagedResult<ClaimStatusResponseDtoForAdmin>>> GetAllClaimsAsync(int page, int size);
        public Task<OperationResult<bool>> ApproveClaimAsync(int claimId);
        public Task<OperationResult<bool>> RejectClaimAsync(int claimId);
        public Task<OperationResult<AvailablePolicyResponseDto>> GetAvailablePolicyByIdAsync(int policyId);
        public Task<OperationResult<CustomerProfileResponseDto>> GetCustomerByIdAsync(int customerId);
        public Task<OperationResult<AgentProfileResponseDto>> GetAgentByIdAsync(int agentId);
        public Task<OperationResult<IEnumerable<ClaimsFiledByCustomerResponseDtoForAdmin>>> GetClaimsByCustomerIdAsync(int customerId);





        public Task<OperationResult<bool>> IsFiledByAgentAsync(int claimId);
        public Task<OperationResult<IEnumerable<UserWithRoleResponseDto>>> GetAllUsersAsync();
        public Task<OperationResult<CustomerRegisterResponseDto>> AddCustomerAsync(CustomerRegisterRequestDto dto);
        public Task<OperationResult<AgentRegisterResponseDto>> AddAgentAsync(AgentRegisterRequestDto dto);
    }
    


    

}
