using System;
using System.Collections.Generic;
using System.Linq;

namespace holdem_odds
{
    class Tests
    {
		public static class EquityTest
		{
			private static Dictionary<List<Card>, float> winsByCards = new Dictionary<List<Card>, float>();

			public static void Run()
			{
				for (Card.Value i = Card.Value.V2; i <= Card.Value.VA; i++)
				{
					for (Card.Value j = Card.Value.V2; j <= Card.Value.VA; j++)
					{
						List<Card> holeCards = new List<Card>();

						Card oc1 = new Card(Card.Suit.Hearts, i);
						Card oc2 = new Card(Card.Suit.Spades, j);

						holeCards.Add(oc1);
						holeCards.Add(oc2);

						bool alreadyDone = false;

						foreach (var x in winsByCards)
						{
							if (x.Key[0].value == oc1.value && x.Key[1].value == oc2.value)
							{
								alreadyDone = true;
								break;
							}

							if (x.Key[1].value == oc1.value && x.Key[0].value == oc2.value)
							{
								alreadyDone = true;
								break;
							}
						}

						if (!alreadyDone)
						{
							SimulateWinrate(holeCards, 1000);
						}
					}
				}

				winsByCards = winsByCards.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
				var reversed = winsByCards.Reverse();

				Console.Clear();

				foreach(var hc in reversed)
				{
					hc.Key[0].PrintHumanReadable(false);
					hc.Key[1].PrintHumanReadable(false);

					Console.WriteLine(" - " + (int)hc.Value + "%");
				}
			}

			private static void SimulateWinrate(List<Card> hc, int numTests)
			{
				int numWins = 0;

				hc[0].PrintHumanReadable();
				hc[1].PrintHumanReadable();
				Console.Write(" - ");

				for (int i = 0; i < numTests; i++)
				{
					Deck deck = new Deck();
					deck.RemoveCard(hc[0]);
					deck.RemoveCard(hc[1]);
					deck.Shuffle();

					List<Card> p2Cards = new List<Card>();
					p2Cards.Add(deck.DrawNextCard());
					p2Cards.Add(deck.DrawNextCard());
					//p2Cards.Add(deck.DrawSpecificCard(Card.Suit.Clubs, Card.Value.V2));
					//p2Cards.Add(deck.DrawSpecificCard(Card.Suit.Diamonds, Card.Value.V2));

					List<Card> communityCards = new List<Card>();
					communityCards.Add(deck.DrawNextCard());
					communityCards.Add(deck.DrawNextCard());
					communityCards.Add(deck.DrawNextCard());
					communityCards.Add(deck.DrawNextCard());
					communityCards.Add(deck.DrawNextCard());

					Hand p1hand = new Hand(hc, communityCards);
					Hand p2hand = new Hand(p2Cards, communityCards);

					Hand.ShowdownResult result = p1hand.EvaluateAgainst(p2hand);

					if (result == Hand.ShowdownResult.Win)
					{
						numWins++;
					}
				}

				float winRate = numWins / (float)numTests * 100.0f;
				Console.WriteLine((int)winRate + "%");

				if (!winsByCards.ContainsKey(hc))
				{
					winsByCards.Add(hc, winRate);
				}
			}
		}

		public static void TestGame()
		{
			while (true)
			{
				Deck deck = new Deck();
				deck.Shuffle();

				List<Card> p1Cards = new List<Card>();
				p1Cards.Add(deck.DrawNextCard());
				p1Cards.Add(deck.DrawNextCard());

				List<Card> p2Cards = new List<Card>();
				p2Cards.Add(deck.DrawNextCard());
				p2Cards.Add(deck.DrawNextCard());

				List<Card> communityCards = new List<Card>();
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());
				communityCards.Add(deck.DrawNextCard());

				Hand p1Hand = new Hand(p1Cards, communityCards);
				Hand p2Hand = new Hand(p2Cards, communityCards);

				Console.Clear();

				Console.Write("Player 1: ");
				p1Cards[0].SetConsoleColorToCardColor();
				Console.Write(p1Cards[0].GetHumanReadable());
				p1Cards[1].ResetConsoleColor();
				Console.Write(" ");
				p1Cards[1].SetConsoleColorToCardColor();
				Console.Write(p1Cards[1].GetHumanReadable());
				p1Cards[1].ResetConsoleColor();

				Console.WriteLine();

				Console.Write("Player 2: ");
				p2Cards[0].SetConsoleColorToCardColor();
				Console.Write(p2Cards[0].GetHumanReadable());
				p2Cards[1].ResetConsoleColor();
				Console.Write(" ");
				p2Cards[1].SetConsoleColorToCardColor();
				Console.Write(p2Cards[1].GetHumanReadable());
				p2Cards[1].ResetConsoleColor();

				Console.WriteLine();
				Console.WriteLine();

				Console.Write("Table:    ");
				for (int i = 0; i < communityCards.Count; i++)
				{
					communityCards[i].SetConsoleColorToCardColor();
					Console.Write(communityCards[i].GetHumanReadable());
					communityCards[i].ResetConsoleColor();
					Console.Write(" ");
				}

				Console.WriteLine();
				Console.WriteLine();

				Console.Write("Player 1: ");
				p1Hand.PrintHumanReadable(false, true, true);

				Console.Write("Player 2: ");
				p2Hand.PrintHumanReadable(false, true, true);

				Console.WriteLine();

				Hand.ShowdownResult result = p1Hand.EvaluateAgainst(p2Hand);
				if (result == Hand.ShowdownResult.Loss)
				{
					Console.WriteLine("Player 2 wins.");
				}
				else if (result == Hand.ShowdownResult.Tie)
				{
					Console.WriteLine("It's a chop.");
				}
				else if (result == Hand.ShowdownResult.Win)
				{
					Console.WriteLine("Player 1 wins.");
				}

				Console.ReadLine();
			}
		}

		public static void TestFrequencies(int maxHands, int updateInterval, int numCommunityCards)
		{
			Deck deck = new Deck();

			int totalHands = 0;
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
				for (int i = 0; i < numCommunityCards; i++)
				{
					communityCards.Add(deck.DrawNextCard());
				}

				Hand bestHand = new Hand(holeCards, communityCards);
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
			}

			Console.ReadLine();
		}

        public static void TestCardRandomness(int maxDraws, int updateInterval)
        {
			Dictionary<string, int> drawnCards = new Dictionary<string, int>();

			Deck deck = new Deck();

			int totalDraws = 0;

			while (totalDraws < maxDraws)
			{
				totalDraws++;

				deck.Reset();
				deck.Shuffle();

				var c = deck.DrawNextCard(false);

				if (drawnCards.ContainsKey(c.GetHumanReadable()))
				{
					drawnCards[c.GetHumanReadable()]++;
				}

				else
				{
					drawnCards.Add(c.GetHumanReadable(), 1);
				}

				int highestValue = 0;
				string highestKey = "";

				foreach (var k in drawnCards)
				{
					if (k.Value >= highestValue)
					{
						highestKey = k.Key;
						highestValue = k.Value;
					}
				}

				if ((totalDraws % updateInterval == 0) || (totalDraws == 1))
				{
					Console.WriteLine("[" + totalDraws + "/" + maxDraws + "]");
				}
			}

			drawnCards = drawnCards.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
			foreach (var k in drawnCards)
			{
				float percent = (k.Value / (float)totalDraws) * 100.0f;
				Console.WriteLine(k.Key + " - " + percent);
			}

			Console.ReadLine();
		}
    }
}
