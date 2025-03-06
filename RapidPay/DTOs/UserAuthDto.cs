namespace RapidPay.DTOs
{
    /// <summary>
    /// Data Transfer Object for user authentication.
    /// </summary>
    public class UserAuthDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}