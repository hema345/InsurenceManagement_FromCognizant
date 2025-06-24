using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        /// <summary>
        /// Retrieves the profile details of the currently authenticated agent.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the agent's profile details.
        /// If successful, returns an <see cref="OkObjectResult"/> with the profile information.
        /// If the profile is not found, returns a <see cref="NotFoundObjectResult"/> with an error message.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Agent role.
        /// </remarks>
        [HttpGet("profile")]
        [Authorize(Roles = Roles.Agent)]
        [ProducesResponseType(typeof(OperationResult<AgentProfileResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles =Roles.Agent)]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _agentService.GetProfileAsync();
            if (!profile.IsSuccess)
            {
                return NotFound("Agent profile not found.");

            }

            return Ok(profile);
        }


        /// <summary>
        /// Updates the profile details of the currently authenticated agent.
        /// </summary>
        /// <param name="updateAgentProfileDto">The updated profile details.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the update operation.
        /// If successful, returns an <see cref="OkObjectResult"/> with the updated profile.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Agent role.
        /// </remarks>
        [HttpPut("profile")]
        [Authorize(Roles = Roles.Agent)]
        [ProducesResponseType(typeof(OperationResult<AgentProfileResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AgentProfileResponseDto>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Agent)]
        public async Task<IActionResult> UpdateProfile([FromBody] AgentProfileUpdateRequestDto updateAgentProfileDto)
        {
            var result = await _agentService.UpdateProfileAsync(updateAgentProfileDto );
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        /// <summary>
        /// Retrieves the list of policies assigned to the currently authenticated agent.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the assigned policies.
        /// If successful, returns an <see cref="OkObjectResult"/> with the list of assigned policies.
        /// If no policies are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Agent role.
        /// </remarks>
        [HttpGet("assigned-policies")]
        [Authorize(Roles = Roles.Agent)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<AgentAssignedPolicyResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<AgentAssignedPolicyResponseDto>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Agent)]
        public async Task<IActionResult> GetAssignedPolicies()
        {
            var policies = await _agentService.GetAssignedPoliciesAsync();
            if (!policies.IsSuccess)
            {
                return NotFound(policies);
            }
            return Ok(policies);
        }

        /// <summary>
        /// Retrieves the list of claims filed by the currently authenticated agent.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the filed claims.
        /// If successful, returns an <see cref="OkObjectResult"/> with the list of filed claims.
        /// If no claims are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Agent role.
        /// </remarks>
        [HttpGet("claims")]
        [Authorize(Roles = Roles.Agent)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<ClaimStatusResponseDtoForAgent>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<ClaimStatusResponseDtoForAgent>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Agent)]
        public async Task<IActionResult> GetFiledClaims()
        {
            var claims = await _agentService.GetFiledClaimsAsync();
            if (!claims.IsSuccess)
            {
                return NotFound(claims);
            }
            return Ok(claims);
        }


        /// <summary>
        /// Allows an agent to file an insurance claim.
        /// </summary>
        /// <param name="dto">The claim filing details provided by the agent.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the status of the claim filing.
        /// If successful, returns an <see cref="OkObjectResult"/> with the claim filing response.
        /// If the request is invalid, returns a <see cref="BadRequestObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Agent role.
        /// </remarks>
        [HttpPost("file-claim")]
        [Authorize(Roles = Roles.Agent)]
        [ProducesResponseType(typeof(OperationResult<ClaimFilingRequestDtoForAgent>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<ClaimFilingRequestDtoForAgent>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Agent)]
        public async Task<IActionResult> FileClaim([FromBody] ClaimFilingRequestDtoForAgent dto)
        {
            var result=await _agentService.FileClaimAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the list of notifications for the currently authenticated agent.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the agent's notifications.
        /// If successful, returns an <see cref="OkObjectResult"/> with the list of notifications.
        /// If no notifications are found, returns a <see cref="NotFoundObjectResult"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint is accessible only to users with the Agent role.
        /// </remarks>
        [HttpGet("notifications")]
        [Authorize(Roles = Roles.Agent)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<NotificationResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<NotificationResponseDto>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Agent)]
        public async Task<IActionResult> GetMyNotifications()
        {
            var notes = await _agentService.GetMyNotificationsAsync();
            if (!notes.IsSuccess)
            {
                return NotFound(notes);
            }
            return Ok(notes);
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
        /// This endpoint is accessible only to users with the Agent role.
        /// </remarks>
        [HttpGet("available-policies")]
        [Authorize(Roles = Roles.Agent)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<AvailablePolicyResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<PagedResult<AvailablePolicyResponseDto>>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Agent)]
        public async Task<ActionResult<PagedResult<AvailablePolicyResponseDto>>> GetAllAvailablePolicies(int page = 1, int size = 10)
        {
            var result = await _agentService.GetAllAvailablePoliciesAsync(page, size);
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
        /// This endpoint is accessible only to users with the Agent role.
        /// </remarks>
        [HttpGet("{AvailablePolicyId}/Available-policy")]
        [Authorize(Roles = Roles.Agent)]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AvailablePolicyResponseDto>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Agent)]
        public async Task<ActionResult<AvailablePolicyResponseDto>> GetAvailablePolicyByIdAsync(int policyId)
        {
            var result = await _agentService.GetAvailablePolicyByIdAsync(policyId);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
