using AutoMapper;
using ECS.Areas.Authen.Models;
using ECS.Dtos;

namespace ECS.MappingProfile
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile() 
        {
            CreateMap<RegisterEmployeeDto, Employee>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<LoginEmployeeDto, Employee>();
            CreateMap<Employee, LoginEmployeeDto>();
        }
    }
}
