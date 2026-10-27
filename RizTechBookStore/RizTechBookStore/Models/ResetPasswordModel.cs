using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RizTechBookStore.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required, DataType(DataType.Password), DisplayName("New Password")]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password)]
        [Compare("NewPassword"), DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public bool IsSuccess { get; set; }
    }
}
