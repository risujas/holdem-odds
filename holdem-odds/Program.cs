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

				List<Card> holeCards = new List<Card>();
				holeCards.Add(deck.DrawNextCard());
				holeCards.Add(deck.DrawNextCard());

				Console.Write("Hole cards: ");

				holeCards[0].SetConsoleColorToCardColor();
				Console.Write(holeCards[0].GetHumanReadable());
				holeCards[0].ResetConsoleColor();

				Console.Write(" ");

				holeCards[1].SetConsoleColorToCardColor();
				Console.Write(holeCards[1].GetHumanReadable());
				holeCards[1].ResetConsoleColor();

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


				var bh = Hand.FindBest(holeCards, communityCards);
				if (bh.type == Hand.Type.Flush)
				{
					Console.WriteLine();
					Console.WriteLine("Hand: ");

					for (int i = 0; i < bh.cards.Count; i++)
					{
						bh.cards[i].SetConsoleColorToCardColor();
						Console.Write(bh.cards[i].GetHumanReadable());
						bh.cards[i].ResetConsoleColor();
						Console.Write(" ");
					}

					Console.ReadLine();
				}

				Console.WriteLine("\n");
			}
		}
	}
}
