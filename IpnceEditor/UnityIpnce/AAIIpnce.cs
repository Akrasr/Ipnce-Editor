using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace IpnceEditor.UnityIpnce
{
    public class AAIIpnce : I_Ipnce
    {
		public enum DataTypes
		{
			NDS,
			HD,
			HalfHD,
			Direct,
			FHD,
            HalfFHD
        }

		public AAIIpnce(BinaryReader br) : base()
		{
			Load(br);
        }

        public override I_Sprite GetNewSprite()
        {
			return new AAISprite();
        }

        public override I_Anim GetNewAnim()
        {
            return new AAIAnim();
        }

        public AAIIpnce(Ipnce ipnce)
        {
			ipnce.CopyTo(this);
			this.DataType = ipnce.IsHD ? DataTypes.HD : DataTypes.HalfHD;
            SpriteAtlas = ipnce.SpriteAtlasAr[0];
            SpriteAtlasOverflow = ipnce.SpriteAtlasAr[1];
            this.FHDDataType = ipnce.IsHD ? AAIIpnce.DataTypes.FHD : AAIIpnce.DataTypes.HalfFHD;
        }

        public AAIIpnce(CollectionIpnce ipnce)
        {
            ipnce.CopyTo(this);
            this.DataType = ipnce.IsHD ? DataTypes.HD : DataTypes.HalfHD;
            SpriteAtlas = new Texture2D()
			{
				in1 = string.IsNullOrEmpty(ipnce.SpriteAtlasNames[0]) ? 1 : 0,
				in2 = 0,
				in3 = 0
			};
            SpriteAtlasOverflow = new Texture2D()
            {
                in1 = string.IsNullOrEmpty(ipnce.SpriteAtlasNames[1]) ? 2 : 0,
                in2 = 0,
                in3 = 0
            };
			ColorPalette = new Texture2D()
			{
				in1 = IsUseColorPalette ? (IsSplitLongTexture ? 3 : 2) : 0,
                in2 = 0,
                in3 = 0
            };
        }

        public AAIIpnce(CollectionAAI1Ipnce ipnce)
        {
            ipnce.CopyTo(this);
            SpriteAtlas = new Texture2D()
            {
                in1 = string.IsNullOrEmpty(ipnce.m_SpriteAtlasName) ? 1 : 0,
                in2 = 0,
                in3 = 0
            };
            SpriteAtlasOverflow = new Texture2D()
            {
                in1 = string.IsNullOrEmpty(ipnce.m_SpriteAtlasOverflowName) ? 2 : 0,
                in2 = 0,
                in3 = 0
            };
            ColorPalette = new Texture2D()
            {
                in1 = IsUseColorPalette ? (IsSplitLongTexture ? 3 : 2) : 0,
                in2 = 0,
                in3 = 0
            };
        }

        public AAIIpnce(AJIpnce ipnce)
        {
            ipnce.CopyTo(this);
            DataType = ipnce.IsHD ? DataTypes.HD : DataTypes.NDS;
        }

        public override void Load(BinaryReader br)
        {
			this.DataType = (AAIIpnce.DataTypes)br.ReadInt32();
			this.IsUseColorPalette = br.ReadInt32() == 1;
			this.IsOffScreenRendering = br.ReadInt32() == 1;
			this.IsSplitLongTexture = br.ReadInt32() == 1;
			this.SpriteAtlas = new Texture2D();
			this.SpriteAtlas.Load(br);
			this.SpriteAtlasOverflow = new Texture2D();
			this.SpriteAtlasOverflow.Load(br);
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
			this.AnimList = new AAIAnim[len];
			for (int i = 0; i < len; i++)
			{
				this.AnimList[i] = new AAIAnim();
				this.AnimList[i].Load(br);
			}
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
			SpriteAtlas.Save(bw);
			SpriteAtlasOverflow.Save(bw);
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

	public class AAISprite : I_Sprite
    {

		public AAISprite() : base()
        {
            DestX = 0;
            DestY = 0;
            Parts = new SpriteParts[1];
            Parts[0] = SpriteParts.CreateEmpty();
        }

        public AAISprite(CollectionSprite sprite) : base()
        {
            DestX = sprite.DestX;
            DestY = sprite.DestY;
            Parts = sprite.Parts;
        }

        public AAISprite(Sprite sprite) : base()
        {
		    DestX = sprite.DestX;
			DestY = sprite.DestY;
			Parts = sprite.Parts;
		}

		public override void Load(BinaryReader br)
		{
			this.DestX = br.ReadSingle();
			this.DestY = br.ReadSingle();
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
			bw.Write(Parts.Length);
			for (int i = 0; i < Parts.Length; i++)
			{
				Parts[i].Save(bw);
			}
        }
    }

	public class AAIAnim : I_Anim
	{
		public const uint FLAG_LOOP = 1u;

		public AAIAnim() : base()
		{ }

		public AAIAnim(Anim an) : base()
        {
			DestX = an.DestX;
			DestY = an.DestY;
			Flag = an.Flag;
			KeyFrames = an.KeyFrames;
			TotalFrameSize = an.TotalFrameSize;
			RestartFrame = an.RestartFrame;
		}

		public override void Load(BinaryReader br)
		{
			this.TotalFrameSize = br.ReadInt32();
			this.RestartFrame = br.ReadInt32();
			this.DestX = br.ReadSingle();
			this.DestY = br.ReadSingle();
			this.Flag = br.ReadInt32();
			int len = br.ReadInt32();
			KeyFrames = new AnimKeyframe[len];
			for (int i = 0; i < len; i++)
			{
				KeyFrames[i] = new AnimKeyframe();
				KeyFrames[i].Load(br);
			}
		}

		public override void Save(BinaryWriter bw)
		{
			bw.Write(TotalFrameSize);
			bw.Write(RestartFrame);
			bw.Write(DestX);
			bw.Write(DestY);
			bw.Write(Flag);
			bw.Write(KeyFrames.Length);
			for (int i = 0; i < KeyFrames.Length; i++)
			{
				KeyFrames[i].Save(bw);
			}
        }
    }
}
