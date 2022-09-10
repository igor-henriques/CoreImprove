using System.Runtime.InteropServices;

namespace CoreImprove.Infra.DiscordIntegration;

public class DiscordRpc
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void DisconnectedCallback(int errorCode, string message);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ErrorCallback(int errorCode, string message);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ReadyCallback();

	[DllImport("client-init.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Initialize")]
	public static extern void Initialize(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);

	[DllImport("client-init.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_RunCallbacks")]
	public static extern void RunCallbacks();

	[DllImport("client-init.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Shutdown")]
	public static extern void Shutdown();

	[DllImport("client-init.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_UpdatePresence")]
	public static extern void UpdatePresence(ref RichPresence presence);
}
