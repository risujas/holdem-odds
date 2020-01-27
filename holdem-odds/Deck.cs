using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace holdem_odds
{
	class Deck
	{
		private List<Card> cards = null;

		public Deck()
		{
			Reset();
		}

		public void Reset()
		{
			cards = null;
			cards = new List<Card>();

			AddSuit(Card.Suit.Clubs);
			AddSuit(Card.Suit.Diamonds);
			AddSuit(Card.Suit.Hearts);
			AddSuit(Card.Suit.Spades);
		}

		public void Shuffle()
		{
			Random random = new Random();
			cards = cards.OrderBy(x => random.Next()).ToList();
		}

		public Card DrawNextCard(bool printInfo = false)
		{
			if (cards == null || cards.Count == 0)
			{
				throw new InvalidOperationException("Couldn't draw a card because no cards remain in the deck.");
			}

			Card nextCard = cards[cards.Count - 1];
			cards.Remove(nextCard);

			if (printInfo)
			{
				Console.WriteLine(nextCard.GetHumanReadable() + " was drawn from the deck. " + cards.Count + " cards remain.");
			}

			return nextCard;
		}

		public void PrintInfo()
		{
			Console.WriteLine("Cards remaining in the deck: " + cards.Count);

			foreach (var c in cards)
			{
				Console.WriteLine(c.GetHumanReadable());
			}
		}

		private void AddSuit(Card.Suit suit)
		{
			for (int i = (int)Card.Value.ValueMin; i <= (int)Card.Value.ValueMax; i++)
			{
				Card card = new Card(suit, (Card.Value)i);
				cards.Add(card);
			}
		}
	}
}
