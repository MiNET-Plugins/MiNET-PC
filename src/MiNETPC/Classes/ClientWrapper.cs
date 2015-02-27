using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using MiNETPC.Packages;
using Timer = System.Timers.Timer;

namespace MiNETPC.Classes
{
	public class ClientWrapper
	{
		public TcpClient TCPClient;
		public Player Player;
		public bool PlayMode = false;
		private Queue<byte[]> Commands = new Queue<byte[]>();
		private AutoResetEvent Resume = new AutoResetEvent(false);
		private bool PEPlayer = false;

		public ClientWrapper(TcpClient client)
		{
			TCPClient = client;
			new Thread(() => ThreadRun()).Start();
		}

		public ClientWrapper()
		{
			PEPlayer = true;
		}

		public void AddToQuee(byte[] data, bool quee = false)
		{
			if (!PEPlayer)
			{
				//ConsoleFunctions.WriteDebugLine("Data length: " + data.Length);
				if (quee)
				{
					lock (Commands)
					{
						Commands.Enqueue(data);
					}
					Resume.Set();
				}
				else
				{
					SendData(data);
				}
			}
		}

		private void ThreadRun()
		{
			while (Resume.WaitOne())
			{
				byte[] command;
				lock (Commands)
				{
					command = Commands.Dequeue();
				}
				SendData(command);
			}
		}

		public void SendData(byte[] Data, int Length)
		{
			try
			{
				NetworkStream a = TCPClient.GetStream();
				a.Write(Data, 0, Length);
				a.Flush();
			}
			catch
			{
				ConsoleFunctions.WriteErrorLine("Failed to send a packet!");
			}
		}

		public void SendData(byte[] Data, int Offset, int Length)
		{
			try
			{
				NetworkStream a = TCPClient.GetStream();
				a.Write(Data, Offset, Length);
				a.Flush();
			}
			catch
			{
				ConsoleFunctions.WriteErrorLine("Failed to send a packet!");
			}
		}

		public void SendData(byte[] data)
		{
			try
			{
				NetworkStream a = TCPClient.GetStream();
				a.Write(data, 0, data.Length);
				a.Flush();
			}
			catch
			{
				ConsoleFunctions.WriteErrorLine("Failed to send a packet!");
			}
		}

		Timer kTimer = new Timer();

		public void StartKeepAliveTimer()
		{
			kTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
			kTimer.Interval = 5000;
			kTimer.Start();
		}

		public void StopKeepAliveTimer()
		{
			kTimer.Stop();
		}

		public void DisplayTimeEvent(object source, ElapsedEventArgs e)
		{
			new KeepAlive(this).Write();
			//Do keep alive :)
		}
	}
}
