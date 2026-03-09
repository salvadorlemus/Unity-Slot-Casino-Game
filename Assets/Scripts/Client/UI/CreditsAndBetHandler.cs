using TMPro;
using UnityEngine;

namespace Client.UI
{
	public class CreditsAndBetHandler: MonoBehaviour
	{
		 [SerializeField]
		TMP_Text _creditsText;

		[SerializeField]
		TMP_Text _betText;

		/// <summary>
		/// Sets payout text
		/// </summary>
		/// <param name="payout"></param>
		public void UpdateCredits(string payout)
		{
			_creditsText.text = payout;
		}

		// Hides the payout text
		public void UpdateBet(string bet)
		{
			_betText.text = bet;
		}
	}
}
