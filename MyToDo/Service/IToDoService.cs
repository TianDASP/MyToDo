using MyToDo.Common.Models;

namespace MyToDo.Service
{
    public interface IToDoService : IBaseService<int, ToDoDto>
    {
    }
}