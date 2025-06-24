
using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Applications.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ITokenService _tokenService;
        private readonly IAgentRepository _agentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger; 

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ICustomerRepository customerRepository,
            IPasswordHasherService passwordHasher,
            IUserRoleRepository userRoleRepository,
            ITokenService tokenService,
            IAgentRepository agentRepository,
            IMapper mapper,
            ILogger<AuthService> logger) 
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _customerRepository = customerRepository;
            _passwordHasher = passwordHasher;
            _userRoleRepository = userRoleRepository;
            _tokenService = tokenService;
            _agentRepository = agentRepository;
            _mapper = mapper;
            _logger = logger; 
        }

        public OperationResult<string> GetRoleService()
        {
            var role = _tokenService.GetRoleFromCurrentRequest();
            if (role == null)
            {
                return OperationResult<string>.Failure("Please login");
            }
            return OperationResult<string>.Success(role);
        }

        public async Task<OperationResult<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
        {
            _logger.LogInformation("Attempting login for username: {Username}", dto.Username);

            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            var login = user.Data;

            if (login == null || login.IsDeleted)
            {
                if (login == null)
                {
                    _logger.LogWarning("Login failed for username '{Username}': User not found.", dto.Username);
                }
                else
                {
                    _logger.LogWarning("Login failed for username '{Username}': User is marked as deleted.", dto.Username);
                }
                return OperationResult<LoginResponseDto>.Failure("UserName Not found");
            }

            var verified = _passwordHasher.VerifyPassword(dto.Password, login.PasswordHash);

            if (!verified)
            {
                _logger.LogWarning("Login failed for username '{Username}': Invalid password.", dto.Username);
                return OperationResult<LoginResponseDto>.Failure("Invalid Password or username");
            }

            var userId = login.Id;
            var role = login.UserRole?.Role?.Name ?? "Unknown";
            _logger.LogDebug("User '{Username}' (ID: {UserId}) has role: {Role}", dto.Username, userId, role);

            int? id = null;
            if (role == Roles.Customer.ToString())
            {
                var response = await _customerRepository.GetCustomerIdFromUserIdAsync(userId);
                if (!response.IsSuccess)
                {
                    _logger.LogError("Failed to retrieve Customer ID for user '{Username}' (User ID: {UserId}). Error: {ErrorMessage}", dto.Username, userId, response.Message);
                    return OperationResult<LoginResponseDto>.Failure(response.Message);
                }
                id = response.Data;
                _logger.LogDebug("Customer ID for user '{Username}' (User ID: {UserId}): {CustomerId}", dto.Username, userId, id);
            }
            else if (role == Roles.Agent.ToString())
            {
                var response = await _agentRepository.GetAgentIdFromUserIdAsync(userId);
                if (!response.IsSuccess)
                {
                    _logger.LogError("Failed to retrieve Agent ID for user '{Username}' (User ID: {UserId}). Error: {ErrorMessage}", dto.Username, userId, response.Message);
                    return OperationResult<LoginResponseDto>.Failure(response.Message);
                }
                id = response.Data;
                _logger.LogDebug("Agent ID for user '{Username}' (User ID: {UserId}): {AgentId}", dto.Username, userId, id);
            }
            
            _logger.LogDebug("ID for token generation (specific entity ID): {SpecificEntityId}", id);


            var jwtToken = _tokenService.GenerateToken(userId, role, id);

            var log = new LoginResponseDto
            {
                Success = true,
                Role = role,
                Message = "Login successful",
                Token = jwtToken
            };

            _logger.LogInformation("User '{Username}' (User ID: {UserId}, Role: {Role}) logged in successfully.", dto.Username, userId, role);
            return OperationResult<LoginResponseDto>.Success(log);
        }

        public async Task<OperationResult<CustomerRegisterResponseDto>> RegisterAsync(CustomerRegisterRequestDto dto)
        {
            _logger.LogInformation("Attempting customer registration for username: {Username}", dto.Username);

            var roleResult = await _roleRepository.GetByNameAsync("Customer");
            if (roleResult == null || roleResult.Data == null)
            {
                _logger.LogError("Customer Role not found in the database during registration for username: {Username}", dto.Username);
                return OperationResult<CustomerRegisterResponseDto>.Failure("Customer Role Not found");
            }
            var role = roleResult.Data;
            _logger.LogDebug("Customer role '{RoleName}' (ID: {RoleId}) retrieved for registration.", role.Name, role.Id);

            var existingUserResult = await _userRepository.GetByUsernameAsync(dto.Username);

            if (!existingUserResult.IsSuccess) 
            {
                _logger.LogWarning("Registration failed for username '{Username}': User already exists.", dto.Username);
                return OperationResult<CustomerRegisterResponseDto>.Failure("User with this username already exists.");
            }
           

            var user = _mapper.Map<User>(dto);
            user.Id = Guid.NewGuid();
            user.IsDeleted = false;
            user.PasswordHash = _passwordHasher.HashPassword(dto.Password);
            _logger.LogDebug("User entity mapped and password hashed for username: {Username}. User ID: {UserId}", dto.Username, user.Id);

            var addUserResult = await _userRepository.AddAsync(user);

            if (!addUserResult.IsSuccess)
            {
                _logger.LogError("Failed to add new user '{Username}' to repository. Error: {ErrorMessage}", dto.Username, addUserResult.Message);
                return OperationResult<CustomerRegisterResponseDto>.Failure(addUserResult.Message);
            }
            _logger.LogInformation("New user '{Username}' (User ID: {UserId}) added to repository.", dto.Username, user.Id);

            var userRole = _mapper.Map<UserRole>(user);
            userRole.RoleId = role.Id; 
            userRole.UserId = user.Id; 
            _logger.LogDebug("UserRole entity mapped for user '{Username}' (User ID: {UserId}) with Role ID: {RoleId}.", dto.Username, user.Id, role.Id);


            var userRoleResult = await _userRoleRepository.AddAsync(userRole);
            if (!userRoleResult.IsSuccess)
            {
                _logger.LogError("Failed to assign role 'Customer' to user '{Username}' (User ID: {UserId}). Error: {ErrorMessage}", dto.Username, user.Id, userRoleResult.Message);
               
                return OperationResult<CustomerRegisterResponseDto>.Failure(userRoleResult.Message);
            }
            _logger.LogInformation("Role 'Customer' assigned to user '{Username}' (User ID: {UserId}).", dto.Username, user.Id);


            var customer = _mapper.Map<Customer>(dto);
            customer.UserId = user.Id; 
            _logger.LogDebug("Customer entity mapped for user '{Username}' (User ID: {UserId}).", dto.Username, user.Id);

            var addCustomerResult = await _customerRepository.AddAsync(customer);
            if (!addCustomerResult.IsSuccess)
            {
                _logger.LogError("Failed to create customer profile for user '{Username}' (User ID: {UserId}). Error: {ErrorMessage}", dto.Username, user.Id, addCustomerResult.Message);
              
                return OperationResult<CustomerRegisterResponseDto>.Failure(addCustomerResult.Message);
            }
            _logger.LogInformation("Customer profile (Customer ID: {CustomerId}) created for user '{Username}' (User ID: {UserId}).", customer.CustomerId, dto.Username, user.Id);

            _logger.LogInformation("Customer registration successful for username: {Username}", dto.Username);
            return OperationResult<CustomerRegisterResponseDto>.Success("Registered successfully");
        }
    }
}