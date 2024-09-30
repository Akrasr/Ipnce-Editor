using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.Interfaces;
using IpnceEditor.NDS.ImageManagers;
using IpnceEditor.UnityIpnce;

namespace IpnceEditor.GUI
{
    public partial class Form1 : Form
    {
        Manager manager;
        IpnceDrawer id;
        Graphics gr;
        bool played;
        public static string AnimName = "";
        public Form1()
        {
            InitializeComponent();
            manager = new Manager();
            DisableAll();
            gr = pictureBox1.CreateGraphics();
            this.Text = "Ipnce Editor";
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            saveAsAJToolStripMenuItem.Enabled = false;
            saveAsDSToolStripMenuItem.Enabled = false;
            saveAsAAI2ToolStripMenuItem.Enabled = false;
            saveAsAAIC1ToolStripMenuItem.Enabled = false;
            saveAsAAIC2ToolStripMenuItem.Enabled = false;
            saveAsAAIToolStripMenuItem.Enabled = false;
            backgroundToolStripMenuItem.Enabled = false;
            addToolStripMenuItem.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
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
                openFileDialog.Filter = "Unity file (*.dat, *.114, *.ipnce)|*.dat;*.114;*.ipnce|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ipncePath = openFileDialog.FileName;
                }
                else return;
            }
            NitroObjectManager man = manager.objmanager;
            AnimationType type = manager.animationtype;
            manager.SetIpnce(ipncePath);
            /*try
            {
                manager.SetIpnce(ipncePath); //Attempt to load Ipnce
            } catch (Exception e)
            {
                MessageBox.Show("The file is invalid " + e.Message);
                manager.objmanager = man;
                manager.animationtype = type;
            }*/
            SetMainObjects();
            manager.SetGroupBox(groupBox1);
            saveToolStripMenuItem.Enabled = true; //Enabling save buttons
            saveAsToolStripMenuItem.Enabled = true;
            saveAsDSToolStripMenuItem.Enabled = true;
            saveAsAJToolStripMenuItem.Enabled = true;
            saveAsAAI2ToolStripMenuItem.Enabled = true;
            saveAsAAIToolStripMenuItem.Enabled = true;
            saveAsAAIC1ToolStripMenuItem.Enabled = true;
            saveAsAAIC2ToolStripMenuItem.Enabled = true;
            addToolStripMenuItem.Enabled = true;
            spriteToolStripMenuItem.Enabled = true;
            spritePartToolStripMenuItem.Enabled = false;
            animToolStripMenuItem.Enabled = true;
            animKeyframeToolStripMenuItem.Enabled = false;
            backgroundToolStripMenuItem.Enabled = true;
            button3.Enabled = true;
            button6.Enabled = true;
        }

        public void SetMainObjects()
        {
            EnableAll();
            id = new IpnceDrawer(gr, manager.GetImageManager(), trackBar1);
            UpdateData();
            id.ShowTest();
        }

        private void SaveScreenshot()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"PNG|*.png" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    id.SaveScreenshot(pictureBox1, saveFileDialog.FileName);
                }
            }
        }

        private void GetScreenshot()
        {
            using (OpenFileDialog saveFileDialog = new OpenFileDialog() { Filter = @"PNG|*.png" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    id.GetScreenshot(saveFileDialog.FileName);
                }
            }
        }

        private void SaveGIF()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"GIF|*.gif" })
            {
                saveFileDialog.FileName = AnimName + ".gif";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {

                    int[] inds = GetIndexes(checkedListBox1);
                    int frs = manager.MaxFrames(inds);
                    id.SaveGIF(inds, frs, saveFileDialog.FileName, false);
                    //id.SaveScreenshot(pictureBox1, saveFileDialog.FileName);
                }
            }
        }

        private void SaveSeq()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"Webp|*.webp" })
            {
                saveFileDialog.FileName = AnimName + ".webp";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {

                    int[] inds = GetIndexes(checkedListBox1);
                    int frs = manager.MaxFrames(inds);
                    id.SaveGIF(inds, frs, saveFileDialog.FileName, true);
                    //id.SaveScreenshot(pictureBox1, saveFileDialog.FileName);
                }
            }
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
            //Ipnce ip = manager.ipnce; //Showing Ipnce Data on the screen
            this.checkBox1.Checked = manager.HDCheck();
            this.checkBox2.Checked = manager.GetUsePalette();
            this.checkBox3.Checked = manager.GetOffScreenRendering();
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.AddRange(manager.GetAtlasList());
            this.label5.Text = "ColorPaletteNum: " + manager.GetColorPaletteNum();
            //Texture2D plt = ip.ColorPalette;
            this.label2.Text = "Param 1: ";
            this.label3.Text = "Param 2: ";
            this.label4.Text = "Param 3: ";
            this.label6.Text = "Param 1: ";// + plt.in1;
            this.label7.Text = "Param 2: ";// + plt.in2;
            this.label8.Text = "Param 3: ";// + plt.in3;
        }

        private void UpdateData()
        {
            UpdateIpnceData();
            LoadAnimList();
            LoadSpriteList();
            this.Text = "Ipnce Editor " + manager.GetIpnceName();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = comboBox1.SelectedIndex;
            /*Texture2D plt = this.manager.;
            this.label2.Text = "Param 1: " + plt.in1;
            this.label3.Text = "Param 2: " + plt.in2;
            this.label4.Text = "Param 3: " + plt.in3;*/
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

        private void AddSprite()
        {
            manager.AddSprite();
            LoadSpriteList();
            id.AddSprite();
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void AddSpriteParts()
        {
            int ind = listBox1.SelectedIndex;
            manager.AddSpriteParts(ind);
            id.AddSpriteParts(ind);
            listBox1_SelectedIndexChanged(new object(), new EventArgs());
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
        }

        private void AddAnim()
        {
            manager.AddAnim();
            LoadAnimList();
            listBox3.SelectedIndex = listBox3.Items.Count - 1;
        }

        private void AddAnimKeyframe()
        {
            int ind = listBox3.SelectedIndex;
            manager.AddAnimKeyframe(ind);
            listBox3_SelectedIndexChanged(new object(), new EventArgs());
            listBox4.SelectedIndex = listBox4.Items.Count - 1;
        }

        private void SetBackgroundImage()
        {
            string bgPath = String.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open Background Image";
                openFileDialog.Filter = "Image files (*.png, *.jpg)|*.png;*.jpg|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bgPath = openFileDialog.FileName;
                }
            }
            try
            {
                Image img = Image.FromFile(bgPath); //Attempt to load an image
                img.Dispose();
            }
            catch
            {
                MessageBox.Show("The file is invalid");
                return;
            }
            id.SetBackgroundImage(Image.FromFile(bgPath));
        }

        private void ResetBackgroundImage()
        {
            id.ResetBackgroundImage();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = listBox1.SelectedIndex; //showing sprite data
            listBox2.Items.Clear();
            listBox2.Items.AddRange(manager.GetSpritePartsList(ind));
            manager.SetSpriteIndex(ind);
            groupBox1.Controls.Clear(); //adding controls to a groupbox for editing sprite
            groupBox1.Text = (string)(listBox1.Items[listBox1.SelectedIndex]);
            manager.MakeControls(0);
            id.DrawSpriteCl(ind);
            spritePartToolStripMenuItem.Enabled = true;
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = listBox3.SelectedIndex; //showing anim data
            listBox4.Items.Clear();
            listBox4.Items.AddRange(manager.GetAnimKeyList(ind));
            manager.SetAnimIndex(ind);
            groupBox1.Text = (string)(listBox3.Items[listBox3.SelectedIndex]);
            groupBox1.Controls.Clear();
            manager.MakeControls(2);
            //LoadParamsAnim();
            animKeyframeToolStripMenuItem.Enabled = true;
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Text = (string)(listBox4.Items[listBox4.SelectedIndex]);
            groupBox1.Controls.Clear();
            manager.SetAnimFrameIndex(listBox4.SelectedIndex);
            manager.MakeControls(3);
            id.DrawCertainFrame(listBox3.SelectedIndex, listBox4.SelectedIndex);
        }
        //Setting sprite part data

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Text = (string)(listBox2.Items[listBox2.SelectedIndex]);
            groupBox1.Controls.Clear();
            manager.SetSpritePartIndex(listBox2.SelectedIndex);
            manager.MakeControls(1);
            id.DrawPart(listBox1.SelectedIndex, listBox2.SelectedIndex);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] inds = GetIndexes(checkedListBox1);
            if (inds == null) {
                AnimName = manager.GetIpnceName();
                trackBar1.Enabled = false; //Unselecting all animations
                button1.Enabled = false;
                trackBar1.Maximum = 10;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
            }
            else
            {
                AnimName = manager.GetIpnceName() + "[" + inds[0] + "]";
                trackBar1.Enabled = true; //Selecting an animation that user wants to play
                button1.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
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
            for (int i = 0; i <= frames; i++)
            {
                if (!played)
                {
                    b = true;
                    return;
                }
                await Task.Delay(20); //delay for optimisation
                trackBar1.Value = i;
                trackBar1_Scroll(trackBar1, new EventArgs());
            }
            if (id.GetLoop(GetIndexes(checkedListBox1)))
            {
                Play();
                b = true;
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

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.SaveAs();
        }

        private void saveAsAAI2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.SaveAsAAI2();
        }

        private void saveAsAAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.SaveAsAAI();
        }

        private void saveAsAJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.SaveAsAJ();
        }

        private void saveAsAAIC2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.SaveAsCollectionIpnce();
        }

        private void saveAsAAIC1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.SaveAsCollectionAAI1Ipnce();
        }

        private void spriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSprite();
        }

        private void spritePartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSpriteParts();
        }

        private void animToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddAnim();
        }

        private void animKeyframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddAnimKeyframe();
        }

        private void setBackgroundImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBackgroundImage();
        }

        private void resetBackgroundImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetBackgroundImage();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveScreenshot();
        }

        private void setGreenScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            id.SetGreenScreenBackground();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveGIF();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveSeq();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            GetScreenshot();
        }

        private void checkBox4_Changed(object sender, EventArgs e)
        {
            NDSImageManager.UpdateTransparency(checkBox4.Checked);
        }

        private void saveAsDSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.SaveAsDS();
        }
    }
}
