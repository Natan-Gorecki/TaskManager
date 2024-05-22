using AutoMapper;
using TaskManager.Service.Api.v1.Spaces;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DbSpace, SpaceDTO>();
    }
}
