using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace holdem_odds
{
    class Hand
    {
        public enum Type
        {
            None,
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush,
            RoyalFlush
        }

        public Type type { get; private set; }
        public List<Card> cards { get; private set; }

        public string GetHumanReadable()
        {
            return cards[0].GetHumanReadable() + " " + 
                   cards[1].GetHumanReadable() + " " + 
                   cards[2].GetHumanReadable() + " " + 
                   cards[3].GetHumanReadable() + " " + 
                   cards[4].GetHumanReadable();
        }

        public static Hand FindBest(List<Card> holeCards, List<Card> communityCards)
        {
            Hand bestHand = new Hand();

            List<Card> allCards = new List<Card>();
            allCards.AddRange(holeCards);
            allCards.AddRange(communityCards);

            if (bestHand.type == Type.None)
            {
                // Check for a royal flush
                var royalFlush = GetRoyalFlush(allCards.ToList());
                if (royalFlush != null)
                {
                    bestHand.SetCards(royalFlush, Type.RoyalFlush);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for a straight flush
                var straightFlush = GetStraightFlush(allCards.ToList());
                if (straightFlush != null)
                {
                    bestHand.SetCards(straightFlush, Type.StraightFlush);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for four of a kind
                var quads = GetSeries(allCards.ToList(), 4);
                if (quads != null)
                {
                    bestHand.SetCards(quads, Type.FourOfAKind);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for a full house
                var fullHouse = GetFullHouse(allCards.ToList());
                if (fullHouse != null)
                {
                    bestHand.SetCards(fullHouse, Type.FullHouse);
                }
            }
            
            if (bestHand.type == Type.None)
            {
                // Check for a flush
                var flush = GetFlush(allCards.ToList());
                if (flush != null)
                {
                    bestHand.SetCards(flush, Type.Flush);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for a straight
                var straight = GetStraight(allCards.ToList(), Card.Suit.NotSet);
                if (straight != null)
                {
                    bestHand.SetCards(straight, Type.Straight);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for three of a kind
                var trips = GetSeries(allCards.ToList(), 3);
                if (trips != null)
                {
                    bestHand.SetCards(trips, Type.ThreeOfAKind);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for two pair
                var twoPair = GetTwoPair(allCards.ToList());
                if (twoPair != null)
                {
                    bestHand.SetCards(twoPair, Type.TwoPair);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for one pair
                var onePair = GetSeries(allCards.ToList(), 2);
                if (onePair != null)
                {
                    bestHand.SetCards(onePair, Type.OnePair);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for a high card
                bestHand.SetCards(GetHighCard(allCards.ToList()), Type.HighCard);
            }

            return bestHand;
        }

        private void SetCards(List<Card> c, Type t)
        {
            cards = c;
            type = t;
        }

        // Returns the best 5-card royal flush hand from the available cards
        private static List<Card> GetRoyalFlush(List<Card> allCards)
        {
            List<Card> royalFlushCards = null;
            List<Card> selection;

            selection = GetRoyalFlushBySuit(allCards, Card.Suit.Clubs);
            if (selection != null)
            {
                royalFlushCards = selection;
            }

            selection = GetRoyalFlushBySuit(allCards, Card.Suit.Diamonds);
            if (selection != null)
            {
                royalFlushCards = selection;
            }

            selection = GetRoyalFlushBySuit(allCards, Card.Suit.Hearts);
            if (selection != null)
            {
                royalFlushCards = selection;
            }

            selection = GetRoyalFlushBySuit(allCards, Card.Suit.Spades);
            if (selection != null)
            {
                royalFlushCards = selection;
            }

            return royalFlushCards;
        }

        private static List<Card> GetRoyalFlushBySuit(List<Card> allCards, Card.Suit suit)
        {
            if (GetNumberOfMatchingCards(allCards, suit, Card.Value.VT) == 1 &&
                GetNumberOfMatchingCards(allCards, suit, Card.Value.VJ) == 1 &&
                GetNumberOfMatchingCards(allCards, suit, Card.Value.VQ) == 1 &&
                GetNumberOfMatchingCards(allCards, suit, Card.Value.VK) == 1 &&
                GetNumberOfMatchingCards(allCards, suit, Card.Value.VA) == 1)
            {
                List<Card> royalFlushCards = new List<Card>();

                royalFlushCards.Add(GetMatchingCard(allCards, suit, Card.Value.VT));
                royalFlushCards.Add(GetMatchingCard(allCards, suit, Card.Value.VJ));
                royalFlushCards.Add(GetMatchingCard(allCards, suit, Card.Value.VQ));
                royalFlushCards.Add(GetMatchingCard(allCards, suit, Card.Value.VK));
                royalFlushCards.Add(GetMatchingCard(allCards, suit, Card.Value.VA));

                return royalFlushCards;
            }

            return null;
        }

        // Returns the best 5-card straight flush hand from the available cards
        private static List<Card> GetStraightFlush(List<Card> allCards)
        {
            List<Card> clubStraightFlush = GetStraight(allCards, Card.Suit.Clubs);
            if (clubStraightFlush != null)
            {
                return clubStraightFlush;
            }

            List<Card> diamondStraightFlush = GetStraight(allCards, Card.Suit.Diamonds);
            if (diamondStraightFlush != null)
            {
                return diamondStraightFlush;
            }

            List<Card> heartStraightFlush = GetStraight(allCards, Card.Suit.Hearts);
            if (heartStraightFlush != null)
            {
                return heartStraightFlush;
            }

            List<Card> spadeStraightFlush = GetStraight(allCards, Card.Suit.Spades);
            if (spadeStraightFlush != null)
            {
                return spadeStraightFlush;
            }

            return null;
        }

        // Returns the best 5-card full house hand from the available cards
        private static List<Card> GetFullHouse(List<Card> allCards)
        {
            List<Card> fullHouse = null;
            List<Card> trips = null;
            List<Card> pair = null;

            for (int i = (int)Card.Value.V2; i <= (int)Card.Value.VA; i++)
            {
                if (GetNumberOfMatchingCards(allCards, Card.Suit.NotSet, (Card.Value)i) == 3)
                {
                    trips = GetCardsByValue(allCards, (Card.Value)i);
                }
                else if (GetNumberOfMatchingCards(allCards, Card.Suit.NotSet, (Card.Value)i) == 2)
                {
                    pair = GetCardsByValue(allCards, (Card.Value)i);
                }
            }

            if (trips != null && pair != null)
            {
                fullHouse = new List<Card>();
                fullHouse.AddRange(trips);
                fullHouse.AddRange(pair);
            }

            return fullHouse;
        }

        // Returns the best 5-card flush hand from the available cards
        private static List<Card> GetFlush(List<Card> allCards)
        {
            List<Card> flushCards = null;

            var clubs = GetCardsBySuite(allCards, Card.Suit.Clubs);
            if (clubs.Count >= 5)
            {
                flushCards = clubs;
            }

            var diamonds = GetCardsBySuite(allCards, Card.Suit.Diamonds);
            if (diamonds.Count >= 5)
            {
                flushCards = diamonds;
            }

            var hearts = GetCardsBySuite(allCards, Card.Suit.Hearts);
            if (hearts.Count >= 5)
            {
                flushCards = hearts;
            }

            var spades = GetCardsBySuite(allCards, Card.Suit.Spades);
            if (spades.Count >= 5)
            {
                flushCards = spades;
            }

            if (flushCards != null)
            {
                flushCards = flushCards.OrderBy(o => (int)o.value).ToList();
                while (flushCards.Count > 5)
                {
                    flushCards.RemoveAt(0);
                }
            }

            return flushCards;
        }

        // Returns the best 5-card straight hand from the available cards
        private static List<Card> GetStraight(List<Card> allCards, Card.Suit suit = Card.Suit.NotSet)
        {
            List<Card> straightCards = null;
            List<Card> selection = new List<Card>();

            if (    GetNumberOfMatchingCards(allCards, suit, Card.Value.VA) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, Card.Value.V2) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, Card.Value.V3) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, Card.Value.V4) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, Card.Value.V5) > 0)
            {
                selection.Add(GetMatchingCard(allCards, suit, Card.Value.VA));
                selection.Add(GetMatchingCard(allCards, suit, Card.Value.V2));
                selection.Add(GetMatchingCard(allCards, suit, Card.Value.V3));
                selection.Add(GetMatchingCard(allCards, suit, Card.Value.V4));
                selection.Add(GetMatchingCard(allCards, suit, Card.Value.V5));
            }

            for (int i = 0; i < allCards.Count; i++)
            {
                if (allCards[i].suit != suit && suit != Card.Suit.NotSet)
                {
                    continue;
                }

                if  ( GetNumberOfMatchingCards(allCards, suit, allCards[i].value + 1) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, allCards[i].value + 2) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, allCards[i].value + 3) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, allCards[i].value + 4) > 0 )
                {
                    selection.Clear();

                    selection.Add(allCards[i]);
                    selection.Add(GetMatchingCard(allCards, suit, allCards[i].value + 1));
                    selection.Add(GetMatchingCard(allCards, suit, allCards[i].value + 2));
                    selection.Add(GetMatchingCard(allCards, suit, allCards[i].value + 3));
                    selection.Add(GetMatchingCard(allCards, suit, allCards[i].value + 4));
                }
            }

            if (selection.Count != 0)
            {
                straightCards = selection;
            }

            return straightCards;
        }

        // Returns the best 5-card two pair hand from the available cards
        private static List<Card> GetTwoPair(List<Card> allCards)
        {
            List<Card> highPair = null;
            List<Card> lowPair = null;

            for (int i = (int)Card.Value.V2; i <= (int)Card.Value.VA; i++)
            {
                if (GetNumberOfMatchingCards(allCards, Card.Suit.NotSet, (Card.Value)i) == 2)
                {
                    if (highPair != null)
                    {
                        lowPair = highPair;
                    }

                    highPair = GetCardsByValue(allCards, (Card.Value)i);
                }
            }

            if (highPair != null && lowPair != null)
            {
                List<Card> otherCards = allCards;
                otherCards.Remove(highPair[0]);
                otherCards.Remove(highPair[1]);
                otherCards.Remove(lowPair[0]);
                otherCards.Remove(lowPair[1]);

                otherCards = otherCards.OrderBy(o => (int)o.value).ToList();
                while (otherCards.Count > 1)
                {
                    otherCards.RemoveAt(0);
                }

                List<Card> fullHand = new List<Card>();
                fullHand.AddRange(highPair);
                fullHand.AddRange(lowPair);
                fullHand.AddRange(otherCards);
                return fullHand;
            }

            return null;
        }

        // Returns the best 5-card (pair / trips / quads / N) from the available cards
        private static List<Card> GetSeries(List<Card> allCards, int numCardsInSeries)
        {
            List<Card> series = null;

            for (int i = (int)Card.Value.V2; i <= (int)Card.Value.VA; i++)
            {
                if (GetNumberOfMatchingCards(allCards, Card.Suit.NotSet, (Card.Value)i) == numCardsInSeries)
                {
                    series = GetCardsByValue(allCards, (Card.Value)i);
                }
            }

            if (series != null)
            {
                List<Card> hand = new List<Card>();

                List<Card> otherCards = allCards;
                for (int i = 0; i < series.Count; i++)
                {
                    otherCards.Remove(series[i]);
                }

                otherCards = otherCards.OrderBy(o => (int)o.value).ToList();
                while (otherCards.Count > (5 - numCardsInSeries))
                {
                    otherCards.RemoveAt(0);
                }

                hand.AddRange(series);
                hand.AddRange(otherCards);

                return hand;
            }

            return null;
        }

        // Returns the best 5-card high card hand from the available cards
        private static List<Card> GetHighCard(List<Card> allCards)
        {
            allCards = allCards.OrderBy(o => (int)o.value).ToList();
            while (allCards.Count > 5)
            {
                allCards.RemoveAt(0);
            }

            return allCards;
        }

        private static int GetNumberOfMatchingCards(List<Card> collection, Card.Suit suit, Card.Value value)
        {
            if (collection == null || collection.Count == 0)
            {
                throw new ArgumentException("GetNumberOfMatchingCards: collection was null or empty");
            }

            int num = 0;

            for (int i = 0; i < collection.Count; i++)
            {
                if (
                    ((value == Card.Value.NotSet) || (collection[i].value == value)) &&
                    ((suit == Card.Suit.NotSet) || (collection[i].suit == suit))
                    )
                {
                    num++;
                }
            }

            return num;
        }

        private static Card GetMatchingCard(List<Card> collection, Card.Suit suit, Card.Value value)
        {
            if (collection == null || collection.Count == 0)
            {
                throw new ArgumentException("GetMatchingCard: collection was null or empty");
            }

            Card card = null;

            for (int i = 0; i < collection.Count; i++)
            {
                if  ( 
                    (collection[i].value == value || value == Card.Value.NotSet) && 
                    (collection[i].suit == suit || suit == Card.Suit.NotSet)
                    )
                {
                    card = collection[i];
                    break;
                }
            }

            return card;
        }

        private static List<Card> GetCardsBySuite(List<Card> allCards, Card.Suit suit)
        {
            List<Card> suitedCards = new List<Card>();

            for (int i = 0; i < allCards.Count; i++)
            {
                if (allCards[i].suit == suit)
                {
                    suitedCards.Add(allCards[i]);
                }
            }

            return suitedCards;
        }

        private static List<Card> GetCardsByValue(List<Card> allCards, Card.Value value)
        {
            List<Card> cardsOfValue = new List<Card>();

            for (int i = 0; i < allCards.Count; i++)
            {
                if (allCards[i].value == value)
                {
                    cardsOfValue.Add(allCards[i]);
                }
            }

            return cardsOfValue;
        }
    }
}
