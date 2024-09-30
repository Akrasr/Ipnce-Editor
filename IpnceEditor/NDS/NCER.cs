using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.NDS
{
    public class NCER
    {
        public const uint magic = 0x4e434552;
        NitroHeader nheader;
        public CEBK cebk;
        public LABL labl;
        TXEU uext;
        byte[] additional;

        public NCER(CEBK bk)
        {
            cebk = bk;
            nheader = new NitroHeader(magic, 3);
            labl = new LABL();
            uext = new TXEU();
            nheader.fileSize = 0x10 + cebk.size + labl.size + uext.size;
        }

        public NCER(BinaryReader br)
        { Load(br); }

        public void Load(BinaryReader reader)
        {
            nheader = new NitroHeader(reader);
            if (nheader.magic != magic)
                throw new Exception("This is not NCER file");
            cebk = new CEBK(reader);
            byte tmp = reader.ReadByte();
            if (tmp == 0)
                reader.ReadByte();
            else
                reader.BaseStream.Position--;
            labl = new LABL(reader);
            uext = new TXEU(reader);
            long leng = nheader.fileSize - reader.BaseStream.Position;
            if (leng > 0)
                additional = reader.ReadBytes((int)leng);
            else additional = new byte[0];
        }

        public void Save(BinaryWriter bw)
        {
            nheader.Save(bw);
            cebk.Save(bw);
            labl.Save(bw);
            uext.Save(bw);
        }

        public string[] GetNames()
        {
            return labl.names;
        }

        public void Adjust()
        {
            cebk.Adjust();
            nheader.fileSize = 0x10 + cebk.size + labl.size + uext.size;
        }
    }
}
