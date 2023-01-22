using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandNewDay.Bank.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        protected BrandNewDay.Bank.Web.API.Models.BrandNewDayContext db { get; set; }

        public TransferController(BrandNewDay.Bank.Web.API.Models.BrandNewDayContext _dbContext)
        {
            db = _dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string sourceIban, [FromForm] string toIban, [FromForm] decimal amount)
        {
            decimal newBlance;

            if (amount <= 0)
                return BadRequest("Amount must greater than zero.");

            var acc = await db.BankAccounts.SingleAsync(a => a.IbanNumber == sourceIban);

            if (acc == null)
                return NotFound();

            if (acc.Balance - amount < 0)
                return BadRequest("Insufficient funds");

            newBlance = acc.Balance;

            var isSuccess = db.TransferMoney(sourceIban, toIban, amount);

            await db.SaveChangesAsync();

            if (isSuccess)
            {
                acc = await db.BankAccounts.AsNoTracking().SingleAsync(a => a.IbanNumber == sourceIban);
                newBlance = acc.Balance;
            }

            return Content(newBlance.ToString("F3"));
        }

    }
}
