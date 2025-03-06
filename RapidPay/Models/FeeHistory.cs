using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidPay.Models
{
    /// <summary>
    /// Represents the history of transaction fees applied, including the fee amount and date.
    /// </summary>
    public class FeeHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public decimal Fee { get; set; }

        public DateTime FeeDate { get; set; }
    }
}