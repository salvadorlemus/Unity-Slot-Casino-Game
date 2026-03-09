using Miscellaneous.Loggers;

namespace Client.FSM
{
	/// <summary>
	/// Class that holds game states
	/// </summary>
	public static class GameStates
	{
		private static States _state;

		public static States State => _state;

		/// <summary>
		/// Update current game state
		/// </summary>
		/// <param name="newState"></param>
		public static void UpdateState(States newState)
		{
			_state = newState;
		}
	}
}
