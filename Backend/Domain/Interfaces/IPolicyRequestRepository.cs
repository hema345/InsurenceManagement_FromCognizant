using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Domain.Interfaces
{
    public interface IPolicyRequestRepository 
    {
        public Task<OperationResult<IEnumerable<PolicyRequest>>> GetAllAsync(); 
        public Task<OperationResult<PolicyRequest?>> GetByIdAsync(int requestId);
        public Task<OperationResult<IEnumerable<PolicyRequest>>> GetByCustomerIdAsync(int customerId); 
        public Task<OperationResult<bool>> AddAsync(PolicyRequest request); 
        public Task<OperationResult<bool>> UpdateStatusAsync(int requestId, string status);
    }
}
