using System.Collections;
using System.Collections.Generic;
using Client.Reels;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Client.Symbols.Animator
{
	/// <summary>
	/// Class in charge of animate symbols in case of a winline
	/// </summary>
	public class SymbolAnimator : MonoBehaviour
	{
		public void PlaySymbolAnimations(List<ReelUsher> reelUsher, Dictionary<int, Vector2[]> betWindowWinLines)
		{
			foreach (ReelUsher usher in reelUsher)
			{
				foreach (var symbol in usher.SymbolsInReel)
					symbol.GetComponent<ISymbolAnimation>().ObsqureSymbol();
			}

			StartCoroutine(AnimateWinlines(reelUsher, betWindowWinLines));
		}

		/// <summary>
		/// Animate winlines in sequence, one by one with a delay between them. It uses the symbol animation to create a blinking effect on the symbols that are part of the winline.
		/// </summary>
		/// <param name="reelUshers"></param>
		/// <param name="betWindowWinLines"></param>
		/// <returns></returns>
		private IEnumerator AnimateWinlines(List<ReelUsher> reelUshers, Dictionary<int, Vector2[]> betWindowWinLines)
		{
			while (true)
			{
				foreach (KeyValuePair<int, Vector2[]> kvp in betWindowWinLines)
				{
					int id = kvp.Key;
					Vector2[] patterns = kvp.Value;


					for (int i = 0; i < patterns.Length; i++)
					{
						if (patterns[i].X >= 0)
						{
							int symbolIndex = ((int)patterns[i].X + 1) % reelUshers[(int)patterns[i].X].SymbolsInReel.Count;

							int reelIndex = (int)patterns[i].Y;
							ReelUsher reelUsher = reelUshers[reelIndex];

							Transform symbol = reelUsher.gameObject.transform.GetChild(symbolIndex);
							symbol.GetComponent<ISymbolAnimation>().StartAnimation();
						}
					}

					yield return new WaitForSeconds(2f);

					foreach (ReelUsher usher in reelUshers)
					{
						foreach (var symbol in usher.SymbolsInReel)
						{
							symbol.GetComponent<ISymbolAnimation>().StopAnimation();
							symbol.GetComponent<ISymbolAnimation>().ObsqureSymbol();
						}
					}
				}
			}
		}

		/// <summary>
		/// Stops the symbol animation and resets the symbol color to the default one.
		/// </summary>
		/// <param name="reelUshers"></param>
		public void StopSymbolAnimations(List<ReelUsher> reelUshers)
		{
			StopAllCoroutines();
			foreach (ReelUsher usher in reelUshers)
			{
				foreach (var symbol in usher.SymbolsInReel)
					symbol.GetComponent<ISymbolAnimation>().StopAnimation();
			}
		}
	}
}
