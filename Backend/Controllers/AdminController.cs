using InsurenceManagementSystemWebApi.Domain.Models;
using InsurenceManagementSystemWebApi.Shared.Common;


namespace InsurenceManagementSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }


        /// <summary>
        /// Retrieves a paginated list of all customer profiles. Accessible only to users with the Admin role.
        /// </summary>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="size">The number of customer profiles per page. Defaults to 10.</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing a <see cref="PagedResult{CustomerProfileResponseDto}"/>.
        /// Returns 200 OK with the paginated customer data if successful, or 404 Not Found if no customers are found or the operation fails.
        /// </returns>
        /// <remarks>
        /// This endpoint is restricted to users with the Admin role. It is used to fetch customer profile data in a paginated format.
        /// The response includes metadata such as total count, current page, and page size.
        /// </remarks>


        [HttpGet("customers")]
        [Authorize(Roles = Roles.Admin)]

        [ProducesResponseType(typeof(OperationResult<PagedResult<CustomerProfileResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<CustomerProfileResponseDto>>), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAllCustomers(int page = 1, int size = 10)
        {
            var result = await _adminService.GetAllCustomersAsync(page, size);
            if(!result.IsSuccess)
            {

                return NotFound(result);

            }
            return Ok(result);
        }



        /// <summary>
        /// Retrieves a paginated list of all agents. Accessible only by users with the Admin role.
        /// </summary>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="size">The number of agents per page. Defaults to 10.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the result of the operation.
        /// If successful, returns an HTTP 200 OK response with the list of agents.
        /// If the operation fails, returns an HTTP 404 Not Found response with the error details.
        /// </returns>
        /// <remarks>
        /// This endpoint is protected and requires the user to have the Admin role.
        /// </remarks>
        [HttpGet("agents")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<AgentProfileResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<AgentProfileResponseDto>>), StatusCodes.Status404NotFound)]

        [Authorize(Roles = Roles.Admin)]        
        public async Task<IActionResult> GetAllAgents(int page = 1, int size = 10)
        {
            var result = await _adminService.GetAllAgentsAsync(page, size);
            if(!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of all available insurance policies.
        /// </summary>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="size">The number of items per page. Defaults to 10.</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing a <see cref="PagedResult{AvailablePolicyResponseDto}"/> with the available policies.
        /// Returns 200 OK with the paged result if successful, or 404 Not Found if no policies are found.
        /// </returns>
        /// <remarks>
        /// Returns 200 OK with the paged result if successful, or 404 Not Found if no policies are found.
        /// </remarks>
        [HttpGet("available-policies")]
        [ProducesResponseType(typeof(OperationResult<PagedResult<AvailablePolicyResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<AvailablePolicyResponseDto>>),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAvailablePolicies(int page = 1, int size = 10)
        {
            var result = await _adminService.GetAllAvailablePoliciesAsync(page, size);
            if(!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }


        /// <summary>
        /// Retrieves a paginated list of all policy requests.
        /// </summary>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="size">The number of items per page. Defaults to 10.</param>
        /// <returns>
        /// A paginated list of <see cref="PolicyRequestStatusResponseDto"/> wrapped in an <see cref="ActionResult"/>.
        /// </returns>
        /// <response code="200">Returns the list of policy requests.</response>
        /// <response code="404">Returned when no policy requests are found or the operation fails.</response>
        /// <remarks>
        /// This endpoint is restricted to users with the Admin role. It supports pagination through the <c>page</c> and <c>size</c> query parameters.
        /// </remarks>
        [HttpGet("policy-requests")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<PolicyRequestStatusResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<PolicyRequestStatusResponseDto>>), StatusCodes.Status404NotFound)]

        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<PagedResult<PolicyRequestStatusResponseDto>>> GetAllPolicyRequests(int page = 1, int size = 10)
        {
            var result = await _adminService.GetAllPolicyRequestsAsync(page, size);
            if(!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Retrieves a paginated list of claims.
        /// </summary>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="size">The number of claims per page. Defaults to 10.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing a paginated list of claims.
        /// If successful, returns an <see cref="OkObjectResult"/> with the claims.
        /// If no claims are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpGet("claims")]
        [ProducesResponseType(typeof(OperationResult<PagedResult<ClaimStatusResponseDtoForCustomer>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<ClaimStatusResponseDtoForCustomer>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<PagedResult<ClaimStatusResponseDtoForCustomer>>> GetAllClaims(int page = 1, int size = 10)
        {
            var result = await _adminService.GetAllClaimsAsync(page, size);
            if(!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }




        /// <summary>
        /// Adds a new available policy.
        /// </summary>
        /// <param name="dto">The policy details to be added.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the operation result.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpPost("available-policy")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddAvailablePolicy([FromBody] AvailablePolicyRequestDto dto)
        {
           var result= await _adminService.AddAvailablePolicyAsync(dto);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing available policy.
        /// </summary>
        /// <param name="dto">The updated policy details.</param>
        /// <param name="policyId">The ID of the policy to update.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the update operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the operation result.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpPut("{policyId}/available-policy")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateAvailablePolicy([FromBody] AvailablePolicyRequestDto dto, int policyId)
        {
            var result =await _adminService.UpdateAvailablePolicyAsync(dto, policyId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Deletes an available policy by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the policy to delete.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the delete operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the operation result.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpDelete("{id}/available-policy")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteAvailablePolicy(int id)
        {
           var result= await _adminService.DeleteAvailablePolicyAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }



        /// <summary>
        /// Approves a policy request by assigning an agent.
        /// </summary>
        /// <param name="dto">The agent assignment details.</param>
        /// <param name="requestId">The unique identifier of the policy request to approve.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the approval operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the operation result.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpPut("{requestId}/approve-policy-request")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<AssignAgentRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AssignAgentRequestDto>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ApprovePolicyRequest([FromBody] AssignAgentRequestDto dto, int requestId)
        {
            var result=await _adminService.ApprovePolicyRequestAsync(dto, requestId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }





        /// <summary>
        /// Rejects a policy request.
        /// </summary>
        /// <param name="requestId">The unique identifier of the policy request to reject.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the rejection operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the operation result.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpPost("{requestId}/reject-policy-request")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RejectPolicyRequest(int requestId)
        {
            var result=await _adminService.RejectPolicyRequestAsync(requestId);
            if (!result.IsSuccess)
            { 
               return BadRequest(result);
            }
            return Ok(result);
        }



        /// <summary>
        /// Approves an insurance claim.
        /// </summary>
        /// <param name="claimId">The unique identifier of the claim to approve.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the approval operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the operation result.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpPost("{claimId}/approve-claim")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var result=await _adminService.ApproveClaimAsync(claimId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Rejects an insurance claim.
        /// </summary>
        /// <param name="claimId">The unique identifier of the claim to reject.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the rejection operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the operation result.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpPost("{claimId}/reject-claim")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> RejectClaim(int claimId)
        {
            var result =await _adminService.RejectClaimAsync(claimId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of all users with their roles.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing a list of users with their roles.
        /// If successful, returns an <see cref="OkObjectResult"/> with the list of users.
        /// If no users are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpGet("users")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<UserWithRoleResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<UserWithRoleResponseDto>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin)]

        public async Task<ActionResult<IEnumerable<UserWithRoleResponseDto>>> GetAllUsers()
        {
            var result = await _adminService.GetAllUsersAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Adds a new customer to the system.
        /// </summary>
        /// <param name="dto">The customer registration details.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the operation result.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpPost("add-customer")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<CustomerRegisterResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<CustomerRegisterResponseDto>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerRegisterRequestDto dto)
        {
            var result = await _adminService.AddCustomerAsync(dto);
            if (!result.IsSuccess)
            {
                   return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Adds a new agent to the system.
        /// </summary>
        /// <param name="dto">The agent registration details.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the operation result.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpPost("add-agent")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<AgentRegisterResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AgentRegisterResponseDto>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddAgent([FromBody] AgentRegisterRequestDto dto)
        {
            var result = await _adminService.AddAgentAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
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
        /// This endpoint fetches the policy details based on the given policy ID.
        /// </remarks>
        [HttpGet("{policyId}/Available-policy")]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AvailablePolicyResponseDto>> GetAvailablePolicyByIdAsync(int policyId)
        {
            var result = await _adminService.GetAvailablePolicyByIdAsync(policyId);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Retrieves customer profile details by their unique ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to retrieve.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing the customer profile details.
        /// If successful, returns an <see cref="OkObjectResult"/> with the customer information.
        /// If the customer is not found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpGet("{customerId}/Customer")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<CustomerProfileResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<CustomerProfileResponseDto>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<CustomerProfileResponseDto>> GetCustomerByIdAsync(int customerId)
        {
            var result = await _adminService.GetCustomerByIdAsync(customerId);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Retrieves an agent's profile details by their unique ID.
        /// </summary>
        /// <param name="agentId">The unique identifier of the agent to retrieve.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing the agent profile details.
        /// If successful, returns an <see cref="OkObjectResult"/> with the agent information.
        /// If the agent is not found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpGet("{agentId}/Agent")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<AgentProfileResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AgentProfileResponseDto>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<AgentProfileResponseDto>> GetAgentByIdAsync(int agentId)
        {
            var result = await _adminService.GetAgentByIdAsync(agentId);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);

        }


        /// <summary>
        /// Retrieves all claims associated with a specific customer ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer whose claims are being retrieved.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the list of claims.
        /// If successful, returns an <see cref="OkObjectResult"/> with the claims.
        /// If no claims are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Admin role.
        /// </remarks>
        [HttpGet("{customerId}/claims")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<ClaimStatusResponseDtoForCustomer>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin)]

        public async Task<IActionResult> GetMyClaims(int customerId)
        {
            var claims = await _adminService.GetClaimsByCustomerIdAsync(customerId);
            if (claims == null)
            {
                return NotFound(claims);
            }
            return Ok(claims);
        }
    }
}
