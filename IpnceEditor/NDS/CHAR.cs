using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.NDS
{
    public class CHAR
    {
        const uint magic = 0x43484152;
        public uint sectionsize;
        public short height;
        public short width;
        public int bpp;
        public int mapping;
        public int mode;
        public int tiledatsize;
        public int unk;
        public int realbpp;
        //public byte[][] tyles;
        public byte[] pixelData;

        public CHAR() { }

        public CHAR(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader reader)
        {
            uint m = reader.ReadUInt32();
            if (m != magic)
            {
                throw new Exception("CHAR magic is not present");
            }
            sectionsize = reader.ReadUInt32();
            height = reader.ReadInt16();
            width = reader.ReadInt16();
            bpp = reader.ReadInt32();
            realbpp = bpp == 3 ? 4 : 8;
            mapping = reader.ReadInt32();
            mode = reader.ReadInt32();
            tiledatsize = reader.ReadInt32();
            unk = reader.ReadInt32();
            pixelData = new byte[tiledatsize * (8 / realbpp)];
            /*int bytecount = (8 / realbpp);
            int pixelcount = tiledatsize * bytecount;
            int tilescount = pixelcount / 64;
            tyles = new byte[tilescount][];
            for (int i = 0; i < tilescount; i++)
            {
                tyles[i] = new byte[64];
                for (int j = 0; j < 64 / bytecount; j++)
                {
                    byte tmp = reader.ReadByte();
                    if (bytecount == 2)
                    {
                        tyles[i][j * 2] = (byte)(tmp % 16);
                        tyles[i][j * 2 + 1] = (byte)(tmp / 16);
                    }
                    else
                        tyles[i][j] = tmp;
                }
            } */
            for (int i = 0; i < tiledatsize; i++)
            {
                byte tmp = reader.ReadByte();
                if (realbpp == 4)
                {
                    pixelData[i * 2] = (byte)(tmp % 16);
                    pixelData[i * 2 + 1] = (byte)(tmp / 16);
                }
                else
                    pixelData[i] = tmp;
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(magic);
            writer.Write(sectionsize);
            writer.Write(height);
            writer.Write(width);
            writer.Write(bpp);
            writer.Write(mapping);
            writer.Write(mode);
            writer.Write(tiledatsize);
            writer.Write(unk);
            for (int i = 0; i < tiledatsize; i++)
            {
                if (realbpp == 4)
                {
                    writer.Write((byte)(pixelData[i * 2] + 16 * pixelData[i * 2 + 1]));
                }
                else
                    writer.Write(pixelData[i]);
            }
        }

        public void Adjust()
        {
            tiledatsize = pixelData.Length * realbpp / 8;
            sectionsize = (uint)tiledatsize + 0x20;
        }

        public byte[][] GetImageData(int pointer, int width, int height)
        {
            pointer *= 64;
            byte[][] res = new byte[height * 8][];
            for (int i = 0; i < height * 8; i++)
            {
                res[i] = new byte[width * 8];
            }
            if (mode == 0)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            for (int z = 0; z < 8; z++)
                            {
                                res[i * 8 + k][j * 8 + z] = pixelData[pointer++];
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < height * 8; i++)
                {
                    for (int j = 0; j < width * 8; j++)
                    {
                        res[i][j] = pixelData[pointer++];
                    }
                }
            }
            return res;
        }

        public void SetImageData(byte[][] dat, int pointer)
        {
            int height = dat.Length / 8;
            int width = dat[0].Length / 8;
            if (mode == 0)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            for (int z = 0; z < 8; z++)
                            {
                                pixelData[pointer++] = dat[i * 8 + k][j * 8 + z];
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < height * 8; i++)
                {
                    for (int j = 0; j < width * 8; j++)
                    {
                        pixelData[pointer++] = dat[i][j];
                    }
                }
            }
        }

        public void AddNewData(int len)
        {
            List<byte> data = new List<byte>();
            data.AddRange(pixelData);
            byte[] addi = new byte[len];
            data.AddRange(addi);
            pixelData = data.ToArray();
        }

        public void AddNewData(int height, int width)
        {
            int len = height * width * 64;
            AddNewData(len);
        }
        public void AddNewData(byte[][] dat)
        {
            int poi = pixelData.Length;
            AddNewData(dat.Length * dat[0].Length);
            SetImageData(dat, poi);
        }
    }
}
