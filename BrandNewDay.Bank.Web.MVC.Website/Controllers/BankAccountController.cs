using BrandNewDay.Bank.Web.MVC.Website.Areas.Identity.Data;
using BrandNewDay.Bank.Web.MVC.Website.Data;
using BrandNewDay.Bank.Web.MVC.Website.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrandNewDay.Bank.Web.MVC.Website.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly UserManager<BrandNewDayBankUser> _userManager;
        private readonly BankService _bankService;
        protected IdentityDataContext db { get; set; }

        public BankAccountController(IdentityDataContext _db, 
                                     UserManager<BrandNewDayBankUser> userManager,
                                     BankService BankService
                                    )
        {
            this.db = _db;
            _userManager = userManager;
            _bankService = BankService;
        }

        public async Task<IActionResult> My()
        {
            var user = await _userManager.GetUserAsync(this.User);

            var bankAccount = db.BankAccounts.Where(b => b.NetUserId == user.Id).FirstOrDefault();

            ViewData["bankAccount"] = bankAccount;

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(decimal amount)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var bankAccount = db.BankAccounts.Where(b => b.NetUserId == user.Id).FirstOrDefault();

                var updateBalance = await _bankService.Deposit(bankAccount.IbanNumber, amount);

                return Json(new { result = true, updateBalance = updateBalance.ToString("N3") });
            }
            catch(Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(string toAccount, decimal amount)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var bankAccount = db.BankAccounts.Where(b => b.NetUserId == user.Id).FirstOrDefault();

                var updateBalance = await _bankService.Transfer(bankAccount.IbanNumber, toAccount, amount);

                return Json(new { result = true, updateBalance = updateBalance.ToString("N3") });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }
    }
}
