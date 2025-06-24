

namespace InsurenceManagementSystemWebApi.Applications.Services
{
     public interface ICustomerService
     { 
        public Task<OperationResult<CustomerProfileResponseDto>> GetProfileAsync();
        public Task<OperationResult<IEnumerable<CustomerPoliciesResponseDto>>> GetRegisteredPoliciesAsync();
        public Task<OperationResult<IEnumerable<PolicyRequestStatusResponseDto>>> GetPolicyRequestsAsync(); 
        public Task<OperationResult<bool>> RequestPolicyAsync(PolicyRequestDto requestDto);
        public Task<OperationResult<bool>> FileClaimAsync(ClaimFilingRequestDtoForCustomer claimDto);
        public Task<OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>> GetMyClaimsAsync();
        public Task<OperationResult<IEnumerable<NotificationResponseDto>>> GetMyNotificationsAsync();
        public Task<OperationResult<bool>> UpdateProfileAsync(CustomerProfileUpdateRequestDto updateCustomerProfile);
        public Task<OperationResult<PagedResult<AvailablePolicyResponseDto>>> GetAllAvailablePoliciesAsync(int page, int size);
        public Task<OperationResult<AvailablePolicyResponseDto>> GetAvailablePolicyByIdAsync(int policyId);
        public Task<OperationResult<PolicyRequestStatusResponseDto>> GetPolicyRequestByIdAsync(int requestId);
        public Task<OperationResult<bool>> DeleteAccountAsync();
       
    }
    
}
