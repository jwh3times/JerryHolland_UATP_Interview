using Microsoft.EntityFrameworkCore;
using RapidPay.Data;
using RapidPay.Models;

namespace RapidPay.Services
{
    /// <summary>
    /// Provides implementation for card-related operations.
    /// </summary>
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFeeService _feeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CardService(ApplicationDbContext context)
        {
            _context = context;
            _feeService = new FeeService(context);
        }

        /// <summary>
        /// Creates a new card with a random balance and optional credit limit.
        /// </summary>
        /// <param name="creditLimit">The optional credit limit for the card.</param>
        /// <returns>The created card.</returns>
        public async Task<Card> CreateCardAsync(decimal? creditLimit)
        {
            // Generate a new card with random balance and optional credit limit
            var card = new Card
            {
                CardNumber = Card.EncryptCardNumber(GenerateCardNumber()),
                Balance = Math.Round(new decimal(new Random().NextDouble() * int.MaxValue), 2),
                CreditLimit = creditLimit,
                IsActive = true
            };

            // Add the new card to the database
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return card;
        }

        /// <summary>
        /// Authorizes a card by validating its status and performing fraud checks.
        /// </summary>
        /// <param name="cardNumber">The number of the card to authorize.</param>
        /// <returns>True if the card is authorized; otherwise, false.</returns>
        public async Task<bool> AuthorizeCardAsync(string cardNumber)
        {
            try {
                var authorized = false;

                // Retrieve the card from the database
                var card = await _context.Cards.Where(c => c.CardNumber == cardNumber).FirstOrDefaultAsync();
                if (card != null && card.IsActive)
                {
                    authorized = true;

                    // Additional fraud checks can be implemented here
                    var lastTransaction = await _context.Transactions.Where(t => t.CardId == card.Id).OrderBy(t => t.TransactionDate).LastOrDefaultAsync();
                    if (lastTransaction != null && lastTransaction.TransactionDate.AddSeconds(5) > DateTime.UtcNow)
                    {
                        authorized = false;
                    }
                }

                // Log the authorization attempt
                var authorizationLog = new AuthorizationLog
                {
                    CardId = card == null ? 0 : card.Id,
                    Card = card,
                    IsAuthorized = authorized,
                    AuthorizationDate = DateTime.UtcNow
                };

                _context.AuthorizationLogs.Add(authorizationLog);
                await _context.SaveChangesAsync();

                return authorized;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Processes a payment using an authorized card and applies payment fees.
        /// </summary>
        /// <param name="cardNumber">The number of the card to use for the payment.</param>
        /// <param name="amount">The amount to pay.</param>
        /// <returns>The transaction details.</returns>
        public async Task<Transaction?> PayWithCardAsync(string cardNumber, decimal amount)
        {
            // Retrieve the card and current fee from the database
            var card = await _context.Cards.Where(c => c.CardNumber == cardNumber).FirstOrDefaultAsync();
            var fee = await _feeService.GetCurrentFeeAsync();

            // Check if the card is authorized and has sufficient balance
            if (card == null || !card.IsActive || (card.Balance + card.CreditLimit) < (amount + fee))
            {
                throw new InvalidOperationException("Card is not authorized or has insufficient balance.");
            }

            // Create a new transaction
            var transaction = new Transaction
            {
                CardId = card.Id,
                Card = card,
                Amount = amount,
                Fee = fee,
                TransactionDate = DateTime.UtcNow
            };

            // Update the card balance and save the transaction
            card.Balance -= amount + fee;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        /// <summary>
        /// Retrieves the current balance and available credit limit of a card.
        /// </summary>
        /// <param name="cardNumber">The number of the card.</param>
        /// <returns>The card details.</returns>
        public async Task<Card?> GetCardBalanceAsync(string cardNumber)
        {
            // Retrieve the card from the database
            var card = await _context.Cards.Where(c => c.CardNumber == cardNumber).FirstOrDefaultAsync();
            return card;
        }

        /// <summary>
        /// Updates the details of a card, including balance, credit limit, and status.
        /// </summary>
        /// <param name="cardNumber">The number of the card to update.</param>
        /// <param name="balance">The new balance of the card.</param>
        /// <param name="creditLimit">The new credit limit of the card.</param>
        /// <param name="isActive">The new status of the card.</param>
        /// <returns>The updated card details.</returns>
        public async Task<Card> UpdateCardDetailsAsync(string cardNumber, decimal? balance, decimal? creditLimit, bool? isActive)
        {
            // Retrieve the card from the database
            var card = await _context.Cards.Where(c => c.CardNumber == cardNumber).FirstOrDefaultAsync();
            if (card == null)
            {
                throw new InvalidOperationException("Card not found.");
            }

            var cardUpdates = new List<CardUpdateLog>();

            // Update the card details if provided
            if (balance.HasValue && balance.Value != card.Balance)
            {
                cardUpdates.Add(new CardUpdateLog
                {
                    CardId = card.Id,
                    Card = card,
                    UpdateDate = DateTime.UtcNow,
                    UpdatedField = "Balance",
                    OldValue = card.Balance.ToString(),
                    NewValue = balance.Value.ToString()
                });
                card.Balance = balance.Value;
            }

            if (creditLimit.HasValue && creditLimit.Value != card.CreditLimit)
            {
                cardUpdates.Add(new CardUpdateLog
                {
                    CardId = card.Id,
                    Card = card,
                    UpdateDate = DateTime.UtcNow,
                    UpdatedField = "CreditLimit",
                    OldValue = card.CreditLimit?.ToString(),
                    NewValue = creditLimit.Value.ToString()
                });
                card.CreditLimit = creditLimit.Value;
            }

            if (isActive.HasValue && isActive.Value != card.IsActive)
            {
                cardUpdates.Add(new CardUpdateLog
                {
                    CardId = card.Id,
                    Card = card,
                    UpdateDate = DateTime.UtcNow,
                    UpdatedField = "IsActive",
                    OldValue = card.IsActive.ToString(),
                    NewValue = isActive.Value.ToString()
                });
                card.IsActive = isActive.Value;
            }

            // Log the card updates and save changes
            _context.CardUpdateLogs.AddRange(cardUpdates);
            await _context.SaveChangesAsync();

            return card;
        }

        /// <summary>
        /// Generates a 15-digit card number.
        /// </summary>
        /// <returns>A 15-digit card number.</returns>
        private string GenerateCardNumber()
        {
            // Generate a random 15-digit card number
            var random = new Random();
            return new string(Enumerable.Range(0, 15).Select(_ => (char)('0' + random.Next(10))).ToArray());
        }
    }
}