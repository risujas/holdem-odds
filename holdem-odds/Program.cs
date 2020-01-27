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
				Deck deck = new Deck();
				deck.Shuffle();

				HoleCards hand = new HoleCards();
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
				Console.Write("Table: ");

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


				var bh = hand.GetBestHand(communityCards);
				if (bh.Count > 0)
				{
					Console.WriteLine();
					Console.WriteLine("Hand: ");

					for (int i = 0; i < bh.Count; i++)
					{
						bh[i].SetConsoleColorToCardColor();
						Console.Write(bh[i].GetHumanReadable());
						bh[i].ResetConsoleColor();
						Console.Write(" ");
					}

					Console.ReadLine();
				}

				Console.WriteLine("\n");
			}
		}
	}
}
