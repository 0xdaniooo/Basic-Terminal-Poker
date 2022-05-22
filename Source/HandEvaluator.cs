using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTest
{
    public enum Hand
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeKind,
        Straight,
        Flush,
        FullHouse,
        FourKind,
        StraightFlush,
        RoyalFlush,

        //RoyalFlush,
        //StraightFlush,
        //FourKind,
        //FullHouse,
        //Flush,
        //Straight,
        //ThreeKind,
        //TwoPair,
        //OnePair,
    }

    public struct HandValue
    {
        public int Total { get; set; }
        public int HighCard { get; set; }
        public Hand Combination { get; set; }
    }

    class HandEvaluator : Card
    {
        private int heartsSum;
        private int diamondSum;
        private int clubSum;
        private int spadesSum;
        //private Card[] cards;
        private List<Card> cards = new List<Card>();
        private HandValue handValue;

        public HandEvaluator(List<Card> sortedHand)
        {
            heartsSum = 0;
            diamondSum = 0;
            clubSum = 0;
            spadesSum = 0;

            for (int i = 0; i < sortedHand.Count; i++)
            {
                cards.Add(sortedHand[i]);
            }

            handValue = new HandValue();
        }

        public HandValue HandValues
        {
            get { return handValue; }
            set { handValue = value; }
        }

        public Hand EvaluateHand()
        {
            // get the number of each suit on hand
            getNumberOfSuit();

            // 1st best hand
            if (RoyalFlush())
            {
                handValue.Combination = Hand.RoyalFlush;
                return Hand.RoyalFlush;
            }

            // 2nd best hand
            else if (StraightFlush())
            {
                handValue.Combination = Hand.StraightFlush;
                return Hand.StraightFlush;
            }

            // 3rd best hand
            else if (FourOfKind())
            {
                handValue.Combination = Hand.FourKind;
                return Hand.FourKind;
            }

            // 4th best hand
            else if (FullHouse())
            {
                handValue.Combination = Hand.FullHouse; 
                return Hand.FullHouse;
            }

            // 5th best hand
            else if (Flush())
            {
                handValue.Combination = Hand.Flush;
                return Hand.Flush;
            }

            // 6th best hand
            else if (Straight())
            {
                handValue.Combination = Hand.Straight;
                return Hand.Straight;
            }

            // 7th best hand
            else if (ThreeOfKind())
            {
                handValue.Combination = Hand.ThreeKind;
                return Hand.ThreeKind;
            }

            // 8th best hand
            else if (TwoPairs())
            {
                handValue.Combination = Hand.TwoPair;
                return Hand.TwoPair;
            }

            // 9th best hand
            else if (OnePair())
            {
                handValue.Combination = Hand.OnePair;
                return Hand.OnePair;
            }

            // high card - 10th best hand
            handValue.HighCard = (int)cards[cards.Count() - 1].MyValue;
            handValue.Combination = Hand.HighCard;
            return Hand.HighCard;
        }

        private void getNumberOfSuit()
        {
            foreach (var element in cards)
            {
                if (element.MySuit == Card.SUIT.HEARTS)
                {
                    heartsSum++;
                }

                else if (element.MySuit == Card.SUIT.DIAMONDS)
                {
                    diamondSum++;
                }

                else if (element.MySuit == Card.SUIT.CLUBS)
                {
                    clubSum++;
                }

                else if (element.MySuit == Card.SUIT.SPADES)
                {
                    spadesSum++;
                }
            }
        }

        private bool RoyalFlush()
        {
            // if all suits are the same
            if (heartsSum >= 5 || diamondSum >= 5 || clubSum >= 5 || spadesSum >= 5)
            {
                // if 5 consecutive values in ascending order
                if (cards[0].MyValue == Card.VALUE.TEN &&
                    cards[1].MyValue == Card.VALUE.JACK &&
                    cards[2].MyValue == Card.VALUE.QUEEN &&
                    cards[3].MyValue == Card.VALUE.KING && 
                    cards[4].MyValue == Card.VALUE.ACE)
                {
                    handValue.Total += (int)cards[0].MyValue;
                    handValue.Total += (int)cards[1].MyValue;
                    handValue.Total += (int)cards[2].MyValue;
                    handValue.Total += (int)cards[3].MyValue;
                    handValue.Total += (int)cards[4].MyValue;
                    return true;
                }

                else if (cards[1].MyValue == Card.VALUE.TEN &&
                    cards[2].MyValue == Card.VALUE.JACK &&
                    cards[3].MyValue == Card.VALUE.QUEEN &&
                    cards[4].MyValue == Card.VALUE.KING &&
                    cards[5].MyValue == Card.VALUE.ACE)
                {
                    handValue.Total += (int)cards[1].MyValue;
                    handValue.Total += (int)cards[2].MyValue;
                    handValue.Total += (int)cards[3].MyValue;
                    handValue.Total += (int)cards[4].MyValue;
                    handValue.Total += (int)cards[5].MyValue;
                    return true;
                }

                else if (cards[2].MyValue == Card.VALUE.TEN &&
                    cards[3].MyValue == Card.VALUE.JACK &&
                    cards[4].MyValue == Card.VALUE.QUEEN &&
                    cards[5].MyValue == Card.VALUE.KING &&
                    cards[6].MyValue == Card.VALUE.ACE)
                {
                    handValue.Total += (int)cards[2].MyValue;
                    handValue.Total += (int)cards[3].MyValue;
                    handValue.Total += (int)cards[4].MyValue;
                    handValue.Total += (int)cards[5].MyValue;
                    handValue.Total += (int)cards[6].MyValue;
                    return true;
                }
            }

            return false;
        }

        private bool StraightFlush()
        {
            if (Straight() && Flush())
            {
                return true;
            }

            return false;
        }

        private bool FourOfKind()
        {
            try
            {
                if (cards[0].MyValue == cards[1].MyValue && cards[0].MyValue == cards[2].MyValue && cards[0].MyValue == cards[3].MyValue)
                {
                    handValue.Total = (int)cards[1].MyValue * 4;
                    return true;
                }

                else if (cards[1].MyValue == cards[2].MyValue && cards[1].MyValue == cards[3].MyValue && cards[1].MyValue == cards[4].MyValue)
                {
                    handValue.Total = (int)cards[1].MyValue * 4;
                    return true;
                }

                else if (cards[2].MyValue == cards[3].MyValue && cards[2].MyValue == cards[4].MyValue && cards[2].MyValue == cards[5].MyValue)
                {
                    handValue.Total = (int)cards[1].MyValue * 4;
                    return true;
                }

                else if (cards[3].MyValue == cards[4].MyValue && cards[3].MyValue == cards[5].MyValue && cards[3].MyValue == cards[6].MyValue)
                {
                    handValue.Total = (int)cards[1].MyValue * 4;
                    return true;
                }
            }

            catch (Exception e)
            {

            }

            return false;
        }

        private bool FullHouse()
        {
            try
            {
                if (cards[0].MyValue == cards[1].MyValue && cards[2].MyValue == cards[3].MyValue && cards[2].MyValue == cards[4].MyValue)
                {
                    handValue.Total = (int)(cards[0].MyValue) + (int)(cards[1].MyValue) + (int)(cards[2].MyValue) +
                        (int)(cards[3].MyValue) + (int)(cards[4].MyValue);
                    return true;
                }

                else if (cards[0].MyValue == cards[1].MyValue && cards[3].MyValue == cards[4].MyValue && cards[3].MyValue == cards[5].MyValue)
                {
                    handValue.Total = (int)(cards[0].MyValue) + (int)(cards[1].MyValue) + (int)(cards[2].MyValue) +
                        (int)(cards[3].MyValue) + (int)(cards[4].MyValue);
                    return true;
                }

                else if (cards[0].MyValue == cards[1].MyValue && cards[4].MyValue == cards[5].MyValue && cards[4].MyValue == cards[6].MyValue)
                {
                    handValue.Total = (int)(cards[0].MyValue) + (int)(cards[1].MyValue) + (int)(cards[2].MyValue) +
                        (int)(cards[3].MyValue) + (int)(cards[4].MyValue);
                    return true;
                }

                else if (cards[1].MyValue == cards[2].MyValue && cards[3].MyValue == cards[4].MyValue && cards[3].MyValue == cards[5].MyValue)
                {
                    handValue.Total = (int)(cards[0].MyValue) + (int)(cards[1].MyValue) + (int)(cards[2].MyValue) +
                        (int)(cards[3].MyValue) + (int)(cards[4].MyValue);
                    return true;
                }

                else if (cards[1].MyValue == cards[2].MyValue && cards[4].MyValue == cards[5].MyValue && cards[4].MyValue == cards[6].MyValue)
                {
                    handValue.Total = (int)(cards[0].MyValue) + (int)(cards[1].MyValue) + (int)(cards[2].MyValue) +
                        (int)(cards[3].MyValue) + (int)(cards[4].MyValue);
                    return true;
                }

                else if (cards[2].MyValue == cards[3].MyValue && cards[4].MyValue == cards[5].MyValue && cards[4].MyValue == cards[6].MyValue)
                {
                    handValue.Total = (int)(cards[0].MyValue) + (int)(cards[1].MyValue) + (int)(cards[2].MyValue) +
                        (int)(cards[3].MyValue) + (int)(cards[4].MyValue);
                    return true;
                }

                else if (cards[0].MyValue == cards[1].MyValue && cards[0].MyValue == cards[2].MyValue && cards[3].MyValue == cards[4].MyValue)
                {
                    handValue.Total = (int)(cards[0].MyValue) + (int)(cards[1].MyValue) + (int)(cards[2].MyValue) +
                        (int)(cards[3].MyValue) + (int)(cards[4].MyValue);
                    return true;
                }

                else if (cards[0].MyValue == cards[1].MyValue && cards[0].MyValue == cards[2].MyValue && cards[4].MyValue == cards[5].MyValue)
                {
                    handValue.Total = (int)(cards[0].MyValue) + (int)(cards[1].MyValue) + (int)(cards[2].MyValue) +
                        (int)(cards[3].MyValue) + (int)(cards[4].MyValue);
                    return true;
                }

                else if (cards[0].MyValue == cards[1].MyValue && cards[0].MyValue == cards[2].MyValue && cards[5].MyValue == cards[6].MyValue)
                {
                    handValue.Total = (int)(cards[0].MyValue) + (int)(cards[1].MyValue) + (int)(cards[2].MyValue) +
                        (int)(cards[3].MyValue) + (int)(cards[4].MyValue);
                    return true;
                }
            }

            catch (Exception e)
            {

            }

            return false;
        }

        private bool Flush()
        {
            // if all suits are the same
            if (heartsSum >= 5 || diamondSum >= 5 || clubSum >= 5 || spadesSum >= 5)
            {
                handValue.Total = (int)cards[4].MyValue;
                return true;
            }

            return false;
        }

        private bool Straight()
        {
            try
            {
                // if 5 consecutive values 
                if (cards[0].MyValue + 1 == cards[1].MyValue &&
                    cards[1].MyValue + 1 == cards[2].MyValue &&
                    cards[2].MyValue + 1 == cards[3].MyValue &&
                    cards[3].MyValue + 1 == cards[4].MyValue)
                {
                    handValue.Total = (int)cards[4].MyValue;
                    return true;
                }

                else if (cards[1].MyValue + 1 == cards[2].MyValue &&
                    cards[2].MyValue + 1 == cards[3].MyValue &&
                    cards[3].MyValue + 1 == cards[4].MyValue &&
                    cards[4].MyValue + 1 == cards[5].MyValue)
                {
                    handValue.Total = (int)cards[4].MyValue;
                    return true;
                }

                else if (cards[2].MyValue + 1 == cards[3].MyValue &&
                    cards[3].MyValue + 1 == cards[4].MyValue &&
                    cards[4].MyValue + 1 == cards[5].MyValue &&
                    cards[5].MyValue + 1 == cards[6].MyValue)
                {
                    handValue.Total = (int)cards[4].MyValue;
                    return true;
                }
            }

            catch (Exception e)
            {

            }

            return false;
        }

        private bool ThreeOfKind()
        {
            try
            {
                if ((cards[0].MyValue == cards[1].MyValue && cards[0].MyValue == cards[2].MyValue))
                {
                    handValue.Total = (int)cards[2].MyValue * 3;
                    return true;
                }

                else if (cards[1].MyValue == cards[2].MyValue && cards[1].MyValue == cards[3].MyValue)
                {
                    handValue.Total = (int)cards[2].MyValue * 3;
                    return true;
                }

                else if (cards[2].MyValue == cards[3].MyValue && cards[2].MyValue == cards[4].MyValue)
                {
                    handValue.Total = (int)cards[2].MyValue * 3;
                    return true;
                }

                else if (cards[3].MyValue == cards[4].MyValue && cards[3].MyValue == cards[5].MyValue)
                {
                    handValue.Total = (int)cards[2].MyValue * 3;
                    return true;
                }

                else if (cards[4].MyValue == cards[5].MyValue && cards[4].MyValue == cards[6].MyValue)
                {
                    handValue.Total = (int)cards[2].MyValue * 3;
                    return true;
                }
            }

            catch (Exception e)
            {

            }

            return false;
        }

        private bool TwoPairs()
        {
            try
            {
                if (cards[0].MyValue == cards[1].MyValue && cards[2].MyValue == cards[3].MyValue)
                {
                    handValue.Total = ((int)cards[0].MyValue * 2) + ((int)cards[2].MyValue * 2);
                    return true;
                }

                else if (cards[0].MyValue == cards[1].MyValue && cards[3].MyValue == cards[4].MyValue)
                {
                    handValue.Total = ((int)cards[1].MyValue * 2) + ((int)cards[3].MyValue * 2);
                    return true;
                }

                else if (cards[0].MyValue == cards[1].MyValue && cards[4].MyValue == cards[5].MyValue)
                {
                    handValue.Total = ((int)cards[2].MyValue * 2) + ((int)cards[4].MyValue * 2);
                    return true;
                }

                else if (cards[0].MyValue == cards[1].MyValue && cards[5].MyValue == cards[6].MyValue)
                {
                    handValue.Total = ((int)cards[3].MyValue * 2) + ((int)cards[5].MyValue * 2);
                    return true;
                }

                else if (cards[1].MyValue == cards[2].MyValue && cards[3].MyValue == cards[4].MyValue)
                {
                    handValue.Total = ((int)cards[3].MyValue * 2) + ((int)cards[5].MyValue * 2);
                    return true;
                }

                else if (cards[1].MyValue == cards[2].MyValue && cards[4].MyValue == cards[5].MyValue)
                {
                    handValue.Total = ((int)cards[3].MyValue * 2) + ((int)cards[5].MyValue * 2);
                    return true;
                }

                else if (cards[1].MyValue == cards[2].MyValue && cards[5].MyValue == cards[6].MyValue)
                {
                    handValue.Total = ((int)cards[3].MyValue * 2) + ((int)cards[5].MyValue * 2);
                    return true;
                }

                else if (cards[2].MyValue == cards[3].MyValue && cards[4].MyValue == cards[5].MyValue)
                {
                    handValue.Total = ((int)cards[3].MyValue * 2) + ((int)cards[5].MyValue * 2);
                    return true;
                }

                else if (cards[2].MyValue == cards[3].MyValue && cards[5].MyValue == cards[6].MyValue)
                {
                    handValue.Total = ((int)cards[3].MyValue * 2) + ((int)cards[5].MyValue * 2);
                    return true;
                }

                else if (cards[3].MyValue == cards[4].MyValue && cards[5].MyValue == cards[6].MyValue)
                {
                    handValue.Total = ((int)cards[3].MyValue * 2) + ((int)cards[5].MyValue * 2);
                    return true;
                }
            }

            catch (Exception e)
            {

            }

            return false;
        }

        private bool OnePair()
        {
            try
            {
                if (cards[0].MyValue == cards[1].MyValue)
                {
                    handValue.Total = (int)cards[0].MyValue * 2;
                    return true;
                }

                else if (cards[1].MyValue == cards[2].MyValue)
                {
                    handValue.Total = (int)cards[1].MyValue * 2;
                    return true;
                }

                else if (cards[2].MyValue == cards[3].MyValue)
                {
                    handValue.Total = (int)cards[2].MyValue * 2;
                    return true;
                }

                else if (cards[3].MyValue == cards[4].MyValue)
                {
                    handValue.Total = (int)cards[3].MyValue * 2;
                    return true;
                }

                else if (cards[4].MyValue == cards[5].MyValue)
                {
                    handValue.Total = (int)cards[4].MyValue * 2;
                    return true;
                }

                else if (cards[5].MyValue == cards[6].MyValue)
                {
                    handValue.Total = (int)cards[5].MyValue * 2;
                    return true;
                }
            }

            catch (Exception e)
            {

            }

            return false;
        }
    }
}
