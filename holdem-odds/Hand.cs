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
            StraightFlush
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
                // Check for a straight flush
                var straightFlush = GetStraightFlushCards(allCards);
                if (straightFlush != null)
                {
                    bestHand.SetCards(straightFlush, Type.StraightFlush);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for a flush
                var flush = GetFlushCards(allCards);
                if (flush != null)
                {
                    bestHand.SetCards(flush, Type.Flush);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for a straight
                var straight = GetStraightCards(allCards);
                if (straight != null)
                {
                    bestHand.SetCards(straight, Type.Straight);
                }
            }

            if (bestHand.type == Type.None)
            {
                // Check for a high card
                bestHand.SetCards(GetHighestCardsByValue(allCards), Type.HighCard);
            }

            return bestHand;
        }

        private void SetCards(List<Card> c, Type t)
        {
            cards = c;
            type = t;
        }

        private static List<Card> GetHighestCardsByValue(List<Card> allCards)
        {
            allCards = allCards.OrderBy(o => (int)o.value).ToList();
            while (allCards.Count > 5)
            {
                allCards.RemoveAt(0);
            }

            return allCards;
        }

        private static List<Card> GetStraightFlushCards(List<Card> allCards)
        {
            List<Card> clubStraightFlush = GetStraightCards(allCards, Card.Suit.Clubs);
            if (clubStraightFlush != null)
            {
                return clubStraightFlush;
            }

            List<Card> diamondStraightFlush = GetStraightCards(allCards, Card.Suit.Diamonds);
            if (diamondStraightFlush != null)
            {
                return diamondStraightFlush;
            }

            List<Card> heartStraightFlush = GetStraightCards(allCards, Card.Suit.Hearts);
            if (heartStraightFlush != null)
            {
                return heartStraightFlush;
            }

            List<Card> spadeStraightFlush = GetStraightCards(allCards, Card.Suit.Spades);
            if (spadeStraightFlush != null)
            {
                return spadeStraightFlush;
            }

            return null;
        }

        // If the player has a flush, returns the flushed cards. Otherwise, returns null.
        private static List<Card> GetFlushCards(List<Card> allCards)
        {
            List<Card> flushCards = null;

            var clubs = GetSuitedCards(allCards, Card.Suit.Clubs);
            if (clubs.Count >= 5)
            {
                flushCards = clubs;
            }

            var diamonds = GetSuitedCards(allCards, Card.Suit.Diamonds);
            if (diamonds.Count >= 5)
            {
                flushCards = diamonds;
            }

            var hearts = GetSuitedCards(allCards, Card.Suit.Hearts);
            if (hearts.Count >= 5)
            {
                flushCards = hearts;
            }

            var spades = GetSuitedCards(allCards, Card.Suit.Spades);
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

        private static List<Card> GetSuitedCards(List<Card> allCards, Card.Suit suit)
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

        // Returns your highest straight cards if you've made a straight, otherwise returns null
        private static List<Card> GetStraightCards(List<Card> allCards, Card.Suit suit = Card.Suit.NotSet)
        {
            List<Card> straightCards = new List<Card>();

            if (    GetNumberOfMatchingCards(allCards, suit, Card.Value.VA) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, Card.Value.V2) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, Card.Value.V3) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, Card.Value.V4) > 0 &&
                    GetNumberOfMatchingCards(allCards, suit, Card.Value.V5) > 0)
            {
                straightCards.Add(GetMatchingCard(allCards, suit, Card.Value.VA));
                straightCards.Add(GetMatchingCard(allCards, suit, Card.Value.V2));
                straightCards.Add(GetMatchingCard(allCards, suit, Card.Value.V3));
                straightCards.Add(GetMatchingCard(allCards, suit, Card.Value.V4));
                straightCards.Add(GetMatchingCard(allCards, suit, Card.Value.V5));
            }

            for (int i = 0; i < allCards.Count - 1; i++)
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
                    straightCards.Clear();

                    straightCards.Add(allCards[i]);
                    straightCards.Add(GetMatchingCard(allCards, suit, allCards[i].value + 1));
                    straightCards.Add(GetMatchingCard(allCards, suit, allCards[i].value + 2));
                    straightCards.Add(GetMatchingCard(allCards, suit, allCards[i].value + 3));
                    straightCards.Add(GetMatchingCard(allCards, suit, allCards[i].value + 4));
                }
            }

            if (straightCards.Count == 0)
            {
                return null;
            }

            return straightCards;
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
    }
}
