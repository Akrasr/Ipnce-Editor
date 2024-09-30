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
    internal class AJIpnceManager : IpnceObjectManager
    {
        new AJIpnce ipnce
        {
            get
            {
                return (AJIpnce)base.ipnce;
            }
            set
            {
                base.ipnce = value;
            }
        }

        public override Texture2D GetAtlasData(int ind)
        {
            return ipnce.SpriteAtlas;
        }
        public AJIpnceManager(string name) : base(name)
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
            return new Ipnce(ipnce);
        }
        public override AAIIpnce GetAAIIpnce()
        {
            return new AAIIpnce(ipnce);
        }

        public override AJIpnce GetAJIpnce()
        {
            return ipnce;
        }

        public override void SaveAsAJ()
        {
            SaveAs();
        }
        public override void SaveAs()
        {
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save AJ Ipnce As";
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
            imageManager.atlas.Save(filpath);
            firstAtlaspath = filpath;
        }

        public override void Save()
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(lastname, FileMode.Create, FileAccess.Write)))
            {
                bw.Write(header);
                ipnce.Save(bw);
            }
            imageManager.atlas.Save(firstAtlaspath);
        }

        public override bool HDCheck()
        {
            return !(!ipnce.IsHD || (IpnceName.IndexOf("chr") == 0 && !IpnceName.Contains("chrBust")));
        }
        public override void LoadIpnce(BinaryReader br)
        {
            this.ipnce = new AJIpnce(br);
        }

        public override string[] GetAtlasList()
        {
            string[] res = new string[1];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "SpriteAtlas[" + i + "]";
            }
            return res;
        }

        public override void GetNeededFiles()
        {
            firstAtlaspath = string.Empty;
            int atlas_count = 1;
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
            atlas = Image.FromFile(firstAtlaspath);
            imageManager = new IpnceCommonImageManager(ipnce, atlas, palette, HDCheck());
            return imageManager;
        }

        public override void SetGroupBox(GroupBox box)
        {
            controls = new ElementControl[] { new AAISpriteControl(box), new IpnceSpritePartsControl(box, this), new IpnceAnimControl(box), new IpnceAnimFrameControl(box, this) };
        }
    }
}
