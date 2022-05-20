using System;
using System.Drawing;
using System.Windows.Forms;

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
        bool flipped;
        const int ingameX = 520;
        const int ingameY = 390;
        Point center;
        int k = 4;
        float maschtab = 2;
        int[] lasts;

        public IpnceDrawer()
        {

        }

        public IpnceDrawer(Graphics g, Ipnce ip, TrackBar t, Image te, Image cp, bool fl)
        {
            this.SetData(g, ip, t, te, cp, fl);
        }

        public void SetData(Graphics g, Ipnce ip, TrackBar t, Image te, Image cp, bool fl)
        {
            this.flipped = fl;
            this.graph = g;
            this.ipnce = ip;
            this.tb = t;
            this.tex = te;
            if (!this.flipped) { //rotating the image if it's flipped
                te.RotateFlip(RotateFlipType.Rotate180FlipX);
            }
            this.colorplte = cp;
            SetParts();
            SetScreenCenter();
        }

        public void ShowTest()
        {
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

        public void SetSprite(int ind1, int ind2)
        {
            SpriteParts part = ipnce.SpriteList[ind1].Parts[ind2]; //Loading a spritepart
            int x = (int)part.SrcX * k;
            int y = (int)part.SrcY * k;
            int w = (int)part.Width * k;
            int h = (int)part.Height * k;
            if (x + w >= tex.Width)
                w = tex.Width - x -2;
            if (y + h >= tex.Height)
                h = tex.Height - y - 2;
            try
            {
                Image res = Crop(tex, new Rectangle(x, y, w, h));
                if (ipnce.IsUseColorPalette)
                    res = SetPalette(res, part.ColorPlteNum);
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

        public void DrawPart(int ind1, int ind2)
        {
            Clear();
            SpriteParts part = ipnce.SpriteList[ind1].Parts[ind2];
            DrawImage(spriteParts[ind1][ind2], part.DestX, part.DestY);
        }

        public void Clear()
        {
            graph.Clear(Color.LightGray);
            DrawScreenSquare();
        }

        public Image SetPalette(Image img, int num) //coloring a sprite part
        {
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
            int xpad = (wid - ingameX) / 2;
            int ypad = (hei - ingameY) / 2;
            graph.DrawRectangle(new Pen(new SolidBrush(Color.White)), new Rectangle(xpad, ypad, ingameX, ingameY));
        }

        private void SetScreenCenter()
        {
            int wid = (int)(graph.VisibleClipBounds.Width);
            int hei = (int)(graph.VisibleClipBounds.Height);
            int xpad = (wid - ingameX) / 2;
            int ypad = (hei - ingameY) / 2;
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

        public void DrawCertainFrame(int ind1, int ind2)
        {
            Clear();
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

        public void DrawSpriteCl(int ind1) //clearing and drawing sprite
        {
            Clear();
            DrawSprite(ind1);
        }

        public void DrawFrame(int num, int[] inds)
        {
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
            if (lasts != null) //if this frame looks exactly the same as the last one, don't draw it
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
            if (!eqs)
            {
                Clear();
                for (int i = 0; i < inds.Length; i++)
                {
                    DrawFrameOfOne(inds[i], ress[i]);
                }
                lasts = ress;
            }
        }
    }
}
