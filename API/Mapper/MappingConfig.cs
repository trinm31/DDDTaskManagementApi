using API.DTOs.Assignments;
using API.DTOs.ListTasks;
using API.DTOs.Projects;
using API.DTOs.TaskItems;
using API.DTOs.Todos;
using API.DTOs.Users;
using AutoMapper;
using Domain.Entities.Assigments;
using Domain.Entities.ListTasks;
using Domain.Entities.Projects;
using Domain.Entities.Tasks;
using Domain.Entities.Todos;
using Domain.Entities.Users;

namespace API.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                //mapping Task
                config.CreateMap<TaskItemUpSertRequest, TaskItem>().ReverseMap();
                //mapping user
                config.CreateMap<User, AuthenticateResponse>();
                config.CreateMap<RegisterRequest, User>();
                config.CreateMap<UpdateRequest, User>()
                    .ForAllMembers(x => x.Condition(
                        (src, dest, prop) =>
                        {
                            // ignore null & empty string properties
                            if (prop == null) return false;
                            if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                            return true;
                        }
                    ));
                // mapping project
                config.CreateMap<ProjectUpSertRequest, Project>().ReverseMap();
                // mapping list task
                config.CreateMap<ListTaskUpsertRequest, ListTask>().ReverseMap();
                // mapping TodoList
                config.CreateMap<TodoUpSertRequest, Todo>().ReverseMap();
                // mapping assignment
                config.CreateMap<AssignmentUpSertRequest, Assignment>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
