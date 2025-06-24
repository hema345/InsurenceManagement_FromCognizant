using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Applications.Services
{
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _agentRepo;
        private readonly IPolicyRepository _policyRepo;
        private readonly IClaimRepository _claimRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly IAvailablePolicyRepository _availablePolicyRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AgentService> _logger; 

        public AgentService(
            IAgentRepository agentRepo,
            IPolicyRepository policyRepo,
            IClaimRepository claimRepo,
            INotificationRepository notificationRepo,
            ITokenService tokenService,
            IAvailablePolicyRepository availablePolicyRepo,
            IMapper mapper,
            ILogger<AgentService> logger) 
        {
            _agentRepo = agentRepo;
            _policyRepo = policyRepo;
            _claimRepo = claimRepo;
            _notificationRepo = notificationRepo;
            _availablePolicyRepo = availablePolicyRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger; 
        }

        public async Task<OperationResult<AgentProfileResponseDto>> GetProfileAsync()
        {
            _logger.LogInformation("Attempting to retrieve agent profile.");
            int? agentId = _tokenService.GetAgentIdFromCurrentRequest();

            if (agentId == null)
            {
                _logger.LogWarning("Failed to retrieve AgentId from JWT token for GetProfileAsync.");
                return OperationResult<AgentProfileResponseDto>.Failure("Unable to Get the AgentId from the Jwt Token");
            }
            _logger.LogDebug("AgentId retrieved from token: {AgentId}", agentId);

            var agent = await _agentRepo.GetByUserIdAsync((int)agentId);
            if (!agent.IsSuccess)
            {
                _logger.LogError("Failed to fetch agent profile for User ID {AgentId}. Error: {ErrorMessage}", agentId, agent.Message);
                return OperationResult<AgentProfileResponseDto>.Failure(agent.Message);
            }
            if (agent.Data == null)
            {
                _logger.LogWarning("Agent profile not found for User ID {AgentId}.", agentId);
                return OperationResult<AgentProfileResponseDto>.Failure("Agent Profile not found");
            }

            var result = _mapper.Map<AgentProfileResponseDto>(agent.Data);
            _logger.LogInformation("Successfully retrieved agent profile for User ID {AgentId}.", agentId);
            return OperationResult<AgentProfileResponseDto>.Success(result);
        }

        public async Task<OperationResult<IEnumerable<AgentAssignedPolicyResponseDto>>> GetAssignedPoliciesAsync()
        {
            _logger.LogInformation("Attempting to retrieve assigned policies for agent.");
            int? agentId = _tokenService.GetAgentIdFromCurrentRequest();

            if (agentId == null)
            {
                _logger.LogWarning("Failed to retrieve AgentId from JWT token for GetAssignedPoliciesAsync.");
                return OperationResult<IEnumerable<AgentAssignedPolicyResponseDto>>.Failure("Unable to Get the AgentId from the Jwt Token");
            }
            _logger.LogDebug("AgentId retrieved from token: {AgentId}", agentId);


            var result = await _policyRepo.GetByAgentIdAsync((int)agentId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to fetch policies for agent ID {AgentId}. Error: {ErrorMessage}", agentId, result.Message);
                return OperationResult<IEnumerable<AgentAssignedPolicyResponseDto>>.Failure("No policies found for the specified agent.");
            }
            var policies = result.Data;
            if (policies?.Any() != true)
            {
                _logger.LogInformation("No policies found for agent ID {AgentId}.", agentId);
                return OperationResult<IEnumerable<AgentAssignedPolicyResponseDto>>.Failure("No Policies Found");
            }

            var assignedPolicies = policies.Select(p => _mapper.Map<AgentAssignedPolicyResponseDto>(p)).ToList();
            _logger.LogInformation("Successfully retrieved {Count} assigned policies for agent ID {AgentId}.", assignedPolicies.Count, agentId);
            return OperationResult<IEnumerable<AgentAssignedPolicyResponseDto>>.Success(assignedPolicies);
        }

        public async Task<OperationResult<IEnumerable<ClaimStatusResponseDtoForAgent>>> GetFiledClaimsAsync()
        {
            _logger.LogInformation("Attempting to retrieve filed claims for agent.");
            int? agentId = _tokenService.GetAgentIdFromCurrentRequest();

            if (agentId == null)
            {
                _logger.LogWarning("Failed to retrieve AgentId from JWT token for GetFiledClaimsAsync.");
                return OperationResult<IEnumerable<ClaimStatusResponseDtoForAgent>>.Failure("Unable to Get the AgentId from the Jwt Token");
            }
            _logger.LogDebug("AgentId retrieved from token: {AgentId}", agentId);


            var result = await _claimRepo.GetByAgentIdAsync((int)agentId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to fetch claims for agent ID {AgentId}. Error: {ErrorMessage}", agentId, result.Message);
                return OperationResult<IEnumerable<ClaimStatusResponseDtoForAgent>>.Failure("No claims found for the specified agent.");
            }
            var claims = result.Data;
            if (claims?.Any() != true)
            {
                _logger.LogInformation("No claims found for agent ID {AgentId}.", agentId);
                return OperationResult<IEnumerable<ClaimStatusResponseDtoForAgent>>.Failure("No claims found.");
            }

            var filedClaims = claims.Select(c => _mapper.Map<ClaimStatusResponseDtoForAgent>(c));
            _logger.LogInformation("Successfully retrieved {Count} filed claims for agent ID {AgentId}.", filedClaims.Count(), agentId);
            return OperationResult<IEnumerable<ClaimStatusResponseDtoForAgent>>.Success(filedClaims);
        }

        public async Task<OperationResult<bool>> FileClaimAsync(ClaimFilingRequestDtoForAgent claimDto)
        {
            _logger.LogInformation("Agent attempting to file a new claim for Policy ID: {PolicyId}, Customer ID: {CustomerId}", claimDto.PolicyId, claimDto.CustomerId);
            var claim = _mapper.Map<Claim>(claimDto);
            claim.Status = "Pending";
            claim.FiledDate = DateTime.UtcNow;

            var addResult = await _claimRepo.AddAsync(claim);
            if (!addResult.IsSuccess)
            {
                _logger.LogError("Failed to file claim for Policy ID {PolicyId}, Customer ID {CustomerId}. Error: {ErrorMessage}", claimDto.PolicyId, claimDto.CustomerId, addResult.Message);
                return OperationResult<bool>.Failure($"Failed to file claim: {addResult.Message}");
            }

            _logger.LogInformation("Claim filed successfully for Policy ID {PolicyId}, Customer ID {CustomerId}. Claim ID: {ClaimId}", claimDto.PolicyId, claimDto.CustomerId, claim.ClaimId);
            return OperationResult<bool>.Success(true, "Claim filed Succesfully");
        }

        public async Task<OperationResult<IEnumerable<NotificationResponseDto>>> GetMyNotificationsAsync()
        {
            _logger.LogInformation("Attempting to retrieve notifications for agent.");
            int? agentId = _tokenService.GetAgentIdFromCurrentRequest();

            if (agentId == null)
            {
                _logger.LogWarning("Failed to retrieve AgentId from JWT token for GetMyNotificationsAsync.");
                return OperationResult<IEnumerable<NotificationResponseDto>>.Failure("Unable to Get the AgentId from the Jwt Token");
            }
            _logger.LogDebug("AgentId retrieved from token: {AgentId}", agentId);


            var notes = await _notificationRepo.GetByAgentIdAsync((int)agentId);
            if (!notes.IsSuccess)
            {
                _logger.LogError("Failed to fetch notifications for agent ID {AgentId}. Error: {ErrorMessage}", agentId, notes.Message);
                return OperationResult<IEnumerable<NotificationResponseDto>>.Failure("No notifications found for the specified agent.");
            }
            var notifications = notes.Data;
            if (notifications?.Any() != true)
            {
                _logger.LogInformation("No notifications found for agent ID {AgentId}.", agentId);
                return OperationResult<IEnumerable<NotificationResponseDto>>.Failure(" No notifications Found");
            }

            var result = notifications.Select(n => _mapper.Map<NotificationResponseDto>(n));
            _logger.LogInformation("Successfully retrieved {Count} notifications for agent ID {AgentId}.", result.Count(), agentId);
            return OperationResult<IEnumerable<NotificationResponseDto>>.Success(result);
        }

        public async Task<OperationResult<bool>> UpdateProfileAsync(AgentProfileUpdateRequestDto updateAgentProfileDto)
        {
            _logger.LogInformation("Attempting to update agent profile.");
            int? agentId = _tokenService.GetAgentIdFromCurrentRequest();
            if (agentId == null)
            {
                _logger.LogWarning("Unable to fetch AgentId from token for profile update.");
                return OperationResult<bool>.Failure("Unable to Fetch the AgentId from The Token ");
            }
            _logger.LogDebug("AgentId retrieved from token for profile update: {AgentId}", agentId);

            var existingAgentResult = await _agentRepo.GetByUserIdAsync((int)agentId);
            if (!existingAgentResult.IsSuccess || existingAgentResult.Data == null)
            {
                _logger.LogError("Failed to find existing agent profile for User ID {AgentId} during update. Error: {ErrorMessage}", agentId, existingAgentResult.Message);
                return OperationResult<bool>.Failure("Agent profile not found for update.");
            }

      
            var agentToUpdate = existingAgentResult.Data;
            _mapper.Map(updateAgentProfileDto, agentToUpdate);

            var updateResult = await _agentRepo.UpdateAsync(agentToUpdate, (int)agentId);
            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update agent profile for User ID {AgentId}. Error: {ErrorMessage}", agentId, updateResult.Message);
                return OperationResult<bool>.Failure($"Failed to update profile: {updateResult.Message}");
            }

            _logger.LogInformation("Agent profile for User ID {AgentId} updated successfully.", agentId);
            return OperationResult<bool>.Success(true, "Profile Updated Successfully");
        }

        public async Task<OperationResult<PagedResult<AvailablePolicyResponseDto>>> GetAllAvailablePoliciesAsync(int page, int size)
        {
            _logger.LogInformation("Attempting to retrieve all available policies. Page: {Page}, Size: {Size}", page, size);
            var allAvailablePolicies = await _availablePolicyRepo.GetAllAsync();
            if (!allAvailablePolicies.IsSuccess)
            {
                _logger.LogError("Failed to fetch all available policies. Error: {ErrorMessage}", allAvailablePolicies.Message);
                return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Failure("Failed to fetch all the Available Policies");
            }
            var allAvailablePoliciesList = allAvailablePolicies.Data?.ToList();
            if (allAvailablePoliciesList == null)
            {
                _logger.LogWarning("No data found in the available policy list.");
                return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Failure("There is no Data in the Available Policy list ");
            }

            var paged = allAvailablePoliciesList.Skip((page - 1) * size).Take(size).ToList();

            if (paged.Any() && paged != null)
            {
                var allAvailablePolicyResult = new PagedResult<AvailablePolicyResponseDto>
                {
                    Items = paged.Select(p => _mapper.Map<AvailablePolicyResponseDto>(p)),
                    PageNumber = page,
                    PageSize = size,
                    TotalCount = allAvailablePoliciesList.Count
                };
                _logger.LogInformation("Successfully retrieved {Count} available policies for page {Page}.", allAvailablePolicyResult.Items.Count(), page);
                return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Success(allAvailablePolicyResult);
            }
            _logger.LogWarning("No available policies found on page {Page} with size {Size}.", page, size);
            return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Failure("Available Policies not Found");
        }

        public async Task<OperationResult<AvailablePolicyResponseDto>> GetAvailablePolicyByIdAsync(int policyId)
        {
            _logger.LogInformation("Attempting to retrieve available policy by ID: {PolicyId}", policyId);
            var availablePolicy = await _availablePolicyRepo.GetByIdAsync(policyId);
            if (!availablePolicy.IsSuccess)
            {
                _logger.LogError("Failed to fetch available policy with ID {PolicyId}. Error: {ErrorMessage}", policyId, availablePolicy.Message);
                return OperationResult<AvailablePolicyResponseDto>.Failure($" Failed To fetch the Available Policy of Id{policyId}");
            }
            var availablePolicyResult = availablePolicy.Data;
            if (availablePolicyResult == null)
            {
                _logger.LogWarning("Available policy with ID {PolicyId} not found.", policyId);
                return OperationResult<AvailablePolicyResponseDto>.Failure($"Available Policy with ID {policyId} not found.");
            }
            var result = _mapper.Map<AvailablePolicyResponseDto>(availablePolicyResult);
            _logger.LogInformation("Successfully retrieved available policy with ID {PolicyId}.", policyId);
            return OperationResult<AvailablePolicyResponseDto>.Success(result);
        }
    }
}