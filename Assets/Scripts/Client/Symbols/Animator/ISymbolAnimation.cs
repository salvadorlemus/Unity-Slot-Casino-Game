namespace Client.Symbols.Animator
{
	/// <summary>
	/// Interface in charge of animate symbols in the reels.
	/// </summary>
	public interface ISymbolAnimation
	{
		/// <summary>
		/// Starts the animation of the symbols in the reels.
		/// </summary>
		public void StartAnimation();

		/// <summary>
		/// Stops the animation of the symbols in the reels.
		/// </summary>
		public void StopAnimation();

		/// <summary>
		/// Obsqures the symbol, darken all non wining symbols.
		/// </summary>
		public void ObsqureSymbol();
	}
}
