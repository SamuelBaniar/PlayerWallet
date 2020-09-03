namespace PlayerWallet.Data.Internal
{
    public class SaveTransactionResponse
    {
        public bool Sucess { get; private set; }
        public string Message { get; private set; }

        public SaveTransactionResponse(bool success, string message = null)
        {
            Sucess = success;
            Message = message ?? (success ? "Accepted" : "Rejected");
        }
    }
}
