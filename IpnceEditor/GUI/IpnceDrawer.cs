using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using IpnceEditor.GUI.Export;
using IpnceEditor.Interfaces;

namespace IpnceEditor.GUI
{
    public class IpnceDrawer
    {
        Graphics graph;
        TrackBar tb;
        public Image BGImage = null;
        public static IpnceDrawer Instance;
        int ingameX = 512;
        int ingameY = 384;
        Point center;
        float maschtab = 0.5f;
        int[] lasts;
        Bitmap screenshottmp = new Bitmap(1024, 768);
        Graphics tmpg;
        int tmpk;
        float tmpm;
        List<int> lastcommand = new List<int>();
        NitroImageManager imageManager;

        public IpnceDrawer()
        {

        }

        public IpnceDrawer(Graphics g, NitroImageManager man, TrackBar t)
        {
            this.SetData(g, man, t);
            Instance = this;
        }

        private void SwapParams()
        {
            tmpg = graph;
            ingameX = imageManager.GetRealScreenWidth();
            ingameY = imageManager.GetRealScreenHeight();
            tmpm = imageManager.DrawingMasch;
            screenshottmp.Dispose();
            screenshottmp = new Bitmap(ingameX, ingameY);
            graph = Graphics.FromImage(screenshottmp);
            maschtab = 1;
            imageManager.DrawingMasch = 1;
        }

        private void SwapParamsBack()
        {
            graph = tmpg;
            maschtab = 0.5f;
            imageManager.DrawingMasch = tmpm;
            ingameX = 512;
            ingameY = 384;
        }

        public void SetData(Graphics g, NitroImageManager man, TrackBar t)
        {
            this.graph = g;
            imageManager = man;
            SetScreenCenter();
        }

        public bool GetLoop(int[] inds)
        {
            return imageManager.GetLoop(inds);
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

        public void GetScreenshot(string path)
        {
            SwapParams();
            SetScreenCenter();
            Image scr = Image.FromFile(path);
            if (lastcommand.Count != 0)
            {
                if (lastcommand[0] == 0)
                    imageManager.UpdateSpritePartWithScreenshot(scr, lastcommand[1], lastcommand[2], graph);
                else if (lastcommand[0] == 2)
                    imageManager.UpdateSpriteWithScreenshot(scr, lastcommand[1], graph);

            }
            //screenshottmp.Save(path);
            SwapParamsBack();
            SetScreenCenter();
        }

        public void AddSprite()
        {
            imageManager.AddSprite();
        }

        public void AddSpriteParts(int ind)
        {
            imageManager.AddSpritePart(ind);
        }

        public void UpdatePart(int ind1, int ind2) //used while editing sprite part data
        {
            imageManager.SetAllSpriteParts(ind1);
            DrawPart(ind1, ind2);
        }

        public void DrawPart(int ind1, int ind2, bool cleared = false)
        {
            lastcommand.Clear();
            lastcommand.AddRange(new int[]{ 0, ind1, ind2});
            Clear(cleared);
            imageManager.DrawSpritePart(ind1, ind2, graph);
            /*Clear(cleared);
            SpriteParts part = ipnce.SpriteList[ind1].Parts[ind2];
            DrawImage(spriteParts[ind1][ind2], part.DestX, part.DestY);*/
        }

        public void Clear(bool alphad = false)
        {
            if (!alphad)
                graph.Clear(Color.DarkGray);
            else
                graph.Clear(Color.FromArgb(0, 0, 0, 0));
            DrawScreenSquare();
            DrawBackgroudImage();
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
            int xpad = (int)((wid - ingameX) * maschtab);
            int ypad = (int)((hei - ingameY) * maschtab);
            graph.DrawRectangle(new Pen(new SolidBrush(Color.White)), new Rectangle(xpad - 1, ypad - 1, ingameX + 2, ingameY + 2));
        }

        public void DrawBackgroudImage()
        {
            if (this.BGImage == null)
                return;
            int wid = (int)(graph.VisibleClipBounds.Width);
            int hei = (int)(graph.VisibleClipBounds.Height);
            int xpad = (int)((wid - ingameX) * maschtab);
            int ypad = (int)((hei - ingameY) * maschtab);
            //MessageBox.Show("" + xpad + " " + ypad);
            graph.DrawImage(this.BGImage, new Rectangle(xpad, ypad, ingameX, ingameY));
        }

        private void SetScreenCenter()
        {
            int wid = (int)(graph.VisibleClipBounds.Width);
            int hei = (int)(graph.VisibleClipBounds.Height);
            int xpad = (int)((wid - ingameX) * maschtab);
            int ypad = (int)((hei - ingameY) * maschtab);
            center = new Point(xpad + ingameX / 2, ypad + ingameY / 2);
            imageManager.SetCenter(center);
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
            imageManager.DrawFrameOfOne(ind1, ind2, graph);
            /*for (int i = spriteParts[akf.Index].Length - 1; i >= 0; i--)
            {
                Image sprite = spriteParts[akf.Index][i];
                SpriteParts sp = ipnce.SpriteList[akf.Index].Parts[i];
                DrawImage(sprite, sp.DestX * (Manager.AAI1 ? 1 : akf.ScaleX) + ipnce.AnimList[ind1].DestX + akf.TranslateX, sp.DestY * (Manager.AAI1 ? 1 : akf.ScaleY) + ipnce.AnimList[ind1].DestY + akf.TranslateY, (Manager.AAI1 ? 1 : akf.ScaleX), (Manager.AAI1 ? 1 : akf.ScaleY));
            }*/
        }

        public void DrawSprite(int ind1) //drawing the full sprite
        {
            imageManager.DrawSprite(ind1, graph, 0, 0, 1, 1);
            /*for (int i = spriteParts[ind1].Length - 1; i >= 0; i--) //drawing all the sprite parts 
            {
                Image sprite = spriteParts[ind1][i];
                SpriteParts sp = ipnce.SpriteList[ind1].Parts[i];
                DrawImage(sprite, sp.DestX, sp.DestY);
            }*/
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
            ress = imageManager.GetDrawIt(inds, num);
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
