using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Models
{
    public class AccessTokenResult
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }

        public AccessTokenResult(string username, string token, DateTime expireDate)
        {
            Username = username;
            Token = token;
            ExpireDate = expireDate;
        }
    }
}
