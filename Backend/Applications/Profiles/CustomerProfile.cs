
using InsurenceManagementSystemWebApi.Domain.Models;
namespace InsurenceManagementSystemWebApi.Applications.Profiles
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile()
        {
            CreateMap<Policy, CustomerPoliciesResponseDto>().
                ForMember(dest => dest.AvailablePolicyName,opt => opt.MapFrom(src => src.AvailablePolicy.Name)).
                ForMember(dest=>dest.AgentName,opt=>opt.MapFrom(src =>src.Agent.Name)).
                ForMember(dest => dest.AgentContact, opt => opt.MapFrom(src => src.Agent.ContactInfo)).
                ForMember(dest => dest.PremiumAmount,opt => opt.MapFrom(src=> src.AvailablePolicy.BasePremium)).ReverseMap();

            CreateMap<PolicyRequest, PolicyRequestStatusResponseDto>().ForMember(dest=>dest.CustomerName,opt =>opt.MapFrom(src=>src.Customer.Name)).
                ForMember(dest =>dest.AvailablePolicyName,opt=>opt.MapFrom(src=>src.AvailablePolicy.Name)).ReverseMap();
            CreateMap<Claim, ClaimStatusResponseDtoForCustomer>().
                ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.Policy.AvailablePolicy.Name)).ReverseMap();
            CreateMap<PolicyRequestDto, PolicyRequest>().ReverseMap();
            CreateMap<Notification, NotificationResponseDto>().ReverseMap();
            CreateMap<CustomerProfileUpdateRequestDto, Customer>().ReverseMap();
            CreateMap<AvailablePolicy, AvailablePolicyResponseDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AvailablePolicyId)).ReverseMap();
            CreateMap<ClaimFilingRequestDtoForCustomer, Domain.Models.Claim>().ReverseMap();
        }
    }
}
