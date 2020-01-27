using System;
using System.Collections.Generic;
using System.Text;

namespace holdem_odds
{
	class Deck
	{
		public List<Card> cards = null;

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
