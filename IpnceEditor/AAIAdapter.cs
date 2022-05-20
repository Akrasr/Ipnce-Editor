using System.IO;

namespace IpnceEditor
{
    class AAIAdapter : IpnceAdapter
    {
        public Ipnce Load(BinaryReader br)
        {
            AAIIpnce ip = new AAIIpnce();
            ip.Load(br);
            return this.ToIpnce(ip);
        }
        public void Save(Ipnce ip, BinaryWriter bw)
        {
            AAIIpnce aip = (AAIIpnce)FromIpnce(ip);
            aip.Save(bw);
        }

        public Ipnce ToIpnce(object ob)
        {
            AAIIpnce ip = (AAIIpnce) ob;
            Ipnce rip = new Ipnce();
            rip.IsHD = ip.DataType == AAIIpnce.DataTypes.HD;
            rip.IsUseColorPalette = ip.IsUseColorPalette;
            rip.IsOffScreenRendering = ip.IsOffScreenRendering;
            Texture2D[] tex = new Texture2D[2];
            tex[0] = ip.SpriteAtlas;
            tex[1] = ip.SpriteAtlasOverflow;
            rip.SpriteAtlas = tex;
            rip.ColorPaletteNum = ip.ColorPaletteNum;
            rip.ColorPalette = ip.ColorPalette;
            Sprite[] splist = new Sprite[ip.SpriteList.Length];
            for (int i = 0; i < splist.Length; i++)
            {
                Sprite sp = new Sprite();
                AAISprite asp = ip.SpriteList[i];
                sp.DestX = asp.DestX;
                sp.DestY = asp.DestY;
                sp.IsBlend = false;
                sp.Parts = asp.Parts;
                splist[i] = sp;
            }
            rip.SpriteList = splist;
            Anim[] anims = new Anim[ip.AnimList.Length];
            for (int i = 0; i < anims.Length; i++)
            {
                Anim an = new Anim();
                AAIAnim aan = ip.AnimList[i];
                an.KeySize = aan.KeyFrames.Length;
                an.TotalFrameSize = aan.TotalFrameSize;
                an.RestartFrame = aan.RestartFrame;
                an.DestX = aan.DestX;
                an.DestY = aan.DestY;
                an.Flag = aan.Flag;
                an.KeyFrames = aan.KeyFrames;
                anims[i] = an;
            }
            rip.AnimList = anims;
            return rip;
        }

        public object FromIpnce(Ipnce ip)
        {
            AAIIpnce rip = new AAIIpnce();
            rip.DataType = ip.IsHD ? AAIIpnce.DataTypes.HD : AAIIpnce.DataTypes.NDS;
            rip.IsUseColorPalette = ip.IsUseColorPalette;
            rip.IsOffScreenRendering = ip.IsOffScreenRendering;
            rip.IsSplitLongTexture = false;
            rip.SpriteAtlas = ip.SpriteAtlas[0];
            rip.SpriteAtlasOverflow = ip.SpriteAtlas[1];
            rip.ColorPaletteNum = ip.ColorPaletteNum;
            rip.ColorPalette = ip.ColorPalette;
            AAISprite[] splist = new AAISprite[ip.SpriteList.Length];
            for (int i = 0; i < splist.Length; i++)
            {
                AAISprite sp = new AAISprite();
                Sprite asp = ip.SpriteList[i];
                sp.DestX = asp.DestX;
                sp.DestY = asp.DestY;
                sp.Parts = asp.Parts;
                splist[i] = sp;
            }
            rip.SpriteList = splist;
            AAIAnim[] anims = new AAIAnim[ip.AnimList.Length];
            for (int i = 0; i < anims.Length; i++)
            {
                AAIAnim an = new AAIAnim();
                Anim aan = ip.AnimList[i];
                an.TotalFrameSize = aan.TotalFrameSize;
                an.RestartFrame = aan.RestartFrame;
                an.DestX = aan.DestX;
                an.DestY = aan.DestY;
                an.Flag = aan.Flag;
                an.KeyFrames = aan.KeyFrames;
                anims[i] = an;
            }
            rip.AnimList = anims;
            return rip;
        }
    }
}
