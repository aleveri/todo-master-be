namespace TodoMaster.Domain.Entities
{
    public class TodoItem(int id, string title, string description, string category)
    {
        public int Id { get; } = id;
        public string Title { get; } = title;
        public string Description { get; private set; } = description;
        public string Category { get; } = category;
        private List<Progression> _progressions = [];
        public IReadOnlyList<Progression> Progressions => _progressions.AsReadOnly();

        public bool IsCompleted => GetTotalProgress() >= 100;

        public void UpdateDescription(string newDescription)
        {
            if (GetTotalProgress() > 50)
                throw new InvalidOperationException("Cannot update item with more than 50% progress.");

            Description = newDescription;
        }

        public void AddProgression(Progression progression)
        {
            if (_progressions.Any(p => p.Date >= progression.Date))
                throw new ArgumentException("Progression date must be greater than existing dates.");

            if (progression.Percent <= 0 || progression.Percent >= 100)
                throw new ArgumentException("Percent must be > 0 and < 100.");

            decimal newTotal = GetTotalProgress() + progression.Percent;
            if (newTotal > 100)
                throw new InvalidOperationException("Total progress cannot exceed 100%.");

            _progressions.Add(progression);
        }

        public decimal GetTotalProgress()
        {
            return _progressions.Sum(p => p.Percent);
        }
    }
}
