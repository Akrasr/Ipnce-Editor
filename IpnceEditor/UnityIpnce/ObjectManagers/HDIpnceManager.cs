using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using IpnceEditor.NDS;
using System.Diagnostics.Eventing.Reader;
using IpnceEditor.Interfaces;
using IpnceEditor.UnityIpnce.Controls;
using IpnceEditor.UnityIpnce.ImageManagers;

namespace IpnceEditor.UnityIpnce.ObjectManagers
{
    public class HDIpnceManager : IpnceObjectManager
    {
        new Ipnce ipnce
        {
            get
            {
                return (Ipnce)base.ipnce;
            }
            set
            {
                base.ipnce = value;
            }
        }
        public HDIpnceManager(string name) : base(name)
        { }
        public override CollectionAAI1Ipnce GetCollectionAAI1Ipnce()
        {
            return new CollectionAAI1Ipnce(ipnce);
        }
        public override CollectionIpnce GetCollectionIpnce()
        {
            return new CollectionIpnce(ipnce);
        }
        public override Ipnce GetAAI2Ipnce()
        {
            return ipnce;
        }
        public override AAIIpnce GetAAIIpnce()
        {
            return new AAIIpnce(ipnce);
        }

        public override AJIpnce GetAJIpnce()
        {
            return new AJIpnce(ipnce);
        }

        public override Texture2D GetAtlasData(int ind)
        {
            return ipnce.SpriteAtlasAr[ind];
        }

        public override void SaveAsAAI2()
        {
            SaveAs();
        }

        
        public override void SaveAs()
        {
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save AAI2 Ipnce As";
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
                atlassplit[0] = new Bitmap(imageManager.atlas.Width, imageManager.atlas.Height / 2);
                Graphics g = Graphics.FromImage(atlassplit[0]);
                g.DrawImage(imageManager.atlas, 0, 0);
                atlassplit[1] = new Bitmap(imageManager.atlas.Width, imageManager.atlas.Height / 2);
                g = Graphics.FromImage(atlassplit[1]);
                g.DrawImage(imageManager.atlas, 0, 0 - imageManager.atlas.Height / 2);
                atlassplit[0].RotateFlip(RotateFlipType.Rotate180FlipX);
                atlassplit[1].RotateFlip(RotateFlipType.Rotate180FlipX);
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
                atlassplit[0].RotateFlip(RotateFlipType.Rotate180FlipX);
                atlassplit[1].RotateFlip(RotateFlipType.Rotate180FlipX);
                atlassplit[0].Save(firstAtlaspath);
                atlassplit[1].Save(secondAtlaspath);
            }
            else
            {
                imageManager.atlas.RotateFlip(RotateFlipType.Rotate180FlipX);
                imageManager.atlas.Save(firstAtlaspath);
                imageManager.atlas.RotateFlip(RotateFlipType.Rotate180FlipX);
            }
        }

        public override bool HDCheck()
        {
            return !(!ipnce.IsHD || (IpnceName.IndexOf("chr") == 0 && !IpnceName.Contains("chrBust")));
        }
        public override void LoadIpnce(BinaryReader br)
        {
            this.ipnce = new Ipnce(br);
        }

        public override string[] GetAtlasList()
        {
            string[] res = new string[ipnce.SpriteAtlasAr.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "SpriteAtlas[" + i + "]";
            }
            return res;
        }
        //finding an amount of frames for selected animations

        public override void GetNeededFiles()
        {
            firstAtlaspath = string.Empty;
            int atlas_count = ipnce.SpriteAtlasAr[1].in1 != 0 ? 2 : 1;
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
            if (ipnce.IsUseColorPalette)
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

        public override NitroImageManager GetImageManager()
        {
            if (ipnce.IsUseColorPalette)
                palette = Image.FromFile(palettepath);
            else palette = null;
            if (secondAtlaspath != null && secondAtlaspath != String.Empty)
            {
                atlas = NitroImageManager.ConcatDown(new Image[] { Image.FromFile(secondAtlaspath), Image.FromFile(firstAtlaspath) });
            }
            else
            {
                atlas = Image.FromFile(firstAtlaspath);
            }
            atlas.RotateFlip(RotateFlipType.Rotate180FlipX);
            imageManager = new IpnceCommonImageManager(ipnce, atlas, palette, HDCheck());
            return imageManager;
        }

        public override void SetGroupBox(GroupBox box)
        {
            controls = new ElementControl[] { new IpnceSpriteControl(box), new IpnceSpritePartsControl(box, this), new IpnceAnimControl(box), new IpnceAnimFrameControl(box, this) };
        }
    }
}
