using System;
using System.Linq;

namespace CoreImprove.Infra.Models;

public class Packet
{
	internal byte[] Pkt;

	internal int Address;

	internal byte Size => (byte)Pkt.Length;

	public Packet(short header, int size)
	{
		Pkt = new byte[size];
		byte[] bytes = BitConverter.GetBytes(header);
		bytes.Reverse();
		Buffer.BlockCopy(bytes, 0, Pkt, 0, 2);
	}
}
