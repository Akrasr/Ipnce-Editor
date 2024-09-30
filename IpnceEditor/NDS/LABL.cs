using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace IpnceEditor.NDS
{
    public class LABL
    {
        const uint magic = 0x4C41424C;
        public uint size;
        public string[] names;
        uint unk;

        public LABL()
        {
            names = null;
            size = 12;
        }

        public LABL(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader reader)
        {
            uint m = reader.ReadUInt32();
            if (m != magic)
            {
                throw new Exception("LABL magic is not present");
            }
            size = reader.ReadUInt32();
            if (size != 12)
            {
                long lastpoint = GetLastPlace(reader);
                List<uint> pointers = new List<uint>();
                long x = 0;
                //MessageBox.Show("last " + lastpoint.ToString("X"));
                while (true)
                {
                    x += 4;
                    uint pointer = reader.ReadUInt32();
                    pointers.Add(pointer);
                    //MessageBox.Show("" + (x + pointer).ToString("X"));
                    if (x + pointer >= lastpoint)
                        break;
                }
                names = new string[pointers.Count];
                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = ReadStringToNull(reader);
                }
            } else unk = reader.ReadUInt32();
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(magic);
            bw.Write(size);
            if (names == null)
            {
                bw.Write(0);
                return;
            }
            uint len = 0;
            for (int i = 0; i < names.Length; i++)
            {
                bw.Write(len);
                len += (uint)(names[i].Length + 1);
            }
            foreach (string name in names)
            {
                bw.Write(Encoding.UTF8.GetBytes(name));
                bw.Write((byte)0);
            }
        }

        public long GetLastPlace(BinaryReader br)
        {
            long lastpos = br.BaseStream.Position;
            br.BaseStream.Position = lastpos + size - 10;
            byte mean = br.ReadByte();
            while (mean != 0)
            {
                br.BaseStream.Position -= 2;
                mean = br.ReadByte();
            }
            long res = br.BaseStream.Position - lastpos;
            br.BaseStream.Position = lastpos;
            return res;
        }

        public string ReadStringToNull(BinaryReader br)
        {
            List<byte> tmp = new List<byte>();
            byte mean = br.ReadByte();
            while (mean != 0)
            {
                tmp.Add(mean);
                mean = br.ReadByte();
            }
            string res = Encoding.UTF8.GetString(tmp.ToArray());
            return res;
        }
    }
}
