using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidPay.Models
{
    /// <summary>
    /// Represents a log entry for card authorization attempts, including the authorization status and date.
    /// </summary>
    public class AuthorizationLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CardId { get; set; }

        [ForeignKey("CardId")]
        public Card? Card { get; set; }

        [Required]
        public bool IsAuthorized { get; set; }

        public DateTime AuthorizationDate { get; set; }
    }
}