using AutoMapper;
using BeirutWalksDomains.Dto;
using BeirutWalksDomains.Models;

namespace BeirutWalksWebApi.MappingConfig
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Walks,WalksDto>().ReverseMap();
            CreateMap<Regions, RegionsDto>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<Regions, RegionsCreateDto>().ReverseMap();
            CreateMap<Regions, RegionUpdateDto>().ReverseMap();
            CreateMap<Image, ImageUploadRequestDto>().ReverseMap();

        }
    }
}
