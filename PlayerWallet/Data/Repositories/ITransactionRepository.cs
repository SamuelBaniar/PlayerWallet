using PlayerWallet.Data.Models;
using System;
using System.Threading.Tasks;

namespace PlayerWallet.Data.Repositories
{
    public interface ITransactionRepository
    {
        public Task<WalletTransaction> AddAsync(WalletTransaction transaction);

        public Task<WalletTransaction> GetByGuidAsync(Guid guid);
     }
}