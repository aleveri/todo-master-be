using TodoMaster.Domain.Entities;
using AutoMapper;
using TodoMaster.Models;

namespace TodoMaster.Mappers
{
    public class TodoMappingProfile : Profile
    {
        public TodoMappingProfile()
        {
            CreateMap<TodoItem, TodoItemModel>();
            CreateMap<Progression, ProgressionModel>();
        }
    }
}
