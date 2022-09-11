using System;
using System.Diagnostics;
using CoreImprove.Infra.Models;

namespace CoreImprove.Infra.Utils;

public class Manager
{
    public bool Connected;

    public Client elementclient;

    private LoopMonitor Monitor;

    public EventHandler<TriggerEventArgs> OnSettingsChanged;

    public Manager(int processPID)
    {
        IntPtr intPtr = AttachProcess(processPID);

        if (intPtr != IntPtr.Zero)
        {
            elementclient = new Client(intPtr);
            Connected = true;
            Monitor = new LoopMonitor(elementclient);
            OnSettingsChanged = (EventHandler<TriggerEventArgs>)Delegate.Combine(OnSettingsChanged, new EventHandler<TriggerEventArgs>(Monitor.OnSettingsChanged));
        }
    }

    public void Connect()
    {
        Monitor?.Start();
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
