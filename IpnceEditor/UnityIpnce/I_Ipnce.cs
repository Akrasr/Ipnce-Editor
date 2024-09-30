using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IpnceEditor.UnityIpnce
{
    public abstract class I_Ipnce
    {
        public AAIIpnce.DataTypes DataType;
        public bool IsHD;
        public bool IsUseColorPalette;
        public bool IsOffScreenRendering;
        public Texture2D SpriteAtlas;
        public Texture2D SpriteAtlasOverflow;
        public byte ColorPaletteNum;
        public Texture2D ColorPalette;
        public I_Sprite[] SpriteList;
        public I_Anim[] AnimList;
        public AAIIpnce.DataTypes FHDDataType;
        public string m_SpriteAtlasName;
        public string m_SpriteAtlasOverflowName;
        public string m_ColorPaletteName;
        public string FHDm_SpriteAtlasName;
        public string FHDm_SpriteAtlasOverflowName;
        public bool PixelSnap;
        public bool HalfTexelOffset;
        public bool IsSplitLongTexture;

        protected I_Ipnce()
        {
            DataType = AAIIpnce.DataTypes.HD;
            FHDDataType = AAIIpnce.DataTypes.FHD;
            IsHD = true;
            IsUseColorPalette = false;
            IsOffScreenRendering = false;
            SpriteAtlas = new Texture2D();
            SpriteAtlasOverflow = new Texture2D();
            ColorPaletteNum = 1;
            ColorPalette = new Texture2D();
            SpriteList = new I_Sprite[0];
            AnimList = new I_Anim[0];
            m_SpriteAtlasName = "";
            m_SpriteAtlasOverflowName = "";
            m_ColorPaletteName = "";
            FHDm_SpriteAtlasName = "";
            FHDm_SpriteAtlasOverflowName = "";
            PixelSnap = false;
            HalfTexelOffset = false;
            IsSplitLongTexture = false;
        }


        public void Load(byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                this.Load(br);
            }
        }

        public abstract void Load(BinaryReader br);

        public abstract void Save(BinaryWriter br);

        public abstract I_Anim GetNewAnim();

        public abstract I_Sprite GetNewSprite();

        public void AddSprite()
        {
            List<I_Sprite> list = SpriteList.ToList<I_Sprite>();
            list.Add(GetNewSprite());
            SpriteList = list.ToArray();
        }

        public void AddAnim()
        {
            List<I_Anim> list = AnimList.ToList<I_Anim>();
            list.Add(GetNewAnim());
            AnimList = list.ToArray();
        }

        public void AddSpriteParts(int ind)
        {
            SpriteList[ind].AddSpriteParts();
        }

        public void AddAnimKeyframe(int ind)
        {
            AnimList[ind].AddAnimKeyframe();
        }

        public void CopyTo(I_Ipnce obj)
        {
            obj.IsHD = IsHD;
            obj.DataType = DataType;
            obj.FHDDataType = FHDDataType;
            obj.IsUseColorPalette = IsUseColorPalette;
            obj.IsOffScreenRendering = IsOffScreenRendering;
            obj.SpriteAtlas = SpriteAtlas;
            obj.ColorPalette = ColorPalette;
            obj.ColorPaletteNum = ColorPaletteNum;
            obj.SpriteAtlasOverflow = SpriteAtlasOverflow;
            obj.m_SpriteAtlasName = m_SpriteAtlasName;
            obj.m_SpriteAtlasOverflowName = m_SpriteAtlasOverflowName;
            obj.m_ColorPaletteName = m_ColorPaletteName;
            obj.PixelSnap = PixelSnap;
            obj.HalfTexelOffset = HalfTexelOffset;
            obj.FHDm_SpriteAtlasName = FHDm_SpriteAtlasName;
            obj.FHDm_SpriteAtlasOverflowName= FHDm_SpriteAtlasOverflowName;
            I_Sprite[] spres = new I_Sprite[SpriteList.Length];
            for (int i = 0; i < spres.Length; i++)
            {
                spres[i] = GetNewSprite();
                SpriteList[i].CopyTo(spres[i]);
            }
            obj.SpriteList = spres;
            I_Anim[] anres = new I_Anim[AnimList.Length];
            for (int i = 0; i < spres.Length; i++)
            {
                anres[i] = GetNewAnim();
                AnimList[i].CopyTo(anres[i]);
            }
            obj.AnimList = anres;
            obj.IsSplitLongTexture = !String.IsNullOrEmpty(FHDm_SpriteAtlasOverflowName) ||
                !String.IsNullOrEmpty(FHDm_SpriteAtlasOverflowName) || SpriteAtlasOverflow.in1 != 0
                || IsSplitLongTexture;
        }
    }

    public abstract class I_Sprite
    {
        public float DestX;
        public float DestY;
        public float ScaleX;
        public float ScaleY;
        public bool IsBlend;
        public SpriteParts[] Parts;

        protected I_Sprite()
        {
            DestX = 0;
            DestY = 0;
            ScaleX = 1;
            ScaleY = 1;
            IsBlend = false;
            Parts = new SpriteParts[1];
            Parts[0] = SpriteParts.CreateEmpty();
        }

        public void AddSpriteParts()
        {
            List<SpriteParts> list = Parts.ToList<SpriteParts>();
            list.Add(SpriteParts.CreateEmpty());
            Parts = list.ToArray();
        }

        public abstract void Load(BinaryReader br);
        public abstract void Save(BinaryWriter bw);

        public void CopyTo(I_Sprite sp)
        {
            sp.DestX = DestX;
            sp.DestY = DestY;
            sp.ScaleX = ScaleX;
            sp.ScaleY = ScaleY;
            sp.Parts = Parts;
            sp.IsBlend = IsBlend;
        }
    }

    public abstract class I_Anim
    {
        public int KeySize;
        public int TotalFrameSize;
        public int RestartFrame;
        public float DestX;
        public float DestY;
        public int Flag;
        public AnimKeyframe[] KeyFrames;

        protected I_Anim()
        {
            KeySize = 1;
            TotalFrameSize = 1;
            RestartFrame = 0;
            DestX = 0;
            DestY = 0;
            Flag = 0;
            KeyFrames = new AnimKeyframe[1];
            KeyFrames[0] = AnimKeyframe.CreateEmpty();
        }

        public void AddAnimKeyframe()
        {
            KeySize++;
            List<AnimKeyframe> list = KeyFrames.ToList<AnimKeyframe>();
            list.Add(AnimKeyframe.CreateEmpty());
            KeyFrames = list.ToArray();
        }

        public abstract void Load(BinaryReader br);
        public abstract void Save(BinaryWriter bw);

        public void CopyTo(I_Anim an)
        {
            an.KeySize = KeySize;
            an.TotalFrameSize = TotalFrameSize;
            an.RestartFrame = RestartFrame;
            an.DestX = DestX;
            an.DestY = DestY;
            an.Flag = Flag;
            an.KeyFrames = KeyFrames;
        }
    }
}
