using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CoreImprove.Infra.Models;

public class MemFunctions
{
	public delegate int ThreadProc(IntPtr param);

	[Flags]
	public enum FreeType
	{
		Decommit = 0x4000,
		Release = 0x8000
	}

	private const uint INFINITE = uint.MaxValue;

	private const uint WAIT_ABANDONED = 256u;

	private const uint WAIT_OBJECT_0 = 0u;

	private const uint WAIT_TIMEOUT = 258u;

	[DllImport("kernel32.dll")]
	private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

	[DllImport("Kernel32.dll")]
	private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, ref uint lpNumberOfBytesRead);

	[DllImport("kernel32.dll")]
	private static extern int CloseHandle(IntPtr hObject);

	[DllImport("kernel32.dll")]
	private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, ref uint lpNumberOfBytesWritten);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, FreeType dwFreeType);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

	[DllImport("kernel32.dll")]
	private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

	public static int AllocateMemory(IntPtr processHandle, int memorySize)
	{
		return (int)VirtualAllocEx(processHandle, (IntPtr)0, (uint)memorySize, 4096u, 64u);
	}

	public static IntPtr CreateRemoteThread(IntPtr processHandle, int address)
	{
		return CreateRemoteThread(processHandle, (IntPtr)0, 0u, (IntPtr)address, (IntPtr)0, 0u, (IntPtr)0);
	}

	public static void WaitForSingleObject(IntPtr threadHandle)
	{
		WaitForSingleObject(threadHandle, uint.MaxValue);
	}

	public static void FreeMemory(IntPtr processHandle, int address)
	{
		VirtualFreeEx(processHandle, (IntPtr)address, 0, FreeType.Release);
	}

	public static IntPtr OpenProcess(int pId)
	{
		return OpenProcess(2035711u, 0, (uint)pId);
	}

	public static void CloseProcess(IntPtr handle)
	{
		CloseHandle(handle);
	}

	public static void MemWriteBytes(IntPtr processHandle, int address, byte[] value)
	{
		uint lpNumberOfBytesWritten = 0u;
		WriteProcessMemory(processHandle, (IntPtr)address, value, (uint)value.Length, ref lpNumberOfBytesWritten);
	}

	public static void MemWriteStruct(IntPtr processHandle, int address, object value)
	{
		byte[] array = RawSerialize(value);
		uint lpNumberOfBytesWritten = 0u;
		WriteProcessMemory(processHandle, (IntPtr)address, array, (uint)array.Length, ref lpNumberOfBytesWritten);
	}

	public static void MemWriteInt(IntPtr processHandle, int address, int value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		uint lpNumberOfBytesWritten = 0u;
		WriteProcessMemory(processHandle, (IntPtr)address, bytes, 4u, ref lpNumberOfBytesWritten);
	}

	public static void MemWriteFloat(IntPtr processHandle, int address, float value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		uint lpNumberOfBytesWritten = 0u;
		WriteProcessMemory(processHandle, (IntPtr)address, bytes, 4u, ref lpNumberOfBytesWritten);
	}

	public static void MemWriteShort(IntPtr processHandle, int address, short value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		uint lpNumberOfBytesWritten = 0u;
		WriteProcessMemory(processHandle, (IntPtr)address, bytes, 2u, ref lpNumberOfBytesWritten);
	}

	public static void MemWriteByte(IntPtr processHandle, int address, byte value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		uint lpNumberOfBytesWritten = 0u;
		WriteProcessMemory(processHandle, (IntPtr)address, bytes, 1u, ref lpNumberOfBytesWritten);
	}

	public static byte[] MemReadBytes(IntPtr processHandle, int address, int size)
	{
		byte[] array = new byte[size];
		uint lpNumberOfBytesRead = 0u;
		ReadProcessMemory(processHandle, (IntPtr)address, array, (uint)size, ref lpNumberOfBytesRead);
		return array;
	}

	public static int MemReadInt(IntPtr processHandle, int address)
	{
		byte[] array = new byte[4];
		uint lpNumberOfBytesRead = 0u;
		ReadProcessMemory(processHandle, (IntPtr)address, array, 4u, ref lpNumberOfBytesRead);
		return BitConverter.ToInt32(array, 0);
	}

	public static uint MemReadUInt(IntPtr processHandle, int address)
	{
		byte[] array = new byte[4];
		uint lpNumberOfBytesRead = 0u;
		ReadProcessMemory(processHandle, (IntPtr)address, array, 4u, ref lpNumberOfBytesRead);
		return BitConverter.ToUInt32(array, 0);
	}

	public static float MemReadFloat(IntPtr processHandle, int address)
	{
		byte[] array = new byte[4];
		uint lpNumberOfBytesRead = 0u;
		ReadProcessMemory(processHandle, (IntPtr)address, array, 4u, ref lpNumberOfBytesRead);
		return BitConverter.ToSingle(array, 0);
	}

	public static string MemReadUnicode(IntPtr processHandle, int address)
	{
		byte[] array = new byte[400];
		uint lpNumberOfBytesRead = 0u;
		ReadProcessMemory(processHandle, (IntPtr)address, array, 400u, ref lpNumberOfBytesRead);
		return ByteArrayToString(array);
	}

	public static object MemReadStruct(IntPtr processHandle, int address, Type anyType)
	{
		int num = Marshal.SizeOf(anyType);
		byte[] array = new byte[num];
		uint lpNumberOfBytesRead = 0u;
		ReadProcessMemory(processHandle, (IntPtr)address, array, (uint)num, ref lpNumberOfBytesRead);
		return RawDeserialize(array, 0, anyType);
	}

	private static object RawDeserialize(byte[] rawData, int position, Type anyType)
	{
		int num = Marshal.SizeOf(anyType);
		if (num > rawData.Length)
		{
			return null;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.Copy(rawData, position, intPtr, num);
		object? result = Marshal.PtrToStructure(intPtr, anyType);
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	private static byte[] RawSerialize(object anything)
	{
		int num = Marshal.SizeOf(anything);
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.StructureToPtr(anything, intPtr, fDeleteOld: false);
		byte[] array = new byte[num];
		Marshal.Copy(intPtr, array, 0, num);
		Marshal.FreeHGlobal(intPtr);
		return array;
	}

	private static string ByteArrayToString(byte[] bytes)
	{
		Encoding encoding = new UnicodeEncoding();
		string result = "";
		for (int i = 0; i < bytes.Length - 1; i += 2)
		{
			if ((bytes[i] == 0) & (bytes[i + 1] == 0))
			{
				result = encoding.GetString(bytes, 0, i);
				break;
			}
		}
		return result;
	}
}
