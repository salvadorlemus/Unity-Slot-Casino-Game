using System;
using Client.FSM;
using UnityEngine;

namespace Client.IO
{
	/// <summary>
	/// Class in charge of catch user input
	/// </summary>
	public class UserInput : MonoBehaviour
	{
		private void Update()
		{
			if (GameStates.State == States.Idle)
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					SpinRequest();
				}
			}
		}

		/// <summary>
		/// User requested a spin
		/// </summary>
		public void SpinRequest()
		{
			GameEvents.SpinEvent?.Invoke();
		}
	}
}
