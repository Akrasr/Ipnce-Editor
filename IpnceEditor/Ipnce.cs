using System.IO;

namespace IpnceEditor
{
	public class Ipnce //Based on AAI2 version of Ipnce
	{
		public bool IsHD;
		public bool IsUseColorPalette;
		public bool IsOffScreenRendering;
		public Texture2D[] SpriteAtlas;
		public byte ColorPaletteNum;
		public Texture2D ColorPalette;
		public Sprite[] SpriteList;
		public Anim[] AnimList;

		public override string ToString()
		{
			string res = "IsHD: " + IsHD + "\nIsUseColorPalette: " + IsUseColorPalette + "\nIsOffScreenRendering: " + IsOffScreenRendering + "\nSpriteAtlas Length: " + SpriteAtlas.Length + "\n";
			for (int i = 0; i < SpriteAtlas.Length; i++)
			{
				res += "\nSpriteAtlas[" + i + "]:\n" + SpriteAtlas[i] + "\n";
			}
			res += "ColorPaletteNum: " + ColorPaletteNum + "ColorPalette:\n" + ColorPalette + "\nSpriteList Length: " + SpriteList.Length + "\n";
			for (int i = 0; i < SpriteList.Length; i++)
			{
				res += "\nSpriteList[" + i + "]:\n" + SpriteList[i] + "\n";
			}
			res += "AnimList Length: " + AnimList.Length + "\n";
			for (int i = 0; i < AnimList.Length; i++)
			{
				res += "AnimList[" + i + "]:\n" + AnimList[i] + "\n";
			}
			return res;
		}

		public void Load(byte[] data)
		{
			using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
			{
				this.Load(br);
			}

		}

		public void Load(BinaryReader br)
        {
			this.IsHD = br.ReadInt32() == 1;
			this.IsUseColorPalette = br.ReadInt32() == 1;
			this.IsOffScreenRendering = br.ReadInt32() == 1;
			int len = br.ReadInt32();
			this.SpriteAtlas = new Texture2D[len];
			for (int i = 0; i < len; i++)
            {
				this.SpriteAtlas[i] = new Texture2D();
				this.SpriteAtlas[i].Load(br);
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

		public void Save(BinaryWriter bw)
        {
			int b = IsHD ? 1 : 0;
			bw.Write(b);
			b = IsUseColorPalette ? 1 : 0;
			bw.Write(b);
			b = IsOffScreenRendering ? 1 : 0;
			bw.Write(b);
			bw.Write(SpriteAtlas.Length);
			for (int i = 0; i < SpriteAtlas.Length; i++)
            {
				SpriteAtlas[i].Save(bw);
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
	public class Sprite
	{
		public float DestX;
		public float DestY;
		public bool IsBlend;
		public SpriteParts[] Parts;

		public override string ToString()
		{
			string res = "DestX: " + DestX + "\nDestY: " + DestY + "\nIsBlend: " + IsBlend + "\nParts Length" + Parts.Length + "\n";
			for (int i = 0; i < Parts.Length; i++)
			{
				res += "\nParts[" + i + "]:\n" + Parts[i] + "\n";
			}
			return res;
		}

		public void Load(BinaryReader br)
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

		public void Save(BinaryWriter bw)
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
	}
	public class Anim
	{
		public int KeySize;
		public int TotalFrameSize;
		public int RestartFrame;
		public float DestX;
		public float DestY;
		public int Flag;
		public AnimKeyframe[] KeyFrames;
		public const uint FLAG_LOOP = 1;

		public override string ToString()
		{
			string res = "KeySize: " + KeySize + "\nTotalFrameSize: " + TotalFrameSize + "\nRestartFrame: " + RestartFrame + "\nDestX: " + DestX + "\nDestY: " + DestY + "\nFlag: " + Flag
				+ "\nKeyFrames Length: " + KeyFrames.Length + "\n";
			for (int i = 0; i < KeyFrames.Length; i++)
			{
				res += "\nKeyFrames[" + i + "]:\n" + KeyFrames[i] + "\n";
			}
			return res;
		}

		public void Load(BinaryReader br)
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

		public void Save(BinaryWriter bw)
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
