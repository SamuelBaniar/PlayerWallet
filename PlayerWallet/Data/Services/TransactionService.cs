using PlayerWallet.Data.Internal;
using PlayerWallet.Data.Models;
using PlayerWallet.Data.Repositories;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace PlayerWallet.Data.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletService _walletService;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IWalletService walletService)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
        }

        public async Task<SaveTransactionResponse> AddAsync(WalletTransaction walletTransaction)
        {
            if (walletTransaction is null)
            {
                throw new ArgumentNullException(nameof(walletTransaction));
            }

            var wallet = await _walletService.GetByGuidAsync(walletTransaction.WalletGuid);
            if (wallet is null)
            {
                return new SaveTransactionResponse(false, "Transaction for invalid Wallet");
            }

            var transaction = await GetByGuidAsync(walletTransaction.Guid);
            if (transaction != null)
            {
                return new SaveTransactionResponse(true, "Transaction was already processed.");
            }

            int amountModifier;
            try
            {
                amountModifier = TransactionType.GetAmountModifierByTransactionType(walletTransaction.TransactionType);
            }
            catch (ArgumentException ex)
            {
                return new SaveTransactionResponse(false, ex.Message);
            }

            wallet.Balance += walletTransaction.Amount * amountModifier;
            if (wallet.Balance < 0)
            {
                return new SaveTransactionResponse(false, "Not enough gold.");
            }

            using (var transactionScope = new TransactionScope())
            {
                await _transactionRepository.AddAsync(walletTransaction);
                await _walletService.UpdateAsync(wallet);

                transactionScope.Complete();
            }

            return new SaveTransactionResponse(true);
        }

        private async Task<WalletTransaction> GetByGuidAsync(Guid guid)
        {
            return await _transactionRepository.GetByGuidAsync(guid);
        }
    }
}
