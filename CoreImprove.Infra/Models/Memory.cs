using System;
using System.Linq;

namespace CoreImprove.Infra.Models;

public class Memory
{
	private IntPtr pHandle;

	public Memory(IntPtr pHandle)
	{
		this.pHandle = pHandle;
	}

	public int ReadInt32(int address)
	{
		return MemFunctions.MemReadInt(pHandle, address);
	}

	public uint ReadUInt32(int address)
	{
		return MemFunctions.MemReadUInt(pHandle, address);
	}

	public float ReadFloat(int address)
	{
		return MemFunctions.MemReadFloat(pHandle, address);
	}

	public byte[] ReadBytes(int address, int amount)
	{
		return MemFunctions.MemReadBytes(pHandle, address, amount);
	}

	public string ReadUnicode(int address)
	{
		return MemFunctions.MemReadUnicode(pHandle, address);
	}

	public byte ReadByte(int address)
	{
		return (byte)MemFunctions.MemReadInt(pHandle, address);
	}

	public void Write(int address, object data, bool reverse)
	{
		if (data is byte)
		{
			MemFunctions.MemWriteByte(pHandle, address, (byte)data);
			return;
		}
		byte[] array = null;
		if (data is int)
		{
			array = BitConverter.GetBytes((int)data);
		}
		if (data is uint)
		{
			array = BitConverter.GetBytes((uint)data);
		}
		if (data is short)
		{
			array = BitConverter.GetBytes((short)data);
		}
		if (data is float)
		{
			array = BitConverter.GetBytes((float)data);
		}
		if (data is bool)
		{
			array = BitConverter.GetBytes((bool)data);
		}
		if (data is byte[])
		{
			array = (byte[])data;
		}
		if (reverse)
		{
			array?.Reverse();
		}
		MemFunctions.MemWriteBytes(pHandle, address, array);
	}

	public void Write(int address, object data)
	{
		Write(address, data, reverse: false);
	}

	public int Allocate(int amount)
	{
		return MemFunctions.AllocateMemory(pHandle, amount);
	}

	public IntPtr CreateRemoteThread(int address)
	{
		return MemFunctions.CreateRemoteThread(pHandle, address);
	}

	public void WaitThread(IntPtr thread)
	{
		MemFunctions.WaitForSingleObject(thread);
	}

	public void CloseThread(IntPtr thread)
	{
		MemFunctions.CloseProcess(thread);
	}
}
