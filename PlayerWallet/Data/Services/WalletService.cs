using PlayerWallet.Data.Models;
using PlayerWallet.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace PlayerWallet.Data.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
        }

        public async Task AddAsync(Guid newGuid)
        {
            if (await _walletRepository.GetByGuidAsync(newGuid) != null)
            {
                throw new ArgumentException("Provided Guid already exists.");                
            }

            await _walletRepository.AddAsync(newGuid);
        }

        public async Task<Wallet> GetByGuidAsync(Guid guid)
        {
            return await _walletRepository.GetByGuidAsync(guid);
        }

        public async Task<decimal> GetBalanceAsync(Guid guid)
        {
            var wallet = await _walletRepository.GetByGuidAsync(guid) ?? throw new ArgumentException("Wallet Not Found");
            return wallet.Balance;
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            await _walletRepository.UpdateAsync(wallet);
        }
    }
}
