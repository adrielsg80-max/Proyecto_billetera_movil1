using System.ComponentModel.DataAnnotations;

namespace Banca_movil.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public bool EmailConfirmed { get; set; }
        public string? EmailConfirmationToken { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpiryTime { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
