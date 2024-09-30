using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpnceEditor.Interfaces
{

    public abstract class NitroImageManager
    {
        public float GetingMasch = 4;
        public float DrawingMasch = 0.5f;
        protected Point center;
        protected Image[][] spriteParts;
        protected Image[] Sprites;
        protected float[] centerx;
        protected float[] centery;

        public Image GetSpritePart(int sprind, int parind)
        {
            return spriteParts[sprind][parind];
        }
        public Image GetSprite(int sprind)
        {
            return Sprites[sprind];
        }
        public abstract void DrawSpritePart(int sprind, int partind, Graphics g);

        public void DrawSprite(int sprind, Graphics g, float x, float y, float scx, float scy)
        {
            Image sprite = Sprites[sprind];
            float nw = sprite.Width * scx * DrawingMasch;
            float nh = sprite.Height * scy * DrawingMasch;
            float nx = (x + (sprite.Width * DrawingMasch - nw) / 4 - centerx[sprind]) * DrawingMasch * GetingMasch + center.X;
            float ny = (y + (sprite.Height * DrawingMasch - nh) / 4 - centery[sprind]) * DrawingMasch * GetingMasch + center.Y;
            g.DrawImage(sprite, nx, ny, nw, nh);
        }
        public abstract void DrawFrameOfOne(int ind1, int ind2, Graphics g);
        public abstract int[] GetDrawIt(int[] inds, int num);
        public abstract void AddSpritePart(int ind);
        public void AddSprite()
        {
            List<float> cenlist = new List<float>();
            cenlist.AddRange(centerx);
            cenlist.Add(0);
            centerx = cenlist.ToArray();
            cenlist = new List<float>();
            cenlist.AddRange(centery);
            cenlist.Add(0);
            centery = cenlist.ToArray();
            List<Image[]> list = spriteParts.ToList<Image[]>();
            list.Add(new Image[] { });
            spriteParts = list.ToArray();
            List<Image> sprs = Sprites.ToList<Image>();
            sprs.Add(new Bitmap(10, 10));
            Sprites = sprs.ToArray();
            AddSpritePart(list.Count - 1);
        }
        public abstract void SetAllSpriteParts(int ind);
        public abstract int GetTotalFrameSize(int ind);
        public abstract int GetKeyFramesLength(int ind);
        public abstract int GetSpriteLength(int ind);
        public abstract int GetSpriteListLength();
        public abstract bool GetLoop(int[] inds);
        public abstract Image GetBlank();
        public abstract int GetRealScreenWidth();
        public abstract int GetRealScreenHeight();
        public abstract void UpdateSpritePartWithScreenshot(Image scr, int ind, int ind2, Graphics graph);
        public abstract void UpdateSpriteWithScreenshot(Image scr, int ind, Graphics graph);
        public Image Crop(Image image, Rectangle rct) //cutting an Image
        {
            Bitmap bmp = image as Bitmap;
            if (bmp == null)
                throw new ArgumentException("No bitmap");
            Bitmap cropBmp = bmp.Clone(rct, bmp.PixelFormat);

            return cropBmp;
        }
        public void SetCenter(Point c)
        {
            center = c;
        }

        public static Image ConcatRight(Image[] images)
        {
            int width = 0, height = 0;
            for (int i = 0; i < images.Length; i++)
            {
                width += images[i].Width;
                if (height < images[i].Height)
                    height = images[i].Height;
            }
            Bitmap res = new Bitmap(width, height);
            width = 0;
            for (int i = 0; i < images.Length; i++)
            {
                for (int x = 0; x < images[i].Width; x++)
                    for (int y = 0; y < images[i].Height; y++)
                        res.SetPixel(width + x, y, (images[i] as Bitmap).GetPixel(x, y));
                width += images[i].Width;
            }
            return res;
        }

        public static Image ConcatDown(Image[] images)
        {
            int width = 0, height = 0;
            for (int i = 0; i < images.Length; i++)
            {
                height += images[i].Height;
                if (width < images[i].Width)
                    width = images[i].Width;
            }
            Bitmap res = new Bitmap(width, height);
            height = 0;
            for (int i = 0; i < images.Length; i++)
            {
                for (int x = 0; x < images[i].Width; x++)
                    for (int y = 0; y < images[i].Height; y++)
                        res.SetPixel(x, height + y, (images[i] as Bitmap).GetPixel(x, y));
                height += images[i].Height;
            }
            return res;
        }
    }
}
