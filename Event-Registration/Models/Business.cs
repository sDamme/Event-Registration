using System.ComponentModel.DataAnnotations;

namespace Event_Registration.Models
{
    public class Business : Guest
    {
        public string LegalName { get; set; }
        public string RegistrationCode { get; set; }
        public int NumberOfAttendees { get; set; }

        [StringLength(5000, ErrorMessage = "Lubatud on ainult 5000 tähemärki.")]
        public override string ExtraInformation { get; set; }
    }
}
