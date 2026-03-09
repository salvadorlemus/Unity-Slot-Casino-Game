namespace Server.Player
{
	/// <summary>
	/// Class that holds all the data for the given player
	/// </summary>
	public class PlayerData
	{
		private decimal playerCredits;
		public decimal PlayerCredits => playerCredits;

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="playerCredits"></param>
		public PlayerData(decimal playerCredits)
		{
			this.playerCredits = playerCredits;
		}

		/// <summary>
		/// Decreases player credits by given amiount
		/// </summary>
		/// <param name="amount"></param>
		public void DecreasePlayerCredits(decimal amount)
		{
			playerCredits -= amount;
		}

		/// <summary>
		/// True if user has enough credits to make a Spin
		/// </summary>
		/// <returns></returns>
		public bool HasEnoughCredits()
		{
			return playerCredits >= Constants.SpinCost;
		}

		public void UpdatePlayerCredits(decimal amount)
		{
			playerCredits += amount;
		}
	}
}
