using TodoMaster.Domain.Entities;

namespace TodoMaster.Domain.Interfaces
{
    public interface ITodoListRepository
    {
        int GetNextId();

        List<string> GetAllCategories();

        void Save(TodoItem item);

        void Delete(int id);

        TodoItem? GetById(int id);

        List<TodoItem> GetAll();
    }
}