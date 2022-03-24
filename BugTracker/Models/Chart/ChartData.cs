namespace BugTracker.Models
{
    public class ChartData
    {
        public ICollection<string> Labels { get; set; } = new List<string>();

        public ICollection<int> Values { get; set; } = new List<int>();
    }
}
