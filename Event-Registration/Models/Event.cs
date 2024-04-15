using System.ComponentModel.DataAnnotations;

namespace Event_Registration.Models
{
    public class Event
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required DateTime Time { get; set; }

        public required string Location { get; set; }

        [StringLength(1000, ErrorMessage = "Lubatud on ainult 1000 tähemärki.")]
        public required string ExtraInformation { get; set; }

        public ICollection<Guest>? Guests { get; set; }
    }
}
