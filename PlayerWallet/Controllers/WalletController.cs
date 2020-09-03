using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlayerWallet.Data.Services;

namespace PlayerWallet.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
        }

        [HttpGet]
        public async Task<IActionResult> GetBalanceAsync(Guid guid)
        {
            decimal balance;
            try
            {
                balance = await _walletService.GetBalanceAsync(guid);
            }
            catch(ArgumentException)
            {
                return BadRequest("Wallet does not exist.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(balance);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Guid guid)
        {
            try
            {
                await _walletService.AddAsync(guid);
            }
            catch(ArgumentException)
            {
                return BadRequest("Wallet already exists.");
            }
            
            return Ok();
        }
    }
}
