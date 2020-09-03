using PlayerWallet.Data.Contexts;

namespace PlayerWallet.Data.Repositories
{
    public class RepositoryBase
    {
        protected AppDbContext Context { get; }

        public RepositoryBase(AppDbContext context)
        {
            Context = context;
        }
    }
}
