using System;

namespace Toolbox.Utilities
{
	public abstract class Timer
	{
		public Action OnTimerStart = delegate { };
		public Action OnTimerStop = delegate { };

		protected float _initialTime;

		protected float Time { get; set; }
		public bool IsRunning { get; private set; }
		public float Progress => 1f - (Time / _initialTime);

		protected Timer(float value)
		{
			_initialTime = value;
			IsRunning = false;
		}

		public void Start()
		{
			Time = _initialTime;

			if (!IsRunning)
			{
				IsRunning = true;
				OnTimerStart?.Invoke();
			}
		}

		public void Stop()
		{
			if (IsRunning)
			{
				IsRunning = false;
				OnTimerStop?.Invoke();
			}
		}

		public void Resume() => IsRunning = true;
		public void Pause() => IsRunning = false;
		public abstract void Tick(float deltaTime);
	}

	public class CountdownTimer : Timer
	{
		public CountdownTimer(float value) : base(value) { }

		public override void Tick(float deltaTime)
		{
			if (IsRunning && Time > 0f)
			{
				Time -= deltaTime;
			}

			if (IsRunning && Time <= 0f)
			{
				Stop();
			}
		}

		public bool IsFinished() => Time <= 0f;
		public void Reset() => Time = _initialTime;

		public void Reset(float newTime)
		{
			_initialTime = newTime;
			Reset();
		}
	}

	public class StopwatchTimer : Timer
	{
		public StopwatchTimer(float value) : base(value) { }

		public override void Tick(float deltaTime)
		{
			if (IsRunning)
			{
				Time += deltaTime;
			}
		}

		public void Reset() => Time = 0f;
		public float GetTime() => Time;
	}
}