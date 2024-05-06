using AutoMapper;
using Ipz.Models.Database;
using Ipz.Models.Dto;
using Ipz.Models.Dto.Auth;

namespace Ipz
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, AuthUserResponseDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Location.Street));
        }
    }
}
