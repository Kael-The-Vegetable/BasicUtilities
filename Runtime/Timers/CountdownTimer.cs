using System.Threading;
using UnityEngine;

namespace BasicUtilities
{
	/// <summary>
	/// A timer that counts down from a specified duration to zero.
	/// </summary>
	public class CountdownTimer : TimerBase<CountdownTimer>
    {
		private float _duration = 0;
		/// <summary>
		/// The duration the timer counts down from (in seconds).
		/// </summary>
		public float Duration
		{
			get => _duration;
			set
			{
				if (value < 0) value = 0;
				_duration = value;
			}
		}

		/// <summary>
		/// Set this boolean to alter whether or not this timer will be affected by <seealso cref="Time.timeScale"/>
		/// </summary>
		public bool Realtime { get; set; } = false;
		
		/// <summary>
		/// The remaining time until the countdown finishes.
		/// </summary>
		public float TimeRemaining => Mathf.Clamp(Duration - ElapsedTime, 0, Duration);

		public CountdownTimer(float duration = 0) => Duration = duration;

		#region Alternate Constructors
		public static CountdownTimer One => new(1);
		public static CountdownTimer NewTimer(float duration) => new(duration);
		#endregion

		/// <inheritdoc/>
		public override void Start(bool triggerAction = true)
        {
			if (IsRunning) return;
			IsRunning = true;

			cancelSource?.Dispose();
			cancelSource = new CancellationTokenSource();
			RunAsync(cancelSource.Token);

			if (triggerAction) InvokeStart(this);
		}
		protected async void RunAsync(CancellationToken cToken)
		{
			while (IsRunning && TimeRemaining > 0f)
			{
				await Awaitable.NextFrameAsync(cToken);
				if (cToken.IsCancellationRequested) return;

				if (IsPaused || (!Realtime && Time.timeScale == 0)) continue;

				ElapsedTime += Realtime ? Time.unscaledDeltaTime : Time.deltaTime;
				InvokeTick(this);
			}

			Cancel(true);
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
	}
}
