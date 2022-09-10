using System;
using System.Threading;
using System.Threading.Tasks;
using CoreImprove.Infra.Models;

namespace CoreImprove.Infra.Utils;

public class LoopMonitor
{
	private Client elementclient;

	private bool Attack;

	private bool Self;

	private bool Buff;

	private bool Charge;

	private bool Running;

	private Thread TMonitor;

	private Thread TAutoComplete;

	public LoopMonitor(Client elementclient)
	{
		this.elementclient = elementclient;
	}

	public void OnSettingsChanged(object sender, TriggerEventArgs e)
	{
		Attack = true;
		Self = true;
		Buff = true;
		Charge = true;
	}

	public void Start()
	{
		Running = true;
		Attack = true;
		Self = true;
		Buff = true;
		Charge = true;
		TMonitor = new Thread((ThreadStart)async delegate
		{
			await InterceptAdv();
		});
		TAutoComplete = new Thread((ThreadStart)delegate
		{
			AutoComplete();
		});
		TMonitor.Start();
		TAutoComplete.Start();
	}

	private async Task InterceptAdv()
	{
		try
		{
			while (Running)
			{
				int queuePtr = elementclient.QueuePtr;
				int queueSkillId = elementclient.QueueSkillID;
				if (queuePtr != 0 && elementclient.QueueType == 2 && elementclient.QueueSkillType != 8 && elementclient.QueueTarget != 0 && (elementclient.QueueTarget != elementclient.UID || Self) && (elementclient.QueueSkillType != 2 || elementclient.QueueTarget == elementclient.UID || Buff) && (elementclient.QueueSkillType != 1 || Attack))
				{
					if (elementclient.QueueSkillCType == 0)
					{
						elementclient.SendPacket(elementclient.SkillPkt(queueSkillId, elementclient.QueueTarget));
					}
					else
					{
						elementclient.SendPacket(elementclient.cSkillPkt(queueSkillId, elementclient.QueueTarget));
					}
					while (elementclient.QueueSkillID == queueSkillId && Running)
					{
						await Task.Delay(10);
					}
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void AutoComplete()
	{
		try
		{
			while (Running)
			{
				Thread.Sleep(1);
				if (Charge && elementclient.Charging)
				{
					elementclient.FinishTakeAim();
					while (elementclient.Charging && Running)
					{
						Thread.Sleep(1);
					}
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void Abort()
	{
		Running = false;
	}
}
