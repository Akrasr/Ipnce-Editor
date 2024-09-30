using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using IpnceEditor.UnityIpnce;

namespace IpnceEditor.UnityIpnce
{
	public class Ipnce : I_Ipnce //Based on AAI2 version of Ipnce
	{
		public Texture2D[] SpriteAtlasAr;

		public Ipnce() { }

        public Ipnce(AAIIpnce ipnce)
        {
			ipnce.CopyTo(this);
            IsHD = ipnce.DataType == AAIIpnce.DataTypes.HD;
            SpriteAtlasAr = new Texture2D[] { ipnce.SpriteAtlas, ipnce.SpriteAtlasOverflow };
        }

        public Ipnce(AJIpnce ipnce)
        {
            ipnce.CopyTo(this);
            SpriteAtlasAr = new Texture2D[] { ipnce.SpriteAtlas, new Texture2D() };
        }

        public Ipnce(CollectionIpnce ipnce)
        {
            ipnce.CopyTo(this);
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
			SpriteAtlasAr = new Texture2D[] { SpriteAtlas, SpriteAtlasOverflow };
        }

        public Ipnce(CollectionAAI1Ipnce ipnce)
        {
            ipnce.CopyTo(this);
            IsHD = ipnce.DataType == AAIIpnce.DataTypes.HD;
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
            SpriteAtlasAr = new Texture2D[] { SpriteAtlas, SpriteAtlasOverflow };
        }

        public Ipnce(byte[] dat)
		{
			Load(dat);
		}

		public Ipnce(BinaryReader br)
		{
			Load(br);
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
			int len = br.ReadInt32();
			this.SpriteAtlasAr = new Texture2D[len];
			for (int i = 0; i < len; i++)
            {
				this.SpriteAtlasAr[i] = new Texture2D();
				this.SpriteAtlasAr[i].Load(br);
            }
			this.ColorPaletteNum = (byte)(br.ReadInt32());
			ColorPalette = new Texture2D();
			ColorPalette.Load(br);
			len = br.ReadInt32();
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
		}

		public override void Save(BinaryWriter bw)
        {
			int b = IsHD ? 1 : 0;
			bw.Write(b);
			b = IsUseColorPalette ? 1 : 0;
			bw.Write(b);
			b = IsOffScreenRendering ? 1 : 0;
			bw.Write(b);
			bw.Write(SpriteAtlasAr.Length);
			for (int i = 0; i < SpriteAtlasAr.Length; i++)
            {
				SpriteAtlasAr[i].Save(bw);
            }
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
	public class Sprite : I_Sprite
    {

		public Sprite() : base() { }

        public Sprite(CollectionSprite sprite) : base()
        {
            DestX = sprite.DestX;
            DestY = sprite.DestY;
            IsBlend = false;
            Parts = sprite.Parts;
        }

        public Sprite(AAISprite sprite) : base()
        {
			DestX = sprite.DestX;
			DestY = sprite.DestY;
			IsBlend = false;
			Parts = sprite.Parts;
		}

		public override void Load(BinaryReader br)
        {
			this.DestX = br.ReadSingle();
			this.DestY = br.ReadSingle();
			this.IsBlend = br.ReadInt32() == 1;
			int len = br.ReadInt32();
			this.Parts = new SpriteParts[len];
			for (int i = 0; i < len; i++) {
				this.Parts[i] = new SpriteParts();
				this.Parts[i].Load(br);
			}
		}

        public override void Save(BinaryWriter bw)
        {
			bw.Write(DestX);
			bw.Write(DestY);
			int b = IsBlend ? 1 : 0;
			bw.Write(b);
			bw.Write(Parts.Length);
			for (int i = 0; i < Parts.Length; i++)
            {
				Parts[i].Save(bw);
            }
        }
	}
	public class SpriteParts
	{
		public float DestX;
		public float DestY;
		public float SrcX;
		public float SrcY;
		public float Width;
		public float Height;
		public int Flag;
		public byte Priority;
		public byte ColorPlteNum;
		public const uint FLAG_FLIPH = 1;
		public const uint FLAG_FLIPV = 2;
		public const uint FLAG_DRAW_BLEND = 4;
		public const uint FLAG_DRAW_OVERWRITE = 8;
		public const uint FLAG_DRAW_MASK = 12;

		public override string ToString()
		{
			return "DestX: " + DestX + "\nDestY: " + DestY + "\nSrcX: " + SrcX + "\nSrcY: " + SrcY + "\nWidth: " + Width + "\nHeight: " + Height
				+ "\nFlag: " + Flag + "\nPriority: " + Priority + "\nColorPlteNum: " + ColorPlteNum;
		}

		public void Load(BinaryReader br)
        {
			this.DestX = br.ReadSingle();
			this.DestY = br.ReadSingle();
			this.SrcX = br.ReadSingle();
			this.SrcY = br.ReadSingle();
			this.Width = br.ReadSingle();
			this.Height = br.ReadSingle();
			this.Flag = br.ReadInt32();
			this.Priority = (byte)(br.ReadInt32());
			this.ColorPlteNum = (byte)(br.ReadInt32());
		}

		public void Save(BinaryWriter bw)
		{
			bw.Write(DestX);
			bw.Write(DestY);
			bw.Write(SrcX);
			bw.Write(SrcY);
			bw.Write(Width);
			bw.Write(Height);
			bw.Write(Flag);
			bw.Write((int)Priority);
			bw.Write((int)ColorPlteNum);
		}

		public static SpriteParts CreateEmpty()
        {
			SpriteParts sp = new SpriteParts();
			sp.DestX = 0;
			sp.DestY = 0;
			sp.SrcX = 0;
			sp.SrcY = 0;
			sp.Width = 8;
			sp.Height = 8;
			sp.Flag = 0;
			sp.Priority = 0;
			sp.ColorPlteNum = 0;
			return sp;
        }
	}
	public class Anim : I_Anim
    {
		public const uint FLAG_LOOP = 1;

		public Anim() : base() { }
		public Anim(AAIAnim an) : base()
        {
			KeySize = an.KeyFrames.Length;
			TotalFrameSize = an.TotalFrameSize;
			RestartFrame = an.RestartFrame;
			DestX = an.DestX;
			DestY = an.DestY;
			Flag = an.Flag;
			KeyFrames = an.KeyFrames;
		}

		public override void Load(BinaryReader br)
        {
			this.KeySize = br.ReadInt32();
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
			bw.Write(KeySize);
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
	public class AnimKeyframe
	{
		public int Frame;
		public int Index;
		public float Rotate;
		public float ScaleX;
		public float ScaleY;
		public float TranslateX;
		public float TranslateY;
		public uint Attribute;

		public override string ToString()
		{
			return "Frame: " + Frame + "\nIndex: " + Index + "\nRotate: " + Rotate + "\nScaleX: " + ScaleX + "\nScaleY: " + ScaleY + "\nTranslateX: " + TranslateX
				+ "\nTranslateY: " + TranslateY + "\nAttribute: " + Attribute;
		}

		public void Load(BinaryReader br)
        {
            this.Frame = br.ReadInt32();
			this.Index = br.ReadInt32();
			this.Rotate = br.ReadSingle();
			this.ScaleX = br.ReadSingle();
			this.ScaleY = br.ReadSingle();
			this.TranslateX = br.ReadSingle();
			this.TranslateY = br.ReadSingle();
			this.Attribute = br.ReadUInt32();
		}

		public void Save(BinaryWriter bw)
		{
			bw.Write(Frame);
			bw.Write(Index);
			bw.Write(Rotate);
			bw.Write(ScaleX);
			bw.Write(ScaleY);
			bw.Write(TranslateX);
			bw.Write(TranslateY);
			bw.Write(Attribute);
		}

		public static AnimKeyframe CreateEmpty()
		{
			AnimKeyframe an = new AnimKeyframe();
			an.Frame = 0;
			an.Index = 0;
			an.Rotate = 0;
			an.ScaleX = 1;
			an.ScaleY = 1;
			an.TranslateX = 0;
			an.TranslateY = 0;
			an.Attribute = 0;
			return an;
		}
	}
	public class Texture2D //I don't really know what these params mean, so I just saved them as 3 ints
	{
		public int in1 = 0;
		public int in2 = 0;
		public int in3 = 0;
		public override string ToString()
		{
			return "In1: " + in1 + " In2: " + in2 + " In3: " + in3;
		}

		public void Load(BinaryReader br)
        {
			this.in1 = br.ReadInt32();
			this.in2 = br.ReadInt32();
			this.in3 = br.ReadInt32();
		}

		public void Save(BinaryWriter bw)
		{
			bw.Write(in1);
			bw.Write(in2);
			bw.Write(in3);
		}
	}
}
