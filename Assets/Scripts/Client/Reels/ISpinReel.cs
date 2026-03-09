namespace Client.Reels
{
	/// <summary>
	/// Interface in charge of the spin animation
	/// </summary>
	public interface ISpinReel
	{
		/// <summary>
		/// Add your reel spin logic here
		/// </summary>
		public void SpinReel();

		/// <summary>
		/// Notifies reels to start spinning
		/// </summary>
		public void StartSpinning();

		/// <summary>
		/// Notifies reels to stop spinning
		/// </summary>
		public void StopSpinning();
	}
}
