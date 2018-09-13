using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Scheme.Services
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "http://localhost:53117/"; // потребитель токена
        const string KEY = "hpiuy07YP&y&To87yaduiyenpieypYo7nn9O";   // ключ для шифрации
        public const int LIFETIME = 2; // время жизни токена - 2 Дня
        public const int SaltSize = 30;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
