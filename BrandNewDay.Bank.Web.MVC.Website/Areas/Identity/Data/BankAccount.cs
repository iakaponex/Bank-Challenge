using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrandNewDay.Bank.Web.MVC.Website.Areas.Identity.Data
{
    [Table("BankAccount", Schema = "Bank")]
    public class BankAccount
    {
        [Key]
        [StringLength(100)]
        public string IbanNumber { get; set; }

        /// <summary>
        /// Email relate with aspnet user
        /// </summary>
        [ForeignKey("User")]
        [StringLength(450)]
        public string NetUserId { get; set; }

        public BrandNewDayBankUser User { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? CloseAccountDate { get; set; } 
    }
}
