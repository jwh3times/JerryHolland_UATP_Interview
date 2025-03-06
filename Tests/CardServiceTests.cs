using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RapidPay.Data;
using RapidPay.Models;
using RapidPay.Services;
using Xunit;

namespace Tests
{
    public class CardServiceTests
    {
        private readonly CardService _cardService;
        private readonly ApplicationDbContext _context;

        public CardServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _cardService = new CardService(_context);
        }

        [Fact]
        public async Task CreateCardAsync_ShouldCreateCard()
        {
            // Act
            var card = await _cardService.CreateCardAsync(1000);

            // Assert
            Assert.NotNull(card);
            Assert.Equal(24, card.CardNumber.Length);
            Assert.True(card.Balance >= 0 && card.Balance <= int.MaxValue);
            Assert.Equal(1000, card.CreditLimit);
        }

        [Fact]
        public async Task AuthorizeCardAsync_ShouldAuthorizeCard()
        {
            // Arrange
            var card = new Card { Id = 2, CardNumber = "123456789012345", IsActive = true };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            // Act
            var isAuthorized = await _cardService.AuthorizeCardAsync("123456789012345");

            // Assert
            Assert.True(isAuthorized);
        }

        [Fact]
        public async Task PayWithCardAsync_ShouldProcessPayment()
        {
            // Arrange
            var card = new Card { Id = 3, CardNumber = "234567890123456", Balance = 1000, IsActive = true };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            // Act
            var transaction = await _cardService.PayWithCardAsync("234567890123456", 100);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(100, transaction.Amount);
            Assert.True(transaction.Fee >= 0);
            Assert.Equal(900 - transaction.Fee, card.Balance);
        }

        [Fact]
        public async Task GetCardBalanceAsync_ShouldReturnCardBalance()
        {
            // Arrange
            var card = new Card { Id = 4, CardNumber = "345678901234567", Balance = 1000 };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            // Act
            var result = await _cardService.GetCardBalanceAsync("345678901234567");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000, result.Balance);
        }

        [Fact]
        public async Task UpdateCardDetailsAsync_ShouldUpdateCardDetails()
        {
            // Arrange
            var card = new Card { Id = 5, CardNumber = "456789012345678", Balance = 1000, CreditLimit = 1000, IsActive = true };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            // Act
            var updatedCard = await _cardService.UpdateCardDetailsAsync("456789012345678", 500, 2000, false);

            // Assert
            Assert.NotNull(updatedCard);
            Assert.Equal(500, updatedCard.Balance);
            Assert.Equal(2000, updatedCard.CreditLimit);
            Assert.False(updatedCard.IsActive);
        }
    }
}