using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Helper.JwtConfigure
{
    public interface IJwtFactory
    {
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id, string role);
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
    }
}
