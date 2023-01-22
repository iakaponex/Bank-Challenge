using System;
using System.Collections.Generic;

namespace BrandNewDay.Bank.Web.API.Models
{
    public partial class BankAccount
    {
        public string IbanNumber { get; set; } = null!;
        public string NetUserId { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CloseAccountDate { get; set; }

        public virtual AspNetUser NetUser { get; set; } = null!;
    }
}
