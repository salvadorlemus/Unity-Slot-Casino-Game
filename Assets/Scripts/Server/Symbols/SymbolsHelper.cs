using System;
using System.Linq;
using Miscellaneous.Loggers;

namespace Server.Symbols
{
	/// <summary>
	/// Helperclass to manage symbols in the game, such as getting symbol definitions, symbol values, etc.
	/// </summary>
	public class SymbolsHelper
	{
		private readonly SymbolDefinition _symbolDefinition;

		private readonly char[] _reelOneSymbols;
		private readonly char[] _reelTwoSymbols;
		private readonly char[] _reelTreeSymbols;
		private readonly char[] _reelFourSymbols;
		private readonly char[] _reelFiveSymbols;

		public int ReelOneLength => _reelOneSymbols.Length;
		public int ReelTwoLength => _reelTwoSymbols.Length;
		public int ReelTreeLength => _reelTreeSymbols.Length;
		public int ReelFourLength => _reelFourSymbols.Length;
		public int ReelFiveLength => _reelFiveSymbols.Length;

		public SymbolsHelper(SymbolDefinition symbolDefinition)
		{
			_symbolDefinition = symbolDefinition;

			_reelOneSymbols = GetSymbolsInString(Constants.ReelOneSymbols);
			_reelTwoSymbols = GetSymbolsInString(Constants.ReelTwoSymbols);
			_reelTreeSymbols = GetSymbolsInString(Constants.ReelTreeSymbols);
			_reelFourSymbols = GetSymbolsInString(Constants.ReelFourSymbols);
			_reelFiveSymbols = GetSymbolsInString(Constants.ReelFiveSymbols);
		}

		/// <summary>
		/// returns the symbos in a given string
		/// </summary>
		/// <param name="symbols"></param>
		private char[] GetSymbolsInString(String symbols)
		{
			char[] arraySymbols = symbols.ToArray();

			if(SymbolsMatch(arraySymbols))
				return arraySymbols;

			return null;
		}

		/// <summary>
		/// Checks if all symbols in the array exist in the original symbols array
		/// </summary>
		/// <param name="symbolsInReel"></param>
		/// <returns></returns>
		private bool SymbolsMatch(char[] symbolsInReel)
		{
			bool currentSymbolExist = false;

			for (int i = 0; i < symbolsInReel.Length; i++)
			{
				currentSymbolExist = false;

				for (int j = 0; j < _symbolDefinition.Symbols.Length; j++)
				{
					if (_symbolDefinition.Symbols[j] == symbolsInReel[i])
						currentSymbolExist = true;
				}

				if (!currentSymbolExist)
				{
					Logger.LogError($"Symbol [{symbolsInReel[i]}] does not exist");
					return false;
				}
			}

			return true;
		}


		/// <summary>
		/// Returns a given symbol from the reel bases on reel index
		/// </summary>
		/// <param name="reelID"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public char GetSymbol(int reelID, int startIndex)
		{
			char[] symbolsInReel = null;

			switch (reelID)
			{
				case 0:
					symbolsInReel = _reelOneSymbols;
					break;
				case 1:
					symbolsInReel = _reelTwoSymbols;
					break;
				case 2:
					symbolsInReel = _reelTreeSymbols;
					break;
				case 3:
					symbolsInReel = _reelFourSymbols;
					break;
				case 4:
					symbolsInReel = _reelFiveSymbols;
					break;
			}

			if(symbolsInReel == null)
				return ' ';

			return symbolsInReel[startIndex % symbolsInReel.Length];
		}

		/// <summary>
		/// Returns all the symbols in the reel
		/// </summary>
		/// <param name="reelId"></param>
		/// <returns></returns>
		public char[] GetSymbolsInReel(int reelId)
		{
			switch (reelId)
			{
				case 0:
					return _reelOneSymbols;
				case 1:
					return _reelTwoSymbols;
				case 2:
					return _reelTreeSymbols;
				case 3:
					return _reelFourSymbols;
				case 4:
					return _reelFiveSymbols;
				default:
					return _reelOneSymbols;
			}
		}
	}
}
