using System.Threading.Tasks;

namespace RapidPay.Services
{
    /// <summary>
    /// Defines the contract for fee-related operations.
    /// </summary>
    public interface IFeeService
    {
        Task<decimal> GetCurrentFeeAsync();
        Task<decimal> UpdateFeeAsync();
        Task<decimal> InitializeFeeAsync();
    }
}