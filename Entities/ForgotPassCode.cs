using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Entities
{
    public class ForgotPassCode
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int Code { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
