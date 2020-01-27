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

				Console.Write("Hole cards: ");

				hand.holeCard1.SetConsoleColorToCardColor();
				Console.Write(hand.holeCard1.GetHumanReadable());
				hand.holeCard1.ResetConsoleColor();

				Console.Write(" ");

				hand.holeCard2.SetConsoleColorToCardColor();
				Console.Write(hand.holeCard2.GetHumanReadable());
				hand.holeCard2.ResetConsoleColor();

				Console.WriteLine();

				List<Card> communityCards = new List<Card>();
				for (int i = 0; i < 5; i++)
				{
					var nextCard = deck.DrawNextCard();
					communityCards.Add(nextCard);

					nextCard.SetConsoleColorToCardColor();
					Console.Write(nextCard.GetHumanReadable());
					nextCard.ResetConsoleColor();

					Console.Write(" ");
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
