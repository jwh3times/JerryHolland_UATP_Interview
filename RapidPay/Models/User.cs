using System.ComponentModel.DataAnnotations;

namespace RapidPay.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string? PasswordHash { get; set; }
    }
}