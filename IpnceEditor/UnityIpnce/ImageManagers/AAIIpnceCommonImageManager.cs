using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace IpnceEditor.UnityIpnce.ImageManagers
{
    public class AAIIpnceCommonImageManager : IpnceCommonImageManager
    {
        public AAIIpnceCommonImageManager(I_Ipnce _ipnce, Image _atlas, Image _palette, bool hd) : base(_ipnce, _atlas, _palette, hd) { }
        public AAIIpnceCommonImageManager(I_Ipnce _ipnce, Image _atlas, Image _palette, bool hd, float get, float draw) : base(_ipnce, _atlas, _palette, hd, get, draw) { }

        public override Image GetSpritePartImage(SpriteParts part)
        {
            int x = (int)(part.SrcX * GetingMasch);
            int y = (int)(part.SrcY * GetingMasch);
            if ((int)part.SrcY / 512 > 0)
            {
                x += (int)(256 * GetingMasch * ((int)part.SrcY / 512));
                y -= (int)(512 * GetingMasch * ((int)part.SrcY / 512));
            }
            int w = (int)(part.Width * GetingMasch);
            if (w == 0)
                w = 1;
            int h = (int)(part.Height * GetingMasch);
            if (h == 0)
                h = 1;
            if (x + w > atlas.Width || y + h > atlas.Height)
            {
                x /= 2;
                w /= 2;
                h /= 2;
                y /= 2;
            }
            if (y + h > atlas.Height)
                y /= 2;
            try
            {
                Image res = Crop(atlas, new Rectangle(x, y, w, h));
                if (ipnce.IsUseColorPalette)
                    res = SetPalette(res, part.ColorPlteNum);
                if (part.Flag == 1)
                    res.RotateFlip(RotateFlipType.RotateNoneFlipX);
                return res;
            }
            catch
            {
                MessageBox.Show("" + x + " " + y + " " + w + " " + h + " " + part.Width);
            }
            return null;
        }

        public override void UpdateSpritePartWithScreenshot(Image scr, int sprind, int partind, Graphics graph)
        {
            SpriteParts p = ipnce.SpriteList[sprind].Parts[partind];
            float x = (p.DestX) * DrawingMasch * GetingMasch + center.X;
            float y = (p.DestY) * DrawingMasch * GetingMasch + center.Y;
            Image spritePart = spriteParts[sprind][partind];
            float w = spritePart.Width * DrawingMasch;
            float h = spritePart.Height * DrawingMasch;
            if (y + h > scr.Height)
                h = scr.Height - y;
            if (x + w > scr.Width)
                w = scr.Width - x;
            Image tmp = Crop(scr, new Rectangle((int)x, (int)y, (int)w, (int)h));
            if (p.Flag == 1)
                tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Image clr = CalculateColorless(tmp, p.ColorPlteNum);
            Image tmpAtlas = (Image)atlas.Clone();
            Graphics g = Graphics.FromImage(tmpAtlas);
            x = p.SrcX * GetingMasch;
            y = p.SrcY * GetingMasch;
            if ((int)p.SrcY / 512 > 0)
            {
                x += (int)(256 * GetingMasch * ((int)p.SrcY / 512));
                y -= (int)(512 * GetingMasch * ((int)p.SrcY / 512));
            }
            if (x + w > atlas.Width || y + h > atlas.Height)
            {
                x /= 2;
                w /= 2;
                h /= 2;
                y /= 2;
            }
            RectangleF f = new RectangleF(x, y, w, h);
            g.Clip = new Region(f);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g = Graphics.FromImage(tmpAtlas);
            g.DrawImage(clr, p.SrcX * GetingMasch, p.SrcY * GetingMasch);
            atlas = tmpAtlas;
            for (int i = 0; i < ipnce.SpriteList.Length; i++)
            {
                SetAllSpriteParts(i);
            }
        }

        public override void UpdateSpriteWithScreenshot(Image scr, int sprind, int dx, int dy, Graphics graph)
        {
            Image tmpAtlas = (Image)atlas.Clone();
            for (int partind = 0; partind < spriteParts[sprind].Length; partind++)
            {
                SpriteParts p = ipnce.SpriteList[sprind].Parts[partind];
                float x = (dx + p.DestX) * DrawingMasch * GetingMasch + center.X;
                float y = (dy + p.DestY) * DrawingMasch * GetingMasch + center.Y;
                Image spritePart = spriteParts[sprind][partind];
                float w = spritePart.Width * DrawingMasch;
                float h = spritePart.Height * DrawingMasch;
                if (y + h > scr.Height)
                    h = scr.Height - y;
                if (x + w > scr.Width)
                    w = scr.Width - x;
                Image tmp = Crop(scr, new Rectangle((int)x, (int)y, (int)w, (int)h));
                if (p.Flag == 1)
                    tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                Image clr = CalculateColorless(tmp, p.ColorPlteNum);
                Graphics g = Graphics.FromImage(tmpAtlas);
                x = p.SrcX * GetingMasch;
                y = p.SrcY * GetingMasch;
                if ((int)p.SrcY / 512 > 0)
                {
                    x += (int)(256 * GetingMasch * ((int)p.SrcY / 512));
                    y -= (int)(512 * GetingMasch * ((int)p.SrcY / 512));
                }
                if (x + w > atlas.Width || y + h > atlas.Height)
                {
                    x /= 2;
                    w /= 2;
                    h /= 2;
                    y /= 2;
                }
                RectangleF f = new RectangleF(x, y, w, h);
                g.Clip = new Region(f);
                g.Clear(Color.FromArgb(0, 0, 0, 0));
                g = Graphics.FromImage(tmpAtlas);
                g.DrawImage(clr, p.SrcX * GetingMasch, p.SrcY * GetingMasch);
            }
            atlas = tmpAtlas;
            for (int i = 0; i < ipnce.SpriteList.Length; i++)
            {
                SetAllSpriteParts(i);
            }
        }
    }
}
