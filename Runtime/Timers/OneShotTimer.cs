using System;
using System.Threading;
using UnityEngine;

namespace BasicUtilities
{
	/// <summary>
	/// Provides one-shot (single-fire) delay-based timer utilities for executing actions after a delay.
	/// </summary>
	public static class OneShotTimer
	{
		/// <summary>
		/// Executes an action after a delay using game time (scaled time), running asynchronously.
		/// The delay will count down using <see cref="Time.deltaTime"/>.
		/// </summary>
		/// <param name="delay">The delay in seconds before executing the action.</param>
		/// <param name="action">The callback to execute after the delay.</param>
		/// <param name="t">A cancellation token to cancel the delay before execution.</param>
		/// <remarks>
		/// Uses frame-based timing via <seealso cref="Awaitable.NextFrameAsync"/> for precision aligned with Unity's update loop.
		/// </remarks>
		public static async void Delay(float delay, Action action, CancellationToken t)
		{
			while (delay > 0)
			{
				await Awaitable.NextFrameAsync(t);
				if (t.IsCancellationRequested) return;
				delay -= Time.deltaTime;
			}
			action.Invoke();
		}

		/// <summary>
		/// Executes an action after a delay using game time (scaled time), running asynchronously.
		/// The delay will count down using <see cref="Time.deltaTime"/>.
		/// </summary>
		/// <param name="delay">The delay in seconds before executing the action.</param>
		/// <param name="action">The callback to execute after the delay with a float as a param (being the <see cref="delay")/>.</param>
		/// <param name="t">A cancellation token to cancel the delay before execution.</param>
		/// <remarks>
		/// Uses frame-based timing via <seealso cref="Awaitable.NextFrameAsync"/> for precision aligned with Unity's update loop.
		/// </remarks>
		public static async void Delay(float delay, Action<float> action, CancellationToken t)
		{
			float time = delay;
			while (time > 0)
			{
				await Awaitable.NextFrameAsync(t);
				if (t.IsCancellationRequested) return;
				time -= Time.deltaTime;
			}
			action.Invoke(delay);
		}

		/// <summary>
		/// Executes an action after a delay using real time (unscaled time), running asynchronously.
		/// The delay is unaffected by <see cref="Time.timeScale"/>.
		/// </summary>
		/// <param name="delay">The real-time delay in seconds before executing the action.</param>
		/// <param name="action">The callback to execute after the delay.</param>
		/// <param name="t">A cancellation token to cancel the delay before execution.</param>
		/// <remarks>
		/// Internally uses <seealso cref="Awaitable.WaitForSecondsAsync"/> to ensure time accuracy regardless of game time scale.
		/// </remarks>
		public static async void DelayRealtime(float delay, Action action, CancellationToken t)
		{
			await Awaitable.WaitForSecondsAsync(delay, t);
			if (t.IsCancellationRequested) return;
			action.Invoke();
		}
	}
}
