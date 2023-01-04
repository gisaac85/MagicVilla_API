using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa,VillaDto>();
            CreateMap<VillaDto,Villa>();
            CreateMap<VillaDto,VillaCreateDto>().ReverseMap();
            CreateMap<VillaDto,VillaUpdateDto>().ReverseMap();
            CreateMap<Villa,VillaUpdateDto>().ReverseMap();
            CreateMap<Villa,VillaCreateDto>().ReverseMap();
        }
    }
}
