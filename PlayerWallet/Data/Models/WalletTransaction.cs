using System;

namespace PlayerWallet.Data.Models
{
    public class WalletTransaction
    {
        public Guid Guid { get; set; }

        public string TransactionType { get; set; }

        public decimal Amount { get; set; }

        public Guid WalletGuid { get; set; }

        public virtual Wallet Wallet { get; set; }
    }
}
