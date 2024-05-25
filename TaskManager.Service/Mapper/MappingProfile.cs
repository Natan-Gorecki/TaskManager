using AutoMapper;
using TaskManager.Service.Api.v1.Labels;
using TaskManager.Service.Api.v1.Spaces;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DbSpace, SpaceDTO>();
        CreateMap<CreateSpaceRequest, DbSpace>();
        CreateMap<SpaceDTO, DbSpace>();

        CreateMap<DbLabel, LabelDTO>();
        CreateMap<CreateLabelRequest, DbLabel>();
        CreateMap<UpdateLabelRequest, DbLabel>();
        CreateMap<SpaceDTO, DbSpace>();

    }
}
