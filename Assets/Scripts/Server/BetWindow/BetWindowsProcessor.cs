using System.Collections.Generic;
using System.Text;
using Miscellaneous.Loggers;
using Server.Player;
using Server.Symbols;
using Server.Winlines;
using Random = System.Random;

namespace Server.BetWindow
{
	/// <summary>
	/// Class that holds BetWindows to be passed to the server and then to the client
	/// </summary>
	public class BetWindowsProcessor
	{
		/// <summary>
		/// Ductionary for not winner random bet windows
		/// </summary>
		private readonly Dictionary<int, char[]> betWindows;

		private readonly WinLineData _winlineData;
		private PlayerData _playerData;

		private readonly SymbolsHelper _symbolHelper;
		public SymbolsHelper SymbolHelper => _symbolHelper;

		private int _currentBetWindowId = 0;

		private readonly Random _random;

		public BetWindowsProcessor()
		{
			_random = new Random();

			SymbolDefinition symbolDefinition = new SymbolDefinition(Constants.ReelDefinition);
			// All symbols in game
			_winlineData = new WinLineData(symbolDefinition);
			_symbolHelper = new SymbolsHelper(symbolDefinition);

			betWindows = new Dictionary<int, char[]>();

			InitializeRandomBetWindows();
		}

		private void InitializeRandomBetWindows()
		{
			// Fill the _noWinnerWindows dictionary with random data
			for (int i = 0; i < Constants.MaxBetWindows; i++)
			{
				int reelOneSymbolIndex = _random.Next(_symbolHelper.ReelOneLength);
				int reelTwoSymbolIndex = _random.Next(_symbolHelper.ReelTwoLength);
				int reelTreeSymbolIndex = _random.Next(_symbolHelper.ReelTreeLength);
				int reelFourSymbolIndex = _random.Next(_symbolHelper.ReelFourLength);
				int reelFiveSymbolIndex = _random.Next(_symbolHelper.ReelFiveLength);

				char[] betWindow = new char[]
				{
					_symbolHelper.GetSymbol(0, reelOneSymbolIndex)
					, _symbolHelper.GetSymbol(0, reelOneSymbolIndex + 1)
					, _symbolHelper.GetSymbol(0, reelOneSymbolIndex + 2)
					, _symbolHelper.GetSymbol(1, reelTwoSymbolIndex)
					, _symbolHelper.GetSymbol(1, reelTwoSymbolIndex + 1)
					, _symbolHelper.GetSymbol(1, reelTwoSymbolIndex + 2)
					, _symbolHelper.GetSymbol(2, reelTreeSymbolIndex)
					, _symbolHelper.GetSymbol(2, reelTreeSymbolIndex + 1)
					, _symbolHelper.GetSymbol(2, reelTreeSymbolIndex + 2)
					, _symbolHelper.GetSymbol(3, reelFourSymbolIndex)
					, _symbolHelper.GetSymbol(3, reelFourSymbolIndex + 1)
					, _symbolHelper.GetSymbol(3, reelFourSymbolIndex + 2)
					, _symbolHelper.GetSymbol(4, reelFiveSymbolIndex)
					, _symbolHelper.GetSymbol(4, reelFiveSymbolIndex + 1)
					, _symbolHelper.GetSymbol(4, reelFiveSymbolIndex + 2),
				};

				betWindows.TryAdd(i, betWindow);
			}
		}

		/// <summary>
		/// Returns a new Bet Window from our windows pool
		/// </summary>
		public BetWindow GetBetWindow(PlayerData playerData)
		{
			_playerData = playerData;
			_currentBetWindowId = _random.Next(Constants.MaxBetWindows);
			betWindows.TryGetValue(_currentBetWindowId, out char[] window);

			char [][] formatBetWindow = FormatBetWindow(window);
			PrintWindow(formatBetWindow);

			_playerData.DecreasePlayerCredits(Constants.SpinCost);

			BetWindow betWindow =
				new BetWindow(formatBetWindow, _winlineData.GetWinLinesPattern(formatBetWindow), _playerData
					.PlayerCredits, _winlineData.Payout, Constants.SpinCost);

			if(_winlineData.Payout > 0)
				_playerData.UpdatePlayerCredits(_winlineData.Payout);

			return betWindow;
		}

		/// <summary>
		/// Logs the current bet window
		/// </summary>
		/// <param name="betWindow"></param>
		private void PrintWindow(char[][] betWindow)
		{
			if (betWindow == null || betWindow.Length == 0)
			{
				Logger.Log("Window is empty.");
				return;
			}

			StringBuilder sb = new StringBuilder();

			for (int row = 0; row < betWindow.Length; row++)
			{
				if (betWindow[row] == null)
					continue;

				sb.AppendFormat("\n");
				for (int col = 0; col < betWindow[row].Length; col++)
				{
					sb.AppendFormat($"|{betWindow[row][col]}|");
				}
			}

			Logger.Log(sb.ToString());
		}

		private char[][] FormatBetWindow(char[] betWindow)
		{
			int rows = Constants.BetWindowRows;
			int cols = Constants.BetWindowColumns;

			// Create jagged array
			char[][] result = new char[rows][];

			for (int row = 0; row < rows; row++)
			{
				result[row] = new char[cols];

				for (int col = 0; col < cols; col++)
				{
					// Column-major fill (same logic you had)
					result[row][col] = betWindow[col * rows + row];
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the initial data for the initial state of the game
		/// </summary>
		/// <param name="playerData"></param>
		/// <returns></returns>
		public BetWindow GetInitialData()
		{
			return new BetWindow(null, null, Constants.PlayerInitialCredits, 0m , Constants.SpinCost);
		}
	}
}
