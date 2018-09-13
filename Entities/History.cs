using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Entities
{
    public class History
    {
        public int Id { get; set; }
        public bool IsSended { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public Card Card { get; set; }

        public override string ToString()
        {
            string cardName;
            if (Card.Text.Length > 50)
                cardName = Card.Text.Substring(0, 50) + "...";
            else
                cardName = Card.Text;
            return $"{Date} user: {User.Name} card {cardName}";
        }
    }
}

