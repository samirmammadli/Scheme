using Scheme.Entities;
using Scheme.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scheme.Services.TokenService
{
    public interface ITokenAuthProvider
    {
        Task<ClaimsIdentity> GetIdentityAsync(User user, Role role = null);
        Task<AccessTokenResult> GetTokenAsync(User user, Role role = null);
    }
}
