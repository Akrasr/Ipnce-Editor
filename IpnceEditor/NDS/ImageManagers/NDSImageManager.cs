using IpnceEditor.NDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using IpnceEditor.Interfaces;

namespace IpnceEditor.NDS.ImageManagers
{
    public class NDSImageManager : NitroImageManager
    {
        private static int[][] rectSizes = new int[][] { new int[]{2, 1}, new int[]{ 4, 1}, new int[] { 4, 2 }, new int[] { 8, 4 } };
        NCER ncer;
        NANR nanr;
        NCGR ncgr;
        NCLR nclr;
        private static NDSImageManager instance;
        public static bool transparency = false;
        public static void UpdateTransparency(bool tr)
        {
            transparency = tr;
            if (instance == null)
                return;
            for(int i = 0; i < instance.Sprites.Length; i++)
            {
                instance.SetAllSpriteParts(i);
            }
        }

        public NDSImageManager(NCER cer, NANR anr, NCGR cgr, NCLR clr)
        {
            ncer = cer;
            nanr = anr;
            ncgr = cgr;
            nclr = clr;
            GetingMasch = 1;
            DrawingMasch = 2;
            Sprites = new Image[ncer.cebk.cellCount];
            spriteParts = new Image[ncer.cebk.cellCount][];
            centerx = new float[ncer.cebk.cellCount];
            centery = new float[ncer.cebk.cellCount];
            for (int i = 0; i < ncer.cebk.cellCount; i++)
            {
                SetAllSpriteParts(i);
            }
            instance = this;
        }

        public override Image GetBlank()
        {
            return null;
        }

        public override void AddSpritePart(int ind)
        {
            Image[] arr = spriteParts[ind];
            List<Image> list = arr.ToList<Image>();
            Nitro_Cell cel = ncer.cebk.cells[ind];
            list.Add(MakeSpritePart(ind, cel.oam_count - 1));
            spriteParts[ind] = list.ToArray();
            SetAllSpriteParts(ind);
        }
        public override void DrawSpritePart(int sprind, int partind, Graphics g)
        {
            Image spritePart = spriteParts[sprind][partind];
            Nitro_OAM oam = ncer.cebk.cells[sprind].CellParts[partind];
            float nx = (oam.x) * DrawingMasch * GetingMasch + center.X;
            float ny = (oam.y) * DrawingMasch * GetingMasch + center.Y;
            g.DrawImage(spritePart, nx, ny, spritePart.Width * DrawingMasch, spritePart.Height * DrawingMasch);
        }

        public override void DrawFrameOfOne(int ind1, int ind2, Graphics g)
        {
            DSKeyFrame fr = nanr.bnk.dSSeqs[ind1].keyframes[ind2];
            //ignorecenter = fr.type == 1;
            DrawSprite(fr.GetIndex(), g, fr.GetX(), fr.GetY(), fr.GetSX(), fr.GetSY());
            //ignorecenter = false;
        }

        public override void UpdateSpritePartWithScreenshot(Image scr, int sprind, int partind, Graphics graph)
        {
            Nitro_OAM p = ncer.cebk.cells[sprind].CellParts[partind];
            float x = (p.x) * DrawingMasch * GetingMasch + center.X;
            float y = (p.y) * DrawingMasch * GetingMasch + center.Y;
            Image spritePart = spriteParts[sprind][partind];
            float w = spritePart.Width * DrawingMasch;
            float h = spritePart.Height * DrawingMasch;
            if (y + h > scr.Height)
                h = scr.Height - y;
            if (x + w > scr.Width)
                w = scr.Width - x;
            Image tmp = Crop(scr, new Rectangle((int)x, (int)y, (int)w, (int)h));
            switch (p.rotsca)
            {
                case 16:
                case 17:
                    tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case 32:
                case 33:
                    tmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                case 48:
                case 49:
                    tmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                    break;
            }
            byte[][] dat = CalculateByteData(tmp, nclr.palettes[p.pal]);
            ncgr.maindata.SetImageData(dat, p.cch * (p.col8bit ? 2 : 4) * 64);
            for (int i = 0; i < ncer.cebk.cellCount; i++)
            {
                SetAllSpriteParts(i);
            }
        }
        public override void UpdateSpriteWithScreenshot(Image scr, int sprind, Graphics graph) {
            Nitro_Cell c = ncer.cebk.cells[sprind];
            for (int i = 0; i < c.CellParts.Length; i++)
            {
                Nitro_OAM p = ncer.cebk.cells[sprind].CellParts[i];
                float x = (p.x) * DrawingMasch * GetingMasch + center.X;
                float y = (p.y) * DrawingMasch * GetingMasch + center.Y;
                Image spritePart = spriteParts[sprind][i];
                float w = spritePart.Width * DrawingMasch;
                float h = spritePart.Height * DrawingMasch;
                if (y + h > scr.Height)
                    h = scr.Height - y;
                if (x + w > scr.Width)
                    w = scr.Width - x;
                Image tmp = Crop(scr, new Rectangle((int)x, (int)y, (int)w, (int)h));
                switch (p.rotsca)
                {
                    case 16:
                    case 17:
                        tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 32:
                    case 33:
                        tmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        break;
                    case 48:
                    case 49:
                        tmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        break;
                }
                byte[][] dat = CalculateByteData(tmp, nclr.palettes[p.pal]);
                ncgr.maindata.SetImageData(dat, p.cch * (p.col8bit ? 2 : 4) * 64);
            }
        }

        public static byte[][] CalculateByteData(Image orig, Color[] palette)
        {
            Bitmap bitmap = orig as Bitmap;
            byte[][] res = new byte[orig.Height][];
            for (int i = 0; i < orig.Height; i++)
            {
                res[i] = new byte[orig.Width];
                for (int j = 0; j < orig.Width; j++)
                {
                    res[i][j] = (byte)GetClosestColor(bitmap.GetPixel(j, i), palette);
                }
            }
            return res;
        }

        public static int GetClosestColor(Color c, Color[] palette)
        {
            if (c.A <= 10)
                return 0;
            int rass = int.MaxValue;
            int closind = -1;
            for (int i = 0; i < palette.Length; i++)
            {
                Color cur = palette[i];
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

        public override int[] GetDrawIt(int[] inds, int num)
        {
            int[] ress = new int[inds.Length];
            for (int i = 0; i < inds.Length; i++) //for each anim
            {
                DSSeq seq = nanr.bnk.dSSeqs[inds[i]];
                if (seq.GetDuration() < num) //if animation is over, don't draw anything
                    continue;
                for (int j = 0; j < seq.framecount; j++)
                {
                    if (j == seq.framecount - 1 && seq.keyframes[j].start <= num) // if this is the last key frame and animation is still going
                    {
                        ress[i] = j; //draw it
                        break;
                    }
                    if (seq.keyframes[j + 1].start > num && seq.keyframes[j].start <= num) //else if the next key frame will be too late and last was to early
                    {
                        ress[i] = j; //draw it
                        break;
                    }
                }
            }
            return ress;
        }
        #region setting sprites
        public override void SetAllSpriteParts(int ind)
        {
            float left = 0, right = 0, top = 0, bottom = 0; // loading spriteparts and calculating sprite size
            Nitro_Cell seq = ncer.cebk.cells[ind];
            spriteParts[ind] = new Image[seq.oam_count];
            for (int i = 0; i < seq.oam_count; i++)
            {
                Nitro_OAM oam = seq.CellParts[i];
                float tmp = oam.x;
                if (tmp < left)
                    left = tmp;
                int swidth = 0;
                int sheight = 0;
                if (oam.shape == 0)
                {
                    int s = (int)Math.Pow(2, oam.size);
                    swidth = s;
                    sheight = s;
                }
                else if (oam.shape == 1)
                {
                    int sind = oam.size;
                    swidth = rectSizes[sind][0];
                    sheight = rectSizes[sind][1];
                }
                else if (oam.shape == 2)
                {
                    int sind = oam.size;
                    swidth = rectSizes[sind][1];
                    sheight = rectSizes[sind][0];
                }
                tmp += swidth * 8;
                if (tmp > right)
                    right = tmp;
                tmp = oam.y;
                if (tmp < top)
                    top = tmp;
                tmp += sheight * 8;
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
            for (int i = seq.oam_count - 1; i >= 0; i--)
            {
                Nitro_OAM oam = seq.CellParts[i];
                float x = (centerx[ind] + oam.x) * GetingMasch;
                float y = (centery[ind] + oam.y) * GetingMasch;
                Image impart = MakeSpritePart(ind, i);
                draw.DrawImage(impart, x, y, impart.Width, impart.Height);
                spriteParts[ind][i] = impart;
            }
            Sprites[ind] = res;
        }

        public Image MakeSpritePart(int sprind, int parind)
        {
            CEBK ebk = ncer.cebk;
            Nitro_Cell cell = ebk.cells[sprind];
            Nitro_OAM oam = cell.CellParts[parind];
            Color[] palette = nclr.palettes[oam.pal];
            int width = 0;
            int height = 0;
            if (oam.shape == 0)
            {
                int s = (int)Math.Pow(2, oam.size);
                width = s;
                height = s;
            }
            else if (oam.shape == 1)
            {
                int ind = oam.size;
                width = rectSizes[ind][0];
                height = rectSizes[ind][1];
            }
            else if (oam.shape == 2)
            {
                int ind = oam.size;
                width = rectSizes[ind][1];
                height = rectSizes[ind][0];
            }
            else throw new Exception("Shape == 3");
            int poi = oam.cch;
            byte[][] dat = ncgr.maindata.GetImageData(poi * (oam.col8bit ? 2 : 4), width, height);
            Image res = GetTileImage(dat, palette);
            switch (oam.rotsca)
            {
                case 16:
                case 17:
                    res.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case 32:
                case 33:
                    res.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                case 48:
                case 49:
                    res.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                    break;
            }
            return res;
        }

        public Image GetTileImage(byte[][] dat, Color[] pal, bool vert = false)
        {
            if (dat.Length == 0) return null;
            Color trans = pal[0];
            if (transparency)
            pal[0] = Color.FromArgb(0, trans.R, trans.G, trans.B);
            else pal[0] = Color.FromArgb(255, trans.R, trans.G, trans.B);
            Bitmap bmp = new Bitmap(dat[0].Length, dat.Length);
            for (int y = 0; y < dat.Length; y++)
            {
                for (int x = 0; x < dat[y].Length; x++)
                {
                    bmp.SetPixel(x, y, pal[dat[y][x]]);
                }
            }
            pal[0] = trans;
            return bmp;
        }
        #endregion
        #region animation and sprite data getting
        public override int GetRealScreenWidth()
        {
            return 256;
        }
        public override int GetRealScreenHeight()
        {
            return 192;
        }
        public override int GetTotalFrameSize(int ind)
        {
            return nanr.bnk.dSSeqs[ind].GetDuration();
        }

        public override int GetKeyFramesLength(int ind)
        {
            return nanr.bnk.dSSeqs[ind].framecount;
        }

        public override int GetSpriteLength(int ind)
        {
            return ncer.cebk.cells[ind].oam_count;
        }

        public override int GetSpriteListLength()
        {
            return ncer.cebk.cellCount;
        }

        public override bool GetLoop(int[] inds)
        {
            foreach (int ind in inds)
            {
                DSSeq seq = nanr.bnk.dSSeqs[ind];
                if (seq.seqmode == 2 || seq.seqmode == 4)
                    return true;
            }
            return false;
        }
        #endregion
    }
}
