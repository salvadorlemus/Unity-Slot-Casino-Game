namespace Client.Reels
{
	/// <summary>
	/// Class that holds reels constants values
	/// </summary>
	public static class Constants
	{
		public static readonly int SymbolWidth = 275;
		public static readonly int SymbolHeight = 275;
		public static readonly int SpaceBetweenSymbols = 25;

		public static readonly int NumberOfSymbolsInReels = 6;

		public static readonly int ReelSpinVelocity = 8000;

		/// <summary>
		/// Seconds the reel takes to ease onto its final grid-aligned position when it stops.
		/// </summary>
		public static readonly float ReelStopSettleTime = 0.1f;
	}
}
