using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.NDS
{
    public class NitroHeader
    {
        public uint magic;
        public ushort unk1;
        public byte unk2;
        public byte unk3;
        public uint fileSize;
        public ushort unk4;
        public ushort section_count;

        public NitroHeader(uint m, ushort sec_count)
        {
            magic = m;
            unk1 = 0xfeff;
            unk2 = 0x1;
            unk3 = 0x1;
            unk4 = 0x10;
            section_count = sec_count;
        }

        public NitroHeader(BinaryReader reader)
        { Load(reader); }

        public void Load(BinaryReader reader)
        {
            magic = reader.ReadUInt32();
            unk1 = reader.ReadUInt16();
            unk2 = reader.ReadByte();
            unk3 = reader.ReadByte();
            fileSize = reader.ReadUInt32();
            unk4 = reader.ReadUInt16();
            section_count = reader.ReadUInt16();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(magic);
            writer.Write(unk1);
            writer.Write(unk2);
            writer.Write(unk3);
            writer.Write(fileSize);
            writer.Write(unk4);
            writer.Write(section_count);
        }
    }
}
