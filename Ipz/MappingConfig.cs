using AutoMapper;
using Ipz_server.Models.Database;
using Ipz_server.Models.Dto;
using Ipz_server.Models.Dto.Auth;
using Ipz_server.Models.Dto.Dishes;
using Ipz_server.Models.Dto.Locations;
using Ipz_server.Models.Dto.Restaurants;
using Ipz_server.Models.Dto.Users;
using Microsoft.IdentityModel.Tokens;

namespace Ipz
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserAuthResponseDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Location.Street))
                .ReverseMap();

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Location.Street))
                .ReverseMap();

            CreateMap<Restaurant, RestaurantResponseDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Location.Street))
                .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.Dishes))
                .ReverseMap();

            
            CreateMap<OrderInformation, OrderInformationResponseDto>()
                .ForMember(dest => dest.DishName, opt => opt.MapFrom(src => src.Dish.Name))
                .ReverseMap();

            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurant.Name))
                .ReverseMap();

            CreateMap<Dish, DishToRestaurantRequestDto>().ReverseMap();
            CreateMap<Dish, DishResponseDto>().ReverseMap();
            CreateMap<RestaurantCreateRequestDto, LocationCreateRequestDto>().ReverseMap();
            CreateMap<LocationCreateRequestDto, Location>().ReverseMap();
            CreateMap<LocationUpdateRequestDto, Location>().ReverseMap();
        }
    }
}
