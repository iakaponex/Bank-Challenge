using BrandNewDay.Bank.Web.API.Models;
using IbanNet;
using IbanNet.Registry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandNewDay.Bank.Web.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        protected BrandNewDay.Bank.Web.API.Models.BrandNewDayContext db { get; set; }

        public BankAccountController(BrandNewDay.Bank.Web.API.Models.BrandNewDayContext _dbContext)
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
        public async Task<IActionResult> Post([FromForm] string id)
        {

            var user = await db.AspNetUsers.SingleAsync(a => a.Id == id);

            if (user == null)
                return NotFound("User not found.");

            var bankAccounts = await db.BankAccounts.Where(a => a.NetUserId == id).ToListAsync();

            if (bankAccounts.Count > 0)
                return BadRequest("User already have a bank account.");

            var newIban = GetNewIban(user.CountryCode);

            if(string.IsNullOrEmpty(newIban))
                return BadRequest("unable to generate iban.");

            var defaultBankAccount = new BankAccount()
            {
                NetUserId = id,
                IbanNumber = newIban,
                Balance = 0,
                CreatedDate = DateTime.Now
            };

            db.BankAccounts.Add(defaultBankAccount);

            var roweffect = await db.SaveChangesAsync();

            if(roweffect > 0)
                return Content("OK");
            else
                return BadRequest("Unable to create default bank account.");
        }

        private string GetNewIban(string countryCode)
        {
            Iban iban;
            string result;

            var generator = new IbanGenerator();

            try
            {
                iban = generator.Generate(countryCode);
                result = iban.ToString();
            }
            catch (ArgumentException)
            {
                return null;
            }

            return result;
        }
    }
}
