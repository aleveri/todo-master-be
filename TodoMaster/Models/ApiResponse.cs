namespace TodoMaster.Models
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public List<T> Content { get; set; } = [];
        public List<string> Errors { get; set; } = [];

        public static ApiResponse<T> Success(List<T> content) => new()
        {
            Status = true,
            Content = content
        };

        public static ApiResponse<T> Fail(List<string> errors) => new()
        {
            Status = false,
            Errors = errors
        };
    }
}
