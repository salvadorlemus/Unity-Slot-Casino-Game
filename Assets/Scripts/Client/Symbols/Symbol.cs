using Client.Symbols.Animator;
using UnityEngine.UI;
using UnityEngine;

namespace Client.Symbols
{
	/// <summary>
	/// Class acts as an entry point from Unity to the symbol definition
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	public class Symbol : MonoBehaviour
	{
		[SerializeField]
		private SymbolDefinition definition;

		[Space]
		[SerializeField]
		private Image symbolImage;

		private void Start()
		{
			GetComponent<SymbolAnimation>().Init(symbolImage);
		}

		/// <summary>
		/// Updates the symbol data as well as their visual representation
		/// </summary>
		public void UpdateSymbol(SymbolDefinition symbolDefinition, SymbolsFactory symbolFactory)
		{
			definition =  symbolDefinition;
			char symbolName = symbolDefinition.SymbolName;
			SymbolsSo symbolsSo = symbolFactory.GetSymbolsSo(symbolName);

			symbolImage.sprite = symbolsSo.symbolImage;
			transform.name = $"Symbol_" + symbolDefinition.SymbolName;
		}
	}
}
