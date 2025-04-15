namespace TodoMaster.Models
{
    public class TodoItemModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public bool IsCompleted { get; set; }
        public List<ProgressionModel> Progressions { get; set; } = [];
    }
}
