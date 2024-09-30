using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.UnityIpnce
{
    public class CollectionIpnce : I_Ipnce
    {
        public string[] SpriteAtlasNames;
        public string ColorPaletteName;
        public string[] FHDAtlasNames;

        public CollectionIpnce() : base() { }
        public CollectionIpnce(byte[] dat) : base()
        {
            Load(dat);
        }

        public CollectionIpnce(BinaryReader br)
        {
            Load(br);
        }

        public CollectionIpnce(Ipnce ipnce)
        {
            ipnce.CopyTo(this);
        }

        public CollectionIpnce(AAIIpnce ipnce)
        {
            ipnce.CopyTo(this);
            IsHD = ipnce.DataType == AAIIpnce.DataTypes.HD;
        }

        public CollectionIpnce(AJIpnce ipnce)
        {
            ipnce.CopyTo(this);
        }

        public CollectionIpnce(CollectionAAI1Ipnce ipnce)
        {
            ipnce.CopyTo(this);
            IsHD = ipnce.DataType == AAIIpnce.DataTypes.HD;
            SpriteAtlasNames = new string[2];
            SpriteAtlasNames[0] = ipnce.m_SpriteAtlasName;
            SpriteAtlasNames[1] = ipnce.m_SpriteAtlasOverflowName;
            ColorPaletteName = ipnce.m_ColorPaletteName;
            FHDAtlasNames = new string[2];
            FHDAtlasNames[0] = ipnce.FHDm_SpriteAtlasName;
            FHDAtlasNames[1] = ipnce.FHDm_SpriteAtlasOverflowName;
        }

        public override I_Sprite GetNewSprite()
        {
            return new Sprite();
        }

        public override I_Anim GetNewAnim()
        {
            return new Anim();
        }

        public override void Load(BinaryReader br)
        {
            this.IsHD = br.ReadInt32() == 1;
            this.IsUseColorPalette = br.ReadInt32() == 1;
            this.IsOffScreenRendering = br.ReadInt32() == 1;
            this.ColorPaletteNum = (byte)(br.ReadInt32());
            int len = br.ReadInt32();
            this.SpriteList = new Sprite[len];
            for (int i = 0; i < len; i++)
            {
                this.SpriteList[i] = new Sprite();
                this.SpriteList[i].Load(br);
            }
            len = br.ReadInt32();
            this.AnimList = new Anim[len];
            for (int i = 0; i < len; i++)
            {
                this.AnimList[i] = new Anim();
                this.AnimList[i].Load(br);
            }
            SpriteAtlasNames = new string[br.ReadInt32()];
            for (int i = 0; i < SpriteAtlasNames.Length; i++)
            {
                SpriteAtlasNames[i] = ReadAlgnedString(br);
            }
            this.m_SpriteAtlasName = SpriteAtlasNames[0];
            this.m_SpriteAtlasOverflowName = SpriteAtlasNames[1];
            ColorPaletteName = ReadAlgnedString(br);
            this.m_ColorPaletteName = ColorPaletteName;
            FHDAtlasNames = new string[br.ReadInt32()];
            for (int i = 0; i < FHDAtlasNames.Length; i++)
            {
                FHDAtlasNames[i] = ReadAlgnedString(br);
            }
            this.FHDm_SpriteAtlasName = FHDAtlasNames[0];
            this.FHDm_SpriteAtlasOverflowName = FHDAtlasNames[1];
            this.PixelSnap = br.ReadInt32() == 1;
            this.HalfTexelOffset = br.ReadInt32() == 1;
        }

        public override void Save(BinaryWriter bw)
        {
            int b = IsHD ? 1 : 0;
            bw.Write(b);
            b = IsUseColorPalette ? 1 : 0;
            bw.Write(b);
            b = IsOffScreenRendering ? 1 : 0;
            bw.Write(b);
            bw.Write((int)ColorPaletteNum);
            bw.Write(SpriteList.Length);
            for (int i = 0; i < SpriteList.Length; i++)
            {
                SpriteList[i].Save(bw);
            }
            bw.Write(AnimList.Length);
            for (int i = 0; i < AnimList.Length; i++)
            {
                AnimList[i].Save(bw);
            }
            bw.Write(SpriteAtlasNames.Length);
            for (int i = 0; i < SpriteAtlasNames.Length; i++)
            {
                WriteAlignedString(bw, SpriteAtlasNames[i]);
            }
            WriteAlignedString(bw, ColorPaletteName);
            bw.Write(FHDAtlasNames.Length);
            for (int i = 0; i < FHDAtlasNames.Length; i++)
            {
                WriteAlignedString(bw, FHDAtlasNames[i]);
            }
            b = PixelSnap ? 1 : 0;
            bw.Write(b);
            b = HalfTexelOffset ? 1 : 0;
            bw.Write(b);
        }

        public static string ReadAlgnedString(BinaryReader br)
        {
            int len = br.ReadInt32();
            byte[] strbytes = br.ReadBytes(len);
            if (len % 4 != 0)
                br.ReadBytes(4 - (len % 4));
            return Encoding.UTF8.GetString(strbytes);
        }

        public static void WriteAlignedString(BinaryWriter bw, string str)
        {
            byte[] name = Encoding.UTF8.GetBytes(str);
            bw.Write(name.Length);
            bw.Write(name);
            if (name.Length % 4 != 0)
                for (int i = 0; i < (4 - name.Length % 4);i++)
                    bw.Write((byte)0);
        }
    }
}
