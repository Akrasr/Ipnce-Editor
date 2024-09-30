using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.NDS
{
    public class NCGR
    {
        public const uint magic = 0x4e434752;
        NitroHeader nheader;
        public CHAR maindata;

        public NCGR(CHAR dat)
        {
            maindata = dat;
            nheader = new NitroHeader(magic, 1);
            nheader.fileSize = 0x10 + maindata.sectionsize;
        }

        public NCGR(BinaryReader br)
        { Load(br); }

        public void Load(BinaryReader reader)
        {
            nheader = new NitroHeader(reader);
            if (nheader.magic != magic)
                throw new Exception("This is not NCGR file");
            maindata = new CHAR(reader);
        }

        public void Save(BinaryWriter writer)
        {
            nheader.Save(writer);
            maindata.Save(writer);
        }

        public void Adjust()
        {
            maindata.Adjust();
            nheader.fileSize = 0x10 + maindata.sectionsize;
        }
    }
}
