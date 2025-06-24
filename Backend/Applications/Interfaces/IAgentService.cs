using System.Collections;
using InsurenceManagementSystemWebApi.Applications.DTOs;
using InsurenceManagementSystemWebApi.Shared.Common;

namespace InsurenceManagementSystemWebApi.Applications.Services
{
    public interface IAgentService
    {
        public Task<OperationResult<AgentProfileResponseDto>> GetProfileAsync();
        public Task<OperationResult<IEnumerable<AgentAssignedPolicyResponseDto>>> GetAssignedPoliciesAsync();
        public Task<OperationResult<IEnumerable<ClaimStatusResponseDtoForAgent>>>GetFiledClaimsAsync();
        public Task<OperationResult<bool>> FileClaimAsync(ClaimFilingRequestDtoForAgent claimDto);
        public Task<OperationResult<IEnumerable<NotificationResponseDto>>> GetMyNotificationsAsync();
        public Task<OperationResult<bool>> UpdateProfileAsync( AgentProfileUpdateRequestDto updateAgentProfileDto);
        public Task<OperationResult<PagedResult<AvailablePolicyResponseDto>>> GetAllAvailablePoliciesAsync(int page, int size);
        public Task<OperationResult<AvailablePolicyResponseDto>> GetAvailablePolicyByIdAsync(int policyId);
    }

}
