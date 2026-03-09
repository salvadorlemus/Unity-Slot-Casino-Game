using Server.BetWindow;

namespace Client.FSM
{
	/// <summary>
	/// Class that holds in game events
	/// </summary>
	public class GameEvents
	{
		public delegate void Spin();
		public static Spin SpinEvent;
		public static Spin SpinEndEvent;

		public delegate void ServerResult(BetWindow betWindow);
		public static ServerResult ServerResultEvent;
	}
}
