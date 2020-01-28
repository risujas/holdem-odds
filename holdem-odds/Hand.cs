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

        public enum ShowdownResult
        {
            None = -1,
            Loss = 0,
            Win = 1,
            Tie = 2
        }

        public Type type { get; private set; }
        public List<Card> tier1Cards { get; private set; }
        public List<Card> tier2Cards { get; private set; }
        public List<Card> tier3Cards { get; private set; }
        public List<Card> tier4Cards { get; private set; }
        public List<Card> tier5Cards { get; private set; }

        public Hand()
        {
        }

        public Hand(List<Card> holeCards, List<Card> communityCards)
        {
            FindBest(holeCards, communityCards);
        }

        public bool HasFillerCards()
        {
            if (tier2Cards != null)
            {
                if (tier2Cards.Count != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetHumanReadable(bool plusSeparator = true, bool realName = true)
        {
            string s = "";
            for (int i = 0; i < tier1Cards.Count; i++)
            {
                s += tier1Cards[i].GetHumanReadable();
                s += " ";
            }

            if (HasFillerCards())
            {
                if (plusSeparator)
                {
                    s += "+ ";
                }

                for (int i = 0; i < tier2Cards.Count; i++)
                {
                    s += tier2Cards[i].GetHumanReadable();
                    s += " ";
                }
            }

            if (realName)
            {
                s += "(" + type.ToString() + ")";
            }

            s = s.Trim();

            return s;
        }

        public void PrintHumanReadable(bool plusSeparator = true, bool realName = true, bool newLine = true)
        {
            string humanReadable = GetHumanReadable(plusSeparator, realName);

            foreach (var mc in tier1Cards)
            {
                mc.PrintHumanReadable(true);
                Console.Write(" ");
            }

            if (HasFillerCards())
            {
                if (plusSeparator)
                {
                    Console.Write("+ ");
                }

                foreach (var fc in tier2Cards)
                {
                    fc.PrintHumanReadable(true);
                    Console.Write(" ");
                }
            }

            if (realName)
            {
                Console.Write("(" + type.ToString() + ")");
            }

            if (newLine)
            {
                Console.Write("\n");
            }
        }

        public ShowdownResult EvaluateAgainst(Hand other)
        {
            ShowdownResult result = ShowdownResult.None;

            if (type > other.type)
            {
                result = ShowdownResult.Win;
            }
            else if (type == other.type)
            {
                result = ShowdownResult.Tie;
                // todo - break ties
            }
            else if (type < other.type)
            {
                result = ShowdownResult.Loss;
            }

            return result;
        }

        public void FindBest(List<Card> holeCards, List<Card> communityCards)
        {
            List<Card> allCards = new List<Card>();
            allCards.AddRange(holeCards);
            allCards.AddRange(communityCards);

            if (type == Type.None)
            {
                var hand = GetRoyalFlush(allCards.ToList());
                if (hand != null)
                {
                    SetCards(hand, null, Type.RoyalFlush);
                }
            }

            if (type == Type.None)
            {
                var hand = GetStraightFlush(allCards.ToList());
                if (hand != null)
                {
                    SetCards(hand, null, Type.StraightFlush);
                }
            }

            if (type == Type.None)
            {
                var hand = GetSeries(allCards.ToList(), 4);
                if (hand != null)
                {
                    SetCards(hand.GetRange(0, 4), hand.GetRange(4, 1), Type.FourOfAKind);
                }
            }

            if (type == Type.None)
            {
                var hand = GetFullHouse(allCards.ToList());
                if (hand != null)
                {
                    SetCards(hand.GetRange(0, 3), hand.GetRange(3, 2), Type.FullHouse);
                }
            }
            
            if (type == Type.None)
            {
                var hand = GetFlush(allCards.ToList());
                if (hand != null)
                {
                    SetCards(hand, null, Type.Flush);
                }
            }

            if (type == Type.None)
            {
                var hand = GetStraight(allCards.ToList(), Card.Suit.NotSet);
                if (hand != null)
                {
                    SetCards(hand, null, Type.Straight);
                }
            }

            if (type == Type.None)
            {
                var hand = GetSeries(allCards.ToList(), 3);
                if (hand != null)
                {
                    SetCards(hand.GetRange(0, 3), hand.GetRange(3, 2), Type.ThreeOfAKind);
                }
            }

            if (type == Type.None)
            {
                var hand = GetTwoPair(allCards.ToList());
                if (hand != null)
                {
                    SetCards(hand.GetRange(0, 2), hand.GetRange(2, 3), Type.TwoPair);
                }
            }

            if (type == Type.None)
            {
                var hand = GetSeries(allCards.ToList(), 2);
                if (hand != null)
                {
                    SetCards(hand.GetRange(0, 2), hand.GetRange(2, 3), Type.OnePair);
                }
            }

            if (type == Type.None)
            {
                var hand = GetHighCard(allCards.ToList());
                SetCards(hand.GetRange(0, 1), hand.GetRange(1, 4), Type.HighCard);
            }
        }

        private void SetCards(List<Card> mc, List<Card> fc, Type t)
        {
            tier1Cards = mc;
            tier2Cards = fc;
            type = t;
        }

        private List<Card> GetRoyalFlush(List<Card> allCards)
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

        private List<Card> GetRoyalFlushBySuit(List<Card> allCards, Card.Suit suit)
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

        private List<Card> GetStraightFlush(List<Card> allCards)
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

        private List<Card> GetFullHouse(List<Card> allCards)
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

        private List<Card> GetFlush(List<Card> allCards)
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

        private List<Card> GetStraight(List<Card> allCards, Card.Suit suit = Card.Suit.NotSet)
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

        private List<Card> GetTwoPair(List<Card> allCards)
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

        private List<Card> GetSeries(List<Card> allCards, int numCardsInSeries)
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

        private List<Card> GetHighCard(List<Card> allCards)
        {
            allCards = allCards.OrderBy(o => (int)o.value).ToList();
            while (allCards.Count > 5)
            {
                allCards.RemoveAt(0);
            }

            return allCards;
        }

        private int GetNumberOfMatchingCards(List<Card> collection, Card.Suit suit, Card.Value value)
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

        private Card GetMatchingCard(List<Card> collection, Card.Suit suit, Card.Value value)
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

        private List<Card> GetCardsBySuite(List<Card> allCards, Card.Suit suit)
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

        private List<Card> GetCardsByValue(List<Card> allCards, Card.Value value)
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
