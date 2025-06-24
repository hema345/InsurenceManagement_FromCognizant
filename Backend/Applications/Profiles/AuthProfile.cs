

using InsurenceManagementSystemWebApi.Domain.Models;
using  AutoMapper;
using InsurenceManagementSystemWebApi.Applications.DTOs;

namespace InsurenceManagementSystemWebApi.Applications.Profiles
{
    public class AuthProfile:Profile
    {
        public AuthProfile()
        {
            CreateMap<CustomerRegisterRequestDto, User>().ReverseMap();

            CreateMap<User, UserRole>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<Role, UserRole>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id)).ReverseMap();


            CreateMap<CustomerRegisterRequestDto, Customer>().ReverseMap();
            CreateMap<User, Customer>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
        }
    }
}
