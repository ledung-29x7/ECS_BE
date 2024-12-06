using AutoMapper;
using ECS.Areas.Authen.Models;
using ECS.Dtos;

namespace ECS.MappingProfile
{
    public class RoleProfile : Profile
    {
        public RoleProfile() 
        {
            CreateMap<RoleDto, Role>();
            CreateMap<Role, RoleDto>();

        }
    }
}
