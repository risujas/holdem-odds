using System;
using System.Collections.Generic;

namespace holdem_odds
{
	class Program
	{
		static void Main(string[] args)
		{
			while(true)
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

				Hand p1Hand = Hand.FindBest(p1Cards, communityCards);
				Hand p2Hand = Hand.FindBest(p2Cards, communityCards);

				Console.Clear();

				Console.WriteLine("Player 1: " + p1Cards[0].GetHumanReadable() + " " + p1Cards[1].GetHumanReadable());
				Console.WriteLine("Player 2: " + p2Cards[0].GetHumanReadable() + " " + p2Cards[1].GetHumanReadable());
				Console.WriteLine();

				Console.Write("Table: ");
				for (int i = 0; i < communityCards.Count; i++)
				{
					Console.Write(communityCards[i].GetHumanReadable() + " ");
				}
				Console.WriteLine("\n");

				Console.Write("Player 1: ");
				for (int i = 0; i < p1Hand.cards.Count; i++)
				{
					Console.Write(p1Hand.cards[i].GetHumanReadable() + " ");
				}
				Console.WriteLine(p1Hand.type.ToString());

				Console.Write("Player 2: ");
				for (int i = 0; i < p2Hand.cards.Count; i++)
				{
					Console.Write(p2Hand.cards[i].GetHumanReadable() + " ");
				}
				Console.WriteLine(p2Hand.type.ToString());
				Console.WriteLine();

				Hand.ShowdownResult result = p1Hand.EvaluateAgainst(p2Hand);
				if (result == Hand.ShowdownResult.Loss)
				{
					Console.WriteLine("Player 1 loses.");
				}
				else if (result == Hand.ShowdownResult.Tie)
				{
					Console.WriteLine("Player 1 ties with player 2.");
				}
				else if (result == Hand.ShowdownResult.Win)
				{
					Console.WriteLine("Player 1 wins.");
				}

				Console.ReadLine();
			}
		}
	}
}
