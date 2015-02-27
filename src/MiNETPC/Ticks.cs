using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MiNETPC.Packages;

namespace MiNETPC
{
	class Ticks
	{
		#region TickTimer

		private Thread TimerThread;
		private Thread GameTickThread;

		public void StartTimeOfDayTimer()
		{
			TimerThread = new Thread(() => StartTimeTimer());
			TimerThread.Start();

			GameTickThread = new Thread(() => StartTickTimer());
			GameTickThread.Start();
		}

		public void StopTimeOfDayTimer()
		{
			TimerThread.Abort();
			TimerThread = new Thread(() => StartTimeTimer());
		}

		private static readonly System.Timers.Timer kTimer = new System.Timers.Timer();
		private static readonly System.Timers.Timer kTTimer = new System.Timers.Timer();

		private void StartTimeTimer()
		{
			kTimer.Elapsed += RunDayTick;
			kTimer.Interval = 1000;
			kTimer.Start();
		}

		private void StartTickTimer()
		{
			kTTimer.Elapsed += GameTick;
			kTTimer.Interval = 50;
			kTTimer.Start();
		}

		private void StopTimeTimer()
		{
			kTimer.Stop();
		}

		private void RunDayTick(object source, ElapsedEventArgs e)
		{
			foreach (var i in PluginGlobals.PcPlayers)
			{
				new TimeUpdate(i.Wrapper) { Time = PluginGlobals.Level.CurrentWorldTime, Day = 0 }.Write();
			}
		}

		private void GameTick(object source, ElapsedEventArgs e)
		{
		}

		#endregion
	}
}
