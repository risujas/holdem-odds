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
	}
}
