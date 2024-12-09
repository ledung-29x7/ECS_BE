using AutoMapper;
using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.Dtos;

namespace ECS.MappingProfile
{
    public class ClientProfile : Profile
    {
        public ClientProfile() 
        {
            CreateMap<RegisterClientDto, Client>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<LoginClientDto, Client>();
            CreateMap<Client, LoginClientDto>();
        }
    }
}
