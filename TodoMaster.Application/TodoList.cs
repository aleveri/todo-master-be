using TodoMaster.Domain.Entities;
using TodoMaster.Domain.Interfaces;

namespace TodoMaster.Application
{
    public class TodoList(ITodoListRepository repository) : ITodoList
    {
        public void AddItem(int id, string title, string description, string category)
        {
            if (!repository.GetAllCategories().Contains(category))
                throw new ArgumentException("Invalid category.");

            var item = new TodoItem(id, title, description, category);
            repository.Save(item);
        }

        public void UpdateItem(int id, string description)
        {
            var item = repository.GetById(id) ?? throw new KeyNotFoundException("Item not found.");

            item.UpdateDescription(description);
            repository.Save(item);
        }

        public void RemoveItem(int id)
        {
            var item = repository.GetById(id) ?? throw new KeyNotFoundException("Item not found.");

            if (item.GetTotalProgress() > 50)
                throw new InvalidOperationException("Cannot delete item with more than 50% progress.");

            repository.Delete(id);
        }

        public void RegisterProgression(int id, DateTime dateTime, decimal percent)
        {
            var item = repository.GetById(id) ?? throw new KeyNotFoundException("Item not found.");

            item.AddProgression(new Progression { Date = dateTime, Percent = percent });
            repository.Save(item);
        }

        public List<TodoItem> GetAllItems()
        {
            return [.. repository.GetAll().OrderBy(i => i.Id)];
        }

        public TodoItem? GetItemById(int id)
        {
            return repository.GetById(id);
        }
    }
}