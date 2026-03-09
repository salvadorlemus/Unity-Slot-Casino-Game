using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class PayoutHandler : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _payoutText;

		[SerializeField]
		private Image _payoutImage;

		/// <summary>
		/// Sets payout text
		/// </summary>
		/// <param name="payout"></param>
		public void UpdatePayout(string payout)
		{
			_payoutImage.enabled = true;
			_payoutText.text = payout;
		}

		// Hides the payout text
		public void HidePayout()
		{
			_payoutImage.enabled = false;
			_payoutText.text = String.Empty;
		}
	}
}
