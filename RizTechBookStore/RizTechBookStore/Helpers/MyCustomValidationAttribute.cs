using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using System.ComponentModel.DataAnnotations;

namespace RizTechBookStore.Helpers
{
    public class MyCustomValidationAttribute : ValidationAttribute
    {
        public string Text { get; set; }
        public MyCustomValidationAttribute(string text)
        {
            Text = text;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)            
            {
                string bookName = value.ToString();
                if (bookName.Contains(Text))
                {
                    return ValidationResult.Success;
                }
            }
            // Additional custom validation logic can be added here
            return new ValidationResult(ErrorMessage ?? "The book name does not contain 'MVC'.");
        }
    }
}
