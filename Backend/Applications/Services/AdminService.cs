using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Applications.Services
{
    public class AdminService : IAdminService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IAgentRepository _agentRepo;
        private readonly IAvailablePolicyRepository _availablePolicyRepo;
        private readonly IPolicyRequestRepository _policyRequestRepo;
        private readonly IClaimRepository _claimRepo;
        private readonly IPolicyRepository _policyRepo;
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminService> _logger;
        private readonly INotificationRepository _notificationRepo;

        public AdminService(
            ICustomerRepository customerRepo,
            IAgentRepository agentRepo,
            IAvailablePolicyRepository availablePolicyRepo,
            IPolicyRequestRepository policyRequestRepo,
            IClaimRepository claimRepo,
            IPolicyRepository policyRepo,
            IAuthService authService,
            IUserRepository userRepository,
            IPasswordHasherService passwordHasher,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IAgentRepository agentRepository,
            INotificationRepository notificationRepository,
            ILogger<AdminService> logger,
            IMapper mapper
            )
        {
            _customerRepo = customerRepo;
            _agentRepo = agentRepo;
            _availablePolicyRepo = availablePolicyRepo;
            _policyRequestRepo = policyRequestRepo;
            _claimRepo = claimRepo;
            _policyRepo = policyRepo;
            _authService = authService;
            _userRepo = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepo = roleRepository;
            _userRoleRepo = userRoleRepository;
            _agentRepo = agentRepository; 
            _notificationRepo = notificationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<PagedResult<CustomerProfileResponseDto>>> GetAllCustomersAsync(int page, int size)
        {
            _logger.LogInformation("Attempting to retrieve all customers. Page: {Page}, Size: {Size}", page, size);
            var allcustomer = await _customerRepo.GetAllAsync();
            if (!allcustomer.IsSuccess)
            {
                _logger.LogError("Failed to fetch all customer details. Error: {ErrorMessage}", allcustomer.Message);
                return OperationResult<PagedResult<CustomerProfileResponseDto>>.Failure("Failed To fetch the details of all Customer");
            }
            var allCustomersList = allcustomer.Data?.ToList();
            if (allCustomersList == null)
            {
                _logger.LogWarning("No customer data found in the list.");
                return OperationResult<PagedResult<CustomerProfileResponseDto>>.Failure("There is no data found in the List");
            }
            var paged = allCustomersList.Skip((page - 1) * size).Take(size).ToList();

            if (paged != null && paged.Any())
            {

                var result = new PagedResult<CustomerProfileResponseDto>
                {
                    Items = paged.Select(c => _mapper.Map<CustomerProfileResponseDto>(c)).ToList(),

                    PageNumber = page,
                    PageSize = size,
                    TotalCount = allCustomersList.Count
                };
                
               
                _logger.LogInformation("Successfully retrieved {Count} customers for page {Page}.", result.Items.Count(), page);
                return OperationResult<PagedResult<CustomerProfileResponseDto>>.Success(result);
            }
            _logger.LogWarning("No customers found on page {Page} with size {Size}.", page, size);
            return OperationResult<PagedResult<CustomerProfileResponseDto>>.Failure("There is no customers in the specific page");
        }

        public async Task<OperationResult<PagedResult<AgentProfileResponseDto>>> GetAllAgentsAsync(int page, int size)
        {
            _logger.LogInformation("Attempting to retrieve all agents. Page: {Page}, Size: {Size}", page, size);
            var result = await _agentRepo.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to fetch agent details. Error: {ErrorMessage}", result.Message);
                return OperationResult<PagedResult<AgentProfileResponseDto>>.Failure(" Failed To fetch the Agent");
            }

            var all = result.Data?.ToList();
            if (all == null)
            {
                _logger.LogWarning("No agent data available in the list.");
                return OperationResult<PagedResult<AgentProfileResponseDto>>.Failure(" No Data Avaiable in the Agent List");
            }

            var paged = all.Skip((page - 1) * size).Take(size).ToList();

            if (paged != null && paged.Any())
            {
                var agentResult = new PagedResult<AgentProfileResponseDto>
                {
                    Items = paged.Select(a => _mapper.Map<AgentProfileResponseDto>(a)).ToList(),
                    PageNumber = page,
                    PageSize = size,
                    TotalCount = all.Count
                };
                _logger.LogInformation("Successfully retrieved {Count} agents for page {Page}.", agentResult.Items.Count(), page);
                return OperationResult<PagedResult<AgentProfileResponseDto>>.Success(agentResult);
            }
            _logger.LogWarning("No agents found on page {Page} with size {Size}.", page, size);
            return OperationResult<PagedResult<AgentProfileResponseDto>>.Failure("There is no Agent in the specific page");
        }

        public async Task<OperationResult<PagedResult<AvailablePolicyResponseDto>>> GetAllAvailablePoliciesAsync(int page, int size)
        {
            _logger.LogInformation("Attempting to retrieve all available policies. Page: {Page}, Size: {Size}", page, size);
            var allAvailablePolicies = await _availablePolicyRepo.GetAllAsync();

            if (!allAvailablePolicies.IsSuccess)
            {
                _logger.LogError("Failed to fetch available policies. Error: {ErrorMessage}", allAvailablePolicies.Message);
                return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Failure(" Failed To fetch the Available Policy");
            }

            var allAvailablePoliciesList = allAvailablePolicies.Data?.ToList();

            if (allAvailablePoliciesList == null)
            {
                _logger.LogWarning("No available policy data found in the list.");
                return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Failure(" There is no data in the Available Policy List");
            }

            var paged = allAvailablePoliciesList.Skip((page - 1) * size).Take(size).ToList();
            if (paged != null && paged.Any())
            {
                var policyResult = new PagedResult<AvailablePolicyResponseDto>
                {
                    Items = paged.Select(p => _mapper.Map<AvailablePolicyResponseDto>(p)),
                    PageNumber = page,
                    PageSize = size,
                    TotalCount = allAvailablePoliciesList.Count
                };
                _logger.LogInformation("Successfully retrieved {Count} available policies for page {Page}.", policyResult.Items.Count(), page);
                return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Success(policyResult);
            }
            _logger.LogWarning("No available policies found on page {Page} with size {Size}.", page, size);
            return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Failure(" There is no Available Policy in the specific page");
        }

        public async Task<OperationResult<PagedResult<PolicyRequestStatusResponseDto>>> GetAllPolicyRequestsAsync(int page, int size)
        {
            _logger.LogInformation("Attempting to retrieve all policy requests. Page: {Page}, Size: {Size}", page, size);
            var allpolicyRequests = await _policyRequestRepo.GetAllAsync();
            if (!allpolicyRequests.IsSuccess)
            {
                _logger.LogError("Failed to fetch all policy requests. Error: {ErrorMessage}", allpolicyRequests.Message);
                return OperationResult<PagedResult<PolicyRequestStatusResponseDto>>.Failure(" Failed to fetch all the Policy requests");
            }
            var allPolicyRequestList = allpolicyRequests.Data?.ToList();
            if (allPolicyRequestList == null)
            {
                _logger.LogWarning("No policy request data found in the list.");
                return OperationResult<PagedResult<PolicyRequestStatusResponseDto>>.Failure(" There is no Policy Requests in the List");
            }
            var paged = allPolicyRequestList.Skip((page - 1) * size).Take(size).ToList();
            if (paged.Any() && paged != null)
            {
                var policyRequestResult = new PagedResult<PolicyRequestStatusResponseDto>
                {
                    Items = paged.Select(r => _mapper.Map<PolicyRequestStatusResponseDto>(r)),
                    PageNumber = page,
                    PageSize = size,
                    TotalCount = allPolicyRequestList.Count
                };
                _logger.LogInformation("Successfully retrieved {Count} policy requests for page {Page}.", policyRequestResult.Items.Count(), page);
                return OperationResult<PagedResult<PolicyRequestStatusResponseDto>>.Success(policyRequestResult);
            }
            _logger.LogWarning("No policy requests found on page {Page} with size {Size}.", page, size);
            return OperationResult<PagedResult<PolicyRequestStatusResponseDto>>.Failure("Policy Requests not Found in the specific page");
        }

        public async Task<OperationResult<PagedResult<ClaimStatusResponseDtoForAdmin>>> GetAllClaimsAsync(int page, int size)
        {
            _logger.LogInformation("Attempting to retrieve all claims. Page: {Page}, Size: {Size}", page, size);
            var allClaims = await _claimRepo.GetAllAsync();
            if (!allClaims.IsSuccess)
            {
                _logger.LogError("Failed to fetch claims. Error: {ErrorMessage}", allClaims.Message);
                return OperationResult<PagedResult<ClaimStatusResponseDtoForAdmin>>.Failure(" Failed To fetch the Claims");
            }
            var allClaimsList = allClaims.Data?.ToList();
            if (allClaimsList == null)
            {
                _logger.LogWarning("No claim data available in the claims list.");
                return OperationResult<PagedResult<ClaimStatusResponseDtoForAdmin>>.Failure(" No Data Available in the Claims List");
            }

            var paged = allClaimsList.Skip((page - 1) * size).Take(size).ToList();
            if (paged != null && paged.Any())
            {
                var claimResult = new PagedResult<ClaimStatusResponseDtoForAdmin>
                {
                    Items = paged.Select(c => _mapper.Map<ClaimStatusResponseDtoForAdmin>(c)),
                    PageNumber = page,
                    PageSize = size,
                    TotalCount = allClaimsList.Count
                };
                _logger.LogInformation("Successfully retrieved {Count} claims for page {Page}.", claimResult.Items.Count(), page);
                return OperationResult<PagedResult<ClaimStatusResponseDtoForAdmin>>.Success(claimResult);
            }
            _logger.LogWarning("No claims found on page {Page} with size {Size}.", page, size);
            return OperationResult<PagedResult<ClaimStatusResponseDtoForAdmin>>.Failure(" Claims not Found in the specific page");
        }

        public async Task<OperationResult<bool>> AddAvailablePolicyAsync(AvailablePolicyRequestDto dto)
        {
            _logger.LogInformation("Attempting to add a new available policy. Policy Name: {PolicyName}", dto.Name);
            var policy = _mapper.Map<AvailablePolicy>(dto);

            var addResult = await _availablePolicyRepo.AddAsync(policy);
            if (!addResult.IsSuccess)
            {
                _logger.LogError("Failed to add available policy '{PolicyName}'. Error: {ErrorMessage}", dto.Name, addResult.Message);
                return OperationResult<bool>.Failure($"Failed to add new policy: {addResult.Message}");
            }

            _logger.LogInformation("New available policy '{PolicyName}' added successfully. Policy ID: {PolicyId}", dto.Name, policy.AvailablePolicyId);
            return OperationResult<bool>.Success(true, "New Policy Launched Successfully");
        }

        public async Task<OperationResult<bool>> UpdateAvailablePolicyAsync(AvailablePolicyRequestDto dto, int policyId)
        {
            _logger.LogInformation("Attempting to update available policy with ID: {PolicyId}", policyId);
            var availablePolicy = await _availablePolicyRepo.GetByIdAsync(policyId);
            if (!availablePolicy.IsSuccess)
            {
                _logger.LogError("Failed to fetch available policy with ID {PolicyId} for update. Error: {ErrorMessage}", policyId, availablePolicy.Message);
                return OperationResult<bool>.Failure($"Failed to fetch the available policy of Id{policyId}");
            }

            var policy = _mapper.Map<AvailablePolicy>(dto);
            policy.AvailablePolicyId = policyId; // Ensure the ID is set for update

            var updateResult = await _availablePolicyRepo.UpdateAsync(policy, policyId);
            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update available policy with ID {PolicyId}. Error: {ErrorMessage}", policyId, updateResult.Message);
                return OperationResult<bool>.Failure($"Failed to update available policy: {updateResult.Message}");
            }

            _logger.LogInformation("Available policy with ID {PolicyId} updated successfully.", policyId);
            return OperationResult<bool>.Success(true, "Available Policy updated successfully");
        }

        public async Task<OperationResult<bool>> DeleteAvailablePolicyAsync(int id)
        {
            _logger.LogInformation("Attempting to delete available policy with ID: {PolicyId}", id);
            var deleteResult = await _availablePolicyRepo.DeleteAsync(id);
            if (!deleteResult.IsSuccess)
            {
                _logger.LogError("Failed to delete available policy with ID {PolicyId}. Error: {ErrorMessage}", id, deleteResult.Message);
                return OperationResult<bool>.Failure($"Failed to delete available policy: {deleteResult.Message}");
            }

            _logger.LogInformation("Available policy with ID {PolicyId} deleted successfully.", id);
            return OperationResult<bool>.Success(true, "Available Policy deleted successfully");
        }

        public async Task<OperationResult<bool>> ApprovePolicyRequestAsync(AssignAgentRequestDto dto, int requestId)
        {
            _logger.LogInformation("Attempting to approve policy request with ID: {RequestId} and assign agent ID: {AgentId}", requestId, dto.AgentId);
            var result = await _policyRequestRepo.GetByIdAsync(requestId);

            if (!result.IsSuccess)
            {
                _logger.LogError("Policy request with ID '{RequestId}' not found for approval. Error: {ErrorMessage}", requestId, result.Message);
                return OperationResult<bool>.Failure($"Policy request with ID '{requestId}' not found.");
            }

            PolicyRequest? policyRequest = result.Data;

            var updateResult = await _policyRequestRepo.UpdateStatusAsync(requestId, "Approved");

            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update the status of policy request with ID '{RequestId}' to 'Approved'. Error: {ErrorMessage}", requestId, updateResult.Message);
                return OperationResult<bool>.Failure($"Failed to update the status of policy request with ID '{requestId}' to 'Approved'.");
            }

            if (policyRequest != null && policyRequest.AvailablePolicy != null)
            {
                var newPolicy = _mapper.Map<Policy>(dto);
                newPolicy.IssuedDate = DateTime.UtcNow;
                newPolicy.ExpiryDate = DateTime.UtcNow.AddYears(policyRequest.AvailablePolicy.ValidityPeriod);
                _mapper.Map(policyRequest, newPolicy);
                newPolicy.CustomerId = policyRequest.CustomerId; // Ensure CustomerId is mapped
                newPolicy.AvailablePolicyId = policyRequest.AvailablePolicyId; // Ensure AvailablePolicyId is mapped
                newPolicy.AgentId = dto.AgentId; // Ensure AgentId is mapped

                var response = await _policyRepo.AddAsync(newPolicy);

                if (!response.IsSuccess)
                {
                    _logger.LogError("Failed to register a new policy for Customer ID '{CustomerId}' and Available Policy ID '{AvailablePolicyId}'. Error: {ErrorMessage}", policyRequest.CustomerId, policyRequest.AvailablePolicyId, response.Message);
                    return OperationResult<bool>.Failure($"Failed to register a new policy for Customer ID '{policyRequest.CustomerId}' and Available Policy ID '{policyRequest.AvailablePolicyId}'.");
                }

                var customerNotification = new Notification
                {
                    CustomerId = policyRequest.CustomerId,
                    AgentId = null,
                    Message = $"Congratulations! Your policy request for Registering a new policy of Available Policy ID {policyRequest.AvailablePolicyId} with Request ID {policyRequest.RequestId} has been approved and Assigned with an Agent{dto.AgentId}. Please review the details and proceed with the next steps.",
                    CreatedAt = DateTime.UtcNow
                };

                var agentNotification = new Notification
                {
                    CustomerId = null,
                    AgentId = dto.AgentId,
                    Message = $"Dear Agent, you have been assigned a new policy for Customer ID {policyRequest.CustomerId}. The details are as follows:Available Policy ID: {policyRequest.AvailablePolicyId}, Policy Request ID: {policyRequest.RequestId}. Please review and take the necessary actions.",
                    CreatedAt = DateTime.UtcNow
                };

                var customerNotificationResult = await _notificationRepo.AddAsync(customerNotification);
                var agentNotificationResult = await _notificationRepo.AddAsync(agentNotification);

                if (!customerNotificationResult.IsSuccess || !agentNotificationResult.IsSuccess)
                {
                    _logger.LogError("Failed to add notifications for approved policy request ID {RequestId}. Customer notification success: {CustomerSuccess}, Agent notification success: {AgentSuccess}", requestId, customerNotificationResult.IsSuccess, agentNotificationResult.IsSuccess);
                    return OperationResult<bool>.Failure("Failed to add notifications");
                }
                _logger.LogInformation("Notifications sent for approved policy request ID {RequestId}.", requestId);
            }
            else
            {
                _logger.LogWarning("Policy request data or available policy data was null for request ID {RequestId} during approval.", requestId);
            }

            _logger.LogInformation("Policy request with ID {RequestId} approved and notifications sent.", requestId);
            return OperationResult<bool>.Success(true, "policy added and Notification Sent");
        }

        public async Task<OperationResult<IEnumerable<UserWithRoleResponseDto>>> GetAllUsersAsync()
        {
            _logger.LogInformation("Attempting to retrieve all users with roles.");
            var users = await _userRepo.GetAllWithRolesAsync();
            if (!users.IsSuccess)
            {
                _logger.LogError("Failed to fetch all users with roles. Error: {ErrorMessage}", users.Message);
                return OperationResult<IEnumerable<UserWithRoleResponseDto>>.Failure("Failed to Fetch all the Users");
            }
            var userResult = users.Data;
            if (userResult == null)
            {
                _logger.LogWarning("No user data found when retrieving all users with roles.");
                return OperationResult<IEnumerable<UserWithRoleResponseDto>>.Failure("There is no data found in the list");
            }

            var result = userResult.Select(u => _mapper.Map<UserWithRoleResponseDto>(u));
            _logger.LogInformation("Successfully retrieved {Count} users with roles.", result.Count());
            return OperationResult<IEnumerable<UserWithRoleResponseDto>>.Success(result);
        }

        public async Task<OperationResult<CustomerRegisterResponseDto>> AddCustomerAsync(CustomerRegisterRequestDto dto)
        {
            _logger.LogInformation("Attempting to add a new customer via admin. Username: {Username}", dto.Username);
            var result = await _authService.RegisterAsync(dto);
            if (result.IsSuccess)
            {
                _logger.LogInformation("Customer '{Username}' added successfully by admin.", dto.Username);
                return OperationResult<CustomerRegisterResponseDto>.Success("Customer added by admin");
            }
            else
            {
                _logger.LogError("Failed to add customer '{Username}' by admin. Error: {ErrorMessage}", dto.Username, result.Message);
                return OperationResult<CustomerRegisterResponseDto>.Failure(result.Message);
            }
        }

        public async Task<OperationResult<AgentRegisterResponseDto>> AddAgentAsync(AgentRegisterRequestDto dto)
        {
            _logger.LogInformation("Attempting to add a new agent. Username: {Username}, Name: {Name}", dto.UserName, dto.Name);
            var existingUser = await _userRepo.GetByUsernameAsync(dto.UserName);
            if (existingUser.IsSuccess && existingUser.Data != null) // Check if user already exists
            {
                _logger.LogWarning("Attempt to add agent failed: User with username '{Username}' already exists.", dto.UserName);
                return OperationResult<AgentRegisterResponseDto>.Failure("User with this username already exists.");
            }
            if (!existingUser.IsSuccess && existingUser.Message != "User not found") // Log other failures if not 'not found'
            {
                _logger.LogError("Failed to check for existing user '{Username}'. Error: {ErrorMessage}", dto.UserName, existingUser.Message);
                return OperationResult<AgentRegisterResponseDto>.Failure(existingUser.Message);
            }


            var user = _mapper.Map<User>(dto);
            user.Id = Guid.NewGuid();
            user.IsDeleted = false;
            user.PasswordHash = _passwordHasher.HashPassword(dto.Password);

            var userAddingResult = await _userRepo.AddAsync(user);

            if (!userAddingResult.IsSuccess)
            {
                _logger.LogError("Failed to add user for agent '{Username}'. Error: {ErrorMessage}", dto.UserName, userAddingResult.Message);
                return OperationResult<AgentRegisterResponseDto>.Failure(userAddingResult.Message);
            }
            _logger.LogInformation("User '{Username}' created for new agent.", dto.UserName);

            var role = await _roleRepo.GetByNameAsync("Agent");
            if (role?.Data == null)
            {
                _logger.LogError("Agent role not found in the system. Cannot assign role to user '{UserId}'.", user.Id);
                return OperationResult<AgentRegisterResponseDto>.Failure("Agent role missing");
            }
            var userRole = _mapper.Map<UserRole>(user);
            _mapper.Map(role.Data, userRole);
            userRole.UserId = user.Id; // Ensure UserId is correctly mapped
            userRole.RoleId = role.Data.Id; // Ensure RoleId is correctly mapped

            var userRoleResult = await _userRoleRepo.AddAsync(userRole);
            if (!userRoleResult.IsSuccess)
            {
                _logger.LogError("Failed to assign agent role to user '{UserId}'. Error: {ErrorMessage}", user.Id, userRoleResult.Message);
                return OperationResult<AgentRegisterResponseDto>.Failure(userRoleResult.Message);
            }
            _logger.LogInformation("Agent role assigned to user '{UserId}'.", user.Id);

            var agent = _mapper.Map<Agent>(dto);
            agent.UserId = user.Id; // Link agent to the newly created user

            var agentResult = await _agentRepo.AddAsync(agent);
            if (!agentResult.IsSuccess)
            {
                _logger.LogError("Failed to add agent details for user '{UserId}'. Error: {ErrorMessage}", user.Id, agentResult.Message);
                return OperationResult<AgentRegisterResponseDto>.Failure(agentResult.Message);
            }
            _logger.LogInformation("Agent details added for user '{UserId}'. Agent ID: {AgentId}", user.Id, agent.AgentId);

            var notification = new Notification
            {
                CustomerId = null,
                AgentId = agent.AgentId, // Use the actual agent ID
                Message = $"You have been registered by an admin. Welcome to the team, {dto.Name}! 🎉 We're excited to have you on board.",
                CreatedAt = DateTime.UtcNow
            };
            var notificationAddResult = await _notificationRepo.AddAsync(notification);
            if (!notificationAddResult.IsSuccess)
            {
                _logger.LogError("Failed to send welcome notification to new agent '{AgentId}'. Error: {ErrorMessage}", agent.AgentId, notificationAddResult.Message);
            }
            else
            {
                _logger.LogInformation("Welcome notification sent to new agent '{AgentId}'.", agent.AgentId);
            }

            _logger.LogInformation("New agent '{Name}' (Username: {Username}) added successfully.", dto.Name, dto.UserName);
            return OperationResult<AgentRegisterResponseDto>.Success(new AgentRegisterResponseDto { Message = "Agent added successfully" });
        }

        public async Task<OperationResult<bool>> RejectPolicyRequestAsync(int requestId)
        {
            _logger.LogInformation("Attempting to reject policy request with ID: {RequestId}", requestId);
            var updateResult = await _policyRequestRepo.UpdateStatusAsync(requestId, "Rejected");

            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update policy request ID {RequestId} status to 'Rejected'. Error: {ErrorMessage}", requestId, updateResult.Message);
                return OperationResult<bool>.Failure("Failed to update the policy request status");
            }

            var result = await _policyRequestRepo.GetByIdAsync(requestId);

            if (!result.IsSuccess)
            {
                _logger.LogError("Policy request with ID {RequestId} not found after status update attempt. Error: {ErrorMessage}", requestId, result.Message);
                return OperationResult<bool>.Failure("Policy request not found");
            }

            PolicyRequest? policyRequest = result.Data;

            if (policyRequest != null)
            {
                var customerNotification = new Notification
                {
                    CustomerId = policyRequest.CustomerId,
                    AgentId = null,
                    Message = $"Your policy request For the AvailablePolicyId {policyRequest.AvailablePolicyId} with ID {policyRequest.RequestId} has been rejected.",
                    CreatedAt = DateTime.UtcNow
                };

                var notificationResult = await _notificationRepo.AddAsync(customerNotification);

                if (!notificationResult.IsSuccess)
                {
                    _logger.LogError("Failed to add rejection notification for customer of policy request ID {RequestId}. Error: {ErrorMessage}", requestId, notificationResult.Message);
                    return OperationResult<bool>.Failure("Failed to add notification");
                }
                _logger.LogInformation("Rejection notification sent for policy request ID {RequestId}.", requestId);
            }
            else
            {
                _logger.LogWarning("Policy request data was null for request ID {RequestId} during rejection notification.", requestId);
            }

            _logger.LogInformation("Policy request with ID {RequestId} rejected and notification sent.", requestId);
            return OperationResult<bool>.Success(true, "Policy request rejected and notification sent");
        }

        public async Task<OperationResult<bool>> IsFiledByAgentAsync(int claimId)
        {
            _logger.LogDebug("Checking if claim ID {ClaimId} was filed by an agent.", claimId);
            var claim = await _claimRepo.GetByIdAsync(claimId);
            var agentClaimResult = claim.Data;
            if (agentClaimResult == null)
            {
                _logger.LogWarning("Claim with ID {ClaimId} not found when checking if filed by agent.", claimId);
                return OperationResult<bool>.Failure("No Agnet claim found");
            }
            var result = agentClaimResult.AgentId.HasValue;
            _logger.LogInformation("Claim ID {ClaimId} filed by agent: {IsFiledByAgent}", claimId, result);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> ApproveClaimAsync(int claimId)
        {
            _logger.LogInformation("Attempting to approve claim with ID: {ClaimId}", claimId);

            if (claimId <= 0)
            {
                _logger.LogError("Invalid claim ID provided for approval: {ClaimId}", claimId);
                return OperationResult<bool>.Failure("Invalid claim ID.");
            }

            var updateStatusResult = await _claimRepo.UpdateStatusAsync(claimId, "Approved");
            if (!updateStatusResult.IsSuccess)
            {
                _logger.LogError("Failed to update status of claim ID {ClaimId} to 'Approved'. Error: {ErrorMessage}", claimId, updateStatusResult.Message);
                return OperationResult<bool>.Failure($"Failed to update claim status: {updateStatusResult.Message}");
            }

            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim == null || !claim.IsSuccess || claim.Data == null)
            {
                _logger.LogError("Claim with ID {ClaimId} not found after status update for approval. Error: {ErrorMessage}", claimId, claim?.Message ?? "Unknown error");
                return OperationResult<bool>.Failure("Claim not found.");
            }

            var policyId = claim.Data.PolicyId;
            var customerId = claim.Data.CustomerId;

            var isFiledByAgentResult = await IsFiledByAgentAsync(claimId);
            if (!isFiledByAgentResult.IsSuccess)
            {
                _logger.LogError("Failed to determine if claim ID {ClaimId} was filed by an agent during approval. Error: {ErrorMessage}", claimId, isFiledByAgentResult.Message);
                return OperationResult<bool>.Failure("Failed to determine if claim was filed by an agent.");
            }

            if (!isFiledByAgentResult.Data) // Filed by customer directly
            {
                var notification = new Notification
                {
                    CustomerId = customerId,
                    AgentId = null,
                    Message = $"Great news! Your claim for Policy ID {policyId} has been successfully approved. Thank you for your patience.",
                    CreatedAt = DateTime.UtcNow
                };
                var notificationAddResult = await _notificationRepo.AddAsync(notification);
                if (!notificationAddResult.IsSuccess)
                {
                    _logger.LogError("Failed to send approval notification to customer {CustomerId} for claim ID {ClaimId}. Error: {ErrorMessage}", customerId, claimId, notificationAddResult.Message);
                }
                else
                {
                    _logger.LogInformation("Approval notification sent to customer {CustomerId} for claim ID {ClaimId}.", customerId, claimId);
                }
            }
            else // Filed by agent
            {
                var agentNotification = new Notification
                {
                    AgentId = claim.Data.AgentId,
                    CustomerId = null,
                    Message = $"Good news! The claim associated with Policy ID {policyId} for Customer ID {customerId} has been approved. Thank you for your diligent work.",
                    CreatedAt = DateTime.UtcNow
                };
                var agentNotificationAddResult = await _notificationRepo.AddAsync(agentNotification);
                if (!agentNotificationAddResult.IsSuccess)
                {
                    _logger.LogError("Failed to send approval notification to agent {AgentId} for claim ID {ClaimId}. Error: {ErrorMessage}", claim.Data.AgentId, claimId, agentNotificationAddResult.Message);
                }
                else
                {
                    _logger.LogInformation("Approval notification sent to agent {AgentId} for claim ID {ClaimId}.", claim.Data.AgentId, claimId);
                }

                var customerNotification = new Notification
                {
                    CustomerId = customerId,
                    AgentId = null,
                    Message = $"Great news! Your claim for Policy ID {policyId} has been successfully approved. Thank you for your patience.",
                    CreatedAt = DateTime.UtcNow
                };
                var customerNotificationAddResult = await _notificationRepo.AddAsync(customerNotification);
                if (!customerNotificationAddResult.IsSuccess)
                {
                    _logger.LogError("Failed to send approval notification to customer {CustomerId} for claim ID {ClaimId} (filed by agent). Error: {ErrorMessage}", customerId, claimId, customerNotificationAddResult.Message);
                }
                else
                {
                    _logger.LogInformation("Approval notification sent to customer {CustomerId} for claim ID {ClaimId} (filed by agent).", customerId, claimId);
                }
            }

            _logger.LogInformation("Policy claim with ID {ClaimId} approved and notifications sent.", claimId);
            return OperationResult<bool>.Success(true, "Policy claim approved and notifications sent.");
        }

        public async Task<OperationResult<bool>> RejectClaimAsync(int claimId)
        {
            _logger.LogInformation("Attempting to reject claim with ID: {ClaimId}", claimId);

            if (claimId <= 0)
            {
                _logger.LogError("Invalid claim ID provided for rejection: {ClaimId}", claimId);
                return OperationResult<bool>.Failure("Invalid claim ID.");
            }

            var updateStatusResult = await _claimRepo.UpdateStatusAsync(claimId, "Rejected");
            if (!updateStatusResult.IsSuccess)
            {
                _logger.LogError("Failed to update status of claim ID {ClaimId} to 'Rejected'. Error: {ErrorMessage}", claimId, updateStatusResult.Message);
                return OperationResult<bool>.Failure($"Failed to update claim status: {updateStatusResult.Message}");
            }

            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim == null || !claim.IsSuccess || claim.Data == null)
            {
                _logger.LogError("Claim with ID {ClaimId} not found after status update for rejection. Error: {ErrorMessage}", claimId, claim?.Message ?? "Unknown error");
                return OperationResult<bool>.Failure("Claim not found.");
            }

            var policyId = claim.Data.PolicyId;
            var customerId = claim.Data.CustomerId;

            var isFiledByAgentResult = await IsFiledByAgentAsync(claimId);
            if (!isFiledByAgentResult.IsSuccess)
            {
                _logger.LogError("Failed to determine if claim ID {ClaimId} was filed by an agent during rejection. Error: {ErrorMessage}", claimId, isFiledByAgentResult.Message);
                return OperationResult<bool>.Failure("Failed to determine if claim was filed by an agent.");
            }

            if (isFiledByAgentResult.Data) // Filed by agent
            {
                var agentNotification = new Notification
                {
                    AgentId = claim.Data.AgentId,
                    CustomerId = null,
                    Message = $"Unfortunately, the claim associated with Policy ID {policyId} for Customer ID {customerId} has been rejected. Please review the details and take necessary actions.",
                    CreatedAt = DateTime.UtcNow
                };
                var agentNotificationAddResult = await _notificationRepo.AddAsync(agentNotification);
                if (!agentNotificationAddResult.IsSuccess)
                {
                    _logger.LogError("Failed to send rejection notification to agent {AgentId} for claim ID {ClaimId}. Error: {ErrorMessage}", claim.Data.AgentId, claimId, agentNotificationAddResult.Message);
                }
                else
                {
                    _logger.LogInformation("Rejection notification sent to agent {AgentId} for claim ID {ClaimId}.", claim.Data.AgentId, claimId);
                }

                var customerNotification = new Notification
                {
                    CustomerId = customerId,
                    AgentId = null,
                    Message = $"We regret to inform you that your claim for Policy ID {policyId} has been rejected. Please contact us for further assistance.",
                    CreatedAt = DateTime.UtcNow
                };
                var customerNotificationAddResult = await _notificationRepo.AddAsync(customerNotification);
                if (!customerNotificationAddResult.IsSuccess)
                {
                    _logger.LogError("Failed to send rejection notification to customer {CustomerId} for claim ID {ClaimId} (filed by agent). Error: {ErrorMessage}", customerId, claimId, customerNotificationAddResult.Message);
                }
                else
                {
                    _logger.LogInformation("Rejection notification sent to customer {CustomerId} for claim ID {ClaimId} (filed by agent).", customerId, claimId);
                }
            }
            else // Filed by customer directly
            {
                var notification = new Notification
                {
                    CustomerId = customerId,
                    AgentId = null,
                    Message = $"We regret to inform you that your claim for Policy ID {policyId} has been rejected. Please contact us for further assistance.",
                    CreatedAt = DateTime.UtcNow
                };
                var notificationAddResult = await _notificationRepo.AddAsync(notification);
                if (!notificationAddResult.IsSuccess)
                {
                    _logger.LogError("Failed to send rejection notification to customer {CustomerId} for claim ID {ClaimId}. Error: {ErrorMessage}", customerId, claimId, notificationAddResult.Message);
                }
                else
                {
                    _logger.LogInformation("Rejection notification sent to customer {CustomerId} for claim ID {ClaimId}.", customerId, claimId);
                }
            }

            _logger.LogInformation("Policy claim with ID {ClaimId} rejected and notifications sent.", claimId);
            return OperationResult<bool>.Success(true, "Policy claim rejected and notifications sent.");
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
                return OperationResult<AvailablePolicyResponseDto>.Failure($"Available policy with ID {policyId} not found.");
            }
            var result = _mapper.Map<AvailablePolicyResponseDto>(availablePolicyResult);
            _logger.LogInformation("Successfully retrieved available policy with ID {PolicyId}.", policyId);
            return OperationResult<AvailablePolicyResponseDto>.Success(result);
        }

        public async Task<OperationResult<CustomerProfileResponseDto>> GetCustomerByIdAsync(int customerId)
        {
            _logger.LogInformation("Attempting to retrieve customer by ID: {CustomerId}", customerId);
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (!customer.IsSuccess)
            {
                _logger.LogError("Failed to fetch customer with ID {CustomerId}. Error: {ErrorMessage}", customerId, customer.Message);
                return OperationResult<CustomerProfileResponseDto>.Failure($" Failed To fetch the Customer of Id{customerId}");
            }
            var customerResult = customer.Data;
            if (customerResult == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found.", customerId);
                return OperationResult<CustomerProfileResponseDto>.Failure($"Customer with ID {customerId} not found.");
            }
            var result = _mapper.Map<CustomerProfileResponseDto>(customerResult);
            _logger.LogInformation("Successfully retrieved customer with ID {CustomerId}.", customerId);
            return OperationResult<CustomerProfileResponseDto>.Success(result);
        }

        public async Task<OperationResult<AgentProfileResponseDto>> GetAgentByIdAsync(int agentId)
        {
            _logger.LogInformation("Attempting to retrieve agent by ID: {AgentId}", agentId);
            var agent = await _agentRepo.GetByIdAsync(agentId);
            if (!agent.IsSuccess)
            {
                _logger.LogError("Failed to fetch agent with ID {AgentId}. Error: {ErrorMessage}", agentId, agent.Message);
                return OperationResult<AgentProfileResponseDto>.Failure($" Failed To fetch the Agent of Id{agentId}");
            }
            var agentresult = agent.Data;
            if (agentresult == null)
            {
                _logger.LogWarning("Agent with ID {AgentId} not found.", agentId);
                return OperationResult<AgentProfileResponseDto>.Failure($"Agent with ID {agentId} not found.");
            }
            var result = _mapper.Map<AgentProfileResponseDto>(agentresult);
            _logger.LogInformation("Successfully retrieved agent with ID {AgentId}.", agentId);
            return OperationResult<AgentProfileResponseDto>.Success(result);
        }

        public async Task<OperationResult<IEnumerable<ClaimsFiledByCustomerResponseDtoForAdmin>>> GetClaimsByCustomerIdAsync(int customerId)
        {
            _logger.LogInformation("Attempting to retrieve claims for customer ID: {CustomerId}", customerId);
            var claims = await _claimRepo.GetByCustomerIdAsync(customerId);

            if (!claims.IsSuccess)
            {
                _logger.LogError("Failed to retrieve claims for customer ID {CustomerId}. Error: {ErrorMessage}", customerId, claims.Message);
                return OperationResult<IEnumerable<ClaimsFiledByCustomerResponseDtoForAdmin>>.Failure("Cannot retrive Claims");
            }
            var claimsDto = claims.Data;
            if (claimsDto == null)
            {
                _logger.LogWarning("No claims found for customer ID {CustomerId}.", customerId);
                return OperationResult<IEnumerable<ClaimsFiledByCustomerResponseDtoForAdmin>>.Failure("Claims Not Found ");
            }
            var myClaims = claimsDto.Select(c => _mapper.Map<ClaimsFiledByCustomerResponseDtoForAdmin>(c));
            _logger.LogInformation("Successfully retrieved {Count} claims for customer ID {CustomerId}.", myClaims.Count(), customerId);
            return OperationResult<IEnumerable<ClaimsFiledByCustomerResponseDtoForAdmin>>.Success(myClaims);
        }
    }
}