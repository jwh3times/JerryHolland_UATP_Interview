namespace RapidPay.DTOs
{
    public class CardUpdateDto
    {
        public decimal? Balance { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool? IsActive { get; set; }
    }
}