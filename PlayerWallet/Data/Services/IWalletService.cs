using PlayerWallet.Data.Models;
using System;
using System.Threading.Tasks;

namespace PlayerWallet.Data.Services
{
    public interface IWalletService
    {
        Task AddAsync(Guid guid);
        
        Task<Wallet> GetByGuidAsync(Guid walletGuid);
        
        Task<decimal> GetBalanceAsync(Guid guid);

        Task UpdateAsync(Wallet wallet);
    }
}