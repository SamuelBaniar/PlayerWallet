using PlayerWallet.Data.Internal;
using PlayerWallet.Data.Models;
using System.Threading.Tasks;

namespace PlayerWallet.Data.Services
{
    public interface ITransactionService
    {
        Task<SaveTransactionResponse> AddAsync(WalletTransaction walletTransaction);
    }
}