using RapidPay.DTOs;
using RapidPay.Models;

namespace RapidPay.Services
{
    /// <summary>
    /// Defines the contract for user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="authDto">The user authentication details.</param>
        /// <returns>The registered user.</returns>
        Task<User> RegisterUserAsync(UserAuthDto authDto);

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="authDto">The user authentication details.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        Task<string?> AuthenticateUserAsync(UserAuthDto authDto);
    }
}