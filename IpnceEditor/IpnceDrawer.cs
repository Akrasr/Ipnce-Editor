using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace IpnceEditor
{
    class IpnceDrawer
    {
        Graphics graph;
        Ipnce ipnce;
        TrackBar tb;
        Image tex;
        Image colorplte;
        Image[][] spriteParts;
        Image BGImage = null;
        bool flipped;
        int ingameX = 512;
        int ingameY = 384;
        Point center;
        int k = 4;
        float maschtab = 2;
        int[] lasts;
        Bitmap screenshottmp = new Bitmap(1024, 768);
        Graphics tmpg;
        int tmpk;
        float tmpm;
        List<int> lastcommand = new List<int>();

        public IpnceDrawer()
        {

        }

        public IpnceDrawer(Graphics g, Ipnce ip, TrackBar t, Image te, Image cp, bool fl, bool hd)
        {
            this.SetData(g, ip, t, te, cp, fl, hd);
        }

        private void SwapParams()
        {
            tmpg = graph;
            ingameX = 1024;
            ingameY = 768;
            tmpk = k;
            tmpm = maschtab;
            screenshottmp.Dispose();
            screenshottmp = new Bitmap(ingameX, ingameY);
            graph = Graphics.FromImage(screenshottmp);
            maschtab = 1;
        }

        private void SwapParamsBack()
        {
            graph = tmpg;
            maschtab = tmpm;
            ingameX = 512;
            ingameY = 384;
        }

        public void SetData(Graphics g, Ipnce ip, TrackBar t, Image te, Image cp, bool fl, bool hd)
        {
            this.flipped = fl;
            this.graph = g;
            this.ipnce = ip;
            this.tb = t;
            this.tex = te;
            if (!this.flipped) { //rotating the image if it's flipped
                te.RotateFlip(RotateFlipType.Rotate180FlipX);
            }
            if (hd)
                k = 2;
            this.colorplte = cp;
            SetParts();
            SetScreenCenter();
        }

        public bool GetLoop(int[] inds)
        {
            foreach (int ind in inds)
            {
                Anim an = ipnce.AnimList[ind];
                if (an.Flag == 1)
                    return true;
            }
            return false;
        }

        public void ShowTest()
        {
        }

        public void SetBackgroundImage(Image bg)
        {
            this.BGImage = bg;
        }

        public void SetGreenScreenBackground()
        {
            Bitmap bmp = new Bitmap(ingameX, ingameY);
            for(int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    bmp.SetPixel(x, y, Color.Green);
                }
            }
            this.BGImage = bmp;
        }

        public void SaveGIF(int[] inds, int length, string filename, bool seq)
        {
            SwapParams();
            SetScreenCenter();
            List<Image> imglist = new List<Image>();
            List<int> delays = new List<int>();
            int last = 0;
            for (int i = 0; i < length; i++)
            {
                if (DrawFrame(i, inds, false, this.BGImage == null) == 1)
                {
                    Bitmap bmp = (Bitmap)screenshottmp.Clone();
                    imglist.Add(bmp);
                    delays.Add(i - last);
                    //MessageBox.Show("" + delays.Last<int>());
                    last = i;
                    screenshottmp.Dispose();
                    screenshottmp = new Bitmap(ingameX, ingameY);
                    graph = Graphics.FromImage(screenshottmp);
                }
                /*DrawFrame(i, inds);
                Bitmap bmp = (Bitmap)screenshottmp.Clone();
                imglist.Add(bmp);*/
            }
            delays.Add(length - last);
            if (seq)
                SeqHandler.CreateSeq(imglist, delays, filename);
            else
                GifHandler.CreateGif(imglist, delays, filename);
            SwapParamsBack();
            SetScreenCenter();
        }
        public void ResetBackgroundImage()
        {
            this.BGImage = null;
        }

        public void SetParts() //Loading spriteParts
        {
            int len = ipnce.SpriteList.Length;
            spriteParts = new Image[len][];
            for (int i = 0; i < len; i++)
            {
                Sprite sp = ipnce.SpriteList[i];
                spriteParts[i] = new Image[sp.Parts.Length];
                for (int j = 0; j < sp.Parts.Length; j++)
                {
                    SetSprite(i, j);
                }
            }
        }

        public void SaveScreenshot(Control ctr, string path)
        {
            SwapParams();
            SetScreenCenter();
            if (lastcommand.Count != 0)
            {
                if (lastcommand[0] == 0)
                    DrawPart(lastcommand[1], lastcommand[2], this.BGImage == null);
                else if (lastcommand[0] == 1)
                    DrawCertainFrame(lastcommand[1], lastcommand[2], this.BGImage == null);
                else if (lastcommand[0] == 2)
                    DrawSpriteCl(lastcommand[1], this.BGImage == null);
                else if (lastcommand[0] == 3)
                {
                    int[] inds = new int[lastcommand.Count - 2];
                    for (int i = 0; i < lastcommand.Count - 2; i++)
                        inds[i] = lastcommand[2 + i];
                    DrawFrame(lastcommand[1], inds, true, this.BGImage == null);
                }
            }
            screenshottmp.Save(path);
            SwapParamsBack();
            SetScreenCenter();
        }

        public void AddSprite()
        {
            List<Image[]> list = spriteParts.ToList<Image[]>();
            list.Add(new Image[] { });
            spriteParts = list.ToArray();
            AddSpriteParts(list.Count - 1);
        }

        public void AddSpriteParts(int ind)
        {
            Image[] arr = spriteParts[ind];
            List<Image> list = arr.ToList<Image>();
            list.Add(Crop(tex, new Rectangle(0, 0, 1, 1)));
            spriteParts[ind] = list.ToArray();
            int j = ipnce.SpriteList[ind].Parts.Length - 1;
            SetSprite(ind, j);
        }

        public void SetSprite(int ind1, int ind2)
        {
            SpriteParts part = ipnce.SpriteList[ind1].Parts[ind2]; //Loading a spritepart
            int x = (int)part.SrcX * k;
            int y = (int)part.SrcY * k;
            int w = (int)part.Width * k;
            if (w == 0)
                w = 1;
            int h = (int)part.Height * k;
            if (h == 0)
                h = 1;
            if (x + w >= tex.Width)
                w = tex.Width - x;
            if (y + h >= tex.Height)
                h = tex.Height - y;
            try
            {
                Image res = Crop(tex, new Rectangle(x, y, w, h));
                if (ipnce.IsUseColorPalette)
                    res = SetPalette(res, part.ColorPlteNum);
                if (part.Flag == 1)
                    res.RotateFlip(RotateFlipType.RotateNoneFlipX);
                spriteParts[ind1][ind2] = res;
            } catch
            {
                MessageBox.Show("" + x + " " + y + " " + w + " " + h);
            }
        }

        public void UpdatePart(int ind1, int ind2) //used while editing sprite part data
        {
            SetSprite(ind1, ind2); //ind1 is and index of sprite and ind2 is an index of spritepart
            DrawPart(ind1, ind2);
        }

        public void DrawPart(int ind1, int ind2, bool cleared = false)
        {
            lastcommand.Clear();
            lastcommand.AddRange(new int[]{ 0, ind1, ind2});
            Clear(cleared);
            SpriteParts part = ipnce.SpriteList[ind1].Parts[ind2];
            DrawImage(spriteParts[ind1][ind2], part.DestX, part.DestY);
        }

        public void Clear(bool alphad = false)
        {
            if (!alphad)
                graph.Clear(Color.DarkGray);
            else
            {
                graph.Clear(Color.FromArgb(0, 0, 0, 0));
            }
            DrawScreenSquare();
            DrawBackgroudImage();
        }

        public Image SetPalette(Image img, int num) //coloring a sprite part
        {
            if (num >= colorplte.Height)
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
                    Bitmap pal = colorplte as Bitmap;
                    Color n = pal.GetPixel(alp, num);
                    t.SetPixel(j, i, n);
                }
            }
            return t;
        } 

        public Image Crop(Image image, Rectangle rct) //cutting an Image
        {
            Bitmap bmp = image as Bitmap;
            if (bmp == null)
                throw new ArgumentException("No bitmap");
            Bitmap cropBmp = bmp.Clone(rct, bmp.PixelFormat);

            return cropBmp;
        }

        public void DrawScreenSquare()
        {
            int wid = (int)(graph.VisibleClipBounds.Width);
            int hei = (int)(graph.VisibleClipBounds.Height);
            int xpad = (int)((wid - ingameX) / maschtab);
            int ypad = (int)((hei - ingameY) / maschtab);
            graph.DrawRectangle(new Pen(new SolidBrush(Color.White)), new Rectangle(xpad - 1, ypad - 1, ingameX + 2, ingameY + 2));
        }

        public void DrawBackgroudImage()
        {
            if (this.BGImage == null)
                return;
            int wid = (int)(graph.VisibleClipBounds.Width);
            int hei = (int)(graph.VisibleClipBounds.Height);
            int xpad = (int)((wid - ingameX) / maschtab);
            int ypad = (int)((hei - ingameY) / maschtab);
            //MessageBox.Show("" + xpad + " " + ypad);
            graph.DrawImage(this.BGImage, new Rectangle(xpad, ypad, ingameX, ingameY));
        }

        private void SetScreenCenter()
        {
            int wid = (int)(graph.VisibleClipBounds.Width);
            int hei = (int)(graph.VisibleClipBounds.Height);
            int xpad = (int)((wid - ingameX) / maschtab);
            int ypad = (int)((hei - ingameY) / maschtab);
            center = new Point(xpad + ingameX / 2, ypad + ingameY / 2);
        }

        private async void DrawImage(Image img, float x, float y) //Drawing an image
        {
            int rwid = (int)(img.Width / maschtab);
            int rhei = (int)(img.Height / maschtab);
            int xcoord = (int)(center.X + (x * k / maschtab));
            int ycoord = (int)(center.Y + (y * k / maschtab));
            graph.DrawImage(img, xcoord, ycoord, rwid, rhei);
        }

        private async void DrawImage(Image img, float x, float y, float sizeX, float sizeY) //drawing an image with scale edited
        {
            int rwid = (int)(sizeX * (float)img.Width / (float)maschtab);
            int rhei = (int)(sizeY * (float)img.Height / (float)maschtab);
            int xcoord = (int)(center.X + (x * k / maschtab));// - rwid));
            int ycoord = (int)(center.Y + (y * k / maschtab));
            graph.DrawImage(img, xcoord, ycoord, rwid, rhei);
        }

        public void DrawCertainFrame(int ind1, int ind2, bool cleared = false)
        {
            lastcommand.Clear();
            lastcommand.AddRange(new int[] { 1, ind1, ind2 });
            Clear(cleared);
            DrawFrameOfOne(ind1, ind2);
        }

        public void DrawFrameOfOne(int ind1, int ind2) //drawing a certain frame of one animation
        {
            AnimKeyframe akf = ipnce.AnimList[ind1].KeyFrames[ind2];
            for (int i = spriteParts[akf.Index].Length - 1; i >= 0; i--)
            {
                Image sprite = spriteParts[akf.Index][i];
                SpriteParts sp = ipnce.SpriteList[akf.Index].Parts[i];
                DrawImage(sprite, sp.DestX * akf.ScaleX + ipnce.AnimList[ind1].DestX + akf.TranslateX, sp.DestY * akf.ScaleY + ipnce.AnimList[ind1].DestY + akf.TranslateY, akf.ScaleX, akf.ScaleY);
            }
        }

        public void DrawSprite(int ind1) //drawing the full sprite
        {
            for (int i = spriteParts[ind1].Length - 1; i >= 0; i--) //drawing all the sprite parts 
            {
                Image sprite = spriteParts[ind1][i];
                SpriteParts sp = ipnce.SpriteList[ind1].Parts[i];
                DrawImage(sprite, sp.DestX, sp.DestY);
            }
        }

        public void DrawSpriteCl(int ind1, bool cleared = false) //clearing and drawing sprite
        {
            lastcommand.Clear();
            lastcommand.AddRange(new int[] { 2, ind1 });
            Clear(cleared);
            DrawSprite(ind1);
        }

        public int DrawFrame(int num, int[] inds, bool ignore = false, bool cleared = false)
        {

            lastcommand.Clear();
            lastcommand.AddRange(new int[] { 3, num });
            lastcommand.AddRange(inds);
            int[] ress = new int[inds.Length];
            for (int i = 0; i < inds.Length; i++) //for each anim
            {
                Anim an = ipnce.AnimList[inds[i]];
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
            bool eqs = false;
            if (lasts != null && lasts.Length != 0) //if this frame looks exactly the same as the last one, don't draw it
            {
                eqs = true;
                for (int i = 0; i < ress.Length; i++)
                {
                    if (ress[i] != lasts[i] || lasts.Length != ress.Length)
                    {
                        eqs = false;
                        break;
                    }
                }
            }
            if (!eqs || ignore || num == 0)
            {
                Clear(cleared);
                for (int i = 0; i < inds.Length; i++)
                {
                    DrawFrameOfOne(inds[i], ress[i]);
                }
                lasts = ress;
                return 1;
            }
            return 0;
        }
    }
}
