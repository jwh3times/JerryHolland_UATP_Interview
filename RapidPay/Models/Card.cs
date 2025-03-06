using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace RapidPay.Models
{
    /// <summary>
    /// Represents a payment card with details such as card number, balance, credit limit, and status.
    /// </summary>
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public required string CardNumber { get; set; }

        [Required]
        public decimal Balance { get; set; }

        public decimal? CreditLimit { get; set; }

        public bool IsActive { get; set; }

        public static string EncryptCardNumber(string cardNumber)
        {
            byte[] cardBytes = Encoding.UTF8.GetBytes(cardNumber);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.ASCII.GetBytes("Jerry_Holland_UATP_Interview_Key");
                aes.IV = Encoding.ASCII.GetBytes("UATPInterview_IV");

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    return Convert.ToBase64String(encryptor.TransformFinalBlock(cardBytes, 0, cardBytes.Length));
                }
            }
        }

        public static string DecryptCardNumber(string encryptedCardNumber)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedCardNumber);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.ASCII.GetBytes("Jerry_Holland_UATP_Interview_Key");
                aes.IV = Encoding.ASCII.GetBytes("UATPInterview_IV");

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
                }
            }
        }
    }
}