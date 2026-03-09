namespace Client.Reels
{
	/// <summary>
	/// Class that holds the reel data like all symbols in reel
	/// </summary>
	public class ReelData
	{
		/// <summary>
		/// All the symbols in the reel in order, top to bottom
		/// </summary>
		private readonly char[] _symbolsInReel;

		public char[] SymbolsInReel => _symbolsInReel;

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="symbolsInReel"></param>
		public ReelData(char[] symbolsInReel)
		{
			_symbolsInReel = symbolsInReel;
		}
	}
}
