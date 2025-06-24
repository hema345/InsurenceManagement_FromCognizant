using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Domain.Interfaces
{
    public interface IRoleRepository
    {
        public Task<OperationResult<IEnumerable<Role>>> GetAllAsync();
        public Task<OperationResult<Role?>> GetByNameAsync(string name);
        public Task<OperationResult<bool>> AddAsync(Role role); 
    }
}
