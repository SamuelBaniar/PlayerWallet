using System;

namespace PlayerWallet.Data.Internal
{
    public static class TransactionType
    {
        
        public const string Win = "win";

        public const string Deposit = "deposit";

        public const string Stake = "stake";

        public static int GetAmountModifierByTransactionType(string transactionType)
        {
            switch (transactionType)
            {
                case Win:
                case Deposit:
                    return 1;
                case Stake:
                    return -1;
                default:
                    throw new ArgumentException("Invalid Transaction Type.");
            }
                
        }
    }
}
