using UnityEngine;

namespace Client.Symbols
{
	[CreateAssetMenu(fileName = "Symbol", menuName = "Reels/SymbolSo", order = 1)]
	public class SymbolsSo : ScriptableObject
	{
		public char symbolName;
		public Sprite symbolImage;
	}
}
