namespace Server
{
	public class Constants
	{
		public static readonly int BetWindowRows = 3;
		public static readonly int BetWindowColumns = 5;

		public static readonly int MaxBetWindows = 10;

		public static readonly decimal PlayerInitialCredits = 50;
		public static readonly decimal SpinCost = 5;

		public static readonly string ReelOneSymbols = "OBWCPLGPBBOGLL";
		public static readonly string ReelTwoSymbols = "WCBPCGOLLLCLPLC";
		public static readonly string ReelTreeSymbols = "GWPGBLCBLLOOG";
		public static readonly string ReelFourSymbols = "LPPLGOWWBCCLOPL";
		public static readonly string ReelFiveSymbols = "GCBWOOLPOLGBWC";

		public static readonly string ReelDefinition = "BWGPOLC";

		public static readonly int ServerLatencyMS = 100;
	}
}
