using System;
using System.Collections.Generic;
using System.Text;

namespace holdem_odds
{
	class Card
	{
		public enum Suit
		{
			NotSet,
			Clubs,
			Diamonds,
			Hearts,
			Spades
		}

		public enum Value
		{
			NotSet = 0,

			Deuce = 2,
			Three = 3,
			Four = 4,
			Five = 5,
			Six = 6,
			Seven = 7,
			Eight = 8,
			Nine = 9,
			Ten = 10,
			Jack = 11,
			Queen = 12,
			King = 13,
			Ace = 14
		}

		public Suit suit { get; }
		public Value value { get; set; }

		public Card(Suit s, Value v)
		{
			suit = s;
			value = v;
		}

		public string GetCardSuitString(bool plural)
		{
			if (plural)
			{
				return suit.ToString() + "s";
			}

			return suit.ToString();
		}

		public string GetCardValueString(bool plural)
		{
			if (plural)
			{
				if (value == Value.Six)
				{
					return value.ToString() + "es";
				}
				else
				{
					return value.ToString() + "s";
				}
			}

			return value.ToString();
		}

		public void SetConsoleColorToCardColor()
		{
			Console.BackgroundColor = ConsoleColor.White;

			if (suit == Suit.Clubs)
			{
				Console.ForegroundColor = ConsoleColor.DarkGreen;
			}

			if (suit == Suit.Diamonds)
			{
				Console.ForegroundColor = ConsoleColor.DarkBlue;
			}

			if (suit == Suit.Hearts)
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
			}

			if (suit == Suit.Spades)
			{
				Console.ForegroundColor = ConsoleColor.Black;
			}
		}

		public void ResetConsoleColor()
		{
			Console.ResetColor();
		}

		public string GetHumanReadable(bool includeSuit = true)
		{
			if (value == Value.NotSet || suit == Suit.NotSet)
			{
				throw new InvalidOperationException("Couldn't get a human-readable name for the card: card data not set");
			}

			string name = "";

			if ((int)value >= 2 && (int)value <= 9)
			{
				name += (int)value;
			}
			else
			{
				switch (value)
				{
					case Value.Ten:
						name += "T";
						break;
					case Value.Jack:
						name += "J";
						break;
					case Value.Queen:
						name += "Q";
						break;
					case Value.King:
						name += "K";
						break;
					case Value.Ace:
						name += "A";
						break;
				}
			}

			if (includeSuit)
			{
				switch (suit)
				{
					case Suit.Clubs:
						name += "c";
						break;

					case Suit.Diamonds:
						name += "d";
						break;

					case Suit.Hearts:
						name += "h";
						break;

					case Suit.Spades:
						name += "s";
						break;
				}
			}

			return name;
		}

		public void PrintHumanReadable(bool includeSuit = true)
		{
			SetConsoleColorToCardColor();
			Console.Write(GetHumanReadable(true));
			ResetConsoleColor();
		}
	}
}
