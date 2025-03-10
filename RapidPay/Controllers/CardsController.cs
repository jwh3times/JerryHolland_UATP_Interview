using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.DTOs;
using RapidPay.Services;

namespace RapidPay.Controllers
{
    /// <summary>
    /// Controller for handling card-related API endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardsController"/> class.
        /// </summary>
        /// <param name="cardService">The card service.</param>
        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        /// <summary>
        /// Creates a new card with a random balance and optional credit limit.
        /// </summary>
        /// <param name="creditLimit">The optional credit limit for the card.</param>
        /// <returns>The created card.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCard([FromBody] CardCreateDto createDto)
        {
            try {
                // Call the service to create a new card
                var card = await _cardService.CreateCardAsync(createDto.CreditLimit);
                return Ok(card);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Authorizes a card by validating its status and performing fraud checks.
        /// </summary>
        /// <param name="cardNumber">The encrypted number of the card to authorize.</param>
        /// <returns>Ok if the card is authorized; otherwise, BadRequest.</returns>
        [HttpPost("{cardNumber}/authorize")]
        [Authorize]
        public async Task<IActionResult> AuthorizeCard(string cardNumber)
        {
            try
            {
                // Call the service to authorize the card
                var isAuthorized = await _cardService.AuthorizeCardAsync(cardNumber, null);
                if (!isAuthorized)
                {
                    return BadRequest("Card is not authorized.");
                }

                return Ok("Card authorized successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Processes a payment using an authorized card and applies payment fees.
        /// </summary>
        /// <param name="cardNumber">The encrypted number of the card to authorize.</param>
        /// <param name="amount">The amount to pay.</param>
        /// <returns>The transaction details.</returns>
        [HttpPost("{cardNumber}/pay")]
        [Authorize]
        public async Task<IActionResult> PayWithCard(string cardNumber, [FromBody] CardPaymentDto paymentDto)
        {
            try
            {
                // Call the service to process the payment
                var transaction = await _cardService.PayWithCardAsync(cardNumber, paymentDto.Amount);
                return Ok(transaction);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the current balance and available credit limit of a card.
        /// </summary>
        /// <param name="cardNumber">The encrypted number of the card to authorize.</param>
        /// <returns>The card details.</returns>
        [HttpGet("{cardNumber}/balance")]
        [Authorize]
        public async Task<IActionResult> GetCardBalance(string cardNumber)
        {
            try
            {
                // Call the service to get the card balance
                var card = await _cardService.GetCardBalanceAsync(cardNumber);
                if (card == null)
                {
                    return NotFound("Card not found.");
                }

                return Ok(new CardBalanceDto
                {
                    Balance = card.Balance,
                    CreditLimit = card.CreditLimit
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Updates the details of a card, including balance, credit limit, and status.
        /// </summary>
        /// <param name="cardNumber">The encrypted number of the card to authorize.</param>
        /// <param name="updateDto">The updated card details.</param>
        /// <returns>The updated card details.</returns>
        [HttpPatch("{cardNumber}")]
        [Authorize]
        public async Task<IActionResult> UpdateCardDetails(string cardNumber, [FromBody] CardUpdateDto updateDto)
        {
            try
            {
                // Call the service to update the card details
                var card = await _cardService.UpdateCardDetailsAsync(cardNumber, updateDto.Balance, updateDto.CreditLimit, updateDto.IsActive);
                return Ok(card);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}