using System;

namespace holdem_odds
{
	class Program
	{
		static void Main(string[] args)
		{
			Deck deck = new Deck();
			deck.Reset();
			deck.Shuffle();

			deck.DrawSpecificCard(Card.Suit.Hearts, Card.Value.V5, true);
			Console.Read();
		}
	}
}
