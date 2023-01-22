using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandNewDay.Bank.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        protected BrandNewDay.Bank.Web.API.Models.BrandNewDayContext db { get; set; }
        protected const decimal feeRate = 0.1m/100;

        public DepositController(BrandNewDay.Bank.Web.API.Models.BrandNewDayContext _dbContext)
        {
            db = _dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountNo">Iban Account No.</param>
        /// <param name="amount">Amount</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string iban, [FromForm] decimal amount)
        {
            decimal newBlance;

            if (amount <= 0)
                return BadRequest("Amount must greater than zero.");

            var acc = await db.BankAccounts.SingleAsync(a=> a.IbanNumber == iban);

            if(acc == null)
                return NotFound();

            newBlance = acc.Balance;
            var fee = amount * feeRate;

            var isSuccess = db.DepositMoney(iban, amount, fee);

            if (isSuccess)
            {
                acc = await db.BankAccounts.AsNoTracking().SingleAsync(a => a.IbanNumber == iban);
                newBlance = acc.Balance;
            }

            return Content(newBlance.ToString("F3"));
        }
    }
}
