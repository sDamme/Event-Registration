namespace Event_Registration.Models
{
    public class AttendeesViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string Location { get; set; }
        public List<Guest> Guests { get; set; }
        public string AttendeeType { get; set; }
     
    }
}
