using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RapidPay.Controllers;
using RapidPay.DTOs;
using RapidPay.Models;
using RapidPay.Services;
using Xunit;

namespace Tests
{
    public class CardsControllerTests
    {
        private readonly Mock<ICardService> _cardServiceMock;
        private readonly CardsController _cardsController;

        public CardsControllerTests()
        {
            _cardServiceMock = new Mock<ICardService>();
            _cardsController = new CardsController(_cardServiceMock.Object);
        }

        [Fact]
        public async Task CreateCard_ShouldReturnCreatedCard()
        {
            // Arrange
            var card = new Card { Id = 1, CardNumber = "123456789012345", Balance = 1000, CreditLimit = 500 };
            _cardServiceMock.Setup(s => s.CreateCardAsync(It.IsAny<decimal?>())).ReturnsAsync(card);

            // Act
            var result = await _cardsController.CreateCard(new CardCreateDto() {CreditLimit = 500}) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(card, result.Value);
        }

        [Fact]
        public async Task AuthorizeCard_ShouldReturnOkIfAuthorized()
        {
            // Arrange
            _cardServiceMock.Setup(s => s.AuthorizeCardAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _cardsController.AuthorizeCard("123456789012345") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Card authorized successfully.", result.Value);
        }

        [Fact]
        public async Task PayWithCard_ShouldReturnTransaction()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, Amount = 100, Fee = 1.5m, Card = new Card { Id = 1, CardNumber = "123456789012345" } };
            _cardServiceMock.Setup(s => s.PayWithCardAsync(It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync(transaction);

            // Act
            var result = await _cardsController.PayWithCard("123456789012345", new CardPaymentDto(){Amount=100}) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(transaction, result.Value);
        }

        [Fact]
        public async Task GetCardBalance_ShouldReturnCardBalance()
        {
            // Arrange
            var card = new Card { Id = 1, CardNumber = "123456789012345", Balance = 1000 };
            _cardServiceMock.Setup(s => s.GetCardBalanceAsync(It.IsAny<string>())).ReturnsAsync(card);

            // Act
            var result = await _cardsController.GetCardBalance("123456789012345") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(card, result.Value);
        }

        [Fact]
        public async Task UpdateCardDetails_ShouldReturnUpdatedCard()
        {
            // Arrange
            var card = new Card { Id = 1, CardNumber = "123456789012345", Balance = 500, CreditLimit = 2000, IsActive = false };
            _cardServiceMock.Setup(s => s.UpdateCardDetailsAsync(It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<bool?>())).ReturnsAsync(card);

            var updateDto = new CardUpdateDto { Balance = 500, CreditLimit = 2000, IsActive = false };

            // Act
            var result = await _cardsController.UpdateCardDetails("123456789012345", updateDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(card, result.Value);
        }
    }
}