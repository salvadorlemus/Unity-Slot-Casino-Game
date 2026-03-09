using System.Collections.Generic;
using Miscellaneous.Loggers;
using Server.Symbols;
using Vector2 = System.Numerics.Vector2;

namespace Server.Winlines
{
	/// <summary>
	/// Class that holds Winlines data
	/// </summary>
	public class WinLineData
	{
		/// <summary>
		/// Stores the credits that each symbol count in a winline holds
		/// </summary>
		private readonly Dictionary<char, decimal[][]> _creditsInWinLine;

		/// <summary>
		/// Holds win line patterns to compare to
		/// </summary>
		private readonly Dictionary<int, Vector2[]> _winLinePattern;

		private decimal _payout = 0;

		public decimal Payout => _payout;

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="symbolDefinition"></param>
		public WinLineData(SymbolDefinition symbolDefinition)
		{
			_creditsInWinLine = new Dictionary<char, decimal[][]>();
			_winLinePattern =  new Dictionary<int, Vector2[]>();

			InitWinLineCredits(symbolDefinition);
			InitWinLinePatterns();
		}

		/// <summary>
		/// Initializes the win line data credits
		/// </summary>
		/// <param name="symbolDefinition"></param>
		private void InitWinLineCredits(SymbolDefinition symbolDefinition)
		{
			// BELL
			_creditsInWinLine.TryAdd(symbolDefinition.Symbols[0], new decimal[][]
			{
				new decimal[]{2m,25m},
				new decimal[]{3m,50m},
				new decimal[]{4m,75m},
				new decimal[]{5m,100m}
			});
			// WATER MELLON
			_creditsInWinLine.TryAdd(symbolDefinition.Symbols[1], new decimal[][]
			{
				new decimal[]{2m,10m},
				new decimal[]{3m,20m},
				new decimal[]{4m,30m},
				new decimal[]{5m,60m}
			});
			// GRAPE
			_creditsInWinLine.TryAdd(symbolDefinition.Symbols[2], new decimal[][]
			{
				new decimal[]{2m,5m},
				new decimal[]{3m,10m},
				new decimal[]{4m,20m},
				new decimal[]{5m,50m}
			});
			// PLUM
			_creditsInWinLine.TryAdd(symbolDefinition.Symbols[3], new decimal[][]
			{
				new decimal[]{2m,5m},
				new decimal[]{3m,10m},
				new decimal[]{4m,20m},
				new decimal[]{5m,40m}
			});
			// ORANGE
			_creditsInWinLine.TryAdd(symbolDefinition.Symbols[4], new decimal[][]
			{
				new decimal[]{2m,5m},
				new decimal[]{3m,10m},
				new decimal[]{4m,15m},
				new decimal[]{5m,30m}
			});
			// LEMON
			_creditsInWinLine.TryAdd(symbolDefinition.Symbols[5], new decimal[][]
			{
				new decimal[]{2m,2m},
				new decimal[]{3m,5m},
				new decimal[]{4m,10m},
				new decimal[]{5m,20m}
			});
			// CHERRY
			_creditsInWinLine.TryAdd(symbolDefinition.Symbols[6], new decimal[][]
			{
				new decimal[]{2m,1m},
				new decimal[]{3m,2m},
				new decimal[]{4m,5m},
				new decimal[]{5m,10m}
			});
		}

		/// <summary>
		/// Inits the win line patterns
		/// </summary>
		private void InitWinLinePatterns()
		{
			// Init winlines patterns
			AddWinLinePattern(0, new[]
			{
				new Vector2(0, 0),
				new Vector2(0, 1),
				new Vector2(0, 2),
				new Vector2(0, 3),
				new Vector2(0, 4)
			});

			AddWinLinePattern(1, new[]
			{
				new Vector2(1, 0),
				new Vector2(1, 1),
				new Vector2(1, 2),
				new Vector2(1, 3),
				new Vector2(1, 4)
			});

			AddWinLinePattern(2, new[]
			{
				new Vector2(2, 0),
				new Vector2(2, 1),
				new Vector2(2, 2),
				new Vector2(2, 3),
				new Vector2(2, 4)
			});

			AddWinLinePattern(3, new[]
			{
				new Vector2(2, 0),
				new Vector2(0, 1),
				new Vector2(2, 2),
				new Vector2(0, 3),
				new Vector2(2, 4),
			});

			AddWinLinePattern(4, new[]
			{
				new Vector2(0, 0),
				new Vector2(2, 1),
				new Vector2(0, 2),
				new Vector2(2, 3),
				new Vector2(0, 4),
			});

			AddWinLinePattern(5, new[]
			{
				new Vector2(0, 0),
				new Vector2(1, 1),
				new Vector2(2, 2),
				new Vector2(1, 3),
				new Vector2(0, 4),
			});

			AddWinLinePattern(6, new[]
			{
				new Vector2(2, 0),
				new Vector2(1, 1),
				new Vector2(0, 2),
				new Vector2(1, 3),
				new Vector2(2, 4),
			});

			AddWinLinePattern(7, new[]
			{
				new Vector2(0, 0),
				new Vector2(0, 1),
				new Vector2(1, 2),
				new Vector2(2, 3),
				new Vector2(2, 4),
			});

			AddWinLinePattern(8, new[]
			{
				new Vector2(2, 0),
				new Vector2(2, 1),
				new Vector2(1, 2),
				new Vector2(0, 3),
				new Vector2(0, 4),
			});
		}

		/// <summary>
		/// Adds a win line pattern to our dictionary
		/// </summary>
		/// <param name="id"></param>
		/// <param name="pattern"></param>
		private void AddWinLinePattern(int id, Vector2[] pattern)
		{
			if (!_winLinePattern.TryAdd(id, pattern))
			{
				Logger.Log($"Pattern with id {id} already exists!");
			}
		}

		/// <summary>
		/// Return a dictionary of winner patterns if any
		/// </summary>
		public Dictionary<int, Vector2[]> GetWinLinesPattern(char[][] betWindow)
		{
			Dictionary<int, Vector2[]> winLines = new Dictionary<int, Vector2[]>();

			_payout = 0;
			foreach (var kvp in _winLinePattern)
			{
				int id = kvp.Key;
				Vector2[] patterns = kvp.Value;

				int X = (int)patterns[0].X;
				int Y = (int)patterns[0].Y;

				char firstChar = betWindow[X][Y];

				int numberOfSymbolMatches = 0;

				Vector2[] newPattern = new Vector2[patterns.Length];

				for(int i = 0; i < newPattern.Length; i++)
				{
					newPattern[i] = new Vector2(-1);
				}

				numberOfSymbolMatches = 0;

				foreach (Vector2 pattern in patterns)
				{
					X = (int)pattern.X;
					Y = (int)pattern.Y;

					char currentChar = betWindow[X][Y];

					if (currentChar != firstChar)
					{
						break;
					}

					newPattern[numberOfSymbolMatches] = pattern;
					numberOfSymbolMatches++;
				}

				if (numberOfSymbolMatches < 2) continue;

				winLines[id] = newPattern;
				int payoutRow = numberOfSymbolMatches - 2;
				_payout += GetCreditsInWinPattern(firstChar, payoutRow);
			}
			return winLines;
		}

		/// <summary>
		/// Returns the credits for the given symbol and number of symbols in win pattern
		/// </summary>
		/// <param name="symbol"></param>
		/// <param name="numberPfSymbolsInPattern"></param>
		/// <returns></returns>
		private decimal GetCreditsInWinPattern(char symbol, int numberPfSymbolsInPattern)
		{
			if(numberPfSymbolsInPattern < 0) return 0m;

			if (!_creditsInWinLine.TryGetValue(symbol, out decimal[][] table))
			{
				Logger.LogError($"Symbol {symbol} not found!");
				return 0m;
			}

			if (numberPfSymbolsInPattern < 0 || numberPfSymbolsInPattern >= table.Length)
			{
				Logger.LogError($"Row index {numberPfSymbolsInPattern} is out of range!");
				return 0m;
			}

			return table[numberPfSymbolsInPattern][1];
		}

	}
}
