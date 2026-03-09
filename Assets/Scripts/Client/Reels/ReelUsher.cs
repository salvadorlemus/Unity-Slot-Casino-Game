using System.Collections.Generic;
using Client.FSM;
using Client.Symbols;
using Server.BetWindow;
using UnityEngine;

namespace Client.Reels
{
	/// <summary>
	/// Class in charge of Accomodate all the relevant reel data like all the symbols in the reel and call to fill
	/// the reels once that data is provided
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(ReelSpinner))]
	public class ReelUsher : MonoBehaviour
	{
		[SerializeField]
		private int reelID;

		public int ReelID => reelID;

		[SerializeField]
		private List<Symbol> symbolsInReel;

		public List<Symbol> SymbolsInReel => symbolsInReel;

		private ReelData _reelData;

		private static SymbolsFactory _symbolsFactory;

		private int _lastSymbolIndex = 0;

		public int LastSymbolIndex => _lastSymbolIndex;

		private int[] _symbolsInBetWindowIndex;

		public int [] SymbolsInBetWindowIndex => _symbolsInBetWindowIndex;

		public void Init(char[] allSymbolsInReel)
		{
			if(_symbolsFactory==null)
				_symbolsFactory = FindAnyObjectByType<SymbolsFactory>();

			_reelData = new ReelData(allSymbolsInReel);
			_symbolsFactory.FillReel(this, _reelData.SymbolsInReel);
			symbolsInReel.AddRange(GetComponentsInChildren<Symbol>(true));

			_lastSymbolIndex = 0;
			GameEvents.ServerResultEvent += LookForSymbolsOnReel;
			GetComponent<ReelSpinner>().Init();
		}

		private void OnDisable()
		{
			GameEvents.ServerResultEvent -= LookForSymbolsOnReel;
		}

		/// <summary>
		/// Iterates through the array of symbos searching for a given pattern of chars
		/// </summary>
		/// <param name="betWindow"></param>
		/// <returns></returns>
		private void LookForSymbolsOnReel(BetWindow betWindow)
		{
			_symbolsInBetWindowIndex = new int[5] { -1, -1, -1, -1, -1 };

			if (_reelData.SymbolsInReel == null || betWindow == null)
				return;

			int rows = betWindow.betWindow.Length;
			char[] searchedSymbols = new char[rows];

			for (int i = 0; i < rows; i++)
			{
				searchedSymbols[i] = betWindow.betWindow[i][reelID];
			}

			int n = _reelData.SymbolsInReel.Length;

			for (int i = 0; i < n; i++)
			{
				if (_reelData.SymbolsInReel[i] == searchedSymbols[0] &&
				    _reelData.SymbolsInReel[(i + 1) % n] == searchedSymbols[1] &&
				    _reelData.SymbolsInReel[(i + 2) % n] == searchedSymbols[2])
				{
					int index0 = i;
					int index1 = (i + 1) % n;
					int index2 = (i + 2) % n;

					_symbolsInBetWindowIndex[0] = (index0 - 1 + n) % n;
					_symbolsInBetWindowIndex[1] = index0;
					_symbolsInBetWindowIndex[2] = index1;
					_symbolsInBetWindowIndex[3] = index2;
					_symbolsInBetWindowIndex[4] = (index2 + 1) % n;
				}
			}
		}

		/// <summary>
		/// Updates the symbol in the reel to the next symbol in the symbol list.
		/// </summary>
		/// <param name="symbolIndex"></param>
		public void UpdateSymbolInReel(int symbolIndex)
		{
			int n = _reelData.SymbolsInReel.Length;

			_lastSymbolIndex--;
			_lastSymbolIndex = (_lastSymbolIndex % n + n) % n;

			char nextSymbol = _reelData.SymbolsInReel[_lastSymbolIndex];

			symbolsInReel[symbolIndex].UpdateSymbol(new SymbolDefinition(nextSymbol), _symbolsFactory);
		}

		/// <summary>
		/// Forces the symbol index to match the searched symbol, so teh reel doesn't take long before
		/// stoing the spin
		/// </summary>
		public void ForceLastSymbolIndex()
		{
			_lastSymbolIndex = _symbolsInBetWindowIndex[4];
		}
	}
}
