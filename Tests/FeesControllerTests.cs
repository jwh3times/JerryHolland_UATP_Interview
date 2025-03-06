using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RapidPay.Controllers;
using RapidPay.Services;
using Xunit;

namespace Tests
{
    public class FeesControllerTests
    {
        private readonly Mock<IFeeService> _feeServiceMock;
        private readonly FeesController _feesController;

        public FeesControllerTests()
        {
            _feeServiceMock = new Mock<IFeeService>();
            _feesController = new FeesController(_feeServiceMock.Object);
        }

        [Fact]
        public async Task GetCurrentFee_ShouldReturnCurrentFee()
        {
            // Arrange
            _feeServiceMock.Setup(s => s.GetCurrentFeeAsync()).ReturnsAsync(1.5m);

            // Act
            var result = await _feesController.GetCurrentFee() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(1.5m, result.Value);
        }

        [Fact]
        public async Task UpdateFee_ShouldReturnOk()
        {
            // Act
            var result = await _feesController.UpdateFee() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Fee updated successfully.", result.Value);
        }
    }
}