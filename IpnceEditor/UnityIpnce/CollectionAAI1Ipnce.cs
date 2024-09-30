using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.UnityIpnce
{
    public class CollectionAAI1Ipnce : I_Ipnce
    {
        public CollectionAAI1Ipnce() : base() { }
        public CollectionAAI1Ipnce(byte[] dat) : base()
        {
            Load(dat);
        }

        public CollectionAAI1Ipnce(BinaryReader br) : base()
        {
            Load(br);
        }

        public override I_Sprite GetNewSprite()
        {
            return new CollectionSprite();
        }

        public override I_Anim GetNewAnim()
        {
            return new AAIAnim();
        }

        public CollectionAAI1Ipnce(Ipnce ipnce)
        {
            ipnce.CopyTo(this);
            this.DataType = ipnce.IsHD ? AAIIpnce.DataTypes.HD : AAIIpnce.DataTypes.HalfHD;
            this.FHDDataType = ipnce.IsHD ? AAIIpnce.DataTypes.FHD : AAIIpnce.DataTypes.HalfFHD;
        }

        public CollectionAAI1Ipnce(AAIIpnce ipnce)
        {
            ipnce.CopyTo(this);
            this.FHDDataType = ipnce.DataType == AAIIpnce.DataTypes.HD ? AAIIpnce.DataTypes.FHD : AAIIpnce.DataTypes.HalfFHD;
        }

        public CollectionAAI1Ipnce(AJIpnce ipnce)
        {
            ipnce.CopyTo(this);
            this.DataType = ipnce.IsHD ? AAIIpnce.DataTypes.HD : AAIIpnce.DataTypes.HalfHD;
            this.FHDDataType = ipnce.IsHD ? AAIIpnce.DataTypes.FHD : AAIIpnce.DataTypes.HalfFHD;
        }

        public CollectionAAI1Ipnce(CollectionIpnce ipnce)
        {
            ipnce.CopyTo(this);
            this.DataType = ipnce.IsHD ? AAIIpnce.DataTypes.HD : AAIIpnce.DataTypes.HalfHD;
            this.m_SpriteAtlasName = ipnce.SpriteAtlasNames[0];
            this.m_SpriteAtlasOverflowName = ipnce.SpriteAtlasNames[1];
            this.m_ColorPaletteName = ipnce.ColorPaletteName;
            this.FHDm_SpriteAtlasName = ipnce.FHDAtlasNames[0];
            this.FHDm_SpriteAtlasOverflowName = ipnce.FHDAtlasNames[1];
            this.FHDDataType = ipnce.IsHD ? AAIIpnce.DataTypes.FHD : AAIIpnce.DataTypes.HalfFHD;
        }

        public override void Load(BinaryReader br)
        {
            this.DataType = (AAIIpnce.DataTypes)br.ReadInt32();
            this.IsUseColorPalette = br.ReadInt32() == 1;
            this.IsOffScreenRendering = br.ReadInt32() == 1;
            this.IsSplitLongTexture = br.ReadInt32() == 1;
            m_SpriteAtlasName = CollectionIpnce.ReadAlgnedString(br);
            m_SpriteAtlasOverflowName = CollectionIpnce.ReadAlgnedString(br);
            m_ColorPaletteName = CollectionIpnce.ReadAlgnedString(br);
            this.ColorPaletteNum = (byte)(br.ReadInt32());
            this.FHDDataType = (AAIIpnce.DataTypes)br.ReadInt32();
            FHDm_SpriteAtlasName = CollectionIpnce.ReadAlgnedString(br);
            FHDm_SpriteAtlasOverflowName = CollectionIpnce.ReadAlgnedString(br);
            int len = br.ReadInt32();
            this.SpriteList = new CollectionSprite[len];
            for (int i = 0; i < len; i++)
            {
                this.SpriteList[i] = new CollectionSprite();
                this.SpriteList[i].Load(br);
            }
            len = br.ReadInt32();
            this.AnimList = new AAIAnim[len];
            for (int i = 0; i < len; i++)
            {
                this.AnimList[i] = new AAIAnim();
                this.AnimList[i].Load(br);
            }
            this.PixelSnap = br.ReadInt32() == 1;
            this.HalfTexelOffset = br.ReadInt32() == 1;
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write((int)this.DataType);
            int b = IsUseColorPalette ? 1 : 0;
            bw.Write(b);
            b = IsOffScreenRendering ? 1 : 0;
            bw.Write(b);
            b = IsSplitLongTexture ? 1 : 0;
            bw.Write(b);
            CollectionIpnce.WriteAlignedString(bw, m_SpriteAtlasName);
            CollectionIpnce.WriteAlignedString(bw, m_SpriteAtlasOverflowName);
            CollectionIpnce.WriteAlignedString(bw, m_ColorPaletteName);
            bw.Write((int)ColorPaletteNum);
            bw.Write((int)this.FHDDataType);
            CollectionIpnce.WriteAlignedString(bw, FHDm_SpriteAtlasName);
            CollectionIpnce.WriteAlignedString(bw, FHDm_SpriteAtlasOverflowName);
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
                for (int i = 0; i < (4 - name.Length % 4); i++)
                    bw.Write((byte)0);
        }
    }

    public class CollectionSprite : I_Sprite
    {

        public CollectionSprite() : base()
        {
            DestX = 0;
            DestY = 0;
            ScaleX = 0;
            ScaleY = 0;
            Parts = new SpriteParts[1];
            Parts[0] = SpriteParts.CreateEmpty();
        }

        public CollectionSprite(Sprite sprite) : base()
        {
            DestX = sprite.DestX;
            DestY = sprite.DestY;
            ScaleX = 1;
            ScaleY = 1;
            Parts = sprite.Parts;
        }

        public CollectionSprite(AAISprite sprite) : base()
        {
            DestX = sprite.DestX;
            DestY = sprite.DestY;
            ScaleX = 1;
            ScaleY = 1;
            Parts = sprite.Parts;
        }

        public override void Load(BinaryReader br)
        {
            this.DestX = br.ReadSingle();
            this.DestY = br.ReadSingle();
            this.ScaleX = br.ReadSingle();
            this.ScaleY = br.ReadSingle();
            int len = br.ReadInt32();
            this.Parts = new SpriteParts[len];
            for (int i = 0; i < len; i++)
            {
                this.Parts[i] = new SpriteParts();
                this.Parts[i].Load(br);
            }
        }
        public override void Save(BinaryWriter bw)
        {
            bw.Write(DestX);
            bw.Write(DestY);
            bw.Write(ScaleX);
            bw.Write(ScaleY);
            bw.Write(Parts.Length);
            for (int i = 0; i < Parts.Length; i++)
            {
                Parts[i].Save(bw);
            }
        }
    }
}
