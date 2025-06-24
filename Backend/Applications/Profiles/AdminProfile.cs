using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Applications.Profiles
{
    public class AdminProfile:Profile
    {
        public AdminProfile() 
        {
            CreateMap<AvailablePolicy,AvailablePolicyResponseDto>().
                ForMember(dest=> dest.Id,opt => opt.MapFrom(src =>src.AvailablePolicyId)).ReverseMap();
            CreateMap<Agent, AgentProfileResponseDto>()
                .ForMember(dest=> dest.Username,opt =>opt.MapFrom(src => src.User.Username)).ReverseMap();
            CreateMap<Customer, CustomerProfileResponseDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();
         
          
            CreateMap<Claim,ClaimStatusResponseDtoForAdmin>().
                ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent.Name)).
                ForMember(dest=>dest.CustomerName,opt=>opt.MapFrom(src=>src.Customer.Name)).
                ForMember(dest=>dest.PolicyName,opt =>opt.MapFrom(src =>src.Policy.AvailablePolicy.Name)).ReverseMap();
            CreateMap<AvailablePolicyRequestDto, AvailablePolicy>().ReverseMap();
            CreateMap<AssignAgentRequestDto, Policy>().ReverseMap();
            CreateMap<PolicyRequest, Policy>().ReverseMap();
            CreateMap<User, UserWithRoleResponseDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRole.Role.Name ?? "Unknown"))
                .ForMember(dest => dest.UserId,opt=> opt.MapFrom(src=> src.Id))
                .ReverseMap();
          
            CreateMap<AgentRegisterRequestDto, User>().ReverseMap();
            CreateMap<User, UserRole>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap();

            CreateMap<Role, UserRole>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id)).ReverseMap();

            CreateMap<AgentRegisterRequestDto, Agent>().ReverseMap();

            CreateMap<User, Agent>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap();


           
            CreateMap<Claim, ClaimsFiledByCustomerResponseDtoForAdmin>().ReverseMap();
        }

    }
}
