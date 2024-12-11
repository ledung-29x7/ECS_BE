using AutoMapper;
using ECS.Areas.Client.Models;
using ECS.Dtos;

namespace ECS.MappingProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<Product, CreateProductRequest>();
            CreateMap<ProductWithImagesDTO, Product>();
            CreateMap<Product, ProductWithImagesDTO>();


        }
    }
}
