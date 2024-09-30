using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.NDS;
using IpnceEditor.Interfaces;
using IpnceEditor.NDS.Controls;
using IpnceEditor.NDS.ImageManagers;
using IpnceEditor.UnityIpnce;

namespace IpnceEditor.NDS.ObjectManagers
{
    public class NDSCellManager : NitroObjectManager
    {
        public static readonly uint[] MAGICS = new uint[] {NCLR.magic, NCGR.magic, NCER.magic, NANR.magic };
        bool[] loaded = new bool[MAGICS.Length];
        NCER ncer;
        NANR nanr;
        NCGR ncgr;
        NCLR nclr;
        string NCERpath;
        string NANRpath;
        string NCLRpath;
        string NCGRpath;

        #region realizations
        public NDSCellManager(string name) : base(name)
        { }
        public override void AddAnim()
        {
            nanr.bnk.AddAnim();
        }
        public override void AddAnimKeyFrame(int ind)
        {
            nanr.bnk.dSSeqs[ind].AddKeyFrame();
        }
        public override void AddSprite()
        {
            ncer.cebk.AddCell();
        }
        public override void AddSpriteParts(int ind)
        {
            ncer.cebk.AddOam(ind, ncgr.maindata.realbpp == 8);
        }

        public override string GetIpnceName()
        {
            return Path.GetFileNameWithoutExtension(NCERpath);
        }

        public override Texture2D GetAtlasData(int ind)
        {
            Texture2D tex = new Texture2D();
            tex.in1 = 0;
            tex.in2 = 0;
            tex.in3 = 0;
            return tex;
        }

        public override void SaveAsDS()
        {
            SaveAs();
        }

        public override void SaveAsAAI2()
        {
            MessageBox.Show("DS to Ipnce convertation is not implemented");
        }

        public override void SaveAsAAI()
        {
            MessageBox.Show("DS to Ipnce convertation is not implemented");
        }

        public override void SaveAsAJ()
        {
            MessageBox.Show("DS to Ipnce convertation is not implemented");
        }

        public override void SaveAsCollectionIpnce()
        {
            MessageBox.Show("DS to Ipnce convertation is not implemented");
        }

        public override void SaveAsCollectionAAI1Ipnce()
        {
            MessageBox.Show("DS to Ipnce convertation is not implemented");
        }

        public override void SaveAs()
        {
            ncer.Adjust();
            nanr.Adjust();
            ncgr.Adjust();
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save nclr As";
                sfd.Filter = "Color palette (*.nclr)|*.nclr|Binary (*.dbin)|*.dbin|All files (*.*)|*.*";
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
                nclr.Save(bw);
            }
            NCLRpath = filpath;
            filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save ncgr As";
                sfd.Filter = "Graphics (*.ncgr)|*.ncgr|Binary (*.dbin)|*.dbin|All files (*.*)|*.*";
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
                ncgr.Save(bw);
            }
            NCGRpath = filpath;
            filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save ncer As";
                sfd.Filter = "Nitro cell (*.ncer)|*.ncer|Binary (*.dbin)|*.dbin|All files (*.*)|*.*";
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
                ncer.Save(bw);
            }
            NCERpath = filpath;
            filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save nanr As";
                sfd.Filter = "Animation (*.nanr)|*.nanr|Binary (*.dbin)|*.dbin|All files (*.*)|*.*";
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
                nanr.Save(bw);
            }
            NANRpath = filpath;
        }

        public override void Save()
        {
            ncer.Adjust();
            nanr.Adjust();
            ncgr.Adjust();
            using (BinaryWriter bw = new BinaryWriter(new FileStream(NCLRpath, FileMode.Create, FileAccess.Write)))
            {
                nclr.Save(bw);
            }
            using (BinaryWriter bw = new BinaryWriter(new FileStream(NCGRpath, FileMode.Create, FileAccess.Write)))
            {
                ncgr.Save(bw);
            }
            using (BinaryWriter bw = new BinaryWriter(new FileStream(NCERpath, FileMode.Create, FileAccess.Write)))
            {
                ncer.Save(bw);
            }
            using (BinaryWriter bw = new BinaryWriter(new FileStream(NANRpath, FileMode.Create, FileAccess.Write)))
            {
                nanr.Save(bw);
            }
        }

        public override bool HDCheck()
        {
            return false;
        }
        public override bool GetUsePalette()
        {
            return true;
        }
        public override bool GetOffScreenRendering()
        {
            return false;
        }
        public override int GetColorPaletteNum()
        {
            return nclr.palettes.Count();
        }
        public override void Load(string filename)
        {
            loaded = new bool[MAGICS.Length];
            for (int i = 0; i < MAGICS.Length; i++)
            {
                loaded[i] = false;
            }
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
            {
                uint mag = br.ReadUInt32();
                int index = Array.IndexOf(MAGICS, mag);
                if (index == -1)
                    throw new Exception("File is invalid");
                br.BaseStream.Position = 0;
                switch(index)
                {
                    case 0:
                        nclr = new NCLR(br);
                        break;
                    case 1:
                        ncgr = new NCGR(br);
                        break;
                    case 2:
                        ncer = new NCER(br);
                        break;
                    case 3:
                        nanr = new NANR(br);
                        break;
                }
                loaded[index] = true;
            }
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
        public override string[] GetSpriteList()
        {
            string[] res = new string[ncer.cebk.cellCount];
            int less = 0;
            if (ncer.labl.size != 12)
            {
                less = res.Length < ncer.labl.names.Length ? res.Length : ncer.labl.names.Length;
                for (int i = 0; i < less; i++)
                    res[i] = ncer.labl.names[i];
            }
            //string[] res = new string[ncer.cebk.cellCount];
            for (int i = less; i < res.Length; i++)
            {
                res[i] = "SpriteList[" + i + "]";
            }
            return res;
        }
        public override string[] GetAnimList()
        {
            string[] res = new string[nanr.bnk.dSSeqs.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "AnimList[" + i + "]";
            }
            return res;
        }

        public override string[] GetSpritePartsList(int ind)
        {
            string[] res = new string[ncer.cebk.cells[ind].oam_count];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "Parts[" + i + "]";
            }
            return res;
        }

        public override string[] GetAnimKeyList(int ind)
        {
            string[] res = new string[nanr.bnk.dSSeqs[ind].framecount];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "KeyFrames[" + i + "]";
            }
            return res;
        }

        //finding an amount of frames for selected animations

        public override int MaxFrames(int[] inds)
        {
            int max = 0;
            foreach (int i in inds)
            {
                int dur = nanr.bnk.dSSeqs[i].GetDuration();
                if (dur > max)
                    max = dur;
            }
            return max;
        }

        public override void GetNeededFiles()
        {
            NCLRpath = string.Empty;
            if (!loaded[0])
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Open NCLR palette";
                    openFileDialog.Filter = "Palette files (*.dbin, *.bin, *.nclr)|*.dbin;*.bin;*.nclr|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        NCLRpath = openFileDialog.FileName;
                    }
                }
            }
            else NCLRpath = lastname;
            NCLR clr;
            using (FileStream fs = new FileStream(NCLRpath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
                clr = new NCLR(br);
            NCGRpath = string.Empty;
            if (!loaded[1])
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Open NCGR graphics";
                    openFileDialog.Filter = "Palette files (*.dbin, *.bin, *.ncgr)|*.dbin;*.bin;*.ncgr|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        NCGRpath = openFileDialog.FileName;
                    }
                }
            }
            else NCGRpath = lastname;
            NCGR cgr;
            using (FileStream fs = new FileStream(NCGRpath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
                cgr = new NCGR(br);
            NCERpath = string.Empty;
            if (!loaded[2])
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Open NCER cell file";
                    openFileDialog.Filter = "Palette files (*.dbin, *.bin, *.ncer)|*.dbin;*.bin;*.ncer|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        NCERpath = openFileDialog.FileName;
                    }
                }
            }
            else NCERpath = lastname;
            NCER cer;
            using (FileStream fs = new FileStream(NCERpath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
                cer = new NCER(br);
            NANRpath = string.Empty;
            if (!loaded[3])
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Open NANR animation file";
                    openFileDialog.Filter = "Palette files (*.dbin, *.bin, *.nanr)|*.dbin;*.bin;*.nanr|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        NANRpath = openFileDialog.FileName;
                    }
                }
            }
            else NANRpath = lastname;
            NANR anr;
            using (FileStream fs = new FileStream(NANRpath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
                anr = new NANR(br);
            nclr = clr;
            ncgr = cgr;
            ncer = cer;
            nanr = anr;
        }

        public override NitroImageManager GetImageManager()
        {
            return new NDSImageManager(ncer, nanr, ncgr, nclr);
        }

        public override void SetGroupBox(GroupBox box)
        {
            controls = new ElementControl[] { new NDSCellControl(box), new OAMControl(box, this), new NDSAnimControl(box), new DSKeyFrameControl(box, this) };
        }

        public override void MakeControls(int ind)
        {
            switch (ind)
            {
                case 0:
                    controls[ind].SetEditedObject(ncer.cebk.cells[spriteIndex]);
                    break;
                case 1:
                    controls[ind].SetEditedObject(ncer.cebk.cells[spriteIndex].CellParts[spritePartIndex]);
                    break;
                case 2:
                    controls[ind].SetEditedObject(nanr.bnk.dSSeqs[animIndex]);
                    break;
                case 3:
                    controls[ind].SetEditedObject(nanr.bnk.dSSeqs[animIndex].keyframes[animFrameIndex]);
                    break;
                default:
                    break;
            }
            base.MakeControls(ind);
        }

        public int GetCharLen()
        {
            return ncgr.maindata.pixelData.Length / 64;
        }

        public void AddToChar(int len)
        {
            ncgr.maindata.AddNewData(len);
        }
        #endregion
    }
}
