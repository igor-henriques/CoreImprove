using System;
using CoreImprove.Infra.Models;

namespace CoreImprove.Infra.Utils;

internal class PacketSender
{
	private byte[] Opcode = new byte[31]
	{
		96, 184, 0, 0, 0, 0, 139, 13, 0, 0,
		0, 0, 139, 73, 32, 191, 0, 0, 0, 0,
		106, 0, 87, 255, 208, 97, 195, 0, 0, 0,
		0
	};

	private const int SEND_PACKET_ADDRESS = 8493632;

	private const int RBA = 14959780;

	private Memory Mem;

	private int OpcodeAddr;

	public PacketSender(Memory mem)
	{
		try
		{
			Mem = mem;
			OpcodeAddr = SetUpOpcode();
		}
		catch (Exception)
		{
		}
	}

	private int SetUpOpcode()
	{
		try
		{
			int num = Mem.Allocate(4);
			Mem.Write(num, Opcode);
			Mem.Write(num + 2, 8493632, reverse: true);
			Mem.Write(num + 8, 14959780, reverse: true);
			return num;
		}
		catch (Exception)
		{
			return 0;
		}
	}

	public void SendPacket(Packet pkt)
	{
		try
		{
			if (pkt.Address == 9)
			{
				pkt.Address = Mem.Allocate(4);
			}
			if (Mem.ReadInt32(pkt.Address) == 0)
			{
				Mem.Write(pkt.Address, pkt.Pkt);
			}
			Mem.Write(OpcodeAddr + 16, pkt.Address, reverse: true);
			Mem.Write(OpcodeAddr + 21, pkt.Size);
			IntPtr thread = Mem.CreateRemoteThread(OpcodeAddr);
			Mem.WaitThread(thread);
			Mem.CloseThread(thread);
		}
		catch (Exception)
		{
		}
	}
}
