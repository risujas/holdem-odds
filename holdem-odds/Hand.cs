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
				// Check for a flush
				var flush = GetFlushCards(allCards);
				if (flush != null)
				{
					bestHand.SetCards(flush, Type.Flush);
				}
			}

			if (bestHand.type == Type.None)
			{

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
	}
}
