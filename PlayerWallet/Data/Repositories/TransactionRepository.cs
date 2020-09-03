using Microsoft.EntityFrameworkCore;
using PlayerWallet.Data.Contexts;
using PlayerWallet.Data.Models;
using System;
using System.Threading.Tasks;

namespace PlayerWallet.Data.Repositories
{
    public class TransactionRepository : RepositoryBase, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<WalletTransaction> AddAsync(WalletTransaction transaction)
        {
            if (transaction is null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            await Context.Transactions.AddAsync(transaction);
            await Context.SaveChangesAsync();

            return transaction;
        }

        public async Task<WalletTransaction> GetByGuidAsync(Guid guid)
        {
            return await Context.Transactions.FirstOrDefaultAsync(t => t.Guid == guid);
        }
    }
}
