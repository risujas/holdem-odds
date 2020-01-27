using System;

namespace holdem_odds
{
	class Program
	{
		static void Main(string[] args)
		{
			Deck deck = new Deck();
			deck.Shuffle();

			//deck.PrintInfo();

			while (true)
			{
				deck.DrawNextCard(true);
				Console.ReadLine();
			}
		}
	}
}
