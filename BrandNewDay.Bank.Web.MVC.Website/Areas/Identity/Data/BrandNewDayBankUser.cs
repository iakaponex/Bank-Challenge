using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BrandNewDay.Bank.Web.MVC.Website.Areas.Identity.Data;

// Add profile data for application users by adding properties to the BrandNewDayBankUser class
public class BrandNewDayBankUser : IdentityUser
{
    [StringLength(100)]
    public string CountryCode { get; set; }
}

