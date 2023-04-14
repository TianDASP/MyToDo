using AutoMapper;
using MyToDoWebAPI.Dtos;
using MyToDoWebAPI.Entities;

namespace MyToDoWebAPI.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ToDo, ToDoDto>();
            CreateMap<Memo, MemoDto>();
            CreateMap<User, UserDto>();
        }
    }
}
