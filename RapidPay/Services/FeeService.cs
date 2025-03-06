using Microsoft.EntityFrameworkCore;
using RapidPay.Data;
using RapidPay.Models;

namespace RapidPay.Services
{
    /// <summary>
    /// Provides implementation for fee-related operations.
    /// </summary>
    public class FeeService : IFeeService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public FeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the current fee for transactions.
        /// </summary>
        /// <returns>The current transaction fee.</returns>
        public async Task<decimal> GetCurrentFeeAsync()
        {
            // Get the latest fee from the fee history
            var latestFee = await _context.FeeHistories.OrderByDescending(f => f.FeeDate).FirstOrDefaultAsync();
            return latestFee?.Fee ?? 0.00m;
        }

        /// <summary>
        /// Updates the transaction fee based on a random multiplier.
        /// </summary>
        public async Task<decimal> UpdateFeeAsync()
        {
            var random = new Random();
            var newFeeMultiplier = (decimal)(random.NextDouble() * 2);
            var latestFee = await GetCurrentFeeAsync();
            var newFee = latestFee == 0.00m ? newFeeMultiplier : Math.Round(latestFee * newFeeMultiplier, 2);

            // Prevent fee from being set to zero as the formula
            // 0 * newFeeMultiplier will always be zero meaning
            // the fee will appear to never change.
            if (newFee <= 0.01m)
            {
                newFee = 0.01m; // Minimum fee
            }

            // Add the new fee to the fee history
            var feeHistory = new FeeHistory
            {
                Fee = newFee,
                FeeDate = DateTime.UtcNow
            };

            _context.FeeHistories.Add(feeHistory);
            await _context.SaveChangesAsync();
            return newFee;
        }

        /// <summary>
        /// Initializes the transaction fee to a starter value of 0.25m.
        /// </summary>
        public async Task<decimal> InitializeFeeAsync()
        {
            var random = new Random();
            var initialFee = (decimal)(random.NextDouble() * 2);

            // Add the initial fee to the fee history
            var initialFeeHistory = new FeeHistory
            {
                Fee = initialFee,
                FeeDate = DateTime.UtcNow
            };

            _context.FeeHistories.Add(initialFeeHistory);
            await _context.SaveChangesAsync();
            return initialFeeHistory.Fee;
        }
    }
}