using System;
using System.Collections.Generic;
using System.Text;

namespace holdem_odds
{
    class Hand
    {
		public enum Type
		{
			HighCard,
			OnePair,
			TwoPair,
			ThreeOfAKind,
			Straight,
			Flush,
			FullHouse,
			FourOfAKind,
			StraightFlush
		}

		public Type type;
		public List<Card> cards;
	}
}
