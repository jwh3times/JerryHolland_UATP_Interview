using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RapidPay.Data;
using RapidPay.DTOs;
using RapidPay.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RapidPay.Services
{
    /// <summary>
    /// Provides implementation for user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="configuration">The application configuration.</param>
        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="authDto">The user authentication details.</param>
        /// <returns>The registered user.</returns>
        public async Task<User> RegisterUserAsync(UserAuthDto authDto)
        {
            var hasher = new PasswordHasher<User>();

            // Check if the username already exists
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == authDto.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists.");
            }

            // Create a new user and hash the password
            var user = new User
            {
                Username = authDto.Username
            };
            user.PasswordHash = hasher.HashPassword(user, authDto.Password);

            // Add the user to the database and save changes
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="authDto">The user authentication details.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        public async Task<string?> AuthenticateUserAsync(UserAuthDto authDto)
        {
            var hasher = new PasswordHasher<User>();

            // Retrieve the user from the database
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == authDto.Username);
            if (user == null || user.PasswordHash == null || hasher.VerifyHashedPassword(user, user.PasswordHash, authDto.Password).Equals(PasswordVerificationResult.Failed))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            // Retrieve JWT configuration settings
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            var jwtIssuer = _configuration["Jwt:Issuer"];
            if (string.IsNullOrEmpty(jwtIssuer))
            {
                throw new InvalidOperationException("JWT issuer is not configured.");
            }

            var jwtAudience = _configuration["Jwt:Audience"];
            if (string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("JWT audience is not configured.");
            }

            var key = Encoding.UTF8.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Aud, jwtAudience),
                    new Claim(JwtRegisteredClaimNames.Iss, jwtIssuer)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create and return the JWT token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}