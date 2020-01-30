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

			V2 = 2,
			V3 = 3,
			V4 = 4,
			V5 = 5,
			V6 = 6,
			V7 = 7,
			V8 = 8,
			V9 = 9,
			VT = 10,
			VJ = 11,
			VQ = 12,
			VK = 13,
			VA = 14
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
			string s = "";

			switch (value)
			{
				case Value.V2:
					s = "Deuce";
					break;
				case Value.V3:
					s = "Three";
					break;
				case Value.V4:
					s = "Four";
					break;
				case Value.V5:
					s = "Five";
					break;
				case Value.V6:
					s = "Six";
					break;
				case Value.V7:
					s = "Seven";
					break;
				case Value.V8:
					s = "Eight";
					break;
				case Value.V9:
					s = "Nine";
					break;
				case Value.VT:
					s = "Ten";
					break;
				case Value.VJ:
					s = "Jack";
					break;
				case Value.VQ:
					s = "Queen";
					break;
				case Value.VK:
					s = "King";
					break;
				case Value.VA:
					s = "Ace";
					break;
			}

			if (plural)
			{
				if (value == Value.V6)
				{
					s += "es";
				}
				else
				{
					s += "s";
				}
			}

			return s;
		}

		public string GetCardValueStringShort()
		{
			string s = "";

			switch (value)
			{
				case Value.V2:
					s = "2";
					break;
				case Value.V3:
					s = "3";
					break;
				case Value.V4:
					s = "4";
					break;
				case Value.V5:
					s = "5";
					break;
				case Value.V6:
					s = "6";
					break;
				case Value.V7:
					s = "7";
					break;
				case Value.V8:
					s = "8";
					break;
				case Value.V9:
					s = "9";
					break;
				case Value.VT:
					s = "T";
					break;
				case Value.VJ:
					s = "J";
					break;
				case Value.VQ:
					s = "Q";
					break;
				case Value.VK:
					s = "K";
					break;
				case Value.VA:
					s = "A";
					break;
			}

			return s;
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
					case Value.VT:
						name += "T";
						break;
					case Value.VJ:
						name += "J";
						break;
					case Value.VQ:
						name += "Q";
						break;
					case Value.VK:
						name += "K";
						break;
					case Value.VA:
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
			Console.Write(GetHumanReadable(includeSuit));
			ResetConsoleColor();
		}
	}
}
