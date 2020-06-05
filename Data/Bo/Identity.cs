using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Data.Bo
{
    public class Identity : IIdentity
    {
        public long Get(ClaimsPrincipal user)
        {
            long Id = long.Parse(user.Identity.Name);
            return Id;
            
        }
    }
}
