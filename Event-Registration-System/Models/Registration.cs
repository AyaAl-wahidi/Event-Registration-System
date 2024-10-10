namespace Event_Registration_System.Models
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public String ParticipantName { get; set; }
        public String Email { get; set; }
        public Event Event { get; set; }
        public int EventId { get; set; }
    }
}