using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTest
{
    class DeckOfCards : Card
    {
        const int NUM_OF_CARDS = 52; // number of all cards
        private List<Card> deck = new List<Card>(); // list of all playable cards left in deck

        // create deck of 52 cards: 13 values each, with 4 suits
        public void setUpDeck()
        {
            int i = 0;
            foreach (SUIT s in Enum.GetValues(typeof(SUIT)))
            {
                foreach (VALUE v in Enum.GetValues(typeof(VALUE)))
                {
                    deck.Add(new Card { MySuit = s, MyValue = v });
                    i++;
                }
            }

            ShuffleCards();
        }

        public Card DealCard()
        {
            Card card = new Card { MySuit = deck[deck.Count() - 1].MySuit, MyValue = deck[deck.Count() - 1].MyValue };
            deck.RemoveAt(deck.Count() - 1);
            return card;
        }

        // shuffle the deck
        public void ShuffleCards()
        {
            Random rand = new Random();
            Card temp;

            // run the shuffle 1000 times
            for (int shuffleTimes = 0; shuffleTimes < 1000; shuffleTimes++)
            {
                for (int i = 0; i < NUM_OF_CARDS; i++)
                {
                    // swap the cards
                    int secondCardIndex = rand.Next(13);
                    temp = deck[i];
                    deck[i] = deck[secondCardIndex];
                    deck[secondCardIndex] = temp;
                }
            }
        }

    }
}
