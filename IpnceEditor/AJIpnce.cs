using System.IO;

namespace IpnceEditor
{
	class AJIpnce
	{
		public bool IsHD;
		public bool IsUseColorPalette;
		public bool IsOffScreenRendering;
		public Texture2D SpriteAtlas;
		public byte ColorPaletteNum;
		public Texture2D ColorPalette;
		public AJSprite[] SpriteList;
		public Anim[] AnimList;

		public void Load(BinaryReader br)
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
			this.SpriteList = new AJSprite[len];
			for (int i = 0; i < len; i++)
			{
				this.SpriteList[i] = new AJSprite();
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

		public class AJSprite
		{
			public float DestX;
			public float DestY;
			public SpriteParts[] Parts;

			public void Load(BinaryReader br)
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

			public void Save(BinaryWriter bw)
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
	}
}
