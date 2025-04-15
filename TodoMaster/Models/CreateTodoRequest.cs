namespace TodoMaster.Models
{
    public class CreateTodoRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
    }
}