using System.IO;

namespace IpnceEditor
{
    class AAIIpnce
    {
		public AAIIpnce.DataTypes DataType;

		// Token: 0x040006F9 RID: 1785
		public bool IsUseColorPalette;

		// Token: 0x040006FA RID: 1786
		public bool IsOffScreenRendering;

		// Token: 0x040006FB RID: 1787
		public bool IsSplitLongTexture;

		// Token: 0x040006FC RID: 1788
		public Texture2D SpriteAtlas;

		// Token: 0x040006FD RID: 1789
		public Texture2D SpriteAtlasOverflow;

		// Token: 0x040006FE RID: 1790
		public byte ColorPaletteNum;

		// Token: 0x040006FF RID: 1791
		public Texture2D ColorPalette;

		// Token: 0x04000700 RID: 1792
		public AAISprite[] SpriteList;

		// Token: 0x04000701 RID: 1793
		public AAIAnim[] AnimList;

		// Token: 0x020000F3 RID: 243
		public enum DataTypes
		{
			// Token: 0x04000703 RID: 1795
			NDS,
			// Token: 0x04000704 RID: 1796
			HD,
			// Token: 0x04000705 RID: 1797
			HalfHD,
			// Token: 0x04000706 RID: 1798
			Direct
		}

		public void Load(BinaryReader br)
        {
			int dat = br.ReadInt32();
			AAIIpnce.DataTypes dt;
			if (dat == 0)
            {
				dt = DataTypes.NDS;
            } else if (dat == 1)
            {
				dt = DataTypes.HD;
            } else if (dat == 2)
            {
				dt = DataTypes.HalfHD;
            } else
            {
				dt = DataTypes.Direct;
            }
			this.DataType = dt;
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

		public void Save(BinaryWriter bw)
		{
			int res = 3;
			DataTypes dt = this.DataType;
			if (dt == DataTypes.NDS)
				res = 0;
			else if (dt == DataTypes.HD)
				res = 1;
			else if (dt == DataTypes.HalfHD)
				res = 2;
			bw.Write(res);
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

	public class AAISprite
    {
		public float DestX;

		// Token: 0x04000708 RID: 1800
		public float DestY;

		// Token: 0x04000709 RID: 1801
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

	public class AAIAnim
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0041293D File Offset: 0x00410B3D

		// Token: 0x04000715 RID: 1813
		public int TotalFrameSize;

		// Token: 0x04000716 RID: 1814
		public int RestartFrame;

		// Token: 0x04000717 RID: 1815
		public float DestX;

		// Token: 0x04000718 RID: 1816
		public float DestY;

		// Token: 0x04000719 RID: 1817
		public int Flag;

		// Token: 0x0400071A RID: 1818
		public AnimKeyframe[] KeyFrames;

		// Token: 0x0400071B RID: 1819
		public const uint FLAG_LOOP = 1u;

		public void Load(BinaryReader br)
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

		public void Save(BinaryWriter bw)
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
