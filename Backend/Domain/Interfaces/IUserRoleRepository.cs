using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Domain.Interfaces
{
    public interface IUserRoleRepository
    {
        public Task<OperationResult<bool>> AddAsync(UserRole userRole);
    }
}
