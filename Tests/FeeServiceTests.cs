using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RapidPay.Data;
using RapidPay.Models;
using RapidPay.Services;
using Xunit;

namespace Tests
{
    public class FeeServiceTests
    {
        private readonly FeeService _feeService;
        private readonly ApplicationDbContext _context;

        public FeeServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _feeService = new FeeService(_context);
        }

        [Fact]
        public async Task GetCurrentFeeAsync_ShouldReturnCurrentFee()
        {
            // Arrange
            var feeHistory = new FeeHistory { Fee = 1.5m, FeeDate = DateTime.UtcNow };
            _context.FeeHistories.Add(feeHistory);
            await _context.SaveChangesAsync();

            // Act
            var currentFee = await _feeService.GetCurrentFeeAsync();

            // Assert
            Assert.Equal(1.5m, currentFee);
        }

        [Fact]
        public async Task UpdateFeeAsync_ShouldUpdateFee()
        {
            // Arrange
            var feeHistory = new FeeHistory { Fee = 1.0m, FeeDate = DateTime.UtcNow.AddHours(-1) };
            _context.FeeHistories.Add(feeHistory);
            await _context.SaveChangesAsync();

            // Act
            await _feeService.UpdateFeeAsync();

            // Assert
            var latestFee = _context.FeeHistories.OrderByDescending(f => f.FeeDate).FirstOrDefault();
            Assert.NotNull(latestFee);
            Assert.True(latestFee.Fee >= 0 && latestFee.Fee <= 2);
        }
    }
}