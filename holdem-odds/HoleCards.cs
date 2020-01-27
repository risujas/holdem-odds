using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace holdem_odds
{
	class HoleCards
	{
		public Card holeCard1 = null;
		public Card holeCard2 = null;

		public Hand GetBestHand(List<Card> communityCards)
		{
			Hand bestHand = new Hand();

			var flush = GetFlushCards(communityCards);
			if (flush != null)
			{
				bestHand.cards = flush;
				bestHand.type = Hand.Type.Flush;
			}

			return bestHand;
		}

		// If the player has a flush, returns the flushed cards. Otherwise, returns null.
		private List<Card> GetFlushCards(List<Card> communityCards)
		{
			List<Card> flushCards = null;

			var clubs = GetSuitedCards(communityCards, Card.Suit.Clubs);
			if (clubs.Count >= 5)
			{
				flushCards = clubs;
			}

			var diamonds = GetSuitedCards(communityCards, Card.Suit.Diamonds);
			if (diamonds.Count >= 5)
			{
				flushCards = diamonds;
			}

			var hearts = GetSuitedCards(communityCards, Card.Suit.Hearts);
			if (hearts.Count >= 5)
			{
				flushCards = hearts;
			}

			var spades = GetSuitedCards(communityCards, Card.Suit.Spades);
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
