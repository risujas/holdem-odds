using System;
using System.Collections.Generic;
using System.Linq;

namespace holdem_odds
{
    class Tests
    {
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

			var sortedDict = from entry in drawnCards orderby entry.Value descending select entry;
			foreach (var k in sortedDict)
			{
				float percent = (k.Value / (float)totalDraws) * 100.0f;
				Console.WriteLine(k.Key + " - " + percent);
			}

			Console.ReadLine();
		}
    }
}
