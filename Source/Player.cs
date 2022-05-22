using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTest
{
    class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // unsorted cards
        public List<Card> unsortedHand = new List<Card>();

        public int Money { get; set; }
        public int Bet { get; set; }
        public bool PlayerIngame { get; set; }
        public HandEvaluator handEvaluator = null;
    }
}

