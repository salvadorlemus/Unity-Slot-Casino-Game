using System.Collections.Generic;
using System.Numerics;

namespace Server.BetWindow
{
	/// <summary>
	/// Class that holds all the necesary information from a bet window
	/// </summary>
	public class BetWindow
	{
		/// <summary>
		/// BetWindow for this spin
		/// </summary>
		public readonly char[][] betWindow;

		/// <summary>
		/// Winlines in betWindow if any
		/// </summary>
		public readonly Dictionary<int, Vector2[]> winLines;

		/// <summary>
		/// Credits won in this round
		/// </summary>
		public decimal Credits;

		/// <summary>
		/// How much money the user won
		/// </summary>
		public decimal Payout;

		/// <summary>
		/// Current player bet
		/// </summary>
		public decimal Bet;

		public BetWindow(char[][] betWindow, Dictionary<int, Vector2[]> winLines, decimal credits, decimal payout, decimal bet)
		{
			this.betWindow = betWindow;
			this.winLines = winLines;
			this.Credits = credits;
			this.Payout = payout;
			this.Bet = bet;
		}
	}
}
