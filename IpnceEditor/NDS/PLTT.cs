using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.NDS
{
    public class PLTT
    {
        const uint magic = 0x504c5454;
        uint size;
        public uint bpp;
        uint x;
        uint coldatsize;
        uint colscount;
        public ushort[] coldata;

        public PLTT(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader reader)
        {
            uint m = reader.ReadUInt32();
            if (m != magic)
            {
                throw new Exception("PLTT magic is not present");
            }
            size = reader.ReadUInt32();
            bpp = reader.ReadUInt32();
            x = reader.ReadUInt32();
            coldatsize = reader.ReadUInt32();
            colscount = reader.ReadUInt32();
            coldata = new ushort[coldatsize / 2];
            for (int i = 0; i < coldatsize / 2; i++)
            {
                coldata[i] = reader.ReadUInt16();
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(magic);
            writer.Write(size);
            writer.Write(bpp);
            writer.Write(x);
            writer.Write(coldatsize);
            writer.Write(colscount);
            for (int i = 0; i < coldatsize / 2; i++)
            {
                writer.Write(coldata[i]);
            }
        }
    }
}
