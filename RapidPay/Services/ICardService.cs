using RapidPay.Models;
using System.Threading.Tasks;

namespace RapidPay.Services
{
    /// <summary>
    /// Defines the contract for card-related operations.
    /// </summary>
    public interface ICardService
    {
        Task<Card> CreateCardAsync(decimal? creditLimit);
        Task<bool> AuthorizeCardAsync(string cardNumber);
        Task<Transaction?> PayWithCardAsync(string cardNumber, decimal amount);
        Task<Card?> GetCardBalanceAsync(string cardNumber);
        Task<Card> UpdateCardDetailsAsync(string cardNumber, decimal? balance, decimal? creditLimit, bool? isActive);
    }
}