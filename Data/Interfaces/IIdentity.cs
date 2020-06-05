using System.Security.Claims;

namespace Data.Interfaces
{
    public interface IIdentity
    {
       long Get(ClaimsPrincipal user); 
    }
}
