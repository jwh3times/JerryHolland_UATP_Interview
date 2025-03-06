using System.ComponentModel.DataAnnotations;

namespace RapidPay.DTOs
{
    public class CardPaymentDto
    {
        [Required]
        public required decimal Amount { get; set; }
    }
}