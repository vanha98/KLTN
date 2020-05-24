using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class AppRole : IdentityRole<Guid>
    {
        public string MoTa { get; set; }
    }
}
