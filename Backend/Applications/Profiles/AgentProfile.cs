
using InsurenceManagementSystemWebApi.Domain.Models;
namespace InsurenceManagementSystemWebApi.Applications.Profiles
{
    public class AgentProfile : Profile
    {
        public AgentProfile() {
           

            CreateMap<Domain.Models.Claim, ClaimStatusResponseDtoForAgent>().
                ForMember(dest=>dest.CustomerName,opt=>opt.MapFrom(src =>src.Customer.Name)).
                ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.Policy.AvailablePolicy.Name))
                .ReverseMap();

            CreateMap<ClaimFilingRequestDtoForAgent,Domain.Models.Claim>().ReverseMap();

            CreateMap<Policy ,AgentAssignedPolicyResponseDto>().
                ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email)).
                ForMember(dest=> dest.Phone,opt=>opt.MapFrom(src=> src.Customer.Phone)).
                ForMember(dest =>dest.Name,opt => opt.MapFrom(src =>src.Customer.Name)).
                ForMember(dest => dest.AvailablePolicyName,opt => opt.MapFrom(src => src.AvailablePolicy.Name)).ReverseMap();

            CreateMap<AgentProfileUpdateRequestDto,Agent>().ReverseMap();

            CreateMap<AvailablePolicy, AvailablePolicyResponseDto>().
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AvailablePolicyId))
                .ReverseMap();

            CreateMap<Notification, NotificationResponseDto>().ReverseMap();

           
        }
    }
}
