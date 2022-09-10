using System;
using System.Diagnostics;
using CoreImprove.Infra.Models;

namespace CoreImprove.Infra.Utils;

public class Manager
{
	public bool Connected;

	public EventHandler<ClientEventArgs> OnConnect;

	public Client elementclient;

	private LoopMonitor Monitor;

	public EventHandler<TriggerEventArgs> OnSettingsChanged;

	public Manager(int processPID)
	{
		try
		{
			IntPtr intPtr = AttachProcess(processPID);

			if (!(intPtr == IntPtr.Zero))
			{
				elementclient = new Client(intPtr);
				Connected = true;
				Monitor = new LoopMonitor(elementclient);
				OnSettingsChanged = (EventHandler<TriggerEventArgs>)Delegate.Combine(OnSettingsChanged, new EventHandler<TriggerEventArgs>(Monitor.OnSettingsChanged));
			}
		}
		catch (Exception)
		{
		}
	}

	public void Connect()
	{
		try
		{
			if (OnConnect != null)
			{
				OnConnect(elementclient, new ClientEventArgs(elementclient.Name, 0, 0));
			}

			Monitor.Start();
		}
		catch (Exception)
		{
		}
	}

	private static IntPtr AttachProcess(int processPID)
	{
		try
		{
			return MemFunctions.OpenProcess(Process.GetProcessById(processPID).Id);
		}
		catch (Exception)
		{
			return IntPtr.Zero;
		}
	}
}
