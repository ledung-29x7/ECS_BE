using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Dtos;

namespace ECS.MappingProfile
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() 
        {
            CreateMap<Departments, DeparmentCreateDto>();
            CreateMap<DeparmentCreateDto, Departments>()
            .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId));

        }
    }
}
