namespace Event_Registration.Models
{
    public class AttendeeDetailsViewModel
    {
        public int GuestId { get; set; }
        public int EventId { get; set; }
        public string AttendeeType { get; set; } 
        public Individual Individual { get; set; }
        public Business Business { get; set; }
    }
}