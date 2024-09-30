using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IpnceEditor.NDS
{
    public class NCLR
    {
        public const uint magic = 0x4e434c52;
        NitroHeader nheader;
        PLTT pal;
        PCMP map;
        public Dictionary<ushort, Color[]> palettes;

        public NCLR(BinaryReader br)
        { Load(br); }

        public void Load(BinaryReader reader)
        {
            nheader = new NitroHeader(reader);
            if (nheader.magic != magic)
                throw new Exception("This is not NCLR file");
            pal = new PLTT(reader);
            map = new PCMP(reader);
            LoadColors();
        }

        public void LoadColors()
        {
            Bitmap bmp = new Bitmap((pal.coldata.Length / map.palc), map.palc);
            palettes = new Dictionary<ushort, Color[]>();
            for (int i = 0; i < map.palc; i++)
            {
                int len = pal.coldata.Length / map.palc;
                Color[] col = new Color[len];
                for (int j = 0; j < len; j++)
                {
                    col[j] = GetColorFromShort(pal.coldata[len * i + j]);
                    bmp.SetPixel(j, i, col[j]);
                }
                palettes.Add(map.palindexes[i], col);
            }
            bmp.Save("log.png");
        }

        public void Save(BinaryWriter writer)
        {
            nheader.Save(writer);
            pal.Save(writer);
            map.Save(writer);
        }

        public Color GetColorFromShort(ushort col)
        {
            int c1 = (col & (0x1F)) * 8;
            int c2 = ((col & (0x1F << 5)) >> 5) * 8;
            int c3 = ((col & (0x1F << 10)) >> 10) * 8;
            //MessageBox.Show(col.ToString("X") + " " + c1.ToString("X") + " " + c2.ToString("X") + " " + c3.ToString("X"));
            return Color.FromArgb(c1, c2, c3);
        }
    }
}
