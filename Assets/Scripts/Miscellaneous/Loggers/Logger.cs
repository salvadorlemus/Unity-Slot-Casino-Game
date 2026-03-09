using UnityEngine;

namespace Miscellaneous.Loggers
{
	/// <summary>
	/// Simple class to log in game events
	/// </summary>
	public class Logger
	{
		/// <summary>
		/// Logs the given message if we are in Editor only
		/// </summary>
		/// <param name="msg"></param>
		public static void Log(string msg)
		{
#if UNITY_EDITOR
			Debug.Log(msg);
#endif
		}

		public static void LogError(string msg)
		{
#if UNITY_EDITOR
			Debug.LogError(msg);
#endif
		}
	}
}
