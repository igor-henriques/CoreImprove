using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreImprove.Infra.Models;

namespace CoreImprove.Infra.Utils;

public class TaskUtils
{
	public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
	{
		Task task = Task.Run(async delegate
		{
			while (condition())
			{
				await Task.Delay(frequency);
			}
		});

		await Task.WhenAny(task, Task.Delay(timeout));
    }

	public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
	{
		Task task = Task.Run(async delegate
		{
			while (!condition())
			{
				await Task.Delay(frequency);
			}
		});

        await Task.WhenAny(task, Task.Delay(timeout));
    }

	public static Process GetFirstClientProcess()
	{
		return (from x in Process.GetProcesses()
				where x.ProcessName.Trim().Contains(Settings.ClientName.Trim(), StringComparison.CurrentCultureIgnoreCase)
				select x).FirstOrDefault();
	}
}
