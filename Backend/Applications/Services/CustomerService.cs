
using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Applications.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IPolicyRepository _policyRepo;
        private readonly IPolicyRequestRepository _requestRepo;
        private readonly IClaimRepository _claimRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly IAvailablePolicyRepository _availablePolicyRepo;
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger; 

        public CustomerService(
            ICustomerRepository customerRepo,
            IPolicyRepository policyRepo,
            IPolicyRequestRepository requestRepo,
            IClaimRepository claimRepo,
            INotificationRepository notificationRepo,
            IAvailablePolicyRepository availablePolicyRepo,
            IUserRepository userRepo,
            ITokenService tokenService,
            IMapper mapper,
            ILogger<CustomerService> logger) 
        {
            _customerRepo = customerRepo;
            _policyRepo = policyRepo;
            _requestRepo = requestRepo;
            _claimRepo = claimRepo;
            _notificationRepo = notificationRepo;
            _availablePolicyRepo = availablePolicyRepo;
            _userRepo = userRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger; 
        }

        public async Task<OperationResult<CustomerProfileResponseDto>> GetProfileAsync()
        {
            _logger.LogInformation("Attempting to retrieve customer profile.");
            int? customerId = _tokenService.GetCustomerIdFromCurrentRequest();

            if (customerId == null)
            {
                _logger.LogWarning("Failed to retrieve CustomerId from JWT token for GetProfileAsync.");
                return OperationResult<CustomerProfileResponseDto>.Failure("Unable to Get the CustomerId from the Jwt Token");
            }
            _logger.LogDebug("CustomerId retrieved from token: {CustomerId}", customerId);

            var customerResult = await _customerRepo.GetByUserIdAsync((int)customerId);
            if (!customerResult.IsSuccess)
            {
                _logger.LogError("Failed to fetch customer profile for Customer ID {CustomerId}. Error: {ErrorMessage}", customerId, customerResult.Message);
                return OperationResult<CustomerProfileResponseDto>.Failure(customerResult.Message);
            }
            if (customerResult.Data == null)
            {
                _logger.LogWarning("Customer profile not found for Customer ID {CustomerId}.", customerId);
                return OperationResult<CustomerProfileResponseDto>.Failure("Customer profile not found for the specified user ID.");
            }

            var profile = customerResult.Data;
            var result = _mapper.Map<CustomerProfileResponseDto>(profile);

            _logger.LogInformation("Successfully retrieved customer profile for Customer ID {CustomerId}.", customerId);
            return OperationResult<CustomerProfileResponseDto>.Success(result);
        }

        public async Task<OperationResult<IEnumerable<CustomerPoliciesResponseDto>>> GetRegisteredPoliciesAsync()
        {
            _logger.LogInformation("Attempting to retrieve registered policies for customer.");
            int? customerId = _tokenService.GetCustomerIdFromCurrentRequest();

            if (customerId == null)
            {
                _logger.LogWarning("Failed to retrieve CustomerId from JWT token for GetRegisteredPoliciesAsync.");
                return OperationResult<IEnumerable<CustomerPoliciesResponseDto>>.Failure("Unable to Get the CustomerId from the Jwt Token");
            }
            _logger.LogDebug("CustomerId retrieved from token: {CustomerId}", customerId);

            var existingPolicies = await _policyRepo.GetByCustomerIdAsync((int)customerId);
            if (!existingPolicies.IsSuccess)
            {
                _logger.LogError("Failed to retrieve registered policies for Customer ID {CustomerId}. Error: {ErrorMessage}", customerId, existingPolicies.Message);
                return OperationResult<IEnumerable<CustomerPoliciesResponseDto>>.Failure("Failed to retrieve Registered policies for the customer .");
            }
            var policyResult = existingPolicies.Data;
            if (policyResult == null || !policyResult.Any())
            {
                _logger.LogInformation("No registered policies found for Customer ID {CustomerId}.", customerId);
                return OperationResult<IEnumerable<CustomerPoliciesResponseDto>>.Failure("Registered Policies not found .");
            }

            var Registeredpolicies = policyResult.Select(p => _mapper.Map<CustomerPoliciesResponseDto>(p));

            _logger.LogInformation("Successfully retrieved {Count} registered policies for Customer ID {CustomerId}.", Registeredpolicies.Count(), customerId);
            return OperationResult<IEnumerable<CustomerPoliciesResponseDto>>.Success(Registeredpolicies);
        }

        public async Task<OperationResult<IEnumerable<PolicyRequestStatusResponseDto>>> GetPolicyRequestsAsync()
        {
            _logger.LogInformation("Attempting to retrieve policy requests for customer.");
            int? customerId = _tokenService.GetCustomerIdFromCurrentRequest();

            if (customerId == null)
            {
                _logger.LogWarning("Failed to retrieve CustomerId from JWT token for GetPolicyRequestsAsync.");
                return OperationResult<IEnumerable<PolicyRequestStatusResponseDto>>.Failure("Unable to Get the CustomerId from the Jwt Token");
            }
            _logger.LogDebug("CustomerId retrieved from token: {CustomerId}", customerId);

            var requests = await _requestRepo.GetByCustomerIdAsync((int)customerId);
            if (!requests.IsSuccess)
            {
                _logger.LogError("Failed to retrieve policy requests for Customer ID {CustomerId}. Error: {ErrorMessage}", customerId, requests.Message);
                return OperationResult<IEnumerable<PolicyRequestStatusResponseDto>>.Failure("Failed to retrieve policy requests .");
            }
            var result = requests.Data;
            if (result == null || !result.Any())
            {
                _logger.LogInformation("No policy requests found for Customer ID {CustomerId}.", customerId);
                return OperationResult<IEnumerable<PolicyRequestStatusResponseDto>>.Failure("No policy requests found.");
            }

            var policyRequests = result.Select(r => _mapper.Map<PolicyRequestStatusResponseDto>(r));

            _logger.LogInformation("Successfully retrieved {Count} policy requests for Customer ID {CustomerId}.", policyRequests.Count(), customerId);
            return OperationResult<IEnumerable<PolicyRequestStatusResponseDto>>.Success(policyRequests);
        }

        public async Task<OperationResult<bool>> RequestPolicyAsync(PolicyRequestDto requestDto)
        {
            _logger.LogInformation("Customer attempting to request a new policy for Available Policy ID: {AvailablePolicyId}, Customer ID: {CustomerId}", requestDto.AvailablePolicyId, requestDto.CustomerId);

           
            int? customerIdFromToken = _tokenService.GetCustomerIdFromCurrentRequest();
            if (customerIdFromToken == null || customerIdFromToken != requestDto.CustomerId)
            {
                _logger.LogWarning("Policy request failed: Customer ID from token ({TokenCustomerId}) does not match DTO Customer ID ({DtoCustomerId}).", customerIdFromToken, requestDto.CustomerId);
                return OperationResult<bool>.Failure("Customer ID mismatch or token invalid.");
            }
            _logger.LogDebug("Customer ID {CustomerId} from token matches DTO for policy request.", customerIdFromToken);


            var request = _mapper.Map<PolicyRequest>(requestDto);
            request.RequestedOn = DateTime.UtcNow;
            request.Status = "Pending";

            var addResult = await _requestRepo.AddAsync(request);
            if (!addResult.IsSuccess)
            {
                _logger.LogError("Failed to add policy request for Available Policy ID {AvailablePolicyId}, Customer ID {CustomerId}. Error: {ErrorMessage}", requestDto.AvailablePolicyId, requestDto.CustomerId, addResult.Message);
                return OperationResult<bool>.Failure($"Failed to submit request: {addResult.Message}");
            }

            _logger.LogInformation("Policy request submitted successfully for Available Policy ID {AvailablePolicyId}, Customer ID {CustomerId}. Request ID: {RequestId}", requestDto.AvailablePolicyId, requestDto.CustomerId, request.RequestId);
            return OperationResult<bool>.Success(true, "Request Submitted Successfully");
        }

        public async Task<OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>> GetMyClaimsAsync()
        {
            _logger.LogInformation("Attempting to retrieve claims for customer.");
            int? customerId = _tokenService.GetCustomerIdFromCurrentRequest();
            if (customerId == null)
            {
                _logger.LogWarning("Failed to retrieve CustomerId from JWT token for GetMyClaimsAsync.");
                return OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>.Failure("Unable to Get the CustomerId from the Jwt Token");
            }
            _logger.LogDebug("CustomerId retrieved from token: {CustomerId}", customerId);

            var claims = await _claimRepo.GetByCustomerIdAsync((int)customerId);

            if (!claims.IsSuccess)
            {
                _logger.LogError("Failed to retrieve claims for Customer ID {CustomerId}. Error: {ErrorMessage}", customerId, claims.Message);
                return OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>.Failure("Failed to retrieve the Claims ");
            }
            var claimsDto = claims.Data;
            if (claimsDto == null || !claimsDto.Any())
            {
                _logger.LogInformation("No claims found for Customer ID {CustomerId}.", customerId);
                return OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>.Failure("Claims Not Found ");
            }
            var myClaims = claimsDto.Select(c => _mapper.Map<ClaimStatusResponseDtoForCustomer>(c));

            _logger.LogInformation("Successfully retrieved {Count} claims for Customer ID {CustomerId}.", myClaims.Count(), customerId);
            return OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>.Success(myClaims);
        }

        public async Task<OperationResult<IEnumerable<NotificationResponseDto>>> GetMyNotificationsAsync()
        {
            _logger.LogInformation("Attempting to retrieve notifications for customer.");
            int? customerId = _tokenService.GetCustomerIdFromCurrentRequest();
            if (customerId == null)
            {
                _logger.LogWarning("Failed to retrieve CustomerId from JWT token for GetMyNotificationsAsync.");
                return OperationResult<IEnumerable<NotificationResponseDto>>.Failure("Unable to Get the CustomerId from the Jwt Token");
            }
            _logger.LogDebug("CustomerId retrieved from token: {CustomerId}", customerId);

            var notes = await _notificationRepo.GetByCustomerIdAsync((int)customerId);
            if (!notes.IsSuccess)
            {
                _logger.LogError("Failed to retrieve notifications for Customer ID {CustomerId}. Error: {ErrorMessage}", customerId, notes.Message);
                return OperationResult<IEnumerable<NotificationResponseDto>>.Failure("Failed to retrieve the Notifications.");
            }
            var notesDto = notes.Data;
            if (notesDto == null || !notesDto.Any())
            {
                _logger.LogInformation("No notifications found for Customer ID {CustomerId}.", customerId);
                return OperationResult<IEnumerable<NotificationResponseDto>>.Failure("Notification not found.");
            }

            var myNotifications = notesDto.Select(n => _mapper.Map<NotificationResponseDto>(n));

            _logger.LogInformation("Successfully retrieved {Count} notifications for Customer ID {CustomerId}.", myNotifications.Count(), customerId);
            return OperationResult<IEnumerable<NotificationResponseDto>>.Success(myNotifications);
        }

        public async Task<OperationResult<bool>> UpdateProfileAsync(CustomerProfileUpdateRequestDto updateCustomerProfile)
        {
            _logger.LogInformation("Attempting to update customer profile.");
            int? customerId = _tokenService.GetCustomerIdFromCurrentRequest();

            if (customerId == null)
            {
                _logger.LogWarning("Failed to retrieve CustomerId from JWT token for UpdateProfileAsync.");
                return OperationResult<bool>.Failure("Failed To Fetch the CustomerId from the Jwt Token");
            }
            _logger.LogDebug("CustomerId retrieved from token for profile update: {CustomerId}", customerId);

            var existingCustomerResult = await _customerRepo.GetByIdAsync((int)customerId);
            if (!existingCustomerResult.IsSuccess || existingCustomerResult.Data == null)
            {
                _logger.LogError("Failed to find existing customer profile for Customer ID {CustomerId} during update. Error: {ErrorMessage}", customerId, existingCustomerResult.Message);
                return OperationResult<bool>.Failure("Customer profile not found for update.");
            }

            var customerToUpdate = existingCustomerResult.Data;
            _mapper.Map(updateCustomerProfile, customerToUpdate);

            var updateResult = await _customerRepo.UpdateAsync(customerToUpdate, (int)customerId);
            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update customer profile for Customer ID {CustomerId}. Error: {ErrorMessage}", customerId, updateResult.Message);
                return OperationResult<bool>.Failure($"Failed to update profile: {updateResult.Message}");
            }

            _logger.LogInformation("Customer profile for Customer ID {CustomerId} updated successfully.", customerId);
            return OperationResult<bool>.Success(true, "Profile updated successfully");
        }

        public async Task<OperationResult<PagedResult<AvailablePolicyResponseDto>>> GetAllAvailablePoliciesAsync(int page, int size)
        {
            _logger.LogInformation("Attempting to retrieve all available policies for customer. Page: {Page}, Size: {Size}", page, size);
            var allAvailablePolicies = await _availablePolicyRepo.GetAllAsync();
            if (!allAvailablePolicies.IsSuccess)
            {
                _logger.LogError("Failed to fetch all available policies. Error: {ErrorMessage}", allAvailablePolicies.Message);
                return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Failure("Failed to fetch all the Available polocies from the database");
            }
            var allAvailablePoliciesList = allAvailablePolicies.Data?.ToList();
            if (allAvailablePoliciesList == null || !allAvailablePoliciesList.Any())
            {
                _logger.LogWarning("Available policies list is empty.");
                return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Failure("Available policies List is Empty");
            }

            var paged = allAvailablePoliciesList.Skip((page - 1) * size).Take(size).ToList();
            if (paged == null || !paged.Any())
            {
                _logger.LogWarning("No policies found on page {Page} with size {Size} after pagination.", page, size);
                return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Failure("No policies Available for this page.");
            }

            var allPolicyResult = new PagedResult<AvailablePolicyResponseDto>
            {
                Items = paged.Select(p => _mapper.Map<AvailablePolicyResponseDto>(p)),
                PageNumber = page,
                PageSize = size,
                TotalCount = allAvailablePoliciesList.Count
            };
            _logger.LogInformation("Successfully retrieved {Count} available policies for page {Page}.", allPolicyResult.Items.Count(), page);
            return OperationResult<PagedResult<AvailablePolicyResponseDto>>.Success(allPolicyResult);
        }

        public async Task<OperationResult<bool>> FileClaimAsync(ClaimFilingRequestDtoForCustomer dto)
        {
            _logger.LogInformation("Customer attempting to file a claim for Policy ID: {PolicyId}, Customer ID: {CustomerId}", dto.PolicyId, dto.CustomerId);

            // Validate customer ID from token against DTO
            int? customerIdFromToken = _tokenService.GetCustomerIdFromCurrentRequest();
            if (customerIdFromToken == null || customerIdFromToken != dto.CustomerId)
            {
                _logger.LogWarning("Claim filing failed: Customer ID from token ({TokenCustomerId}) does not match DTO Customer ID ({DtoCustomerId}).", customerIdFromToken, dto.CustomerId);
                return OperationResult<bool>.Failure("Customer ID mismatch or token invalid for claim filing.");
            }
            _logger.LogDebug("Customer ID {CustomerId} from token matches DTO for claim filing.", customerIdFromToken);


            var claim = _mapper.Map<Claim>(dto);
            claim.Status = "Pending";
            claim.FiledDate = DateTime.UtcNow;

            var addResult = await _claimRepo.AddAsync(claim);
            if (!addResult.IsSuccess)
            {
                _logger.LogError("Failed to file claim for Policy ID {PolicyId}, Customer ID {CustomerId}. Error: {ErrorMessage}", dto.PolicyId, dto.CustomerId, addResult.Message);
                return OperationResult<bool>.Failure($"Failed to file claim: {addResult.Message}");
            }

            _logger.LogInformation("Claim filed successfully for Policy ID {PolicyId}, Customer ID {CustomerId}. Claim ID: {ClaimId}", dto.PolicyId, dto.CustomerId, claim.ClaimId);
            return OperationResult<bool>.Success(true, "Claim filed Successfully ");
        }

        public async Task<OperationResult<AvailablePolicyResponseDto>> GetAvailablePolicyByIdAsync(int policyId)
        {
            _logger.LogInformation("Attempting to retrieve available policy by ID: {PolicyId}", policyId);
            var availablePolicy = await _availablePolicyRepo.GetByIdAsync(policyId);
            if (!availablePolicy.IsSuccess)
            {
                _logger.LogError("Failed to fetch available policy with ID {PolicyId}. Error: {ErrorMessage}", policyId, availablePolicy.Message);
                return OperationResult<AvailablePolicyResponseDto>.Failure($"Failed To fetch the Available Policy of Id{policyId}");
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

        public async Task<OperationResult<bool>> DeleteAccountAsync()
        {
            _logger.LogInformation("Attempting to delete customer account via soft delete.");
            Guid? userId = _tokenService.GetUserIdFromCurrentRequest();

            if (userId == null)
            {
                _logger.LogWarning("Failed to retrieve UserId from JWT token for DeleteAccountAsync.");
                return OperationResult<bool>.Failure("Unable to Get the UserId from the Jwt Token");
            }
            _logger.LogDebug("UserId retrieved from token for account deletion: {UserId}", userId);

            var result = await _userRepo.SoftDeleteUserAsync((Guid)userId);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Customer account (User ID: {UserId}) soft deleted successfully.", userId);
                return OperationResult<bool>.Success(true, "Customer account deleted");
            }
            else
            {
                _logger.LogError("Failed to soft delete customer account (User ID: {UserId}). Error: {ErrorMessage}", userId, result.Message);
                return OperationResult<bool>.Failure(result.Message);
            }
        }

        public async Task<OperationResult<PolicyRequestStatusResponseDto>> GetPolicyRequestByIdAsync(int requestId)
        {
            _logger.LogInformation("Attempting to retrieve policy request by ID: {RequestId}", requestId);
            var request = await _requestRepo.GetByIdAsync(requestId);
            if (!request.IsSuccess)
            {
                _logger.LogError("Failed to fetch policy request with ID {RequestId}. Error: {ErrorMessage}", requestId, request.Message);
                return OperationResult<PolicyRequestStatusResponseDto>.Failure($"Failed To fetch the Policy Request of Id{requestId}");
            }
            var requestResult = request.Data;
            if (requestResult == null)
            {
                _logger.LogWarning("Policy request with ID {RequestId} not found.", requestId);
                return OperationResult<PolicyRequestStatusResponseDto>.Failure($"Policy Request with ID {requestId} not found.");
            }
            var result = _mapper.Map<PolicyRequestStatusResponseDto>(requestResult);
            _logger.LogInformation("Successfully retrieved policy request with ID {RequestId}.", requestId);
            return OperationResult<PolicyRequestStatusResponseDto>.Success(result);
        }
    }
}