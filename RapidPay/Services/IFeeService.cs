using RapidPay.DTOs;

namespace RapidPay.Services
{
    /// <summary>
    /// Defines the contract for fee-related operations.
    /// </summary>
    public interface IFeeService
    {
        Task<CurrentFeeDto> GetCurrentFeeAsync();
        Task<decimal> UpdateFeeAsync();
        Task<decimal> InitializeFeeAsync();
    }
}