using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.NDS
{
    public class NANR
    {
        public const uint magic = 0x4e414e52;
        NitroHeader nheader;
        public ABNK bnk;
        public LABL labl;
        TXEU uext;
        byte[] additional;

        public NANR(ABNK bk)
        {
            bnk = bk;
            nheader = new NitroHeader(magic, 3);
            labl = new LABL();
            uext = new TXEU();
            nheader.fileSize = 0x10 + bnk.size + labl.size + uext.size;
            additional = new byte[0];
        }

        public NANR(BinaryReader br)
        { Load(br); }

        public void Load(BinaryReader reader)
        {
            nheader = new NitroHeader(reader);
            if (nheader.magic != magic)
                throw new Exception("This is not NANR file");
            bnk = new ABNK(reader);
            labl = new LABL(reader);
            uext = new TXEU(reader);
            long leng = nheader.fileSize - reader.BaseStream.Position;
            if (leng > 0)
                additional = reader.ReadBytes((int)leng);
            else additional = new byte[0];
        }

        public void Save(BinaryWriter writer)
        {
            nheader.Save(writer);
            bnk.Save(writer);
            labl.Save(writer);
            uext.Save(writer);
            writer.Write(additional);
        }

        public void Adjust()
        {
            bnk.Adjust();
            nheader.fileSize = 0x10 + bnk.size + labl.size + uext.size;
        }
    }
}
