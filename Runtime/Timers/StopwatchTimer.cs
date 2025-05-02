using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BasicUtilities
{
	/// <summary>
	/// A stopwatch-style timer that counts up, can record lap times, and the total elapsed time that the timer has been running with.
	/// </summary>
	public class StopwatchTimer : TimerBase<StopwatchTimer>
	{
		/// <summary>
		/// The current time of this ongoing lap. Total time is found in <seealso cref="TimerBase{T}.ElapsedTime"/>.
		/// </summary>
		public float LapTime { get; private set; } = 0;

		/// <summary>
		/// All recorded laps in this timer. 0 is first.
		/// </summary>
		public List<float> Laps { get; private set; } = new List<float>();

		public event Action<StopwatchTimer> OnLap;

		/// <inheritdoc/>
		public override void Start(bool triggerAction = true)
		{
			if (IsRunning) return;
			IsRunning = true;

			cancelSource?.Dispose();
			cancelSource = new CancellationTokenSource();

			LapTime = 0;
			RunAsync(cancelSource.Token);

			if (triggerAction) InvokeStart(this);
		}
		protected async void RunAsync(CancellationToken cToken)
		{
			while (IsRunning)
			{
				await Awaitable.NextFrameAsync(cToken);
				if (cToken.IsCancellationRequested) return;

				if (IsPaused || (!Realtime && Time.timeScale == 0)) continue;
				float delta = Realtime ? Time.unscaledDeltaTime : Time.deltaTime;
				LapTime += delta;
				ElapsedTime += delta;
				InvokeTick(this);
			}

			// IsRunning is stopped
			cancelSource?.Cancel();
			cancelSource?.Dispose();
			cancelSource = null;
			InvokeStop(this);
		}
		/// <inheritdoc/>
		public override void Cancel(bool triggerAction = true)
		{
			if (!IsRunning) return;
			IsRunning = false;

			cancelSource?.Cancel();
			cancelSource?.Dispose();
			cancelSource = null;

			if (triggerAction) InvokeStop(this);
		}

		/// <summary>
		/// Records a lap time and resets the <see cref="LapTime"/>.
		/// </summary>
		/// <param name="triggerAction">Optional boolean to invoke the lap event.</param>
		public void Lap(bool triggerAction = true)
		{
			if (!IsRunning) return;

			Laps.Add(LapTime);
			LapTime = 0;
			if (triggerAction) OnLap.Invoke(this);
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (disposed) return;
			base.Dispose(disposing);
			if (disposing)
			{
				Laps.Clear();
				Laps = null;
			}
		}
	}
}
