using System;
using System.Collections.Generic;

namespace holdem_odds
{
	class Program
	{
		static void Main(string[] args)
		{
			Tests.TestFrequencies(50000, 1000, 5);
		}
	}
}
