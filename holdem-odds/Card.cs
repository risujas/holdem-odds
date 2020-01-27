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
			NotSet = 1,
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
		public Value value { get; }

		public Card(Suit s, Value v)
		{
			suit = s;
			value = v;
		}
	}
}
