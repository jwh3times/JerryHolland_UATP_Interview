using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidPay.Models
{
    /// <summary>
    /// Represents a transaction made using a card, including the amount, fee, and transaction date.
    /// </summary>
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CardId { get; set; }

        [ForeignKey("CardId")]
        public required Card Card { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal Fee { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}