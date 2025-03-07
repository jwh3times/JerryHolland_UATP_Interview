using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Services;

namespace RapidPay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeesController : ControllerBase
    {
        private readonly IFeeService _feeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeesController"/> class.
        /// </summary>
        /// <param name="feeService">The card service.</param>
        public FeesController(IFeeService feeService)
        {
            _feeService = feeService;
        }

        /// <summary>
        /// Gets the current fee or initializes the fee if not fee history is found.
        /// </summary>
        /// <returns>The current fee.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentFee()
        {
            try
            {
                var currentFee = await _feeService.GetCurrentFeeAsync();
                return Ok(currentFee);
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