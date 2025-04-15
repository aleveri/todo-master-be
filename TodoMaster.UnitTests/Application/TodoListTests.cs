using Moq;
using TodoMaster.Application;
using TodoMaster.Domain.Entities;
using TodoMaster.Domain.Interfaces;

namespace TodoMaster.UnitTests.Application
{
    public class TodoListTests
    {
        private readonly Mock<ITodoListRepository> _mockRepo;
        private readonly TodoList _todoList;

        private readonly Dictionary<int, TodoItem> _storage = [];

        public TodoListTests()
        {
            _mockRepo = new Mock<ITodoListRepository>();

            _mockRepo.Setup(r => r.GetAllCategories()).Returns(["Work", "Personal"]);
            _mockRepo.Setup(r => r.GetNextId()).Returns(1);

            _mockRepo.Setup(r => r.Save(It.IsAny<TodoItem>()))
                     .Callback<TodoItem>(item => _storage[item.Id] = item);

            _mockRepo.Setup(r => r.GetById(It.IsAny<int>()))
                     .Returns<int>(id => _storage.TryGetValue(id, out TodoItem? value) ? value : null);

            _mockRepo.Setup(r => r.Delete(It.IsAny<int>()))
                     .Callback<int>(id => _storage.Remove(id));

            _mockRepo.Setup(r => r.GetAll()).Returns(() => [.. _storage.Values]);

            _todoList = new TodoList(_mockRepo.Object);
        }

        [Fact]
        public void AddItem_Should_Add_Valid_TodoItem()
        {
            _todoList.AddItem(1, "Test", "Testing", "Work");

            var item = _todoList.GetItemById(1);
            Assert.NotNull(item);
            Assert.Equal("Test", item!.Title);
        }

        [Fact]
        public void AddItem_Should_Throw_If_Category_Invalid()
        {
            Assert.Throws<ArgumentException>(() =>
                _todoList.AddItem(1, "Invalid", "Test", "InvalidCategory"));
        }

        [Fact]
        public void RegisterProgression_Should_Add_Valid_Progress()
        {
            _todoList.AddItem(1, "Test", "Desc", "Work");

            _todoList.RegisterProgression(1, new DateTime(2025, 1, 1), 20);
            _todoList.RegisterProgression(1, new DateTime(2025, 1, 2), 30);

            var item = _todoList.GetItemById(1);
            Assert.Equal(2, item!.Progressions.Count);
            Assert.Equal(50, item.GetTotalProgress());
        }

        [Fact]
        public void RegisterProgression_Should_Throw_On_Date_Before_Existing()
        {
            _todoList.AddItem(1, "Test", "Desc", "Work");
            _todoList.RegisterProgression(1, new DateTime(2025, 1, 2), 20);

            Assert.Throws<ArgumentException>(() =>
                _todoList.RegisterProgression(1, new DateTime(2025, 1, 1), 10));
        }

        [Fact]
        public void RegisterProgression_Should_Throw_On_Over_100_Percent()
        {
            _todoList.AddItem(1, "Test", "Desc", "Work");
            _todoList.RegisterProgression(1, new DateTime(2025, 1, 1), 90);

            Assert.Throws<InvalidOperationException>(() =>
                _todoList.RegisterProgression(1, new DateTime(2025, 1, 2), 20));
        }

        [Fact]
        public void RemoveItem_Should_Work_Under_50()
        {
            _todoList.AddItem(1, "Test", "Desc", "Work");
            _todoList.RegisterProgression(1, new DateTime(2025, 1, 1), 30);

            _todoList.RemoveItem(1);
            var item = _todoList.GetItemById(1);

            Assert.Null(item);
        }

        [Fact]
        public void RemoveItem_Should_Fail_Over_50()
        {
            _todoList.AddItem(1, "Test", "Desc", "Work");
            _todoList.RegisterProgression(1, new DateTime(2025, 1, 1), 60);

            Assert.Throws<InvalidOperationException>(() => _todoList.RemoveItem(1));
        }

        [Fact]
        public void UpdateItem_Should_Work_If_Progress_Less_Than_50()
        {
            _todoList.AddItem(1, "Test", "Desc", "Work");
            _todoList.UpdateItem(1, "New Desc");

            var item = _todoList.GetItemById(1);
            Assert.Equal("New Desc", item!.Description);
        }

        [Fact]
        public void UpdateItem_Should_Throw_If_Progress_Over_50()
        {
            _todoList.AddItem(1, "Test", "Desc", "Work");
            _todoList.RegisterProgression(1, new DateTime(2025, 1, 1), 60);

            Assert.Throws<InvalidOperationException>(() =>
                _todoList.UpdateItem(1, "Blocked"));
        }
    }
}