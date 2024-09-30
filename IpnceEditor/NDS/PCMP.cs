using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.NDS
{
    public class PCMP
    {
        const uint magic = 0x50434d50;
        uint size;
        public ushort palc;
        ushort unk1;
        uint unk2;
        public ushort[] palindexes;

        public PCMP(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader reader)
        {
            uint m = reader.ReadUInt32();
            if (m != magic)
            {
                throw new Exception("PCMP magic is not present");
            }
            size = reader.ReadUInt32();
            palc = reader.ReadUInt16();
            unk1 = reader.ReadUInt16();
            unk2 = reader.ReadUInt32();
            palindexes = new ushort[palc];
            for (int i = 0; i < palc; i++)
            {
                palindexes[i] = reader.ReadUInt16();
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(magic);
            writer.Write(size);
            writer.Write(palc);
            writer.Write(unk1);
            writer.Write(unk2);
            for (int i = 0; i < palc; i++)
            {
                writer.Write(palindexes[i]);
            }
        }
    }
}
