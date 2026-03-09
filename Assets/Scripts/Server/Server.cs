using System;
using System.Threading.Tasks;
using Server.BetWindow;
using Server.Player;
using Server.Symbols;

namespace Server
{
	/// <summary>
	/// Class that acts as a server to handle user request
	/// and server responses.
	///
	/// It takes the responses from a pool of possible symbol windows
	/// Also, it uses tasks to simulate latency for the server response.
	/// </summary>
	public class Server
	{
		private BetWindowsProcessor _betWindowsProcessor;
		private PlayerData _playerData;
		private SymbolsHelper _symbolHelper;

		private bool _initialized = false;

		public Server()
		{
			_playerData  = new PlayerData(Constants.PlayerInitialCredits);
			_betWindowsProcessor = new BetWindowsProcessor();
		}

		/// <summary>
		/// Inits the Server
		/// </summary>
		public void Init()
		{
			Miscellaneous.Loggers.Logger.Log("Starting Server");
			_initialized = true;
		}

		/// <summary>
		/// Returns a new Bet Window
		/// </summary>
		public async Task<BetWindow.BetWindow> GetBetWindow()
		{
			if(!_initialized)
					throw new InvalidOperationException("Server not initialized");
			// Simulates server latency
			await Task.Delay(Constants.ServerLatencyMS);
			return _betWindowsProcessor.GetBetWindow(_playerData);
		}

		/// <summary>
		/// Returns all the symbols in reel
		/// </summary>
		/// <param name="reelId"></param>
		/// <returns></returns>
		public async Task<char[]> GetAllSymbolsInReel(int reelId)
		{
			if(!_initialized)
				throw new InvalidOperationException("Server not initialized");
			// Simulates server latency
			await Task.Delay(Constants.ServerLatencyMS);
			return _betWindowsProcessor.SymbolHelper.GetSymbolsInReel(reelId);
		}

		/// <summary>
		/// Receives the player bet to start the spin
		/// </summary>
		/// <param name="playerBet"></param>
		/// <returns></returns>
		public async Task<bool> PlayerHasEnoughCredits()
		{
			if(!_initialized)
				throw new InvalidOperationException("Server not initialized");
			// Simulates server latency
			await Task.Delay(Constants.ServerLatencyMS);

			return _playerData.HasEnoughCredits();
		}

		public async Task<BetWindow.BetWindow> GetInitialData()
		{
			if(!_initialized)
				throw new InvalidOperationException("Server not initialized");
			// Simulates server latency
			await Task.Delay(Constants.ServerLatencyMS);
			return _betWindowsProcessor.GetInitialData();
		}
	}
}
