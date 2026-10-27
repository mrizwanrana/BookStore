namespace RizTechBookStore.Models
{
    public class EmailConfirmModel
    {
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public bool EmailSent { get; set; }
        public bool EmailVerified { get; set; }
        // Indicates whether a verification attempt was performed (uid/token supplied)
        public bool VerificationAttempted { get; set; }
    }
}
