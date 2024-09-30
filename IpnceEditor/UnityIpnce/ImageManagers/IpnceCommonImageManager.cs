using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.Interfaces;

namespace IpnceEditor.UnityIpnce.ImageManagers
{
    public class IpnceCommonImageManager : NitroImageManager
    {
        protected I_Ipnce ipnce;
        public Image atlas;
        protected Image palette;

        public IpnceCommonImageManager(I_Ipnce _ipnce, Image _atlas, Image _palette, bool hd)
        {
            this.ipnce = _ipnce;
            this.atlas = _atlas;
            this.palette = _palette;
            GetingMasch = hd ? 4 : 2;
            DrawingMasch = hd ? 0.5f : 1f;
            if (!ipnce.IsHD)
            {
                GetingMasch = 1;
                DrawingMasch = 2;
            }
            Sprites = new Image[ipnce.SpriteList.Length];
            spriteParts = new Image[ipnce.SpriteList.Length][];
            centerx = new float[ipnce.SpriteList.Length];
            centery = new float[ipnce.SpriteList.Length];
            for (int i = 0; i < ipnce.SpriteList.Length; i++)
            {
                SetAllSpriteParts(i);
            }
        }

        public IpnceCommonImageManager(I_Ipnce _ipnce, Image _atlas, Image _palette, bool hd, float get, float draw)
        {
            this.ipnce = _ipnce;
            this.atlas = _atlas;
            this.palette = _palette;
            GetingMasch = get;
            DrawingMasch = draw;
            if (!ipnce.IsHD)
            {
                GetingMasch = 1;
                DrawingMasch = 2;
            }
            Sprites = new Image[ipnce.SpriteList.Length];
            spriteParts = new Image[ipnce.SpriteList.Length][];
            centerx = new float[ipnce.SpriteList.Length];
            centery = new float[ipnce.SpriteList.Length];
            for (int i = 0; i < ipnce.SpriteList.Length; i++)
            {
                SetAllSpriteParts(i);
            }
        }

        #region Getting Images

        public override Image GetBlank()
        {
            return Crop(atlas, new Rectangle(0, 0, 1, 1));
        }
        #endregion
        #region Drawing sprites
        public override void DrawSpritePart(int sprind, int partind, Graphics g)
        {
            Image spritePart = spriteParts[sprind][partind];
            SpriteParts p = ipnce.SpriteList[sprind].Parts[partind];
            float nx = (p.DestX) * DrawingMasch * GetingMasch + center.X;
            float ny = (p.DestY) * DrawingMasch * GetingMasch + center.Y;
            g.DrawImage(spritePart, nx, ny, spritePart.Width * DrawingMasch, spritePart.Height * DrawingMasch);
        }

        public override void DrawFrameOfOne(int ind1, int ind2, Graphics g)
        {
            AnimKeyframe akf = ipnce.AnimList[ind1].KeyFrames[ind2];
            DrawSprite(akf.Index, g, (ipnce.AnimList[ind1].DestX + akf.TranslateX), ipnce.AnimList[ind1].DestY + akf.TranslateY, (Manager.AAI1 ? 1 : akf.ScaleX), (Manager.AAI1 ? 1 : akf.ScaleY));
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

        public virtual void UpdateSpriteWithScreenshot(Image scr, int sprind, int dx, int dy, Graphics graph)
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

        public override void UpdateSpriteWithScreenshot(Image scr, int sprind, Graphics graph)
        {
            UpdateSpriteWithScreenshot(scr, sprind, 0, 0, graph);
        }

        public override int[] GetDrawIt(int[] inds, int num)
        {
            int[] ress = new int[inds.Length];
            for (int i = 0; i < inds.Length; i++) //for each anim
            {
                I_Anim an = ipnce.AnimList[inds[i]];
                if (an.TotalFrameSize < num) //if animation is over, don't draw anything
                    continue;
                for (int j = 0; j < an.KeyFrames.Length; j++)
                {
                    if (j == an.KeyFrames.Length - 1 && an.KeyFrames[j].Frame <= num) // if this is the last key frame and animation is still going
                    {
                        ress[i] = j; //draw it
                        break;
                    }
                    if (an.KeyFrames[j + 1].Frame > num && an.KeyFrames[j].Frame <= num) //else if the next key frame will be too late and last was to early
                    {
                        ress[i] = j; //draw it
                        break;
                    }
                }
            }
            return ress;
        }
        #endregion

        public override void AddSpritePart(int ind)
        {
            Image[] arr = spriteParts[ind];
            List<Image> list = arr.ToList<Image>();
            I_Sprite sp = ipnce.SpriteList[ind];
            list.Add(GetSpritePartImage(sp.Parts[sp.Parts.Length - 1]));
            spriteParts[ind] = list.ToArray();
            SetAllSpriteParts(ind);
        }

        public override void SetAllSpriteParts(int ind)
        {
            float left = 0, right = 0, top = 0, bottom = 0; // loading spriteparts and calculating sprite size
            spriteParts[ind] = new Image[ipnce.SpriteList[ind].Parts.Length];
            for (int i = 0; i < spriteParts[ind].Length; i++)
            {
                SpriteParts cur = ipnce.SpriteList[ind].Parts[i];
                float tmp = cur.DestX;
                if (tmp < left)
                    left = tmp;
                tmp += cur.Width;
                if (tmp > right)
                    right = tmp;
                tmp = cur.DestY;
                if (tmp < top)
                    top = tmp;
                tmp += cur.Height;
                if (tmp > bottom)
                    bottom = tmp;
            }
            float width = right - left;
            float height = bottom - top;
            //loading sprite
            if (width == 0 || height == 0)
            {
                Sprites[ind] = new Bitmap(8, 8);
                return;
            }
            Bitmap res = new Bitmap((int)(width * GetingMasch), (int)(height * GetingMasch));
            Graphics draw = Graphics.FromImage(res);
            centerx[ind] = left * (-1);
            centery[ind] = top * (-1);
            int c = 0;
            for (int i = ipnce.SpriteList[ind].Parts.Length - 1; i >= 0; i--)
            {
                SpriteParts part = ipnce.SpriteList[ind].Parts[i];
                float x = (centerx[ind] + part.DestX) * GetingMasch;
                float y = (centery[ind] + part.DestY) * GetingMasch;
                Image impart = GetSpritePartImage(part);
                draw.DrawImage(impart, x, y, impart.Width, impart.Height);
                spriteParts[ind][i] = impart;
            }
            Sprites[ind] = res;
        }

        public virtual Image GetSpritePartImage(SpriteParts part)
        {
            int x = (int)(part.SrcX * GetingMasch);
            int y = (int)(part.SrcY * GetingMasch);
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
                if (ipnce.IsUseColorPalette && palette != null)
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

        public Image SetPalette(Image img, int num) //coloring a sprite part
        {
            if (num >= palette.Height)
            {
                num = 0;
            }
            Bitmap t = img as Bitmap;
            for (int i = 0; i < t.Height; i++)
            {
                for (int j = 0; j < t.Width; j++)
                {
                    Color c = t.GetPixel(j, i);
                    byte alp = c.A;
                    Bitmap pal = palette as Bitmap;
                    Color n = pal.GetPixel(alp, num);
                    t.SetPixel(j, i, n);
                }
            }
            return t;
        }

        public Image CalculateColorless(Image orig, int num)
        {
            Bitmap or = orig as Bitmap;
            Bitmap colres = new Bitmap(orig.Width, orig.Height);
            for (int x = 0; x < orig.Width; x++)
            {
                for (int y = 0; y < orig.Height; y++)
                {
                    colres.SetPixel(x, y, Color.FromArgb(GetClosestColor(or.GetPixel(x, y), num), 255, 255, 255));
                }
            }
            return colres;
        }

        public int GetClosestColor(Color c, int num)
        {
            int rass = int.MaxValue;
            int closind = -1;
            Bitmap pal = palette as Bitmap;
            for (int i = 0; i < 256; i++)
            {
                Color cur = pal.GetPixel(i, num);
                int r = (int)(Math.Pow(cur.R - c.R, 2) + Math.Pow(cur.G - c.G, 2) + Math.Pow(cur.B - c.B, 2) + Math.Pow(cur.A - c.A, 2));
                if (r < rass)
                {
                    rass = r;
                    closind = i;
                }
                if (r == 0)
                    return i;
            }
            return closind;
        }

        #region getting sprites and animations data

        public override int GetRealScreenWidth()
        {
            return (int)(GetingMasch * 256);
        }

        public override int GetRealScreenHeight()
        {
            return (int)(GetingMasch * 192);
        }
        public override int GetTotalFrameSize(int ind)
        {
            return ipnce.AnimList[ind].TotalFrameSize;
        }

        public override int GetKeyFramesLength(int ind)
        {
            return ipnce.AnimList[ind].KeyFrames.Length;
        }

        public override int GetSpriteLength(int ind)
        {
            return ipnce.SpriteList[ind].Parts.Length;
        }

        public override int GetSpriteListLength()
        {
            return ipnce.SpriteList.Length;
        }

        public override bool GetLoop(int[] inds)
        {
            foreach (int ind in inds)
            {
                I_Anim an = ipnce.AnimList[ind];
                if (an.Flag == 1)
                    return true;
            }
            return false;
        }
        #endregion
    }
}
