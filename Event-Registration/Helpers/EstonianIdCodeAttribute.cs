using System.ComponentModel.DataAnnotations;

namespace Event_Registration.Helpers
{
    public class EstonianIdCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not string id || id.Length != 11 || !id.All(char.IsDigit))
                return new ValidationResult("Isikukood peab olema 11 numbrit pikk.");

            if (!IsValidChecksum(id))
                return new ValidationResult("Isikukood ei ole kehtiv vastavalt kontrollnumbri arvutusele.");

            return ValidationResult.Success;
        }


        private bool IsValidChecksum(string personalIdCode)
        {
            // https://et.wikipedia.org/wiki/Isikukood Based on the Estonian Wikipedia article

            int[] firstLevelWeights = [1, 2, 3, 4, 5, 6, 7, 8, 9, 1];
            int[] secondLevelWeights = [3, 4, 5, 6, 7, 8, 9, 1, 2, 3];

            int checksum = 0;

            for (int i = 0; i < 10; i++)
            {
                checksum += (personalIdCode[i] - '0') * firstLevelWeights[i];
            }

            int remainder = checksum % 11;

            if (remainder != 10)
                return remainder == (personalIdCode[10] - '0');

            checksum = 0;
            for (int i = 0; i < 10; i++)
            {
                checksum += (personalIdCode[i] - '0') * secondLevelWeights[i];
            }

            remainder = checksum % 11;
            if (remainder == 10)
                remainder = 0;

            return remainder == (personalIdCode[10] - '0');
        }
    }
}
