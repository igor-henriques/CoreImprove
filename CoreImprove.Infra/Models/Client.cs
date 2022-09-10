using System;
using CoreImprove.Infra.Utils;

namespace CoreImprove.Infra.Models;

public class Client
{
	private const int RBA = 14959780;

	private IntPtr pHandle;

	private Memory Mem;

	private PacketSender pSender;

	private Packet skillPkt;

	private Packet cskillPkt;

	private Packet finishPkt;

	private int GamePtr => Mem.ReadInt32(Mem.ReadInt32(14959780) + 28);

	private int CharPtr => Mem.ReadInt32(GamePtr + 52);

	private int ActionPtr => Mem.ReadInt32(CharPtr + 5376);

	public uint UID => Mem.ReadUInt32(CharPtr + 1208);

	public string Name => Mem.ReadUnicode(Mem.ReadInt32(CharPtr + 1788));

    public uint Level => Mem.ReadUInt32(CharPtr + 1220);

	public uint Occupation => Mem.ReadUInt32(CharPtr + 1220);

	public uint MaxHP => Mem.ReadUInt32(CharPtr + 1312);

	public uint HP => Mem.ReadUInt32(CharPtr + 1228);

	public uint MaxMP => Mem.ReadUInt32(CharPtr + 1316);

	public uint MP => Mem.ReadUInt32(CharPtr + 1232);

	public uint EXP => Mem.ReadUInt32(CharPtr + 1236);

	public uint SP => Mem.ReadUInt32(CharPtr + 1240);

	public uint StatusPointAvailable => Mem.ReadUInt32(CharPtr + 1244);

	public uint AtqLevel => Mem.ReadUInt32(CharPtr + 1252);

	public uint DefLevel => Mem.ReadUInt32(CharPtr + 1256);

	public uint CritRate => Mem.ReadUInt32(CharPtr + 1260);

	public int QueuePtr => Mem.ReadInt32(ActionPtr + 64);

	public int QueueType => Mem.ReadInt32(QueuePtr + 4);

	public uint QueueTarget => Mem.ReadUInt32(Mem.ReadInt32(QueuePtr + 64) + 8);

	public int QueueSkillPtr => Mem.ReadInt32(QueuePtr + 52);

	public int QueueSkillID => Mem.ReadInt32(QueueSkillPtr + 8);

	public byte QueueSkillType => (byte)Mem.ReadInt32(Mem.ReadInt32(Mem.ReadInt32(QueueSkillPtr + 4) + 4) + 28);

	public byte QueueSkillCType => (byte)Mem.ReadInt32(Mem.ReadInt32(Mem.ReadInt32(QueueSkillPtr + 4) + 4) + 87);

	public int SkillPtr => Mem.ReadInt32(CharPtr + 2016);

	public bool Charging => Mem.ReadByte(SkillPtr + 36) == 1;

	public bool CurrentSkillCooldown => Mem.ReadInt32(SkillPtr + 16) > 0;

	public int LatestPing => Mem.ReadInt32(GamePtr + 376);

	private byte ProtectionSettings
	{
		get
		{
			int num = Mem.ReadInt32(Mem.ReadInt32(14959780) + 24);
			if ((byte)Mem.ReadInt32(num + 642) == 0)
			{
				return 0;
			}
			byte b = 1;
			if ((byte)Mem.ReadInt32(num + 643) != 0)
			{
				b = 3;
			}
			if ((byte)Mem.ReadInt32(num + 644) != 0)
			{
				b = (byte)(b | 4u);
			}
			if ((byte)Mem.ReadInt32(num + 651) != 0)
			{
				b = (byte)(b | 8u);
			}
			if ((byte)Mem.ReadInt32(num + 654) != 0)
			{
				b = (byte)(b | 0x10u);
			}
			return b;
		}
	}

	public Client(IntPtr pHandle)
	{
		this.pHandle = pHandle;
		Mem = new Memory(pHandle);
		pSender = new PacketSender(Mem);
	}

	public void FinishTakeAim()
	{
		Mem.Write(SkillPtr, (byte)1);
		SendPacket(FinishPkt());
	}

	public Packet SkillPkt(int ID, uint target)
	{
		try
		{
			if (skillPkt == null)
			{
				skillPkt = new Packet(41, 12);
			}
			if (skillPkt.Address == 0)
			{
				skillPkt.Address = Mem.Allocate(4);
			}
			if (Mem.ReadInt32(skillPkt.Address) == 0)
			{
				Mem.Write(skillPkt.Address, skillPkt.Pkt);
			}
			Mem.Write(skillPkt.Address + 2, ID, reverse: true);
			Mem.Write(skillPkt.Address + 6, ProtectionSettings);
			Mem.Write(skillPkt.Address + 7, (byte)1);
			Mem.Write(skillPkt.Address + 8, target, reverse: true);
			return skillPkt;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public Packet cSkillPkt(int ID, uint target)
	{
		try
		{
			if (cskillPkt == null)
			{
				cskillPkt = new Packet(80, 12);
			}
			if (cskillPkt.Address == 0)
			{
				cskillPkt.Address = Mem.Allocate(4);
			}
			if (Mem.ReadInt32(cskillPkt.Address) == 0)
			{
				Mem.Write(cskillPkt.Address, cskillPkt.Pkt);
			}
			Mem.Write(cskillPkt.Address + 2, ID, reverse: true);
			Mem.Write(cskillPkt.Address + 6, ProtectionSettings);
			Mem.Write(cskillPkt.Address + 7, (byte)1);
			Mem.Write(cskillPkt.Address + 8, target, reverse: true);
			return cskillPkt;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public Packet FinishPkt()
	{
		try
		{
			if (finishPkt == null)
			{
				finishPkt = new Packet(51, 2);
			}
			if (finishPkt.Address == 0)
			{
				finishPkt.Address = Mem.Allocate(finishPkt.Size);
			}
			if (Mem.ReadInt32(finishPkt.Address) == 0)
			{
				Mem.Write(finishPkt.Address, finishPkt.Pkt);
			}
			return finishPkt;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public void SendPacket(Packet packet)
	{
		try
		{
			pSender.SendPacket(packet);
		}
		catch (Exception)
		{
		}
	}
}
