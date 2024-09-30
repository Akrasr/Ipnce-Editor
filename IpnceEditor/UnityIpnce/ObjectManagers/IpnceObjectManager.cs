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
using IpnceEditor.NDS.ImageManagers;
using IpnceEditor.UnityIpnce.ImageManagers;

namespace IpnceEditor.UnityIpnce.ObjectManagers
{
    public abstract class IpnceObjectManager : NitroObjectManager
    {
        protected I_Ipnce ipnce;
        protected Image atlas;
        protected Image palette;
        protected string firstAtlaspath;
        protected string secondAtlaspath;
        protected string palettepath;
        protected byte[] header;
        protected IpnceCommonImageManager imageManager;
        protected IpnceObjectManager(string name) : base(name)
        { }
        public override void AddAnim()
        {
            ipnce.AddAnim();
        }
        public override void AddAnimKeyFrame(int ind)
        {
            ipnce.AddAnimKeyframe(ind);
        }
        public override void AddSprite()
        {
            ipnce.AddSprite();
        }
        public override void AddSpriteParts(int ind)
        {
            ipnce.AddSpriteParts(ind);
        }
        public override bool GetUsePalette()
        {
            return ipnce.IsUseColorPalette;
        }
        public override bool GetOffScreenRendering()
        {
            return ipnce.IsOffScreenRendering;
        }
        public override int GetColorPaletteNum()
        {
            return ipnce.ColorPaletteNum;
        }
        public override string[] GetSpriteList()
        {
            string[] res = new string[ipnce.SpriteList.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "SpriteList[" + i + "]";
            }
            return res;
        }
        public override string[] GetAnimList()
        {
            string[] res = new string[ipnce.AnimList.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "AnimList[" + i + "]";
            }
            return res;
        }

        public override string[] GetSpritePartsList(int ind)
        {
            string[] res = new string[ipnce.SpriteList[ind].Parts.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "Parts[" + i + "]";
            }
            return res;
        }

        public override string[] GetAnimKeyList(int ind)
        {
            string[] res = new string[ipnce.AnimList[ind].KeyFrames.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "KeyFrames[" + i + "]";
            }
            return res;
        }

        public override int MaxFrames(int[] inds)
        {
            int max = 0;
            for (int i = 0; i < inds.Length; i++)
            {
                if (ipnce.AnimList[inds[i]].TotalFrameSize > max)
                    max = ipnce.AnimList[inds[i]].TotalFrameSize;
            }
            return max;
        }
        public override void Load(string filename)
        {
            byte[] bytes = File.ReadAllBytes(filename); //reading header
            int name = 4 + bytes[28] - bytes[28] % 4;
            if (bytes[28] % 4 == 0)
                name -= 4;
            byte[] data = new byte[bytes.Length - 32 - name];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = bytes[32 + name + i];
            }
            header = new byte[32 + name];
            for (int i = 0; i < header.Length; i++)
            {
                header[i] = bytes[i];
            }
            byte[] namedata = new byte[bytes[28]];
            for (int i = 0; i < namedata.Length; i++)
            {
                namedata[i] = bytes[32 + i];
            }
            IpnceName = Encoding.UTF8.GetString(namedata);
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                LoadIpnce(br);
                bool cor = true;
                try
                {
                    int d = br.ReadInt32();
                    cor = false;
                }
                catch { }
                if (!cor)
                {
                    throw new Exception("Invalid File");
                }
            }
        }

        public abstract void LoadIpnce(BinaryReader br);
        public abstract Ipnce GetAAI2Ipnce();

        public override void SaveAsAAI2()
        {
            byte tmp = header[0x14];
            header[0x14] = 0xf4;
            Ipnce ip = GetAAI2Ipnce();
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
                ip.Save(bw);
            }
            header[0x14] = tmp;
        }
        public abstract AAIIpnce GetAAIIpnce();

        public override void SaveAsAAI()
        {
            byte tmp = header[0x14];
            header[0x14] = 0xbf;
            AAIIpnce ip = GetAAIIpnce();
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save AAI Ipnce As";
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
                ip.Save(bw);
            }
            header[0x14] = tmp;
        }

        public override string GetIpnceName()
        {
            return IpnceName;
        }

        public abstract AJIpnce GetAJIpnce();

        public override void SaveAsAJ()
        {
            byte tmp = header[0x14];
            header[0x14] = 0x6b;
            AJIpnce ip = GetAJIpnce();
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
                ip.Save(bw);
            }
            header[0x14] = tmp;
        }

        public abstract CollectionIpnce GetCollectionIpnce();

        public override void SaveAsCollectionIpnce()
        {
            byte[] tmp = new byte[] { header[0x14], header[0x15], header[0x16], header[0x17] };
            header[0x14] = 0x67;
            header[0x15] = 0x5d;
            header[0x16] = 0x41;
            header[0x17] = 0x6c;
            CollectionIpnce newipnce = GetCollectionIpnce();
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save AAI2 Collection Ipnce As";
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
                newipnce.Save(bw);
            }
            lastname = filpath;
            header[0x14] = tmp[0];
            header[0x15] = tmp[1];
            header[0x16] = tmp[2];
            header[0x17] = tmp[3];
        }

        public abstract CollectionAAI1Ipnce GetCollectionAAI1Ipnce();

        public override void SaveAsCollectionAAI1Ipnce()
        {
            byte[] tmp = new byte[] { header[0x14], header[0x15], header[0x16], header[0x17] };
            header[0x14] = 0xe8;
            header[0x15] = 0x44;
            header[0x16] = 0xa5;
            header[0x17] = 0x73;
            CollectionAAI1Ipnce newipnce = GetCollectionAAI1Ipnce();
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save AAI1 Collection Ipnce As";
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
                newipnce.Save(bw);
            }
            lastname = filpath;
            header[0x14] = tmp[0];
            header[0x15] = tmp[1];
            header[0x16] = tmp[2];
            header[0x17] = tmp[3];
        }
        public override void SaveAsDS()
        {
            string palpath = String.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open Color palette";
                openFileDialog.Filter = "NCLR (*.nclr)|*.nclr|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    palpath = openFileDialog.FileName;
                }
            }
            NCLR clr = null;
            using (BinaryReader bw = new BinaryReader(new FileStream(palpath, FileMode.Open, FileAccess.Read)))
            {
                clr = new NCLR(bw);
            }
            AdaptToDS(clr.palettes[0]);
        }

        public void AdaptToDS(Color[] palette)
        {
            foreach (I_Sprite sp in ipnce.SpriteList)
            {
                foreach (SpriteParts part in sp.Parts)
                {
                    if (part.Width > 64 || part.Width % 8 != 0 || part.Height > 64 || part.Height % 8 != 0)
                    {
                        MessageBox.Show("Dimensions, sorry");
                        return;
                    }
                }
            }
            string NCGRpath = string.Empty;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save NCGR As";
                sfd.Filter = "NCGR (*.ncgr)|*.ncgr|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    NCGRpath = sfd.FileName;
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            string NCERpath = string.Empty;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save NCER As";
                sfd.Filter = "NCER (*.ncer)|*.ncer|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    NCERpath = sfd.FileName;
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            string NANRpath = string.Empty;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save NANR As";
                sfd.Filter = "NANR (*.nanr)|*.nanr|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    NANRpath = sfd.FileName;
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            List<SpriteParts> tilesused = new List<SpriteParts>();
            List<int> tilesref = new List<int>();
            int pc = 0;
            foreach (I_Sprite sp in ipnce.SpriteList)
            {
                foreach (SpriteParts part in sp.Parts)
                {
                    bool unused = true;
                    int tc = 0;
                    foreach (SpriteParts used in tilesused)
                    {
                        if (part.SrcX == used.SrcX && part.SrcY == used.SrcY && part.Width == used.Width && part.Height == used.Height)
                        {
                            unused = false;
                            tilesref.Add(tc);
                            break;
                        }
                        tc++;
                    }
                    if (unused)
                    {
                        tilesused.Add(part);
                        tilesref.Add(pc);
                        pc++;
                    }
                }
            }
            Color tmpcolor = palette[0];
            palette[0] = Color.FromArgb(0, tmpcolor.R, tmpcolor.G, tmpcolor.B);
            List<byte[][]> newdat = new List<byte[][]>();
            foreach (SpriteParts part in tilesused)
            {
                byte palnumtmp = part.ColorPlteNum;
                part.ColorPlteNum = 0;
                int flagtmp = part.Flag;
                part.Flag = 0;
                Image pimage = imageManager.GetSpritePartImage(part);
                part.Flag = flagtmp;
                part.ColorPlteNum = palnumtmp;
                Bitmap tmp = new Bitmap((int)part.Width, (int)part.Height);
                Graphics g = Graphics.FromImage(tmp);
                g.DrawImage(pimage, 0, 0, part.Width, part.Height);
                byte[][] imagedat = NDSImageManager.CalculateByteData(tmp, palette);
                newdat.Add(imagedat);
            }
            palette[0] = tmpcolor;
            CHAR tileData = new CHAR();
            tileData.height = -1;
            tileData.width = -1;
            int realbpp = palette.Length > 16 ? 8 : 4;
            tileData.realbpp = realbpp;
            tileData.bpp = palette.Length > 16 ? 4 : 3;
            tileData.mapping = 0x200010;
            tileData.mode = 0;
            tileData.unk = 0x18;
            tileData.pixelData = new byte[0];
            int[] cchrs = new int[tilesused.Count()];
            for (int i = 0; i < tilesused.Count(); i++)
            {
                cchrs[i] = tileData.pixelData.Length / (64 * 16 / realbpp);
                tileData.AddNewData(newdat[i]);
                int poi = (64 * 16 / realbpp);
                if (tileData.pixelData.Length % poi != 0)
                {
                    int poilen = poi - (tileData.pixelData.Length % poi);
                    tileData.AddNewData(poilen);
                }
            }
            tileData.Adjust();
            NCGR gr = new NCGR(tileData);
            using (FileStream fs = new FileStream(NCGRpath, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
                gr.Save(bw);
            CEBK cellBank = new CEBK();
            pc = 0;
            for (int i = 0; i < ipnce.SpriteList.Count(); i++)
            {
                cellBank.AddCell();
                Nitro_Cell cell = cellBank.cells[i];
                cell.unk = 0x121;
                I_Sprite sp = ipnce.SpriteList[i];
                foreach (SpriteParts part in sp.Parts)
                {
                    int charpointer = tilesref[pc++];
                    int cch = cchrs[charpointer];
                    int shape = 0;
                    if (part.Width > part.Height)
                        shape = 1;
                    else if (part.Height > part.Width)
                        shape = 2;
                    int size = 0;
                    switch (shape)
                    {
                        case 0:
                            switch (part.Width)
                            {
                                case 16:
                                    size = 1;
                                    break;
                                case 32:
                                    size = 2;
                                    break;
                                case 64:
                                    size = 3;
                                    break;
                            }
                            break;
                        case 1:
                            if (part.Width == 32 && part.Height == 8)
                            {
                                size = 1;
                            }
                            else if (part.Width == 32 && part.Height == 16)
                            {
                                size = 2;
                            }
                            else if (part.Width == 64)
                            {
                                size = 3;
                            }
                            break;
                        case 2:
                            if (part.Width == 8 && part.Height == 32)
                            {
                                size = 1;
                            }
                            else if (part.Width == 16 && part.Height == 32)
                            {
                                size = 2;
                            }
                            else if (part.Height == 64)
                            {
                                size = 3;
                            }
                            break;
                    }
                    byte pal = part.ColorPlteNum;
                    byte prio = part.Priority;
                    bool col8bit = palette.Length > 16;
                    byte rotsca = (byte)(16 * part.Flag);
                    if (part.DestX < 0)
                        rotsca++;
                    Nitro_OAM oam = Nitro_OAM.CreateBlank(col8bit);
                    oam.pal = pal;
                    oam.prio = prio;
                    oam.col8bit = col8bit;
                    oam.rotsca = rotsca;
                    oam.y = (short)part.DestY;
                    oam.x = (short)part.DestX;
                    oam.cch = (ushort)cch;
                    oam.shape = (byte)shape;
                    oam.size = (byte)size;
                    cell.AddOam(oam);
                }
            }
            cellBank.Adjust();
            NCER er = new NCER(cellBank);
            using (FileStream fs = new FileStream(NCERpath, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
                er.Save(bw);
            ABNK animationBank = new ABNK();
            foreach (I_Anim anim in ipnce.AnimList)
            {
                bool frame2 = false;
                bool frame1 = false;
                foreach (AnimKeyframe fr in anim.KeyFrames)
                {
                    if (fr.ScaleX != 1 || fr.ScaleY != 1)
                    {
                        frame1 = true;
                        break;
                    }
                    else if (fr.TranslateX != 0 || fr.TranslateY != 0)
                    {
                        frame2 = true;
                    }
                }
                ushort frametype = (ushort)(frame1 ? 1 : frame2 ? 2 : 0);
                ushort firstframe = 0;
                ushort seqmode = (ushort)(anim.Flag == 1 ? 2 : 1);
                ushort seqtype = 1;
                DSSeq seq = new DSSeq();
                seq.frametype = frametype;
                seq.firstframe = firstframe;
                seq.seqmode = seqmode;
                seq.seqtype = seqtype;
                foreach (AnimKeyframe fr in anim.KeyFrames)
                {
                    DSKeyFrame dsfr = DSKeyFrame.CreateBlank(frametype);
                    dsfr.SetX((int)fr.TranslateX);
                    dsfr.SetY((int)fr.TranslateY);
                    dsfr.SetSX((int)fr.ScaleX);
                    dsfr.SetSY((int)fr.ScaleY);
                    dsfr.SetRotZ((int)fr.Rotate);
                    dsfr.SetIndex(fr.Index);
                    seq.AddKeyFrame(dsfr);
                }
                for (int i = 0; i < anim.KeyFrames.Length - 1; i++)
                {
                    seq.keyframes[i].duration = (ushort)(anim.KeyFrames[i + 1].Frame - anim.KeyFrames[i].Frame);
                }
                seq.keyframes[anim.KeyFrames.Length - 1].duration = (ushort)(anim.TotalFrameSize - anim.KeyFrames[anim.KeyFrames.Length - 1].Frame);
                animationBank.AddAnim(seq);
            }
            animationBank.Adjust();
            NANR anr = new NANR(animationBank);
            using (FileStream fs = new FileStream(NANRpath, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
                anr.Save(bw);
        }

        public override void MakeControls(int ind)
        {
            switch (ind)
            {
                case 0:
                    controls[ind].SetEditedObject(ipnce.SpriteList[spriteIndex]);
                    break;
                case 1:
                    controls[ind].SetEditedObject(ipnce.SpriteList[spriteIndex].Parts[spritePartIndex]);
                    break;
                case 2:
                    controls[ind].SetEditedObject(ipnce.AnimList[animIndex]);
                    break;
                case 3:
                    controls[ind].SetEditedObject(ipnce.AnimList[animIndex].KeyFrames[animFrameIndex]);
                    break;
                default:
                    break;
            }
            base.MakeControls(ind);
        }
    }
}
