namespace TodoMaster.Models
{
    public class RegisterProgressionRequest
    {
        public required DateTime Date { get; set; }
        public required decimal Percent { get; set; }
    }
}
