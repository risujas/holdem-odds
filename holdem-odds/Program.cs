using System;
using System.Collections.Generic;

namespace holdem_odds
{
	class Program
	{
		static void Main(string[] args)
		{
			Deck deck = new Deck();

			int totalHands = 0;
			const long maxHands = 5000000000;
			const int updateInterval = 50000;

			int totalRoyalFlushes = 0;
			int totalStraightFlushes = 0;
			int totalFourOfAKind = 0;
			int totalFullHouses = 0;
			int totalFlushes = 0;
			int totalStraights = 0;
			int totalThreeOfAKind = 0;
			int totalTwoPair = 0;
			int totalOnePair = 0;
			int totalHighCards = 0;

			while (totalHands < maxHands)
			{
				totalHands++;
				deck.Reset();
				deck.Shuffle();

				List<Card> holeCards = new List<Card>();
				holeCards.Add(deck.DrawNextCard());
				holeCards.Add(deck.DrawNextCard());

				List<Card> communityCards = new List<Card>();
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());

				var bestHand = Hand.FindBest(holeCards, communityCards);
				switch (bestHand.type)
				{
					case Hand.Type.RoyalFlush:
						totalRoyalFlushes++;
						break;
					case Hand.Type.StraightFlush:
						totalStraightFlushes++;
						break;
					case Hand.Type.FourOfAKind:
						totalFourOfAKind++;
						break;
					case Hand.Type.FullHouse:
						totalFullHouses++;
						break;
					case Hand.Type.Flush:
						totalFlushes++;
						break;
					case Hand.Type.Straight:
						totalStraights++;
						break;
					case Hand.Type.ThreeOfAKind:
						totalThreeOfAKind++;
						break;
					case Hand.Type.TwoPair:
						totalTwoPair++;
						break;
					case Hand.Type.OnePair:
						totalOnePair++;
						break;
					case Hand.Type.HighCard:
						totalHighCards++;
						break;
				}

				/*if (bestHand.type == Hand.Type.OnePair)
				{
					Console.Write("Hole cards: ");
					holeCards[0].SetConsoleColorToCardColor();
					Console.Write(holeCards[0].GetHumanReadable());
					holeCards[0].ResetConsoleColor();
					Console.Write(" ");
					holeCards[1].SetConsoleColorToCardColor();
					Console.Write(holeCards[1].GetHumanReadable());
					holeCards[1].ResetConsoleColor();
					Console.Write(" ");
					Console.Write("Table: ");
					for (int i = 0; i < communityCards.Count; i++)
					{
						communityCards[i].SetConsoleColorToCardColor();
						Console.Write(communityCards[i].GetHumanReadable());
						communityCards[i].ResetConsoleColor();
						Console.Write(" ");
					}
					Console.Write("Hand: ");
					for (int i = 0; i < bestHand.cards.Count; i++)
					{
						bestHand.cards[i].SetConsoleColorToCardColor();
						Console.Write(bestHand.cards[i].GetHumanReadable());
						bestHand.cards[i].ResetConsoleColor();
						Console.Write(" ");
					}
					Console.Write("(" + bestHand.type.ToString() + ")");
					Console.ReadLine();
				}
				//*/

				///*
				if (totalHands % updateInterval == 0)
				{
					Console.Clear();
					Console.WriteLine("Total hands: " + totalHands);

					float percent = totalRoyalFlushes / (float)totalHands * 100.0f;
					Console.WriteLine("Total royal flushes: " + percent + "% - " + totalRoyalFlushes);

					percent = totalStraightFlushes / (float)totalHands * 100.0f;
					Console.WriteLine("Total straight flush: " + percent + "% - " + totalStraightFlushes);

					percent = totalFourOfAKind / (float)totalHands * 100.0f;
					Console.WriteLine("Total four of a kind: " + percent + "% - " + totalFourOfAKind);

					percent = totalFullHouses / (float)totalHands * 100.0f;
					Console.WriteLine("Total full house: " + percent + "% - " + totalFullHouses);

					percent = totalFlushes / (float)totalHands * 100.0f;
					Console.WriteLine("Total flush: " + percent + "% - " + totalFlushes);

					percent = totalStraights / (float)totalHands * 100.0f;
					Console.WriteLine("Total straight: " + percent + "% - " + totalStraights);

					percent = totalThreeOfAKind / (float)totalHands * 100.0f;
					Console.WriteLine("Total three of a kind: " + percent + "% - " + totalThreeOfAKind);

					percent = totalTwoPair / (float)totalHands * 100.0f;
					Console.WriteLine("Total two pair: " + percent + "% - " + totalTwoPair);

					percent = totalOnePair / (float)totalHands * 100.0f;
					Console.WriteLine("Total one pair: " + percent + "% - " + totalOnePair);

					percent = totalHighCards / (float)totalHands * 100.0f;
					Console.WriteLine("Total high cards: " + percent + "% - " + totalHighCards);
				}
				//*/
			}

			Console.ReadLine();
		}
	}
}
