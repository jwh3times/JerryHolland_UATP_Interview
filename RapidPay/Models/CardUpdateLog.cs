using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidPay.Models
{
    /// <summary>
    /// Represents a log of updates to a card's details.
    /// </summary>
    public class CardUpdateLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CardId { get; set; }

        [ForeignKey("CardId")]
        public Card? Card { get; set; }

        [Required]
        public string? UpdatedField { get; set; }

        [Required]
        public string? OldValue { get; set; }

        [Required]
        public string? NewValue { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}