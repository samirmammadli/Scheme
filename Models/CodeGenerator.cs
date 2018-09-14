using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Models
{
    public class CodeGenerator
    {
        public int Code { get; set; }
        private static Random _rnd = new Random();
        public DateTime ExpireDate { get; set; }

        public void GenerateCode(int hours)
        {
            Code = _rnd.Next(100000, 999999);
            ExpireDate = DateTime.UtcNow.AddHours(hours);
        }
    }
}
