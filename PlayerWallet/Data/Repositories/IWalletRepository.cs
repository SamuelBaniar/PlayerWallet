using PlayerWallet.Data.Models;
using System;
using System.Threading.Tasks;

namespace PlayerWallet.Data.Repositories
{
    public interface IWalletRepository
    {
        Task AddAsync(Guid newGuid);

        Task<Wallet> GetByGuidAsync(Guid guid);

        Task<Wallet> UpdateAsync(Wallet wallet);
    }
}