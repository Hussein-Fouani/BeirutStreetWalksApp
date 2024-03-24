using Microsoft.AspNetCore.Identity;

namespace BeirutWalksWebApi.Repository.IRepository
{
    public interface ITokenRepository
    {
       string CreateToken(IdentityUser user, List<string> roles); 
    }
}
