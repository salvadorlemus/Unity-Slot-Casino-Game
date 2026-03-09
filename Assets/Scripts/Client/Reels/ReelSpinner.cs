using System.Collections.Generic;
using Client.Symbols;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Reels
{
	/// <summary>
	/// Class in charge of spining the reels
	/// </summary>
	[DisallowMultipleComponent]
	public class ReelSpinner : MonoBehaviour, ISpinReel
	{
		private ReelUsher _reelUsher;

		private List<RectTransform> _symbolsRectTransform;

		private RectTransform _reelRectTransform;

		private RectTransform _firstSymbolInBetWindowRect;

		private float _reeLowerLimit;

		private float _firstSymbolVisiblePosition;

		private bool _initialized;

		private bool _spinReels;

		private bool _stopReels;

		private bool _forceSymbolIndex;

		public void Init()
		{
			_reelUsher = GetComponent<ReelUsher>();
			_reelRectTransform  = GetComponent<RectTransform>();
			_reeLowerLimit = -_reelRectTransform.sizeDelta.y / 2 - Constants.SpaceBetweenSymbols;

			_firstSymbolVisiblePosition =
				_reeLowerLimit + (Constants.SymbolHeight + Constants.SpaceBetweenSymbols) * 2 +
				Constants.SymbolHeight / 3f;

			_symbolsRectTransform =  new List<RectTransform>();
			foreach (Symbol symbol in _reelUsher.SymbolsInReel)
			{
				_symbolsRectTransform.Add(symbol.GetComponent<RectTransform>());
			}

			_initialized = true;
		}

		private void Update()
		{
			if (!_initialized) return;

			SpinReel();
		}

		public void StartSpinning()
		{
			_spinReels = true;
		}

		public void StopSpinning()
		{
			_stopReels = true;
		}

		public void SpinReel()
		{
			if(!_spinReels) return;

			// foreach (RectTransform symbolRect in _symbolsRectTransform) {
			for(int i = 0; i < _symbolsRectTransform.Count; i++)
			{
				// moving symbols down
				float currentReelSpeed = Constants.ReelSpinVelocity * Time.deltaTime;

				_symbolsRectTransform[i].localPosition -= Vector3.up * currentReelSpeed;

				// move symbol up to the top if it's go down and no longer visible
				if (_symbolsRectTransform[i].localPosition.y <= _reeLowerLimit)
				{
					Vector3 currentPosition = _symbolsRectTransform[i].localPosition;
					_symbolsRectTransform[i].localPosition =
						new Vector3(0,
							currentPosition.y + (Constants.SpaceBetweenSymbols + Constants.SymbolHeight)
							* Constants.NumberOfSymbolsInReels,
							0);

					_symbolsRectTransform[i].transform.SetAsFirstSibling();

					_reelUsher.UpdateSymbolInReel(i);
				}

				if (!_stopReels) continue;

				if (!_forceSymbolIndex)
				{
					_reelUsher.ForceLastSymbolIndex();
					_forceSymbolIndex = true;
				}

				if (_reelUsher.LastSymbolIndex == _reelUsher.SymbolsInBetWindowIndex[3] &&
				    _firstSymbolInBetWindowRect == null)
				{
					_firstSymbolInBetWindowRect =  _symbolsRectTransform[i];
				}

				if (_firstSymbolInBetWindowRect != null && _firstSymbolInBetWindowRect.localPosition.y <=
					_firstSymbolVisiblePosition)
				{
					_spinReels = false;
					_stopReels = false;
					_forceSymbolIndex = false;
					_firstSymbolInBetWindowRect = null;
				}
			}
		}
	}
}
