using System;
using System.Collections.Generic;

namespace holdem_odds
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				//Console.Clear();

				Deck deck = new Deck();
				deck.Shuffle();

				Hand hand = new Hand();
				hand.holeCard1 = deck.DrawNextCard();
				hand.holeCard2 = deck.DrawNextCard();

				Console.WriteLine("Hole cards: " + hand.holeCard1.GetHumanReadable() + " " + hand.holeCard2.GetHumanReadable());
				Console.Write("Table: ");

				List<Card> communityCards = new List<Card>();
				for (int i = 0; i < 5; i++)
				{
					var nextCard = deck.DrawNextCard();
					communityCards.Add(nextCard);
					Console.Write(nextCard.GetHumanReadable() + " ");
				}

				if (hand.GetBestHand(communityCards))
				{
					Console.Read();
				}
				Console.WriteLine("\n");
			}
		}
	}
}
