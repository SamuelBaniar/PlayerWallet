using Microsoft.EntityFrameworkCore;
using PlayerWallet.Data.Contexts;
using PlayerWallet.Data.Models;
using System;
using System.Threading.Tasks;

namespace PlayerWallet.Data.Repositories
{
    public class WalletRepository : RepositoryBase, IWalletRepository
    {
        public WalletRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Guid newGuid)
        {
            var newWallet = new Wallet {Guid = newGuid, Balance = 0 };
            
            await Context.Wallets.AddAsync(newWallet);
            await Context.SaveChangesAsync();
        }

        public async Task<Wallet> GetByGuidAsync(Guid guid)
        {
            return await Context.Wallets.FirstOrDefaultAsync(w => w.Guid == guid);
        }

        public async Task<Wallet> UpdateAsync(Wallet wallet)
        {
            if (wallet is null)
            {
                throw new ArgumentNullException(nameof(wallet));
            }

            Context.Wallets.Update(wallet);
            await Context.SaveChangesAsync();

            return wallet;
        }
    }
}
