using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
    
namespace Scheme.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public Column Column { get; set; }
        public Card()
        {
            
        }
    }
}
