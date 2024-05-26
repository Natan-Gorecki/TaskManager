using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using TaskManager.Service.Api.v1.Labels;
using TaskManager.Service.Api.v1.Spaces;
using TaskManager.Service.Api.v1.Tasks;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateSpaceRequest, DbSpace>();
        CreateMap<SpaceDTO, DbSpace>().ReverseMap();

        CreateMap<CreateLabelRequest, DbLabel>();
        CreateMap<UpdateLabelRequest, DbLabel>();
        CreateMap<LabelDTO, DbLabel>().ReverseMap();

        CreateMap<CreateTaskRequest, DbTask>();
        CreateMap<UpdateTaskRequest, DbTask>();
        CreateMap<TaskDTO, DbTask>().ReverseMap();
        CreateMapForEnums<TaskStatusDTO, TaskStatus>();
        CreateMapForEnums<TaskTypeDTO, TaskType>();
    }

    private void CreateMapForEnums<TSource, TDestination>()
        where TSource : struct, Enum
        where TDestination : struct, Enum
    {
        CreateMap<TSource, TDestination>()
            .ConvertUsingEnumMapping(opt => opt.MapByName())
            .ReverseMap();
    }
}
