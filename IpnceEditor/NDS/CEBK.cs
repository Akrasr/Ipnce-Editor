using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IpnceEditor.NDS
{
    public class CEBK
    {
        const uint magic = 0x4345424b;
        public uint size;
        public ushort cellCount;
        public short extended;
        public uint unk;
        public uint mapping;
        public Nitro_Cell[] cells;
        public Nitro_OAM[] parts;

        public CEBK()
        {
            unk = 0x18;
            mapping = 2;
            extended = 0;
            cellCount = 0;
            cells = new Nitro_Cell[0];
        }
        public CEBK(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader reader)
        {
            uint m = reader.ReadUInt32();
            if (m != magic)
            {
                throw new Exception("CEBK magic is not present");
            }
            size = reader.ReadUInt32();
            cellCount = reader.ReadUInt16();
            extended = reader.ReadInt16();
            unk = reader.ReadUInt32();
            mapping = reader.ReadUInt32();
            reader.ReadUInt32(); // padding
            reader.ReadUInt32();
            reader.ReadUInt32();
            cells = new Nitro_Cell[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                cells[i] = new Nitro_Cell(reader);
            }
            Nitro_Cell last = cells[cells.Length - 1];
            int oamcount = (int)(last.oamPointer / 6) + last.oam_count;
            parts = new Nitro_OAM[oamcount];
            for (int i = 0; i < oamcount; i++)
            {
                parts[i] = new Nitro_OAM(reader);
                //MessageBox.Show(reader.BaseStream.Position.ToString("X"));
            }
            for (int i = 0; i < cellCount; i++)
            {
                cells[i].GetParts(parts);
                cells[i].SetSize();
            }
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(magic);
            bw.Write(size);
            bw.Write(cellCount);
            bw.Write(extended);
            bw.Write(unk);
            bw.Write(mapping);
            bw.Write(0);
            bw.Write(0);
            bw.Write(0);
            foreach (Nitro_Cell cell in cells)
                cell.Save(bw);
            foreach (Nitro_Cell cell1 in cells)
                cell1.SaveParts(bw);
            /*foreach (Nitro_OAM oam in parts)
                oam.Save(bw);*/
        }

        public void Adjust()
        {
            uint poi = 0;
            foreach (Nitro_Cell cell in cells)
            {
                cell.oamPointer = poi;
                poi += (uint)(cell.CellParts.Length * 6);
                cell.oam_count = (ushort)cell.CellParts.Length;
            }
            size = (uint)(0x20 + cells.Length * 0x8 + poi);
            cellCount = (ushort)cells.Length;
        }

        public void AddCell()
        {
            List<Nitro_Cell> cs = new List<Nitro_Cell>();
            cs.AddRange(cells);
            Nitro_Cell n = new Nitro_Cell();
            if (cs.Count != 0)
                n.unk = cs.Last().unk;
            cs.Add(n);
            cells = cs.ToArray();
            cellCount++;
        }

        public void AddOam(int ind, bool fi)
        {
            cells[ind].AddOam(fi);
        }
    }

    public class Nitro_Cell
    {
        public ushort oam_count;
        public ushort unk;
        public uint oamPointer;
        public uint size;

        public Nitro_OAM[] CellParts;

        public Nitro_Cell() { CellParts = new Nitro_OAM[0]; }

        public Nitro_Cell(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader br)
        {
            oam_count = br.ReadUInt16();
            unk = br.ReadUInt16();
            oamPointer = br.ReadUInt32();
        }

        public void SetSize()
        {
            uint tmps = 0;
            foreach (Nitro_OAM oam in CellParts)
            {
                uint st = 0;
                switch (oam.shape)
                {
                    case 0:
                        st = (uint)Math.Pow(2, oam.size * 2);
                        break;
                    case 1:
                    case 2:
                        switch (oam.size)
                        {
                            case 0:
                                st = 2;
                                break;
                            case 1:
                                st = 4;
                                break;
                            case 2:
                                st = 8;
                                break;
                            case 3:
                                st = 32;
                                break;
                        }
                        break;
                }
                tmps += st;
                break;
            }
            size = tmps * 64;
        }

        public void GetParts(Nitro_OAM[] oams)
        {
            CellParts = new Nitro_OAM[oam_count];
            int st = (int)(oamPointer / 6);
            for (int i = 0; i < oam_count; i++)
            {
                CellParts[i] = oams[st + i];
            }
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(oam_count);
            bw.Write(unk);
            bw.Write(oamPointer);
        }

        public void AddOam(bool fi)
        {
            List<Nitro_OAM> ps = new List<Nitro_OAM>();
            ps.Add(Nitro_OAM.CreateBlank(fi));
            ps.AddRange(CellParts);
            CellParts = ps.ToArray();
            oam_count++;
        }

        public void AddOam(Nitro_OAM oam)
        {
            List<Nitro_OAM> ps = new List<Nitro_OAM>();
            ps.AddRange(CellParts);
            ps.Add(oam);
            CellParts = ps.ToArray();
            oam_count++;
        }

        public void SaveParts(BinaryWriter bw)
        {
            foreach (Nitro_OAM part in CellParts)
                part.Save(bw);
        }
    }

    public class Nitro_OAM
    {
        public short y;
        public bool rot;
        public bool sizeDisable;
        public byte mode;
        public bool mosaic;
        public bool col8bit;
        public byte shape;
        public short x;
        public byte rotsca;
        public byte size;
        public ushort cch;
        public byte prio;
        public byte pal;

        public Nitro_OAM()
        { }

        public static Nitro_OAM CreateBlank(bool bit8)
        {
            Nitro_OAM oam = new Nitro_OAM();
            oam.y = 0;
            oam.rot = false;
            oam.sizeDisable = false;
            oam.mode = 0;
            oam.mosaic = false;
            oam.col8bit = bit8;
            oam.shape = 0;
            oam.x = 0;
            oam.rotsca = 0;
            oam.size = 0;
            oam.cch = 0;
            oam.prio = 0;
            oam.pal = 0;
            return oam;
        }

        public Nitro_OAM(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader br)
        {
            byte first = br.ReadByte();
            if (first > 0x80)
            {
                y = (short)((0x100 - first) * (-1));
            }
            else y = first;
            byte sec = br.ReadByte();
            rot = (sec & 1) != 0;
            sizeDisable = (sec & 2) != 0;
            mode = (byte)((sec >> 2) & 3);
            mosaic = (sec & 0x10) != 0;
            col8bit = (sec & 0x20) != 0;
            shape = (byte)((sec >> 6) & 3);
            byte third = br.ReadByte();
            x = third;
            if (third >= 0x80)
            {
                x = (short)((0x100 - x) * (-1));
            }
            byte fourth = br.ReadByte();
            rotsca = (byte)((fourth) & 0x3f);
            size = (byte)((fourth >> 6) & 3);
            ushort fifth = br.ReadUInt16();
            cch = (ushort)(fifth & 0x3ff);
            prio = (byte)((fifth >> 10) & 3);
            pal = (byte)((fifth >> 12) & 0xf);
        }

        public void Save(BinaryWriter bw)
        {
            byte first = (byte)(y < 0 ? 0x100 + y : y);
            byte second = 0;
            second += (byte)(rot ? 1 : 0);
            second += (byte)(sizeDisable ? 2 : 0);
            second += (byte)(mode << 2);
            second += (byte)(mosaic ? 0x10 : 0);
            second += (byte)(col8bit ? 0x20 : 0);
            second += (byte)(shape << 6);
            byte third = (byte)(x < 0 ? 0x100 + x : x);
            byte fourth = (byte)(rotsca);
            //fourth += (byte)(rotsca << 1);
            fourth += (byte)(size << 6);
            ushort fifth = (ushort)cch;
            fifth += (ushort)(prio << 10);
            fifth += (ushort)(pal << 12);
            bw.Write(first);
            bw.Write(second);
            bw.Write(third);
            bw.Write(fourth);
            bw.Write(fifth);
        }

        public void SetNewForm(int form)
        {
            cch = 0;
            shape = (byte)(form / 4);
            size = (byte)(form % 4);
        }
    }
}
