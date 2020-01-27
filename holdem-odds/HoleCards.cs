using System;
using System.Collections.Generic;
using System.Text;

namespace holdem_odds
{
	class HoleCards
	{
		enum HandType
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

		public Card holeCard1 = null;
		public Card holeCard2 = null;

		public List<Card> GetBestHand(List<Card> communityCards)
		{
			List<Card> bestHand = new List<Card>();

			return bestHand;
		}

		// If the player has a flush, returns the flushed cards. Otherwise, returns null.
		private List<Card> GetFlushCards(List<Card> communityCards)
		{
			List<Card> flushCards = new List<Card>();

			flushCards = GetSuitedCards(communityCards, Card.Suit.Clubs);
			if (flushCards.Count >= 5)
			{
				return flushCards;
			}

			flushCards = GetSuitedCards(communityCards, Card.Suit.Diamonds);
			if (flushCards.Count >= 5)
			{
				return flushCards;
			}

			flushCards = GetSuitedCards(communityCards, Card.Suit.Hearts);
			if (flushCards.Count >= 5)
			{
				return flushCards;
			}

			flushCards = GetSuitedCards(communityCards, Card.Suit.Spades);
			if (flushCards.Count >= 5)
			{
				return flushCards;
			}

			return null;
		}

		private List<Card> GetSuitedCards(List<Card> communityCards, Card.Suit suit)
		{
			List<Card> suitedCards = new List<Card>();

			if (holeCard1.suit == suit)
			{
				suitedCards.Add(holeCard1);
			}

			if (holeCard2.suit == suit)
			{
				suitedCards.Add(holeCard2);
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
