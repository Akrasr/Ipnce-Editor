using System.IO;

namespace IpnceEditor
{
    class AJAdapter : IpnceAdapter
    {
        public Ipnce Load(BinaryReader br)
        {
            AJIpnce ip = new AJIpnce();
            ip.Load(br);
            return this.ToIpnce(ip);
        }

        public void Save(Ipnce ip, BinaryWriter bw)
        {
            AJIpnce aip = (AJIpnce)FromIpnce(ip);
            aip.Save(bw);
        }

        public Ipnce ToIpnce(object ob)
        {
            AJIpnce ip = (AJIpnce)ob;
            Ipnce rip = new Ipnce();
            rip.IsHD = ip.IsHD;
            rip.IsUseColorPalette = ip.IsUseColorPalette;
            rip.IsOffScreenRendering = ip.IsOffScreenRendering;
            Texture2D[] tex = new Texture2D[2];
            tex[0] = ip.SpriteAtlas;
            tex[1] = new Texture2D();
            rip.SpriteAtlas = tex;
            rip.ColorPaletteNum = ip.ColorPaletteNum;
            rip.ColorPalette = ip.ColorPalette;
            Sprite[] splist = new Sprite[ip.SpriteList.Length];
            for (int i = 0; i < splist.Length; i++)
            {
                Sprite sp = new Sprite();
                AJIpnce.AJSprite asp = ip.SpriteList[i];
                sp.DestX = asp.DestX;
                sp.DestY = asp.DestY;
                sp.IsBlend = false;
                sp.Parts = asp.Parts;
                splist[i] = sp;
            }
            rip.SpriteList = splist;
            rip.AnimList = ip.AnimList;
            return rip;
        }

        public object FromIpnce(Ipnce ip)
        {
            AJIpnce rip = new AJIpnce();
            rip.IsHD = ip.IsHD;
            rip.IsUseColorPalette = ip.IsUseColorPalette;
            rip.IsOffScreenRendering = ip.IsOffScreenRendering;
            rip.SpriteAtlas = ip.SpriteAtlas[0];
            rip.ColorPaletteNum = ip.ColorPaletteNum;
            rip.ColorPalette = ip.ColorPalette;
            AJIpnce.AJSprite[] splist = new AJIpnce.AJSprite[ip.SpriteList.Length];
            for (int i = 0; i < splist.Length; i++)
            {
                AJIpnce.AJSprite sp = new AJIpnce.AJSprite();
                Sprite asp = ip.SpriteList[i];
                sp.DestX = asp.DestX;
                sp.DestY = asp.DestY;
                sp.Parts = asp.Parts;
                splist[i] = sp;
            }
            rip.SpriteList = splist;
            rip.AnimList = ip.AnimList;
            return rip;
        }
    }
}
