using System.ComponentModel.DataAnnotations;
using Xunit;
using Event_Registration.Helpers;

namespace Event_Registration.Tests
{
    public class EstonianIdCodeAttributeTests
    {
        [Theory]
        [InlineData("39506036025", true)]  // Assuming this is a valid ID
        [InlineData("12345678901", false)] // Assuming this is an invalid ID
        [InlineData("abc", false)]         // Invalid format
        [InlineData("123456789012345", false)] // Too long
        [InlineData("", false)]            // Empty string
        public void EstonianIdCode_ValidationTests(string idCode, bool expectedIsValid)
        {
            var attribute = new EstonianIdCodeAttribute();
            var validationContext = new ValidationContext(new object()){};
            var objectToValidate = idCode;

            var result = attribute.GetValidationResult(objectToValidate, validationContext);

            if (expectedIsValid)
            {
                Assert.Equal(ValidationResult.Success, result);
            }
            else
            {
                Assert.NotEqual(ValidationResult.Success, result);
            }
        }
    }
}
