using System;
using Client.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace Client.IO
{
	/// <summary>
	/// Class in charge of button states
	/// </summary>
	public class ButtonManager : MonoBehaviour
	{
		private Button _button;

		private void OnEnable()
		{
			_button = GetComponent<Button>();

			_button.onClick.AddListener(FindAnyObjectByType<UserInput>().SpinRequest);
			GameEvents.SpinEvent += () =>
			{
				_button.interactable = false;
			};

			GameEvents.SpinEndEvent += () =>
			{
				_button.interactable = true;
			};
		}

		private void OnDisable()
		{
			_button.onClick.RemoveAllListeners();

			GameEvents.SpinEvent -= () =>
			{
				_button.interactable = false;
			};

			GameEvents.SpinEndEvent += () =>
			{
				_button.interactable = true;
			};
		}
	}
}
