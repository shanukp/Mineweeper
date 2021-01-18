using System;

#if __IOS__
using Foundation;
#elif WINDOWS_UWP
using Windows.UI.Xaml;
#endif

namespace Game.Utility
{

	public class OneShotTimer
	{

#if __IOS__
		public static NSTimer Start(double delay, Action action) {
			return NSTimer.CreateScheduledTimer(delay, (t) => {
				action();
			});
		}
#else
		public static void Start(double delay, Action action)
		{
			RepeatingTimer.StartNew(delay, action, true);
		}
#endif
	}

	public class RepeatingTimer
	{

		public static RepeatingTimer StartNew(double intervalSeconds, Action action)
		{
			return new RepeatingTimer(intervalSeconds, action, false);
		}

		public static RepeatingTimer StartNew(double intervalSeconds, Action action, bool oneShot)
		{
			return new RepeatingTimer(intervalSeconds, action, oneShot);
		}

#if __IOS__
		
		private NSTimer timer;
		private Action action;
		private bool oneShot;

		private RepeatingTimer(double intervalSeconds, Action action, bool oneShot) {

			this.action = action;
			this.oneShot = oneShot;

			this.timer = NSTimer.CreateRepeatingScheduledTimer(intervalSeconds, TimerTick);
		}

		public void Stop() {
			if (this.timer != null) this.timer.Invalidate();
		}

		private void TimerTick(NSTimer t) {
			if (this.oneShot) Stop();
			if (this.action != null) this.action();
		}

#elif WINDOWS_UWP

		private DispatcherTimer timer;
		private Action action;
		private bool oneShot;

		private RepeatingTimer(double intervalSeconds, Action action, bool oneShot) {

			this.action = action;
			this.oneShot = oneShot;

			this.timer = new DispatcherTimer();
			this.timer.Interval = TimeSpan.FromSeconds(intervalSeconds);
			this.timer.Tick += TimerTick;
			this.timer.Start();
		}

		public void Stop() {
			if (this.timer != null) {
				this.timer.Tick -= TimerTick;
				this.timer.Stop();
			}
		}

		private void TimerTick(object sender, object e) {
			if (this.oneShot) Stop();
			if (this.action != null) this.action();
		}

#else

		private System.Timers.Timer timer;
		private Action action;

		private RepeatingTimer(double intervalSeconds, Action action, bool oneShot)
		{

			this.action = action;

			this.timer = new System.Timers.Timer(intervalSeconds * 1000);
			this.timer.AutoReset = !oneShot;
			this.timer.Elapsed += Timer_Elapsed;
			this.timer.Start();
		}

		public void Stop()
		{
			if (this.timer != null)
			{
				this.timer.Elapsed -= Timer_Elapsed;
				this.timer.Stop();
			}
		}

		private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			// dhess: Add this conditional to get the Curator to compile; since
			// this file _technically_ resides in DCC.Game, it can leverage DCC.Game
			// methods like Game.MainThreadRun(), however that breaks in the Curator sln
			// since DCC.Game isn't included; fixed for now by conditionally adding
			// back the inverse of the change from CL 970347.
			//
			// TODO: find a better solution; catch stuff like this at build-time;
			//
#if (__IOS__ || __ANDROID__)
			if (this.action != null) Game.MainThreadRun(this.action);
#else
			if (this.action != null) this.action();
#endif
		}

#endif
	}
}