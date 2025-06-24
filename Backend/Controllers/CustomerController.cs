using Azure.Core;
using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }



        /// <summary>
        /// Retrieves the profile details of the currently authenticated customer.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the customer's profile information.
        /// If successful, returns an <see cref="OkObjectResult"/> with the profile details.
        /// If the profile is not found, returns a <see cref="NotFoundObjectResult"/> with an error message.
        /// </returns>
        /// <remarks>
        /// This endpoint allows a customer to fetch their profile details.
        /// </remarks>
        [HttpGet("profile")]
        [ProducesResponseType(typeof(OperationResult<CustomerProfileResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile()
        {
    
            var profile = await _customerService.GetProfileAsync();
            if (profile == null)
                return NotFound("customer profile not found.");

            return Ok(profile);
        }

        /// <summary>
        /// Updates the profile details of the currently authenticated customer.
        /// </summary>
        /// <param name="updateCustomerProfileDto">The updated profile details.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the update operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the updated profile.
        /// If the customer is not found, returns a <see cref="NotFoundObjectResult"/> with an error message.
        /// </returns>
        /// <remarks>
        /// This endpoint allows a customer to update their profile details.
        /// </remarks>
        [HttpPut("profile")]
        [ProducesResponseType(typeof(OperationResult<CustomerProfileResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfile([FromBody] CustomerProfileUpdateRequestDto updateCustomerProfileDto )
        {
            var result = await _customerService.UpdateProfileAsync(updateCustomerProfileDto );
            if (result==null) 
                return NotFound("Customer not found");
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the list of policies registered by the currently authenticated customer.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the registered policies.
        /// If successful, returns an <see cref="OkObjectResult"/> with the list of registered policies.
        /// If no policies are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpGet("policies")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<CustomerPoliciesResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<CustomerPoliciesResponseDto>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles =Roles.Customer)]
        public async Task<IActionResult> GetRegisteredPolicies()
        {
            var policies = await _customerService.GetRegisteredPoliciesAsync();
            if (!policies.IsSuccess)
            {
                return NotFound(policies);
            }
            return Ok(policies);
        }

        /// <summary>
        /// Retrieves the policy requests made by the currently authenticated customer.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the policy requests.
        /// If successful, returns an <see cref="OkObjectResult"/> with the list of policy requests.
        /// If no policy requests are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpGet("policy-requests")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<PolicyRequestStatusResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<PolicyRequestStatusResponseDto>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> GetPolicyRequests()
        {
            var requests = await _customerService.GetPolicyRequestsAsync();
            if (!requests.IsSuccess)
            {
                return NotFound(requests);
            }
            return Ok(requests);
        }



        /// <summary>
        /// Allows a customer to file an insurance claim.
        /// </summary>
        /// <param name="dto">The claim filing details provided by the customer.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the claim filing.
        /// If successful, returns an <see cref="OkObjectResult"/> with the claim filing response.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpPost("file-claim")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<ClaimFilingRequestDtoForCustomer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<ClaimFilingRequestDtoForCustomer>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> FileClaim([FromBody] ClaimFilingRequestDtoForCustomer dto)
        {
            var result=await _customerService.FileClaimAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Allows a customer to request a new policy.
        /// </summary>
        /// <param name="requestDto">The policy request details provided by the customer.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the policy request.
        /// If successful, returns an <see cref="OkObjectResult"/> with the request response.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpPost("request-policy")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<PolicyRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<PolicyRequestDto>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> RequestPolicy([FromBody] PolicyRequestDto requestDto)
        {
            var result=await _customerService.RequestPolicyAsync(requestDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all claims associated with the currently authenticated customer.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the list of claims.
        /// If successful, returns an <see cref="OkObjectResult"/> with the claims.
        /// If no claims are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpGet("claims")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> GetMyClaims()
        {
            var claims = await _customerService.GetMyClaimsAsync();
            if (!claims.IsSuccess)
            {
                return NotFound(claims);
            }

            return Ok(claims);
        }

        /// <summary>
        /// Retrieves the list of notifications for the currently authenticated customer.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the customer's notifications.
        /// If successful, returns an <see cref="OkObjectResult"/> with the list of notifications.
        /// If no notifications are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpGet("notifications")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<NotificationResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<NotificationResponseDto>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> GetMyNotifications()
        {
            var notifications = await _customerService.GetMyNotificationsAsync();
            if (!notifications.IsSuccess)
            {
                return NotFound(notifications);
            }
            return Ok(notifications);
        }

        /// <summary>
        /// Retrieves a paginated list of available policies.
        /// </summary>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="size">The number of policies per page. Defaults to 10.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing a paginated list of available policies.
        /// If successful, returns an <see cref="OkObjectResult"/> with the policies.
        /// If no policies are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpGet("available-policies")] 
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<AvailablePolicyResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<AvailablePolicyResponseDto>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Customer)]
        public async Task<ActionResult<PagedResult<AvailablePolicyResponseDto>>> GetAllAvailablePolicies(int page = 1, int size = 10)
        {
            var result = await _customerService.GetAllAvailablePoliciesAsync(page, size);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Retrieves an available policy by its unique ID.
        /// </summary>
        /// <param name="policyId">The unique identifier of the policy to retrieve.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing the policy details.
        /// If successful, returns an <see cref="OkObjectResult"/> with the policy information.
        /// If the policy is not found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpGet("{AvailablePolicyId}/Available-policy")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Customer)]
        public async Task<ActionResult<AvailablePolicyResponseDto>> GetAvailablePolicyByIdAsync(int policyId)
        {
            var result = await _customerService.GetAvailablePolicyByIdAsync(policyId);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Retrieves the status of a specific policy request by its unique ID.
        /// </summary>
        /// <param name="requestId">The unique identifier of the policy request to retrieve.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing the policy request status.
        /// If successful, returns an <see cref="OkObjectResult"/> with the request information.
        /// If the request is not found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpGet("{requestId}/policy-request")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<PolicyRequestStatusResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<PolicyRequestStatusResponseDto>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Customer)]
        public async Task<ActionResult<PolicyRequestStatusResponseDto>> GetPolicyRequestByIdAsync(int requestId)
        {
            var result = await _customerService.GetPolicyRequestByIdAsync(requestId);
            if (!result.IsSuccess)
            {
                    return NotFound(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Deletes the account of the currently authenticated customer.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the result of the account deletion process.
        /// If successful, returns an <see cref="OkObjectResult"/> confirming the deletion.
        /// If the deletion fails, returns a <see cref="BadRequestObjectResult"/> with an error message.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Customer role.
        /// </remarks>
        [HttpDelete("delete-account")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> DeleteAccount()
        {
            var result = await _customerService.DeleteAccountAsync();

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return  Ok(result);
        }

    }
}
