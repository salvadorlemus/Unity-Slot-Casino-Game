using System;

namespace Client.Symbols
{
	/// <summary>
	/// Class that holds Symbol related data
	/// </summary>
	[Serializable]
	public class SymbolDefinition
	{
		private char _symbolName;
		public char SymbolName => _symbolName;

		public SymbolDefinition(char symbolName)
		{
			_symbolName = symbolName;
		}
	}
}
