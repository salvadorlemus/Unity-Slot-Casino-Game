using System.Collections.Generic;
using Client.Reels;
using UnityEngine;

namespace Client.Symbols
{
	public class SymbolsFactory : MonoBehaviour
	{
		[SerializeField]
		private List<ReelUsher> reelSpinners;
		[SerializeField]
		private List<SymbolsSo>  symbols;
		[SerializeField]
		private GameObject symbolPrefab;

		private Dictionary<char, SymbolsSo> _symbolsDictionary;

		private void Start()
		{
			if (reelSpinners == null || reelSpinners.Count == 0)
			{
				reelSpinners = new List<ReelUsher>();
				reelSpinners.AddRange(FindObjectsByType<ReelUsher>(FindObjectsInactive.Exclude, FindObjectsSortMode
					.None));
			}

			_symbolsDictionary = new Dictionary<char, SymbolsSo>();
			foreach (SymbolsSo symbolsSo in symbols)
			{
				char symbolName = symbolsSo.symbolName;
				_symbolsDictionary.TryAdd(symbolName, symbolsSo);
			}
		}

		/// <summary>
		/// Return the SO asociated with that char
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns></returns>
		public SymbolsSo GetSymbolsSo(char symbol)
		{
			_symbolsDictionary.TryGetValue(symbol, out SymbolsSo symbolsSo);
			return symbolsSo;
		}

		/// <summary>
		/// Creates a givven symbol by name
		/// </summary>
		/// <param name="symbolName"></param>
		/// <returns></returns>
		private Symbol CreateSymbol(char symbolName)
		{
			GameObject newSymbol = Instantiate(symbolPrefab, transform, true);
			newSymbol.TryGetComponent<Symbol>(out Symbol symbolComponent);

			symbolComponent.UpdateSymbol(new SymbolDefinition(symbolName), this);
			return newSymbol.GetComponent<Symbol>();
		}

		/// <summary>
		/// Fills the given reel using the initial information
		/// </summary>
		public void FillReel(ReelUsher reelUsher, char[] visibleSymbols)
		{
			RectTransform parentTransform = reelUsher.GetComponent<RectTransform>();
			float symbolY = parentTransform.sizeDelta.y / 2 - Constants.SymbolHeight / 2;

			for(int i = 0; i < Constants.NumberOfSymbolsInReels; i++)
			{
				Symbol freeSymbol = CreateSymbol(visibleSymbols[i]);
				freeSymbol.transform.SetParent(reelUsher.transform);

				RectTransform symbolRectTransform = freeSymbol.GetComponent<RectTransform>();

				symbolRectTransform.localPosition =
					new Vector3(0,
						symbolY - (Constants.SymbolHeight + Constants.SpaceBetweenSymbols) * i,
						0);

				symbolRectTransform.rotation = Quaternion.identity;
				symbolRectTransform.localScale = Vector3.one;

				symbolRectTransform.sizeDelta = new Vector2(Constants.SymbolWidth, Constants.SymbolHeight);
			}
		}
	}
}
