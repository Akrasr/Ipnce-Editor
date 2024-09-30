using IpnceEditor.NDS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.Interfaces;
using IpnceEditor.UnityIpnce.Controls;
using IpnceEditor.UnityIpnce.ImageManagers;

namespace IpnceEditor.UnityIpnce.ObjectManagers
{
   public class CollectionAAI1Manager : IpnceObjectManager
    {
        new CollectionAAI1Ipnce ipnce
        {
            get
            {
                return (CollectionAAI1Ipnce)base.ipnce;
            }
            set
            {
                base.ipnce = value;
            }
        }
        Size firstAtlasSize;
        Size secondAtlasSize;
        const int FullWidth = 2880;
        const int FullHeight = 2880;
        public CollectionAAI1Manager(string name) : base(name)
        { }

        public override Texture2D GetAtlasData(int ind)
        {
            return new Texture2D();
        }

        public override void SaveAsAAI2()
        {
            SaveAs();
        }
        public override CollectionAAI1Ipnce GetCollectionAAI1Ipnce()
        {
            return ipnce;
        }
        public override CollectionIpnce GetCollectionIpnce()
        {
            return new CollectionIpnce(ipnce);
        }
        public override Ipnce GetAAI2Ipnce()
        {
            return new Ipnce(ipnce);
        }
        public override AAIIpnce GetAAIIpnce()
        {
            return new AAIIpnce(ipnce);
        }

        public override AJIpnce GetAJIpnce()
        {
            return new AJIpnce(ipnce);
        }

        public override void SaveAs()
        {
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save Collection AAI1 Ipnce As";
                sfd.Filter = "Ipnce (*.ipnce)|*.ipnce|UnityEX MB file (*.114)|*.114|UABE MB file (*.dat)|*.dat|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    filpath = sfd.FileName;
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            using (BinaryWriter bw = new BinaryWriter(new FileStream(filpath, FileMode.Create, FileAccess.Write)))
            {
                bw.Write(header);
                ipnce.Save(bw);
            }
            lastname = filpath;
            if (secondAtlaspath != null && secondAtlaspath != String.Empty)
            {
                Image[] atlassplit = new Image[2];
                atlassplit[0] = new Bitmap(firstAtlasSize.Width, firstAtlasSize.Height);
                Graphics g = Graphics.FromImage(atlassplit[0]);
                g.DrawImage(imageManager.atlas, 0, 0);
                atlassplit[1] = new Bitmap(secondAtlasSize.Width, secondAtlasSize.Height);
                g = Graphics.FromImage(atlassplit[1]);
                g.DrawImage(imageManager.atlas, 0 - FullWidth, 0);
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.RestoreDirectory = true;
                    sfd.Title = "Save First Atlas Image As";
                    sfd.Filter = "PNG|*.png";
                    sfd.FilterIndex = 1;
                    DialogResult dr = sfd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        filpath = sfd.FileName;
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                atlassplit[0].Save(filpath);
                firstAtlaspath = filpath;
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.RestoreDirectory = true;
                    sfd.Title = "Save Second Atlas Image As";
                    sfd.Filter = "PNG|*.png";
                    sfd.FilterIndex = 1;
                    DialogResult dr = sfd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        filpath = sfd.FileName;
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                atlassplit[1].Save(filpath);
                secondAtlaspath = filpath;
            }
            else
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.RestoreDirectory = true;
                    sfd.Title = "Save Atlas Image As";
                    sfd.Filter = "PNG|*.png";
                    sfd.FilterIndex = 1;
                    DialogResult dr = sfd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        filpath = sfd.FileName;
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                imageManager.atlas.RotateFlip(RotateFlipType.Rotate180FlipX);
                imageManager.atlas.Save(filpath);
                imageManager.atlas.RotateFlip(RotateFlipType.Rotate180FlipX);
                firstAtlaspath = filpath;
            }
        }

        public void SaveAsAAI1CollectionIpnce()
        {
            SaveAs();
        }
        public override void Save()
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(lastname, FileMode.Create, FileAccess.Write)))
            {
                bw.Write(header);
                ipnce.Save(bw);
            }
            if (secondAtlaspath != null && secondAtlaspath != String.Empty)
            {
                Image[] atlassplit = new Image[2];
                atlassplit[0] = new Bitmap(imageManager.atlas.Width, imageManager.atlas.Height / 2);
                Graphics g = Graphics.FromImage(atlassplit[0]);
                g.DrawImage(imageManager.atlas, 0, 0);
                atlassplit[1] = new Bitmap(imageManager.atlas.Width, imageManager.atlas.Height / 2);
                g = Graphics.FromImage(atlassplit[1]);
                g.DrawImage(imageManager.atlas, 0, 0 - imageManager.atlas.Height / 2);
                atlassplit[0].Save(firstAtlaspath);
                atlassplit[1].Save(secondAtlaspath);
            }
            else
            {
                imageManager.atlas.Save(firstAtlaspath);
            }
        }

        public override bool HDCheck()
        {
            return !(ipnce.DataType == AAIIpnce.DataTypes.HalfHD || ipnce.DataType == AAIIpnce.DataTypes.NDS || (IpnceName.IndexOf("chr") == 0 && !IpnceName.Contains("chrBust")));
        }
        public override void LoadIpnce(BinaryReader br)
        {
            this.ipnce = new CollectionAAI1Ipnce(br);
        }

        public override string[] GetAtlasList()
        {
            string[] res = new string[2];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "SpriteAtlas[" + i + "]";
            }
            return res;
        }

        public override void GetNeededFiles()
        {
            firstAtlaspath = string.Empty;
            int atlas_count = ipnce.m_SpriteAtlasOverflowName != "" ? 2 : 1;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open Atlas Image";
                openFileDialog.Filter = "Image files (*.png, *.jpg)|*.png;*.jpg|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    firstAtlaspath = openFileDialog.FileName;
                }
            }
            Image img = Image.FromFile(firstAtlaspath); //Attempt to load an image
            img.Dispose();
            secondAtlaspath = string.Empty;
            if (atlas_count > 1)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Open Second Atlas Image";
                    openFileDialog.Filter = "Image files (*.png, *.jpg)|*.png;*.jpg|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        secondAtlaspath = openFileDialog.FileName;
                    }
                }
                img = Image.FromFile(secondAtlaspath); //Attempt to load an image
                img.Dispose();
            }
            palettepath = String.Empty;
            if (GetUsePalette())
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Open Palette Image";
                    openFileDialog.Filter = "Image files (*.png, *.jpg)|*.png;*.jpg|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        palettepath = openFileDialog.FileName;
                    }
                }
                img = Image.FromFile(palettepath); //Attempt to load an image
                img.Dispose();
            }
        }
        public override bool GetUsePalette()
        {
            return ipnce.IsUseColorPalette && !HDCheck();
        }

        public override NitroImageManager GetImageManager()
        {
            if (GetUsePalette())
                palette = Image.FromFile(palettepath);
            else palette = null;
            if (secondAtlaspath != null && secondAtlaspath != String.Empty)
            {
                //MessageBox.Show("Concat? ");
                Image firstat = Image.FromFile(firstAtlaspath);
                Image secat = Image.FromFile(secondAtlaspath);
                firstAtlasSize = new Size()
                {
                    Width = firstat.Width,
                    Height = firstat.Height
                };
                secondAtlasSize = new Size()
                {
                    Width = secat.Width,
                    Height = secat.Height
                };
                Bitmap bmp = new Bitmap(FullWidth * 2, FullHeight);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(firstat, 0, 0);
                g.DrawImage(secat, FullWidth, 0);
                atlas = bmp;//NitroImageManager.ConcatRight(new Image[] { Image.FromFile(firstAtlaspath), Image.FromFile(secondAtlaspath) });
                //atlas.Save("tmp.png");
            }
            else
            {
                //MessageBox.Show("First");
                atlas = Image.FromFile(firstAtlaspath);
            }
            ipnce.IsUseColorPalette = GetUsePalette();
            imageManager = new AAIIpnceCommonImageManager(ipnce, atlas, palette, HDCheck(), 5.625f, 0.35f);
            return imageManager;
        }

        public override void SetGroupBox(GroupBox box)
        {
            controls = new ElementControl[] { new CollectionSpriteControl(box), new IpnceSpritePartsControl(box, this), new AAIAnimControl(box), new IpnceAnimFrameControl(box, this) };
        }
    }
}
