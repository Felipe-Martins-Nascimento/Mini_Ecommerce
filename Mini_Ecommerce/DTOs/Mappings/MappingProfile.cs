﻿using AutoMapper;
using Mini_Ecommerce.Models;

namespace Mini_Ecommerce.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<ProductDTO, Product>();
        CreateMap<Product, ProductDTO>().ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
    }      
}
