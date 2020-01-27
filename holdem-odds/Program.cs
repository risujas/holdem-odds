﻿using System;
using System.Collections.Generic;

namespace holdem_odds
{
	class Program
	{
		static void Main(string[] args)
		{
			int totalHands = 0;
			const int maxHands = 500000000;
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

				Deck deck = new Deck();
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

				if (totalHands % updateInterval == 0)
				{
					Console.Clear();
					Console.WriteLine("Total hands: " + totalHands);

					float percent = totalRoyalFlushes / (float)totalHands * 100.0f;
					Console.WriteLine("Total royal flushes: " + percent + "%");

					percent = totalStraightFlushes / (float)totalHands * 100.0f;
					Console.WriteLine("Total straight flushes: " + percent + "%");

					percent = totalFourOfAKind / (float)totalHands * 100.0f;
					Console.WriteLine("Total quads: " + percent + "%");

					percent = totalFullHouses / (float)totalHands * 100.0f;
					Console.WriteLine("Total full houses: " + percent + "%");

					percent = totalFlushes / (float)totalHands * 100.0f;
					Console.WriteLine("Total flushes: " + percent + "%");

					percent = totalStraights / (float)totalHands * 100.0f;
					Console.WriteLine("Total straights: " + percent + "%");

					percent = totalThreeOfAKind / (float)totalHands * 100.0f;
					Console.WriteLine("Total trips: " + percent + "%");

					percent = totalTwoPair / (float)totalHands * 100.0f;
					Console.WriteLine("Total two pair: " + percent + "%");

					percent = totalOnePair / (float)totalHands * 100.0f;
					Console.WriteLine("Total one pair: " + percent + "%");

					percent = totalHighCards / (float)totalHands * 100.0f;
					Console.WriteLine("Total high cards: " + percent + "%");
				}
			}

			Console.ReadLine();
		}
	}
}
