namespace PokerTest
{
    class Program
    {
        public static List<Player> players = new List<Player>();
        public static List<Card> communityCards = new List<Card>();
        public static int minimumBet;

        public static void Main(string[] args)
        {
            int playerCount;
            Console.WriteLine("How many players?: ");
            playerCount = Int16.Parse(Console.ReadLine());

            int moneyPerPlayer;
            Console.WriteLine("How much money per player?: ");
            moneyPerPlayer = Int32.Parse(Console.ReadLine());

            DeckOfCards deck = new DeckOfCards();
            deck.setUpDeck();

            // Initialize players
            for (int i = 0; i < playerCount; i++)
            {
                Player player = new Player();
                players.Add(player);
                players[i].unsortedHand.Add(deck.DealCard());
                players[i].unsortedHand.Add(deck.DealCard());
                players[i].Money = moneyPerPlayer;
                players[i].PlayerIngame = true;

                players[i].handEvaluator = new HandEvaluator(players[i].unsortedHand);
                players[i].Name = (i + 1).ToString();
            }

            while (players.Count > 1)
            {
                minimumBet = players[0].Money / 50;

                communityCards = new List<Card>();

                for (int c = 0; c < players.Count(); c++)
                {
                    players[c].Bet = 0;
                }

                int pot = 0;

                deck = new DeckOfCards();
                deck.setUpDeck();

                // -- GAMEPLAY --

                // ---- FLOP ----
                pot += Round(ref players);

                Console.WriteLine();
                Console.WriteLine("The pot is: " + pot);
                Console.WriteLine();

                communityCards.Add(deck.DealCard());
                communityCards.Add(deck.DealCard());
                communityCards.Add(deck.DealCard());

                Console.WriteLine("Community cards:");

                for (int i = 0; i < communityCards.Count(); i++)
                {
                    Console.WriteLine("Card " + (i + 1) + " : " + communityCards[i].MyValue + " " + communityCards[i].MySuit);
                }
                Console.WriteLine();
                // ---- FLOP ----

                // ---- TURN ----
                pot += Round(ref players);

                Console.WriteLine();
                Console.WriteLine("The pot is: " + pot);
                Console.WriteLine();

                communityCards.Add(deck.DealCard());

                Console.WriteLine("Community cards:");

                for (int i = 0; i < communityCards.Count(); i++)
                {
                    Console.WriteLine("Card " + (i + 1) + " : " + communityCards[i].MyValue + " " + communityCards[i].MySuit);
                }
                Console.WriteLine();
                // ---- TURN ----

                // ---- RIVER ----
                pot += Round(ref players);

                Console.WriteLine();
                Console.WriteLine("The pot is: " + pot);
                Console.WriteLine();

                communityCards.Add(deck.DealCard());

                Console.WriteLine("Community cards:");

                for (int i = 0; i < communityCards.Count(); i++)
                {
                    Console.WriteLine("Card " + (i + 1) + " : " + communityCards[i].MyValue + " " + communityCards[i].MySuit);
                }
                Console.WriteLine();

                // ---- RIVER ----

                // ---- FINAL ----
                pot += Round(ref players);
                // ---- FINAL ----

                // Assign players with their best combinations
                for (int i = 0; i < players.Count(); i++)
                {
                    Console.WriteLine("Player " + (i + 1) + "'s total cards:");
                    List<Card> c = CardSort(players[i].unsortedHand, communityCards);

                    for (int a = 0; a < c.Count(); a++)
                    {
                        Console.WriteLine("Card " + (a + 1) + " : " + c[a].MyValue + " " + c[a].MySuit);
                    }

                    players[i].handEvaluator = new HandEvaluator(CardSort(players[i].unsortedHand, communityCards));
                    Console.WriteLine("Player " + (i + 1) + "'s best combination: " + players[i].handEvaluator.EvaluateHand());

                    Console.WriteLine();
                }

                // Check who got the highest hand combination using selection sort
                int smallest;
                Player temp;
                for (int e = 0; e < players.Count(); e++)
                {
                    smallest = e;
                    for (int j = e + 1; j < players.Count(); j++)
                    {
                        if (players[j].handEvaluator.HandValues.Combination < players[smallest].handEvaluator.HandValues.Combination)
                        {
                            smallest = j;
                        }
                    }

                    temp = players[smallest];
                    players[smallest] = players[e];
                    players[e] = temp;
                }

                // Announce winner of round
                Console.WriteLine("Player " + players[players.Count() - 1].Name + " wins!!! - " + players[players.Count() - 1].handEvaluator.HandValues.Combination);
                players[players.Count() - 1].Money += pot;
                Console.WriteLine();

                // Kick out players who've lost all money
                for (int m = 0; m < players.Count(); m++)
                { 
                    if (players[m].Money <= 0)
                    {
                        Console.WriteLine(players[m].Name + " has been kicked out. Reason - out of money.");
                        players.RemoveAt(m);
                    }
                }

                if (players.Count == 1)
                {
                    Console.WriteLine(players[0].Name + " IS THE ULTIMATE WINNER!!!!!!!");
                    Console.WriteLine("TOTAL WINNINGS: " + players[0].Money);
                    break;
                }
            }
        }

        static public int Round(ref List<Player> players)
        {
            bool roundComplete = false;

            while (!roundComplete)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    bool playersReady = true;
                    for (int a = 0; a < players.Count(); a++)
                    {
                        if (players[a].Money == 0) continue;

                        if (players[a].Bet < minimumBet)
                        {
                            playersReady = false;
                            break;
                        }
                    }

                    if (playersReady)
                    {
                        roundComplete = true;
                        break;
                    }

                    if (players[i].PlayerIngame)
                    {
                        Console.WriteLine("The minimum bet is: " + minimumBet);
                        Console.WriteLine("You have bet " + players[i].Bet + " this round");

                        Console.WriteLine("Player " + (i + 1) + "'s turn");
                        Console.WriteLine("Money left: " + players[i].Money);

                        // Display cards in hand
                        PrintPlayerCards(players[i].unsortedHand);
                        players[i].handEvaluator = new HandEvaluator(CardSort(players[i].unsortedHand, communityCards));
                        Console.WriteLine("Your best combination as of now: " + players[i].handEvaluator.EvaluateHand());

                        char choice = ' ';
                        if (players[i].Money < minimumBet) Console.WriteLine("A - All in");
                        else if (players[i].Bet < minimumBet) Console.WriteLine("C - Call for " + (minimumBet - players[i].Bet));
                        else Console.WriteLine("C - Check");
                        if (players[i].Money > minimumBet) Console.WriteLine("A - All in");
                        Console.WriteLine("R - Raise");
                        Console.WriteLine("F - Fold");
                        choice = Convert.ToChar(Console.ReadLine().ToUpper());

                        if (choice.Equals('C') && players[i].Bet < minimumBet)
                        {
                            int callAmount;
                            callAmount = minimumBet - players[i].Bet;
                            Console.WriteLine("You have called for " + callAmount);

                            players[i].Bet += callAmount;
                            players[i].Money -= callAmount;
                        }

                        else if (choice.Equals('C') && players[i].Bet == minimumBet)
                        {
                            Console.WriteLine("Checking...");
                        }

                        else if (choice.Equals('A'))
                        {
                            int allInMoney = 0;
                            allInMoney = players[i].Money;
                            players[i].Money = 0;
                            players[i].Bet += allInMoney;

                            Console.WriteLine("You go all in for " + allInMoney);

                            if (allInMoney > minimumBet) minimumBet += allInMoney;
                        }

                        else if (choice.Equals('R'))
                        {
                            Console.Write("How much do you want to raise by? ");
                            int raiseAmount;
                            raiseAmount = Convert.ToInt32(Console.ReadLine());

                            if (raiseAmount < players[i].Money)
                            {
                                players[i].Bet += raiseAmount;
                                players[i].Money -= raiseAmount;

                                players[i].Bet += minimumBet;
                                players[i].Money -= minimumBet;

                                minimumBet += raiseAmount;

                                Console.WriteLine("You have raised by " + raiseAmount);
                            }
                        }

                        else if (choice.Equals('F'))
                        {
                            Console.WriteLine("Folding...");
                            players[i].PlayerIngame = false;
                        }

                        Console.WriteLine();
                    }
                }
            }

            // collect and return the pot
            int collectedAmount = 0;
            for (int i = 0; i < players.Count; i++)
            {
                collectedAmount += players[i].Bet;
                players[i].Bet = 0;
            }

            return collectedAmount;
        }

        static public void PrintPlayerCards(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Console.WriteLine("Card " + (i + 1) + " : " + cards[i].MyValue + " " + cards[i].MySuit);
            }
        }

        static public List<Card> CardSort(List<Card> playerCards, List<Card> communityCards)
        {
            List<Card> totalCards = new List<Card>();
            totalCards.Add(playerCards[0]);
            totalCards.Add(playerCards[1]);

            try
            {
                totalCards.Add(communityCards[0]);
                totalCards.Add(communityCards[1]);
                totalCards.Add(communityCards[2]);
                totalCards.Add(communityCards[3]);
                totalCards.Add(communityCards[4]);
            }

            catch (Exception)
            {

            }

            var queryPlayer = from hand in totalCards
                              orderby hand.MyValue
                              select hand;

            List<Card> sortedCards = new List<Card>();

            foreach (var card in queryPlayer)
            {
                sortedCards.Add(card);
            }

            return sortedCards;
        }
    }
}