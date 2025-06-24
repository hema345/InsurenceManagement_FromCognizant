using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new customer in the system.
    /// </summary>
    /// <param name="request">The customer registration details.</param>
    /// <returns>
    /// Returns an <see cref="IActionResult"/> indicating the status of the registration process.
    /// If successful, returns an <see cref="OkObjectResult"/> with the registration response.
    /// If the request is null or invalid, returns a <see cref="BadRequestObjectResult"/>.
    /// </returns>
    /// <remarks>
    /// This endpoint facilitates customer registration.
    /// </remarks>
    [HttpPost("register")]
    [ProducesResponseType(typeof(OperationResult<CustomerRegisterResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] CustomerRegisterRequestDto request)
    { 

        if (request == null)
        {
            return BadRequest("Customer Request null");
        }

        var response = await _authService.RegisterAsync(request);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    /// <summary>
    /// Authenticates a user and generates an access token upon successful login.
    /// </summary>
    /// <param name="request">The login credentials including username and password.</param>
    /// <returns>
    /// Returns an <see cref="IActionResult"/> indicating the login attempt's result.
    /// If successful, returns an <see cref="OkObjectResult"/> with authentication details.
    /// If the credentials are incorrect, returns an <see cref="UnauthorizedObjectResult"/>.
    /// </returns>
    /// <remarks>
    /// This endpoint handles user authentication.
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(OperationResult<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResult<LoginResponseDto>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult>Login([FromBody] LoginRequestDto request)
    {
       

        var response = await _authService.LoginAsync(request);

        if (!response.IsSuccess)
        {
            return Unauthorized(response);
        }

        if (response.IsSuccess && response.Data != null)
        {
            Response.Cookies.Append("jwt", response.Data.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
        }

        return Ok(response);
    }

    /// <summary>
    /// Logs out the current user by deleting the authentication cookie.
    /// </summary>
    /// <returns>
    /// An <see cref="OperationResult{T}"/> containing a success message.
    /// </returns>
    /// 
    /// <remarks>
    /// Deletes the JWT authentication cookie to log the user out.
    /// The cookie is set to expire immediately.
    /// </remarks>
    [HttpPost("Logout")]
    [ProducesResponseType(typeof(OperationResult<string>), StatusCodes.Status200OK)]
    public ActionResult<OperationResult<string>> Logout()
    {
        Response.Cookies.Delete("jwt", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddHours(-1)
        });

        return Ok(OperationResult<string>.Success("User logged out successfully"));
    }


    /// <summary>
    /// Retrieves the role information for the current user or system.
    /// </summary>
    /// <returns>
    /// An <see cref="ActionResult"/> containing an <see cref="OperationResult{T}"/> of the role data.
    /// Returns 200 OK with the role information if successful.
    /// Returns 400 Bad Request if the retrieval fails (e.g., service error, no role found).
    /// </returns>
    /// <remarks>
    /// This endpoint requires the user to have one of the following roles:
    /// 'MaintenanceEngineer', 'Developer', 'NetworkEngineer', 'DevOpsEngineer',
    /// 'DatabaseAdministrator', 'Tester', or 'Manager'.
    /// It delegates the retrieval of role information to the employee service.
    /// </remarks>

    [Authorize(Roles = $"{Roles.Customer},{Roles.Admin},{Roles.Agent}")]
    [HttpGet("getRole")]
    public ActionResult GetRole()
    {
        var res = _authService.GetRoleService();
        if (!res.IsSuccess)
        {
            return BadRequest(res);
        }
        return Ok(res);
    }
}
