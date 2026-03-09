using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Client.Reels;
using Client.Symbols.Animator;
using Client.UI;
using Server.BetWindow;
using UnityEngine;
using Logger = Miscellaneous.Loggers.Logger;
using Random = System.Random;
using Vector2 = System.Numerics.Vector2;

namespace Client.FSM
{
	/// <summary>
	/// Class that manages game loop
	/// </summary>
	public class GameLoop : MonoBehaviour
	{
		private Server.Server _server;
		private BetWindow _betWindow;
		private BetWindow _initialDataWindow;
		private SymbolAnimator _symbolAnimator;

		[SerializeField]
		private List<ReelUsher> reelUshers;

		private List<ISpinReel> reelSpinners;

		[SerializeField]
		private PayoutHandler _payoutHandler;

		[SerializeField]
		private CreditsAndBetHandler _creditsAndBetHandler;

		Random _random;

		private void OnEnable()
		{
			_server  = new Server.Server();
			_server.Init();

			_random = new Random();

			GameStates.UpdateState(States.Idle);
			_symbolAnimator = GetComponent<SymbolAnimator>();

			if (reelSpinners == null || reelSpinners.Count == 0)
			{
				reelSpinners = new List<ISpinReel>();

				foreach (var reelUsher in reelUshers)
					reelSpinners.Add(reelUsher.gameObject.GetComponent<ISpinReel>());
			}

			GameEvents.SpinEvent += NotifyServerForSpin;
		}

		private void OnDisable()
		{
			GameEvents.SpinEvent -= NotifyServerForSpin;
		}

		private async void Start()
		{
			_payoutHandler.HidePayout();
			try
			{
				StringBuilder sb = new StringBuilder();
				_initialDataWindow = await _server.GetInitialData();

				sb.Append($"BET :$ {_initialDataWindow.Bet}");

				_creditsAndBetHandler.UpdateBet(sb.ToString());

				sb.Clear();
				sb.Append($"CREDITS : $ {_initialDataWindow.Credits}");
				_creditsAndBetHandler.UpdateCredits(sb.ToString());

				foreach (var reelUsher in reelUshers)
				{
					char[] symbolsInReel = await _server.GetAllSymbolsInReel(reelUsher.ReelID);
					reelUsher.Init(symbolsInReel);
				}
			}
			catch (Exception e)
			{
				Logger.LogError(e.Message);
			}
		}

		/// <summary>
		/// Notifies the server about the user spin action
		/// </summary>
		private async void NotifyServerForSpin()
		{
			_payoutHandler.HidePayout();
			try
			{
				_symbolAnimator.StopSymbolAnimations(reelUshers);

				GameStates.UpdateState(States.Betting);

				bool playerHasEnoughCredits = await _server.PlayerHasEnoughCredits();

				StringBuilder sb = new StringBuilder();

				if (playerHasEnoughCredits)
				{
					// Update credits for the first spin
					if (_betWindow == null)
					{
						sb.Append($"CREDITS : $ {_initialDataWindow.Credits - _initialDataWindow.Bet}");
						_creditsAndBetHandler.UpdateCredits(sb.ToString());
					}
					else
					{
						sb.Append($"CREDITS : $ {_betWindow.Credits + _betWindow.Payout - _betWindow.Bet}");
						_creditsAndBetHandler.UpdateCredits(sb.ToString());
					}

					GameStates.UpdateState(States.Spinning);
					foreach (var reelSpinner in reelSpinners)
					{
						reelSpinner.StartSpinning();
						await Task.Delay(100);
					}

					_betWindow = await _server.GetBetWindow();

					sb.Clear();
					sb.Append($"BET : $ {_betWindow.Bet}");

					_creditsAndBetHandler.UpdateBet(sb.ToString());
					GameEvents.ServerResultEvent?.Invoke(_betWindow);

					await Task.Delay(100);

					foreach (var reelSpinner in reelSpinners)
					{
						reelSpinner.StopSpinning();
						await Task.Delay(250);
					}

					if (_betWindow.winLines != null && _betWindow.winLines.Count > 0)
					{
						GameStates.UpdateState(States.Payout);
						await Task.Delay(_random.Next(100));

						sb.Clear();
						sb.Append($"$ {_betWindow.Payout}");
						_payoutHandler.UpdatePayout(sb.ToString());

						sb.Clear();
						sb.Append($"CREDITS : $ {_betWindow.Credits + _betWindow.Payout}");
						_creditsAndBetHandler.UpdateCredits(sb.ToString());

						_symbolAnimator.PlaySymbolAnimations(reelUshers, _betWindow.winLines);
					}
					GameStates.UpdateState(States.Idle);
					GameEvents.SpinEndEvent?.Invoke();
				}
				else
				{
					Logger.LogError("User doesn't have anough credits to make a spin");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
		}
	}
}
