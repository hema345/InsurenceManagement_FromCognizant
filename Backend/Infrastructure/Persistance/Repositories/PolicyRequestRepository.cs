using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Infrastructure.Persistance.Repositories
{
    public class PolicyRequestRepository : IPolicyRequestRepository
    {
        private readonly InsuranceDbContext _context;
        public PolicyRequestRepository(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<IEnumerable<PolicyRequest>>> GetAllAsync()
        {
            var request = await _context.PolicyRequests.Include(c=>c.Customer).Include(a=>a.AvailablePolicy).ToListAsync();
            return OperationResult<IEnumerable<PolicyRequest>>.Success(request);
        }

        public async Task<OperationResult<IEnumerable<PolicyRequest>>> GetByCustomerIdAsync(int customerId)
        {
            var request =  await _context.PolicyRequests.Include(a => a.AvailablePolicy).Where(pr => pr.CustomerId == customerId).ToListAsync();
            if (!request.Any())
            {
                return OperationResult<IEnumerable<PolicyRequest>>.Failure("Id not found");
            }
            return OperationResult<IEnumerable<PolicyRequest>>.Success(request);
 
        }

        public async Task<OperationResult<PolicyRequest?>> GetByIdAsync(int requestId)
        {
            var request = await _context.PolicyRequests.Include(a => a.AvailablePolicy).FirstOrDefaultAsync(r=>r.RequestId == requestId);
            if (request == null)
            {
                return OperationResult<PolicyRequest?>.Failure("Id not found");
            }
            return OperationResult<PolicyRequest?>.Success(request);


        }

        public async Task<OperationResult<bool>> AddAsync(PolicyRequest request)
        {
          var requests =  await _context.PolicyRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true, "requested policy successfully");
        }

        public async Task<OperationResult<bool>> UpdateStatusAsync(int requestId, string status)
        {
            var request = await _context.PolicyRequests.FindAsync(requestId);
            if (request == null)
            {
                return OperationResult<bool>.Failure("not found");
            }
            
                request.Status = status;
                _context.PolicyRequests.Update(request);
            
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true,"updated successfully");
        }

    }
}
