using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.NDS
{
    public class TXEU
    {
        const uint magic = 0x55455854;
        public uint size;
        uint unk;

        public TXEU() {
            size = 12;
            unk = 0;
        }

        public TXEU(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader reader)
        {
            uint m = reader.ReadUInt32();
            if (m != magic)
            {
                throw new Exception("UEXT magic is not present");
            }
            size = reader.ReadUInt32();
            unk = reader.ReadUInt32();
            if (unk != 0)
            {
                throw new Exception("UEXT unk is not zero");
            }
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(magic);
            bw.Write(size);
            bw.Write(unk);
        }
    }
}
