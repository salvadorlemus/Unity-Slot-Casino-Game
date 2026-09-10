using System.Collections.Generic;
using Client.Symbols;
using UnityEngine;

namespace Client.Reels
{
	/// <summary>
	/// Class in charge of spinning the reels.
	///
	/// The reel is driven by a single scalar (<see cref="_scroll"/>) and every symbol position is
	/// *derived* from it each frame, instead of integrating each symbol independently. That keeps
	/// the symbols perfectly evenly spaced no matter how long the game runs (no per-symbol rounding
	/// drift), and when the reel stops <see cref="_scroll"/> is snapped to the exact grid-aligned
	/// value so every spin ends pixel-perfect on the pay line.
	/// </summary>
	[DisallowMultipleComponent]
	public class ReelSpinner : MonoBehaviour, ISpinReel
	{
		private ReelUsher _reelUsher;

		private List<RectTransform> _symbolsRectTransform;

		private RectTransform _reelRectTransform;

		private RectTransform _firstSymbolInBetWindowRect;

		/// <summary>Ideal Y of each symbol when <see cref="_scroll"/> is 0 (a clean, evenly spaced grid).</summary>
		private float[] _slotBaseY;

		/// <summary>How many whole reel-heights each symbol is folded by; used to detect recycles.</summary>
		private int[] _fold;

		private float _reeLowerLimit;

		private float _firstSymbolVisiblePosition;

		/// <summary>Distance between two consecutive symbol slots.</summary>
		private float _step;

		/// <summary>Length of the whole reel loop (one slot per symbol in the reel).</summary>
		private float _reelHeight;

		/// <summary>Residue of the slot grid modulo <see cref="_step"/> (shared by every slot).</summary>
		private float _slotPhase;

		/// <summary>How far the reel has scrolled. All symbol positions are derived from this.</summary>
		private float _scroll;

		private bool _initialized;

		private bool _spinReels;

		private bool _stopRequested;

		private bool _forcedIndex;

		// Settle: the short ease that rewinds the reel onto the grid at the end of a spin.
		private bool _settling;

		private float _settleFrom;

		private float _settleTo;

		private float _settleTime;

		public void Init()
		{
			_reelUsher = GetComponent<ReelUsher>();
			_reelRectTransform = GetComponent<RectTransform>();

			_step = Constants.SymbolHeight + Constants.SpaceBetweenSymbols;
			_reeLowerLimit = -_reelRectTransform.sizeDelta.y / 2f - Constants.SpaceBetweenSymbols;

			_firstSymbolVisiblePosition =
				_reeLowerLimit + _step * 2f + Constants.SymbolHeight / 3f;

			_symbolsRectTransform = new List<RectTransform>();
			foreach (Symbol symbol in _reelUsher.SymbolsInReel)
			{
				_symbolsRectTransform.Add(symbol.GetComponent<RectTransform>());
			}

			int count = _symbolsRectTransform.Count;
			_reelHeight = count * _step;

			// Rebuild the ideal grid from the same formula the factory used to lay symbols out, so
			// the grid is exact and independent of any position drift accumulated at runtime.
			float topSlotY = _reelRectTransform.sizeDelta.y / 2f - Constants.SymbolHeight / 2f;
			_slotBaseY = new float[count];
			_fold = new int[count];
			for (int i = 0; i < count; i++)
			{
				float ideal = topSlotY - _step * i;
				_slotBaseY[i] = _reeLowerLimit + Mathf.Repeat(ideal - _reeLowerLimit, _reelHeight);
				_fold[i] = 0;
			}

			_slotPhase = Mathf.Repeat(_slotBaseY[0] - _reeLowerLimit, _step);

			_scroll = 0f;
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
			_stopRequested = true;
		}

		public void SpinReel()
		{
			if (!_spinReels) return;

			if (_settling)
			{
				TickSettle();
				return;
			}

			_scroll += Constants.ReelSpinVelocity * Time.deltaTime;
			ApplyPositions();

			if (!_stopRequested) return;

			int[] window = _reelUsher.SymbolsInBetWindowIndex;
			bool windowValid = window != null && window.Length >= 5 && window[3] >= 0 && window[4] >= 0;

			if (!_forcedIndex)
			{
				// Only force the strip index when we actually found the pattern on this reel;
				// forcing it with an invalid ( -1 ) index corrupts the reel for later spins.
				if (windowValid)
					_reelUsher.ForceLastSymbolIndex();

				_forcedIndex = true;
			}

			// If the pattern is not on this reel's strip, fall back to a plain aligned stop so the
			// reel still terminates instead of spinning forever.
			if (_firstSymbolInBetWindowRect == null && !windowValid)
				_firstSymbolInBetWindowRect = TopmostSymbol();

			if (_firstSymbolInBetWindowRect != null &&
			    _firstSymbolInBetWindowRect.localPosition.y <= _firstSymbolVisiblePosition)
			{
				BeginSettle();
			}
		}

		/// <summary>
		/// Places every symbol from <see cref="_scroll"/> and fires a recycle whenever a symbol
		/// wraps from the bottom of the reel back to the top.
		/// </summary>
		private void ApplyPositions()
		{
			int[] window = _reelUsher.SymbolsInBetWindowIndex;
			bool windowValid = window != null && window.Length >= 5 && window[3] >= 0;

			for (int i = 0; i < _symbolsRectTransform.Count; i++)
			{
				float rel = _slotBaseY[i] - _reeLowerLimit - _scroll;
				int fold = Mathf.FloorToInt(rel / _reelHeight);

				// Every drop in "fold" means this symbol fell off the bottom and re-enters on top.
				while (_fold[i] > fold)
				{
					_fold[i]--;
					_symbolsRectTransform[i].SetAsFirstSibling();
					_reelUsher.UpdateSymbolInReel(i);

					// Latch the symbol that now carries the bottom row of the bet window; the reel
					// stops once it has travelled down to the pay line.
					if (_stopRequested && _forcedIndex && _firstSymbolInBetWindowRect == null &&
					    windowValid && _reelUsher.LastSymbolIndex == window[3])
					{
						_firstSymbolInBetWindowRect = _symbolsRectTransform[i];
					}
				}

				float y = _reeLowerLimit + (rel - fold * _reelHeight);
				_symbolsRectTransform[i].localPosition = new Vector3(0f, y, 0f);
			}
		}

		private RectTransform TopmostSymbol()
		{
			RectTransform best = _symbolsRectTransform[0];
			for (int i = 1; i < _symbolsRectTransform.Count; i++)
			{
				if (_symbolsRectTransform[i].localPosition.y > best.localPosition.y)
					best = _symbolsRectTransform[i];
			}

			return best;
		}

		/// <summary>
		/// Starts the short settle that rewinds <see cref="_scroll"/> to the closest grid-aligned
		/// value at or below the current one, so the symbols land exactly on their slots. Rewinding
		/// (never advancing) keeps the symbol contents chosen by the stop logic in place.
		/// </summary>
		private void BeginSettle()
		{
			// _scroll ≡ targetResidue (mod _step) puts a symbol exactly on _firstSymbolVisiblePosition.
			float targetResidue =
				Mathf.Repeat(_slotPhase - (_firstSymbolVisiblePosition - _reeLowerLimit), _step);
			float rewind = Mathf.Repeat(Mathf.Repeat(_scroll, _step) - targetResidue, _step);

			_settleFrom = _scroll;
			_settleTo = _scroll - rewind;
			_settleTime = 0f;
			_settling = true;
		}

		private void TickSettle()
		{
			_settleTime += Time.deltaTime / Mathf.Max(Constants.ReelStopSettleTime, 0.0001f);
			float k = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_settleTime));
			_scroll = Mathf.Lerp(_settleFrom, _settleTo, k);
			ApplyPositions();

			if (_settleTime < 1f) return;

			// Land exactly on the grid, then re-base _scroll so it can never grow without bound.
			_scroll = Mathf.Repeat(_settleTo, _reelHeight);
			for (int i = 0; i < _symbolsRectTransform.Count; i++)
			{
				float rel = _slotBaseY[i] - _reeLowerLimit - _scroll;
				_fold[i] = Mathf.FloorToInt(rel / _reelHeight);
			}

			ApplyPositions();

			_spinReels = false;
			_stopRequested = false;
			_forcedIndex = false;
			_settling = false;
			_firstSymbolInBetWindowRect = null;
		}
	}
}
