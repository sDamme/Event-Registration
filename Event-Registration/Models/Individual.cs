using Event_Registration.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Event_Registration.Models
{
    public class Individual : Guest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EstonianIdCode]
        public string PersonalIdCode { get; set; }

        [StringLength(1500, ErrorMessage = "Lubatud on ainult 1500 tähemärki.")]
        public override string ExtraInformation { get; set; }
    }
}
