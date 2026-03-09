using System;

namespace Server.Symbols
{
	/// <summary>
	/// Classs that holds symbol def data
	/// </summary>
	public class SymbolDefinition
	{
		private readonly Char[] _symbols;

		public Char[] Symbols => _symbols;

		public SymbolDefinition(string symbols)
		{
			_symbols =  symbols.ToCharArray();
		}
	}
}
