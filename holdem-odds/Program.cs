using System;

namespace holdem_odds
{
	class Program
	{
		static void Main(string[] args)
		{
			Tests.TestCardRandomness(10000, 1000);
		}
	}
}
