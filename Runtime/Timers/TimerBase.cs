using System.Threading;
using System;

namespace BasicUtilities
{
	/// <summary>
	/// An abstract base class for creating time-based utilities (e.g., countdowns, stopwatches).
	/// <br/>
	/// Handles cancellation, pause/resume, and lifecycle events.
	/// </summary>
	/// <typeparam name="T">The concrete type inheriting this timer.</typeparam>
	public abstract class TimerBase<T> : IDisposable where T : class
    {
		protected bool disposed = false;

		/// <summary>
		/// Whether the timer is currently started.
		/// </summary>
		public bool IsRunning { get; protected set; }

		/// <summary>
		/// Whether the timer is currently paused.
		/// </summary>
		public bool IsPaused { get; protected set; }

		/// <summary>
		/// The current elapsed time of the timer, in seconds.
		/// </summary>
		public float ElapsedTime { get; protected set; }

		/// <summary>Fires when the timer starts.</summary>
		public virtual event Action<T> OnStart;
		/// <summary>Fires every frame while the timer is active and not paused.</summary>
		public virtual event Action<T> OnTick;
		/// <summary>Fires when the timer is canceled or completed.</summary>
		public virtual event Action<T> OnStop;
		/// <summary>Fires when the timer is paused.</summary>
		public virtual event Action<T> OnPause;
		/// <summary>Fires when the timer is resumed from a paused state.</summary>
		public virtual event Action<T> OnResume;

		protected CancellationTokenSource cancelSource;

		#region Event Methods
		protected void InvokeStart(T timer) => OnStart?.Invoke(timer);
		protected void InvokeTick(T timer) => OnTick?.Invoke(timer);
		protected void InvokeStop(T timer) => OnStop?.Invoke(timer);
		protected void InvokePause(T timer) => OnPause?.Invoke(timer);
		protected void InvokeResume(T timer) => OnResume?.Invoke(timer);
		#endregion

		#region Timer Methods
		/// <summary>
		/// Starts the timer to begin running.
		/// </summary>
		/// <param name="triggerAction">Optional boolean to invoke the start event.</param>
		public abstract void Start(bool triggerAction = true);

		/// <summary>
		/// Stops the timer and enables the timer to be started again.
		/// </summary>
		/// <param name="triggerAction">Optional boolean to invoke the stop event.</param>
		public abstract void Cancel(bool triggerAction = true);

		/// <summary>
		/// Pauses or resumes the timer without resetting <see cref="ElapsedTime"/>.
		/// </summary>
		/// <param name="pauseTimer">True to pause, false to resume.</param>
		/// <param name="triggerAction">Optional boolean to invoke pause/resume events.</param>
		public virtual void Pause(bool pauseTimer, bool triggerAction = true)
		{
			if (!IsRunning || IsPaused == pauseTimer) return;

			IsPaused = pauseTimer;

			if (triggerAction)
			{
				if (IsPaused)
					OnPause?.Invoke(this as T);
				else
					OnResume?.Invoke(this as T);
			}
		}

		/// <summary>
		/// Restarts the timer, cancelling the currently running timer and starting the timer from the beginning.
		/// </summary>
		/// <param name="triggerAction">Optional boolean to invoke stop/start events.</param>
		public virtual void Restart(bool triggerAction = false)
		{
			Cancel(triggerAction);
			ElapsedTime = 0;
			Start(triggerAction);
		}
		#endregion

		#region IDisposable Methods
		~TimerBase()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes the timer and cancels any ongoing operation.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases resources used by the timer.
		/// </summary>
		/// <param name="disposing">releases managed resources if true.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposed) return;

			if (disposing)
			{
				cancelSource?.Cancel();
				cancelSource?.Dispose();
				cancelSource = null;
			}
			disposed = true;
		}
		#endregion
	}
}
