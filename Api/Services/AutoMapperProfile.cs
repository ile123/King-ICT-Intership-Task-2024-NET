using AutoMapper;
using Models.Dtos;
using Models.Entities;

namespace Api.Services;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}