using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IpnceEditor.UnityIpnce
{
	public class AJIpnce : I_Ipnce
	{
		public AJIpnce(BinaryReader br)
		{
			Load(br);
        }

        public AJIpnce(AAIIpnce ipnce)
        {
            ipnce.CopyTo(this);
            IsHD = ipnce.DataType == AAIIpnce.DataTypes.HD;
        }

        public AJIpnce(Ipnce ipnce)
        {
            ipnce.CopyTo(this);
            SpriteAtlas = ipnce.SpriteAtlasAr[0];
        }

        public AJIpnce(CollectionIpnce ipnce)
        {
            ipnce.CopyTo(this);
            SpriteAtlas = new Texture2D()
            {
                in1 = string.IsNullOrEmpty(ipnce.SpriteAtlasNames[0]) ? 1 : 0,
                in2 = 0,
                in3 = 0
            };
        }

        public AJIpnce(CollectionAAI1Ipnce ipnce)
        {
            ipnce.CopyTo(this);
            IsHD = ipnce.DataType == AAIIpnce.DataTypes.HD;
            SpriteAtlas = new Texture2D()
            {
                in1 = string.IsNullOrEmpty(ipnce.m_SpriteAtlasName) ? 1 : 0,
                in2 = 0,
                in3 = 0
            };
        }

        public override I_Sprite GetNewSprite()
        {
            return new AAISprite();
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
			this.SpriteAtlas = new Texture2D();
			this.SpriteAtlas.Load(br);
			this.ColorPaletteNum = (byte)(br.ReadInt32());
			ColorPalette = new Texture2D();
			ColorPalette.Load(br);
			int len = br.ReadInt32();
			this.SpriteList = new AAISprite[len];
			for (int i = 0; i < len; i++)
			{
				this.SpriteList[i] = new AAISprite();
				this.SpriteList[i].Load(br);
			}
			len = br.ReadInt32();
			this.AnimList = new Anim[len];
			for (int i = 0; i < len; i++)
			{
				this.AnimList[i] = new Anim();
				this.AnimList[i].Load(br);
			}
		}

		public override void Save(BinaryWriter bw)
		{
			int b = IsHD ? 1 : 0;
			bw.Write(b);
			b = IsUseColorPalette ? 1 : 0;
			bw.Write(b);
			b = IsOffScreenRendering ? 1 : 0;
			bw.Write(b);
			SpriteAtlas.Save(bw);
			bw.Write((int)ColorPaletteNum);
			ColorPalette.Save(bw);
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
        }
    }
}
