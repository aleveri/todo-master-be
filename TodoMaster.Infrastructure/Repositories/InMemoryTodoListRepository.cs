using TodoMaster.Domain.Entities;
using TodoMaster.Domain.Interfaces;

namespace TodoMaster.Infrastructure.Repositories
{
    public class InMemoryTodoListRepository : ITodoListRepository
    {
        private int _currentId = 0;

        private readonly Dictionary<int, TodoItem> _items = [];

        private readonly List<string> _categories = ["Work", "Personal", "Urgent"];

        public int GetNextId() => ++_currentId;

        public List<string> GetAllCategories() => _categories;

        public void Save(TodoItem item)
        {
            _items[item.Id] = item;
        }

        public void Delete(int id)
        {
            _items.Remove(id);
        }

        public TodoItem? GetById(int id)
        {
            return _items.TryGetValue(id, out var item) ? item : null;
        }

        public List<TodoItem> GetAll()
        {
            return [.. _items.Values];
        }
    }
}