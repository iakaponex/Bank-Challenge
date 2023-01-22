using System;
using System.Collections.Generic;

namespace BrandNewDay.Bank.Web.API.Models
{
    public partial class AspNetUserToken
    {
        public string UserId { get; set; } = null!;
        public string LoginProvider { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Value { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
    }
}
