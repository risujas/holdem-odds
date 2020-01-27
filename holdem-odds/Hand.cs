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
            List<Card> straightFlushCards = GetStraightCards(allCards, true);
            return straightFlushCards;
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
        private static List<Card> GetStraightCards(List<Card> allCards, bool requireStraightFlush = false)
        {
            List<Card> straightCards = null;

            List<Card> sortedAll = allCards.OrderBy(o => (int)o.value).ToList();
            sortedAll = sortedAll.GroupBy(x => x.value).Select(x => x.First()).ToList();

            int straightStartIndex = 0;
            bool hasStraight = false;
            bool aceThroughFive = false;

            for (int i = 0; i <= sortedAll.Count - 5; i++)
            {
                if  (
                    (int)sortedAll[i + 0].value + 1 == (int)sortedAll[i + 1].value &&
                    (int)sortedAll[i + 1].value + 1 == (int)sortedAll[i + 2].value &&
                    (int)sortedAll[i + 2].value + 1 == (int)sortedAll[i + 3].value &&
                    (int)sortedAll[i + 3].value + 1 == (int)sortedAll[i + 4].value
                    )
                {
                    straightStartIndex = i;
                    hasStraight = true;
                }
            }

            if (!hasStraight)
            {
                if  (
                    sortedAll[sortedAll.Count - 1].value == Card.Value.VA && 
                    sortedAll[0].value == Card.Value.V2 &&
                    sortedAll[1].value == Card.Value.V3 &&
                    sortedAll[2].value == Card.Value.V4 &&
                    sortedAll[3].value == Card.Value.V5 
                    )
                {
                    aceThroughFive = true;
                    hasStraight = true;
                }
            }

            if (hasStraight)
            {
                if (aceThroughFive)
                {
                    straightCards = sortedAll.GetRange(0, 4);
                    straightCards.Add(sortedAll[sortedAll.Count - 1]);
                }
                else
                {
                    straightCards = sortedAll.GetRange(straightStartIndex, 5);
                }
            }

            return straightCards;
        }
    }
}
