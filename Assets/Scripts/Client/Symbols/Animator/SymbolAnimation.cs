using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Symbols.Animator
{
	public class SymbolAnimation : MonoBehaviour , ISymbolAnimation
	{
		public Color darkSymbolColor = new Color(0.5f, 0.5f, 0.5f, 1);
		public Color lightSymbolColor = new Color(1, 1, 1, 1);

		[SerializeField]
		private Image symbolImage;

		/// <summary>
		/// Inits the Symbol Animator
		/// </summary>
		/// <param name="_symbolImage"></param>
		public void Init(Image _symbolImage)
		{
			this.symbolImage = _symbolImage;
			_symbolImage.color = lightSymbolColor;
		}

		public void StartAnimation()
		{
			StartCoroutine(AnimateSymbol());
		}

		public void StopAnimation()
		{
			StopAllCoroutines();
			symbolImage.color = lightSymbolColor;
		}

		public void ObsqureSymbol()
		{
			symbolImage.color = darkSymbolColor;
		}

		/// <summary>
		/// Simple animation that changes the color of the symbol to create a blinking effect.
		/// </summary>
		/// <returns></returns>
		private IEnumerator AnimateSymbol()
		{
			while (true)
			{
				symbolImage.color = darkSymbolColor;
				yield return new WaitForSeconds(0.5f);
				symbolImage.color = lightSymbolColor;
				yield return new WaitForSeconds(0.5f);
			}
		}
	}
}
