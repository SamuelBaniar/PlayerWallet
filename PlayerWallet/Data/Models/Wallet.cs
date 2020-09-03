using System;
using System.Collections.Generic;

namespace PlayerWallet.Data.Models
{
    public class Wallet
    {
        public Guid Guid { get; set; } 

        public decimal Balance { get; set; }

        public virtual ICollection<WalletTransaction> Transactions { get; set; }
    }
}
