using Microsoft.AspNetCore.Mvc;
using RapidPay.DTOs;
using RapidPay.Services;

namespace RapidPay.Controllers
{
    /// <summary>
    /// Controller for handling user-related API endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="authDto">The user authentication details.</param>
        /// <returns>Ok if the user is registered successfully; otherwise, BadRequest.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserAuthDto authDto)
        {
            try
            {
                // Call the user service to register the user
                var user = await _userService.RegisterUserAsync(authDto);
                if (user == null)
                {
                    // Return BadRequest if registration fails
                    return BadRequest("User registration failed.");
                }

                // Return Ok if registration is successful
                return Ok("User registered successfully.");
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
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="authDto">The user authentication details.</param>
        /// <returns>Ok with the JWT token if authentication is successful; otherwise, Unauthorized.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserAuthDto authDto)
        {
            try
            {
                // Call the user service to authenticate the user
                var token = await _userService.AuthenticateUserAsync(authDto);
                if (token == null)
                {
                    // Return Unauthorized if authentication fails
                    return Unauthorized("Invalid username or password.");
                }
    
                // Return Ok with the JWT token if authentication is successful
                return Ok(new { Token = token });
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