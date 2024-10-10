namespace Event_Registration_System.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
    }
}