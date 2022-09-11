using System;
using System.Collections.Generic;
using CoreImprove.Infra.Utils;

namespace CoreImprove.Infra.Models;

public class Client
{
    private const int RBA = 13067820;

    private IntPtr pHandle;

    private Memory Mem;

    private PacketSender pSender;

    private Packet skillPkt;

    private Packet cskillPkt;

    private Packet finishPkt;

    private int GamePtr => Mem.ReadInt32(Mem.ReadInt32(13067820) + 28);

    private int CharPtr => Mem.ReadInt32(GamePtr + 44);

    private int ActionPtr => Mem.ReadInt32(CharPtr + 4976);

    public uint UID => Mem.ReadUInt32(CharPtr + 1172);

    public string Name => Mem.ReadUnicode(Mem.ReadInt32(CharPtr + 1712));

    public string Occupation => Mem.ReadUnicode(Mem.ReadInt32(CharPtr + 9431200));

    public uint Level => Mem.ReadUInt32(CharPtr + 1184);
    public uint Level2 => Mem.ReadUInt32(CharPtr + 1236);
    public uint Level3 => Mem.ReadUInt32(CharPtr + 2316);

    public int QueuePtr => Mem.ReadInt32(ActionPtr + 32);

    public int QueueType => Mem.ReadInt32(QueuePtr + 4);

    public uint QueueTarget => Mem.ReadUInt32(Mem.ReadInt32(QueuePtr + 64) + 8);

    public int QueueSkillPtr => Mem.ReadInt32(QueuePtr + 80);

    public int QueueSkillID => Mem.ReadInt32(QueueSkillPtr + 8);

    public byte QueueSkillType => (byte)Mem.ReadInt32(Mem.ReadInt32(Mem.ReadInt32(QueueSkillPtr + 4) + 4) + 28);

    public byte QueueSkillCType => (byte)Mem.ReadInt32(Mem.ReadInt32(Mem.ReadInt32(QueueSkillPtr + 4) + 4) + 87);

    public int SkillPtr => Mem.ReadInt32(CharPtr + 1952);

    public bool Charging => Mem.ReadByte(SkillPtr + 36) == 1;

    public bool CurrentSkillCooldown => Mem.ReadInt32(SkillPtr + 16) > 0;

    public int CurrentSkillCastTime => Mem.ReadInt32(Mem.ReadInt32(Mem.ReadInt32(Mem.ReadInt32(Mem.ReadInt32(SkillPtr + 4) + 4)) + 36) + 1);

    public int LatestPing => Mem.ReadInt32(GamePtr + 376);

    private byte ProtectionSettings
    {
        get
        {
            int num = Mem.ReadInt32(Mem.ReadInt32(13067820) + 24);
            if ((byte)Mem.ReadInt32(num + 616) == 0)
            {
                return 0;
            }
            byte b = 1;
            if ((byte)Mem.ReadInt32(num + 617) != 0)
            {
                b = 3;
            }
            if ((byte)Mem.ReadInt32(num + 618) != 0)
            {
                b = (byte)(b | 4u);
            }
            if ((byte)Mem.ReadInt32(num + 625) != 0)
            {
                b = (byte)(b | 8u);
            }
            if ((byte)Mem.ReadInt32(num + 628) != 0)
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
        Mem.Write(SkillPtr + 36, (byte)0);
        SendPacket(FinishPkt());
    }

    public Packet SkillPkt(int ID, uint target)
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

    public Packet cSkillPkt(int ID, uint target)
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

    public Packet FinishPkt()
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

    public void SendPacket(Packet packet)
    {
        pSender.SendPacket(packet);
    }
}
