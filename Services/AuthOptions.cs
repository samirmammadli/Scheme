using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Scheme.Services
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";

        public const string AUDIENCE = "http://localhost:53117/"; 

        const string KEY = "hpiuy07YP&y&To87yaduiyenpieypYo7nn9O";   

        public const int LIFETIME = 2; 

        public const int SaltSize = 30;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
