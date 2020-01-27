using System;
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

			int totalFlushes = 0;
			int totalStraights = 0;
			int totalHighCards = 0;

			while (totalHands < maxHands)
			{
				totalHands++;

				Deck deck = new Deck();
				deck.Shuffle();

				List<Card> holeCards = new List<Card>();
				holeCards.Add(deck.DrawSpecificCard(Card.Suit.Diamonds, Card.Value.VJ));
				holeCards.Add(deck.DrawSpecificCard(Card.Suit.Diamonds, Card.Value.VT));

				List<Card> communityCards = new List<Card>();
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());
				//communityCards.Add(deck.DrawNextCard());
				//communityCards.Add(deck.DrawNextCard());

				var bestHand = Hand.FindBest(holeCards, communityCards);
				switch (bestHand.type)
				{
					case Hand.Type.Flush:
						totalFlushes++;
						break;
					case Hand.Type.Straight:
						totalStraights++;
						break;
					case Hand.Type.HighCard:
						totalHighCards++;
						break;
				}

				if (totalHands % updateInterval == 0)
				{
					Console.Clear();
					Console.WriteLine("Total hands: " + totalHands);

					float percent = totalFlushes / (float)totalHands * 100.0f;
					Console.WriteLine("Total flushes: " + percent + "%");

					percent = totalStraights / (float)totalHands * 100.0f;
					Console.WriteLine("Total straights: " + percent + "%");

					percent = totalHighCards / (float)totalHands * 100.0f;
					Console.WriteLine("Total high cards: " + percent + "%");
				}
			}

			Console.ReadLine();
		}
	}
}
