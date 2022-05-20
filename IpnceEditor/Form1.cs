using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IpnceEditor
{
    public partial class Form1 : Form
    {
        Manager manager;
        IpnceDrawer id;
        Graphics gr;
        bool played;
        public Form1()
        {
            InitializeComponent();
            manager = new Manager();
            DisableAll();
            gr = pictureBox1.CreateGraphics();
            this.Text = "Ipnce Editor";
            saveToolStripMenuItem.Enabled = false;
            saveAsAJToolStripMenuItem.Enabled = false;
            saveAsAAI2ToolStripMenuItem.Enabled = false;
            saveAsAAIToolStripMenuItem.Enabled = false;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void DisableAll()
        {
            this.comboBox1.Enabled = false;
            this.label1.Enabled = false;
            this.label2.Enabled = false;
            this.label3.Enabled = false;
            this.label4.Enabled = false;
            this.label5.Enabled = false;
            this.label6.Enabled = false;
            this.label7.Enabled = false;
            this.label8.Enabled = false;
            this.label9.Enabled = false;
            this.label10.Enabled = false;
            this.checkBox1.Enabled = false;
            this.checkBox2.Enabled = false;
            this.checkBox3.Enabled = false;
            this.listBox1.Enabled = false;
            this.listBox2.Enabled = false;
            this.listBox3.Enabled = false;
            this.listBox4.Enabled = false;
            this.button1.Enabled = false;
            this.button2.Enabled = false;
            checkedListBox1.Enabled = false;
            trackBar1.Enabled = false;
        }

        private void EnableAll()
        {
            this.comboBox1.Enabled = true;
            this.label1.Enabled = true;
            this.label2.Enabled = true;
            this.label3.Enabled = true;
            this.label4.Enabled = true;
            this.label5.Enabled = true;
            this.label6.Enabled = true;
            this.label7.Enabled = true;
            this.label8.Enabled = true;
            this.label9.Enabled = true;
            this.label10.Enabled = true;
            this.listBox1.Enabled = true;
            this.listBox2.Enabled = true;
            this.listBox3.Enabled = true;
            this.listBox4.Enabled = true;
            checkedListBox1.Enabled = true;
        }

        private void LoadPaths() //Called after pressing open button
        {
            string ipncePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open Ipnce File";
                openFileDialog.Filter = "Unity file (*.dat, *.114)|*.dat;*.114|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ipncePath = openFileDialog.FileName;
                }
            }
            Ipnce ip = manager.ipnce;
            string ipPath = manager.ipncePath;
            try
            {
                manager.SetIpnce(ipncePath); //Attempt to load Ipnce
            } catch
            {
                MessageBox.Show("The file is invalid");
                manager.ipnce = ip;
                manager.ipncePath = ipPath;
                return;
            }
            string atlasPath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open Atlas Image";
                openFileDialog.Filter = "Image files (*.png, *.jpg)|*.png;*.jpg|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    atlasPath = openFileDialog.FileName;
                }
            }
            try
            {
                Image img = Image.FromFile(atlasPath); //Attempt to load an image
                img.Dispose();
            } catch
            {
                MessageBox.Show("The file is invalid");
                manager.ipnce = ip;
                manager.ipncePath = ipPath;
                return;
            }
            string palettePath = String.Empty;
            if (manager.ipnce.IsUseColorPalette)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Open Palette Image";
                    openFileDialog.Filter = "Image files (*.png, *.jpg)|*.png;*.jpg|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        palettePath = openFileDialog.FileName;
                    }
                }
                try
                {
                    Image img = Image.FromFile(palettePath); //Attempt to load an image
                    img.Dispose();
                }
                catch
                {
                    MessageBox.Show("The file is invalid");
                    manager.ipnce = ip;
                    manager.ipncePath = ipPath;
                    return;
                }
            }
            SetMainObjects(atlasPath, palettePath);
            saveToolStripMenuItem.Enabled = true; //Enabling save buttons
            saveAsAJToolStripMenuItem.Enabled = true;
            saveAsAAI2ToolStripMenuItem.Enabled = true;
            saveAsAAIToolStripMenuItem.Enabled = true;
        }

        public void SetMainObjects(string atlasPath, string palettePath)
        {
            Image palette = manager.ipnce.IsUseColorPalette ? Image.FromFile(palettePath) : null;
            manager.atlasPath = atlasPath;
            manager.palettePath = palettePath;
            EnableAll();
            id = new IpnceDrawer(gr, manager.ipnce, trackBar1, Image.FromFile(manager.atlasPath), palette, manager.flipped);
            UpdateData();
            id.ShowTest();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(button2, new EventArgs());
            trackBar1.Maximum = 10;
            trackBar1.Value = 0;
            trackBar1.Enabled = false;
            button1.Enabled = false;
            LoadPaths();
        }


        private void UpdateIpnceData()
        {
            Ipnce ip = manager.ipnce; //Showing Ipnce Data on the screen
            this.checkBox1.Checked = ip.IsHD;
            this.checkBox2.Checked = ip.IsUseColorPalette;
            this.checkBox3.Checked = ip.IsOffScreenRendering;
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.AddRange(manager.GetAtlasList());
            this.label5.Text = "ColorPaletteNum: " + ip.ColorPaletteNum;
            Texture2D plt = ip.ColorPalette;
            this.label2.Text = "Param 1: ";
            this.label3.Text = "Param 2: ";
            this.label4.Text = "Param 3: ";
            this.label6.Text = "Param 1: " + plt.in1;
            this.label7.Text = "Param 2: " + plt.in2;
            this.label8.Text = "Param 3: " + plt.in3;
        }

        private void UpdateData()
        {
            UpdateIpnceData();
            LoadAnimList();
            LoadSpriteList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = comboBox1.SelectedIndex;
            Texture2D plt = this.manager.ipnce.SpriteAtlas[ind];
            this.label2.Text = "Param 1: " + plt.in1;
            this.label3.Text = "Param 2: " + plt.in2;
            this.label4.Text = "Param 3: " + plt.in3;
        }

        private void LoadAnimList()
        {
            this.listBox3.Items.Clear();
            this.listBox4.Items.Clear();
            this.listBox3.Items.AddRange(manager.GetAnimList());
            this.checkedListBox1.Items.Clear();
            this.checkedListBox1.Items.AddRange(manager.GetAnimList());
        }
        private void LoadSpriteList()
        {
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            this.listBox1.Items.AddRange(manager.GetSpriteList());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = listBox1.SelectedIndex; //showing sprite data
            listBox2.Items.Clear();
            listBox2.Items.AddRange(manager.GetSpritePartsList(ind));
            LoadParamsSprite();
            id.DrawSpriteCl(ind);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = listBox3.SelectedIndex; //showing anim data
            listBox4.Items.Clear();
            listBox4.Items.AddRange(manager.GetAnimKeyList(ind));
            LoadParamsAnim();
        }

        private void LoadParamsSprite()
        {
            groupBox1.Controls.Clear(); //adding controls to a groupbox for editing sprite
            groupBox1.Text = (string)(listBox1.Items[listBox1.SelectedIndex]);
            Label dstX = new Label();
            dstX.Text = "DestX:";
            dstX.Width = 40;
            dstX.Location = new Point(10, 23);
            groupBox1.Controls.Add(dstX);
            TextBox tbX = new TextBox();
            tbX.Location = new Point(160, 20);
            tbX.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].DestX;
            groupBox1.Controls.Add(tbX);
            Label dstY = new Label();
            dstY.Text = "DestY:";
            dstY.Width = 40;
            dstY.Location = new Point(10, 53);
            groupBox1.Controls.Add(dstY);
            TextBox tbY = new TextBox();
            tbY.Location = new Point(160, 50);
            tbY.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].DestY;
            groupBox1.Controls.Add(tbY);
            CheckBox cb = new CheckBox();
            cb.Text = "IsBlend";
            cb.Location = new Point(10, 80);
            cb.Checked = manager.ipnce.SpriteList[listBox1.SelectedIndex].IsBlend;
            groupBox1.Controls.Add(cb);
            tbX.TextChanged += SpriteX;
            tbY.TextChanged += SpriteY;
            cb.CheckedChanged += IsBlendChanged;
        }

        string[] nums = new string[] {"-", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "," };

        public void FloatSave(object sender)
        {
            TextBox tb = (TextBox)sender; //a function for deleting wrong symbols in text box while parsing to float
            string res = "";
            bool f = false;
            bool p = false;
            for (int i = 0; i < tb.Text.Length; i++)
            {
                if (("" + tb.Text.ToCharArray()[i]) == nums[0] && i != 0)
                {
                    continue;
                }
                if (("" + tb.Text.ToCharArray()[i]) == nums[11])
                {
                    if (p)
                        continue;
                    else
                        p = true;
                }
                if (nums.Contains("" + tb.Text.ToCharArray()[i]))
                {
                    res += tb.Text.ToCharArray()[i];
                }
                else
                    f = true;
            }
            int pos = tb.SelectionStart;
            if (f) pos--;
            tb.Text = res;
            tb.Select(pos, 0);
        }

        public float GetFloat(object sender)
        {
            FloatSave(sender); //parsing float
            string txt = ((TextBox)sender).Text;
            txt = txt.Replace("-,", "-0,");
            if (txt == "-")
            {
                return 0;
            }
            if (txt.Length == 0)
                return 0;
            if ("" + txt.ToCharArray()[0] == nums[11])
            {
                txt = "0" + txt;
            }
            if ("" + txt.ToCharArray()[txt.Length - 1] == nums[11])
            {
                txt = txt.Remove(txt.Length - 1, 1);
            }
            float res = float.Parse(txt);
            return res;
        }

        public void IntSave(object sender) //a function for deleting wrong symbols in text box while parsing to int
        {
            string[] ints = new string[11];
            for (int i = 0; i < 11; i++)
            {
                ints[i] = nums[i];
            }
            TextBox tb = (TextBox)sender;
            string res = "";
            bool f = false;
            for (int i = 0; i < tb.Text.Length; i++)
            {
                if (("" + tb.Text.ToCharArray()[i]) == nums[0] && i != 0)
                {
                    continue;
                }
                if (ints.Contains("" + tb.Text.ToCharArray()[i]))
                {
                    res += tb.Text.ToCharArray()[i];
                }
                else
                    f = true;
            }
            int pos = tb.SelectionStart;
            if (f) pos--;
            tb.Text = res;
            tb.Select(pos, 0);
        }

        public int GetInt(object sender) //parsing int
        {
            IntSave(sender);
            if (((TextBox)sender).Text == "-")
            {
                return 0;
            }
            if (((TextBox)sender).Text.Length == 0)
            {
                return 0;
            }
            return Int32.Parse(((TextBox)sender).Text);
        }

        public byte GetByte(object sender) // parsing to byte
        {
            int res = GetInt(sender);
            if (res < 0)
            {
                ((TextBox)sender).Text = "0";
                return 0;
            }
            else if (res > 255)
            {
                ((TextBox)sender).Text = "255";
                return 255;
            }
            else
                return (byte)res;
        }

        //Setting sprite data

        public void IsBlendChanged(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].IsBlend = ((CheckBox)sender).Checked;
        }

        public void SpriteX(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].DestX = GetFloat(sender);
        }
        public void SpriteY(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].DestY = GetFloat(sender);
        }

        private void LoadParamsAnim() //adding controls to a groupbox for editing anim
        {
            groupBox1.Text = (string)(listBox3.Items[listBox3.SelectedIndex]);
            groupBox1.Controls.Clear();
            Label kes = new Label();
            kes.Text = "KeySize:";
            kes.Width = 150;
            kes.Location = new Point(10, 23);
            groupBox1.Controls.Add(kes);
            TextBox tbkes = new TextBox();
            tbkes.Location = new Point(160, 20);
            tbkes.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].KeySize;
            groupBox1.Controls.Add(tbkes);
            Label tfs = new Label();
            tfs.Text = "TotalFrameSize:";
            tfs.Width = 150;
            tfs.Location = new Point(10, 53);
            groupBox1.Controls.Add(tfs);
            TextBox tbtfs = new TextBox();
            tbtfs.Location = new Point(160, 50);
            tbtfs.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].TotalFrameSize;
            groupBox1.Controls.Add(tbtfs);
            Label rf = new Label();
            rf.Text = "RestartFrame:";
            rf.Width = 150;
            rf.Location = new Point(10, 83);
            groupBox1.Controls.Add(rf);
            TextBox tbrf = new TextBox();
            tbrf.Location = new Point(160, 80);
            tbrf.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].RestartFrame;
            groupBox1.Controls.Add(tbrf);
            Label dstX = new Label();
            dstX.Text = "DestX:";
            dstX.Width = 150;
            dstX.Location = new Point(10, 113);
            groupBox1.Controls.Add(dstX);
            TextBox tbX = new TextBox();
            tbX.Location = new Point(160, 110);
            tbX.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].DestX;
            groupBox1.Controls.Add(tbX);
            Label dstY = new Label();
            dstY.Text = "DestY:";
            dstY.Width = 150;
            dstY.Location = new Point(10, 143);
            groupBox1.Controls.Add(dstY);
            TextBox tbY = new TextBox();
            tbY.Location = new Point(160, 140);
            tbY.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].DestY;
            groupBox1.Controls.Add(tbY);
            Label flag = new Label();
            flag.Text = "Flag:";
            flag.Width = 150;
            flag.Location = new Point(10, 173);
            groupBox1.Controls.Add(flag);
            TextBox tbflag = new TextBox();
            tbflag.Location = new Point(160, 170);
            tbflag.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].Flag;
            groupBox1.Controls.Add(tbflag);
            tbkes.TextChanged += KeySize;
            tbtfs.TextChanged += TotalFrameSize;
            tbrf.TextChanged += RestartFrame;
            tbX.TextChanged += AnimDestX;
            tbY.TextChanged += AnimDestY;
            tbflag.TextChanged += Flag;
        }

        //Setting anim data

        public void KeySize(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].KeySize = GetInt(sender);
        }

        public void TotalFrameSize(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].TotalFrameSize = GetInt(sender);
        }

        public void RestartFrame(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].RestartFrame = GetInt(sender);
        }
        public void Flag(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].Flag = GetInt(sender);
        }

        public void AnimDestX(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].DestX = GetFloat(sender);
        }

        public void AnimDestY(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].DestY = GetFloat(sender);
        }

        private void LoadParamsFrame() //adding controls to a groupbox for editing anim key frame
        {
            groupBox1.Text = (string)(listBox4.Items[listBox4.SelectedIndex]);
            groupBox1.Controls.Clear();
            Label frm = new Label();
            frm.Text = "Frame:";
            frm.Width = 150;
            frm.Location = new Point(10, 23);
            groupBox1.Controls.Add(frm);
            TextBox tbfrm = new TextBox();
            tbfrm.Location = new Point(160, 20);
            tbfrm.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].Frame;
            groupBox1.Controls.Add(tbfrm);
            Label ind = new Label();
            ind.Text = "Index:";
            ind.Width = 150;
            ind.Location = new Point(10, 53);
            groupBox1.Controls.Add(ind);
            TextBox tbind = new TextBox();
            tbind.Location = new Point(160, 50);
            tbind.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].Index;
            groupBox1.Controls.Add(tbind);
            Label rt = new Label();
            rt.Text = "Rotate:";
            rt.Width = 150;
            rt.Location = new Point(10, 83);
            groupBox1.Controls.Add(rt);
            TextBox tbrt = new TextBox();
            tbrt.Location = new Point(160, 80);
            tbrt.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].Rotate;
            groupBox1.Controls.Add(tbrt);
            Label scX = new Label();
            scX.Text = "ScaleX:";
            scX.Width = 150;
            scX.Location = new Point(10, 113);
            groupBox1.Controls.Add(scX);
            TextBox tbsX = new TextBox();
            tbsX.Location = new Point(160, 110);
            tbsX.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].ScaleX;
            groupBox1.Controls.Add(tbsX);
            Label scY = new Label();
            scY.Text = "ScaleY:";
            scY.Width = 150;
            scY.Location = new Point(10, 143);
            groupBox1.Controls.Add(scY);
            TextBox tbsY = new TextBox();
            tbsY.Location = new Point(160, 140);
            tbsY.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].ScaleY;
            groupBox1.Controls.Add(tbsY);
            Label trX = new Label();
            trX.Text = "TranslateX:";
            trX.Width = 150;
            trX.Location = new Point(10, 173);
            groupBox1.Controls.Add(trX);
            TextBox tbtX = new TextBox();
            tbtX.Location = new Point(160, 170);
            tbtX.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].TranslateX;
            groupBox1.Controls.Add(tbtX);
            Label trY = new Label();
            trY.Text = "TranslateY:";
            trY.Width = 150;
            trY.Location = new Point(10, 203);
            groupBox1.Controls.Add(trY);
            TextBox tbtY = new TextBox();
            tbtY.Location = new Point(160, 200);
            tbtY.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].TranslateY;
            groupBox1.Controls.Add(tbtY);
            Label att = new Label();
            att.Text = "Attribute:";
            att.Width = 150;
            att.Location = new Point(10, 233);
            groupBox1.Controls.Add(att);
            TextBox tbatt = new TextBox();
            tbatt.Location = new Point(160, 230);
            tbatt.Text = "" + manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].Attribute;
            groupBox1.Controls.Add(tbatt);
            tbfrm.TextChanged += Frame;
            tbind.TextChanged += Index;
            tbrt.TextChanged += Rotate;
            tbsX.TextChanged += ScaleX;
            tbsY.TextChanged += ScaleY;
            tbtX.TextChanged += TranslateX;
            tbtY.TextChanged += TranslateY;
            tbatt.TextChanged += Attribute;
        }

        //Setting anim key frame data

        public void Frame(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].Frame = GetInt(sender);
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }

        public void Index(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].Index = GetInt(sender);
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }

        public void Rotate(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].Rotate = GetFloat(sender);
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }

        public void ScaleX(object sender, EventArgs args)
        {
            float sc = GetFloat(sender);
            if (sc == 0)
            {
                id.Clear();
                return;
            }
            manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].ScaleX = sc;
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }

        public void ScaleY(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].ScaleY = GetFloat(sender);
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }

        public void TranslateX(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].TranslateX = GetFloat(sender);
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }

        public void TranslateY(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].TranslateY = GetFloat(sender);
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }

        public void Attribute(object sender, EventArgs args)
        {
            manager.ipnce.AnimList[listBox3.SelectedIndex].KeyFrames[listBox4.SelectedIndex].Attribute = (uint)(GetInt(sender));
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadParamsFrame();
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }

        private void LoadParamsParts() //adding controls to a groupbox for editing sprite part
        {
            groupBox1.Text = (string)(listBox2.Items[listBox2.SelectedIndex]);
            groupBox1.Controls.Clear();
            Label dsX = new Label();
            dsX.Text = "DestX:";
            dsX.Width = 150;
            dsX.Location = new Point(10, 23);
            groupBox1.Controls.Add(dsX);
            TextBox tbdX = new TextBox();
            tbdX.Location = new Point(160, 20);
            tbdX.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].DestX;
            groupBox1.Controls.Add(tbdX);
            Label dsY = new Label();
            dsY.Text = "DestY:";
            dsY.Width = 150;
            dsY.Location = new Point(10, 53);
            groupBox1.Controls.Add(dsY);
            TextBox tbdY = new TextBox();
            tbdY.Location = new Point(160, 50);
            tbdY.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].DestY;
            groupBox1.Controls.Add(tbdY);
            Label sx = new Label();
            sx.Text = "SrcX:";
            sx.Width = 150;
            sx.Location = new Point(10, 83);
            groupBox1.Controls.Add(sx);
            TextBox tbsx = new TextBox();
            tbsx.Location = new Point(160, 80);
            tbsx.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].SrcX;
            groupBox1.Controls.Add(tbsx);
            Label sy = new Label();
            sy.Text = "SrcY:";
            sy.Width = 150;
            sy.Location = new Point(10, 113);
            groupBox1.Controls.Add(sy);
            TextBox tbsy = new TextBox();
            tbsy.Location = new Point(160, 110);
            tbsy.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].SrcY;
            groupBox1.Controls.Add(tbsy);
            Label wid = new Label();
            wid.Text = "Width:";
            wid.Width = 150;
            wid.Location = new Point(10, 143);
            groupBox1.Controls.Add(wid);
            TextBox tbwd = new TextBox();
            tbwd.Location = new Point(160, 140);
            tbwd.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].Width;
            groupBox1.Controls.Add(tbwd);
            Label hei = new Label();
            hei.Text = "Height:";
            hei.Width = 150;
            hei.Location = new Point(10, 173);
            groupBox1.Controls.Add(hei);
            TextBox tbhg = new TextBox();
            tbhg.Location = new Point(160, 170);
            tbhg.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].Height;
            groupBox1.Controls.Add(tbhg);
            Label flg = new Label();
            flg.Text = "Flag:";
            flg.Width = 150;
            flg.Location = new Point(10, 203);
            groupBox1.Controls.Add(flg);
            TextBox tbfg = new TextBox();
            tbfg.Location = new Point(160, 200);
            tbfg.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].Flag;
            groupBox1.Controls.Add(tbfg);
            Label pri = new Label();
            pri.Text = "Priority:";
            pri.Width = 150;
            pri.Location = new Point(10, 233);
            groupBox1.Controls.Add(pri);
            TextBox tbpr = new TextBox();
            tbpr.Location = new Point(160, 230);
            tbpr.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].Priority;
            groupBox1.Controls.Add(tbpr);
            Label cpn = new Label();
            cpn.Text = "ColorPlteNum:";
            cpn.Width = 150;
            cpn.Location = new Point(10, 263);
            groupBox1.Controls.Add(cpn);
            TextBox tbcpn = new TextBox();
            tbcpn.Location = new Point(160, 260);
            tbcpn.Text = "" + manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].ColorPlteNum;
            groupBox1.Controls.Add(tbcpn);
            tbdX.TextChanged += PartDestX;
            tbdY.TextChanged += PartDestY;
            tbsx.TextChanged += SrcX;
            tbsy.TextChanged += SrcY;
            tbwd.TextChanged += PartWidth;
            tbhg.TextChanged += PartHeight;
            tbfg.TextChanged += PartFlag;
            tbpr.TextChanged += Priority;
            tbcpn.TextChanged += ColorPlteNum;
        }

        //Setting sprite part data

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadParamsParts();
            id.DrawPart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }

        public void PartDestX(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].DestX = GetFloat(sender);
            id.UpdatePart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }
        public void PartDestY(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].DestY = GetFloat(sender);
            id.UpdatePart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }
        public void SrcX(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].SrcX = GetFloat(sender);
            id.UpdatePart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }
        public void SrcY(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].SrcY = GetFloat(sender);
            id.UpdatePart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }
        public void PartWidth(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].Width = GetFloat(sender);
            id.UpdatePart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }
        public void PartHeight(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].Height = GetFloat(sender);
            id.UpdatePart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }

        public void PartFlag(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].Flag = GetInt(sender);
            id.UpdatePart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }

        public void Priority(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].Priority = GetByte(sender);
            id.UpdatePart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }

        public void ColorPlteNum(object sender, EventArgs args)
        {
            manager.ipnce.SpriteList[listBox1.SelectedIndex].Parts[listBox2.SelectedIndex].ColorPlteNum = GetByte(sender);
            id.UpdatePart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] inds = GetIndexes(checkedListBox1);
            if (inds == null) {
                trackBar1.Enabled = false; //Unselecting all animations
                button1.Enabled = false;
                trackBar1.Maximum = 10;
            }
            else
            {
                trackBar1.Enabled = true; //Selecting an animation that user wants to play
                button1.Enabled = true;
                int frs = manager.MaxFrames(inds);
                trackBar1.Maximum = frs;
            }
        }

        public int[] GetIndexes(CheckedListBox clb) //Getting selected indexes
        {
            if (clb.CheckedItems.Count == 0)
            {
                return null;
            }
            int[] ans = new int[clb.CheckedItems.Count];
            int ind = 0;
            for (int i = 0; i < clb.Items.Count; i++)
            {
                if (clb.GetItemChecked(i))
                {
                    ans[ind] = i;
                    ind++;
                }
            }
            return ans;
        }

        private void trackBar1_Scroll(object sender, EventArgs e) //drawing a frame after scrolling a track bar
        {
            int pos = trackBar1.Value;
            int[] anims = GetIndexes(checkedListBox1);
            id.DrawFrame(pos, anims);
            label11.Text = "Frame: "+ pos;
        }

        public async void Play() //playing an animation
        {
            int frames = trackBar1.Maximum;
            bool b = false;
            for (int i = 0; i < frames; i++)
            {
                if (!played)
                {
                    b = true;
                    break;
                }
                await Task.Delay(20); //delay for optimisation
                trackBar1.Value = i;
                trackBar1_Scroll(trackBar1, new EventArgs());
            }
            if (!b)
            {
                button2_Click(button2, new EventArgs()); //stopping animation
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            played = true;
            button1.Enabled = false;
            button2.Enabled = true;
            Play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            played = false;
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.Save();
        }

        private void saveAsAAI2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save AAI2 Ipnce As";
                sfd.Filter = "UnityEX MB file (*.114)|*.114|UABE MB file (*.dat)|*.dat|All files (*.*)|*.*";
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
            manager.SaveAsAAI2(filpath);
            MessageBox.Show("Don't forget to copy header bytes \nfrom Ipnce that you want to replace and\ninsert this bytes in the saved file.");
        }

        private void saveAsAAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save AAI Ipnce As";
                sfd.Filter = "UnityEX MB file (*.114)|*.114|UABE MB file (*.dat)|*.dat|All files (*.*)|*.*";
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
            manager.SaveAsAAI(filpath);
            MessageBox.Show("Don't forget to copy header bytes \nfrom Ipnce that you want to replace and\ninsert this bytes in the saved file.");
        }

        private void saveAsAJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save AJ Ipnce As";
                sfd.Filter = "UnityEX MB file (*.114)|*.114|UABE MB file (*.dat)|*.dat|All files (*.*)|*.*";
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
            manager.SaveAsAJ(filpath);
            MessageBox.Show("Don't forget to copy header bytes \nfrom Ipnce that you want to replace and\ninsert this bytes in the saved file.");
        }
    }
}
