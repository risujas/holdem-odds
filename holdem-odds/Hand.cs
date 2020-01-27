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

		public Type type;
		public List<Card> cards;

		public static Hand FindBest(List<Card> holeCards, List<Card> communityCards)
		{
			Hand bestHand = new Hand();

			var flush = GetFlushCards(holeCards, communityCards);
			if (flush != null)
			{
				bestHand.cards = flush;
				bestHand.type = Hand.Type.Flush;
			}

			return bestHand;
		}

		// If the player has a flush, returns the flushed cards. Otherwise, returns null.
		private static List<Card> GetFlushCards(List<Card> holeCards, List<Card> communityCards)
		{
			List<Card> flushCards = null;

			var clubs = GetSuitedCards(holeCards, communityCards, Card.Suit.Clubs);
			if (clubs.Count >= 5)
			{
				flushCards = clubs;
			}

			var diamonds = GetSuitedCards(holeCards, communityCards, Card.Suit.Diamonds);
			if (diamonds.Count >= 5)
			{
				flushCards = diamonds;
			}

			var hearts = GetSuitedCards(holeCards, communityCards, Card.Suit.Hearts);
			if (hearts.Count >= 5)
			{
				flushCards = hearts;
			}

			var spades = GetSuitedCards(holeCards, communityCards, Card.Suit.Spades);
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

		private static List<Card> GetSuitedCards(List<Card> holeCards, List<Card> communityCards, Card.Suit suit)
		{
			List<Card> suitedCards = new List<Card>();

			if (holeCards[0].suit == suit)
			{
				suitedCards.Add(holeCards[0]);
			}

			if (holeCards[1].suit == suit)
			{
				suitedCards.Add(holeCards[1]);
			}

			for (int i = 0; i < communityCards.Count; i++)
			{
				if (communityCards[i].suit == suit)
				{
					suitedCards.Add(communityCards[i]);
				}
			}

			return suitedCards;
		}
	}
}
