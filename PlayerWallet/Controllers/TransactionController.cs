using Microsoft.AspNetCore.Mvc;
using PlayerWallet.Data.Internal;
using PlayerWallet.Data.Models;
using PlayerWallet.Data.Services;
using System;
using System.Threading.Tasks;

namespace PlayerWallet.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] WalletTransaction walletTransaction)
        {
            try
            {
                var saveResponse = await _transactionService.AddAsync(walletTransaction);
                if (saveResponse.Sucess)
                {
                    return Ok(saveResponse);
                }
                else
                {
                    return BadRequest(saveResponse);
                }
            }
            catch (Exception)
            {
                return BadRequest("Rejected");
            }
        }
    }
}
