﻿
namespace IpnceEditor.GUI
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsAAI2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsAAIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsAJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsDSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsAAIC1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsAAIC2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spritePartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animKeyframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setBackgroundImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetBackgroundImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setGreenScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.addToolStripMenuItem,
            this.backgroundToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1213, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAsAAI2ToolStripMenuItem,
            this.saveAsAAIToolStripMenuItem,
            this.saveAsAJToolStripMenuItem,
            this.saveAsAAIC1ToolStripMenuItem,
            this.saveAsAAIC2ToolStripMenuItem,
            this.saveAsDSToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveAsAAI2ToolStripMenuItem
            // 
            this.saveAsAAI2ToolStripMenuItem.Name = "saveAsAAI2ToolStripMenuItem";
            this.saveAsAAI2ToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsAAI2ToolStripMenuItem.Text = "Save As AAI2 Ipnce";
            this.saveAsAAI2ToolStripMenuItem.Click += new System.EventHandler(this.saveAsAAI2ToolStripMenuItem_Click);
            // 
            // saveAsAAIToolStripMenuItem
            // 
            this.saveAsAAIToolStripMenuItem.Name = "saveAsAAIToolStripMenuItem";
            this.saveAsAAIToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsAAIToolStripMenuItem.Text = "Save As AAI Ipnce";
            this.saveAsAAIToolStripMenuItem.Click += new System.EventHandler(this.saveAsAAIToolStripMenuItem_Click);
            // 
            // saveAsAJToolStripMenuItem
            // 
            this.saveAsAJToolStripMenuItem.Name = "saveAsAJToolStripMenuItem";
            this.saveAsAJToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsAJToolStripMenuItem.Text = "Save As AJ Ipnce";
            this.saveAsAJToolStripMenuItem.Click += new System.EventHandler(this.saveAsAJToolStripMenuItem_Click);
            // 
            // saveAsAAIC1ToolStripMenuItem
            // 
            this.saveAsAAIC1ToolStripMenuItem.Name = "saveAsAAIC1ToolStripMenuItem";
            this.saveAsAAIC1ToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsAAIC1ToolStripMenuItem.Text = "Save As AAIC1 Ipnce";
            this.saveAsAAIC1ToolStripMenuItem.Click += new System.EventHandler(this.saveAsAAIC1ToolStripMenuItem_Click);
            // 
            // saveAsAAIC2ToolStripMenuItem
            // 
            this.saveAsAAIC2ToolStripMenuItem.Name = "saveAsAAIC2ToolStripMenuItem";
            this.saveAsAAIC2ToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsAAIC2ToolStripMenuItem.Text = "Save As AAIC2 Ipnce";
            this.saveAsAAIC2ToolStripMenuItem.Click += new System.EventHandler(this.saveAsAAIC2ToolStripMenuItem_Click);
            // 
            // SaveAsDSStripMenuItem
            // 
            this.saveAsDSToolStripMenuItem.Name = "saveAsDSToolStripMenuItem";
            this.saveAsDSToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsDSToolStripMenuItem.Text = "Save As DS cell";
            this.saveAsDSToolStripMenuItem.Click += new System.EventHandler(this.saveAsDSToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spriteToolStripMenuItem,
            this.spritePartToolStripMenuItem,
            this.animToolStripMenuItem,
            this.animKeyframeToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // spriteToolStripMenuItem
            // 
            this.spriteToolStripMenuItem.Name = "spriteToolStripMenuItem";
            this.spriteToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.spriteToolStripMenuItem.Text = "Sprite";
            this.spriteToolStripMenuItem.Click += new System.EventHandler(this.spriteToolStripMenuItem_Click);
            // 
            // spritePartToolStripMenuItem
            // 
            this.spritePartToolStripMenuItem.Name = "spritePartToolStripMenuItem";
            this.spritePartToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.spritePartToolStripMenuItem.Text = "Sprite Part";
            this.spritePartToolStripMenuItem.Click += new System.EventHandler(this.spritePartToolStripMenuItem_Click);
            // 
            // animToolStripMenuItem
            // 
            this.animToolStripMenuItem.Name = "animToolStripMenuItem";
            this.animToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.animToolStripMenuItem.Text = "Anim";
            this.animToolStripMenuItem.Click += new System.EventHandler(this.animToolStripMenuItem_Click);
            // 
            // animKeyframeToolStripMenuItem
            // 
            this.animKeyframeToolStripMenuItem.Name = "animKeyframeToolStripMenuItem";
            this.animKeyframeToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.animKeyframeToolStripMenuItem.Text = "Anim Keyframe";
            this.animKeyframeToolStripMenuItem.Click += new System.EventHandler(this.animKeyframeToolStripMenuItem_Click);
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setBackgroundImageToolStripMenuItem,
            this.resetBackgroundImageToolStripMenuItem,
            this.setGreenScreenToolStripMenuItem});
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.backgroundToolStripMenuItem.Text = "Background";
            // 
            // setBackgroundImageToolStripMenuItem
            // 
            this.setBackgroundImageToolStripMenuItem.Name = "setBackgroundImageToolStripMenuItem";
            this.setBackgroundImageToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.setBackgroundImageToolStripMenuItem.Text = "Set Background Image";
            this.setBackgroundImageToolStripMenuItem.Click += new System.EventHandler(this.setBackgroundImageToolStripMenuItem_Click);
            // 
            // resetBackgroundImageToolStripMenuItem
            // 
            this.resetBackgroundImageToolStripMenuItem.Name = "resetBackgroundImageToolStripMenuItem";
            this.resetBackgroundImageToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.resetBackgroundImageToolStripMenuItem.Text = "Reset Background Image";
            this.resetBackgroundImageToolStripMenuItem.Click += new System.EventHandler(this.resetBackgroundImageToolStripMenuItem_Click);
            // 
            // setGreenScreenToolStripMenuItem
            // 
            this.setGreenScreenToolStripMenuItem.Name = "setGreenScreenToolStripMenuItem";
            this.setGreenScreenToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.setGreenScreenToolStripMenuItem.Text = "Set Green Screen";
            this.setGreenScreenToolStripMenuItem.Click += new System.EventHandler(this.setGreenScreenToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(518, 48);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(683, 528);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 48);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(136, 147);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(12, 201);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(136, 147);
            this.listBox2.TabIndex = 3;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(12, 354);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(136, 147);
            this.listBox3.TabIndex = 4;
            this.listBox3.SelectedIndexChanged += new System.EventHandler(this.listBox3_SelectedIndexChanged);
            // 
            // listBox4
            // 
            this.listBox4.FormattingEnabled = true;
            this.listBox4.Location = new System.Drawing.Point(12, 507);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(136, 147);
            this.listBox4.TabIndex = 5;
            this.listBox4.SelectedIndexChanged += new System.EventHandler(this.listBox4_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(171, 49);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(50, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "IsHD";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(171, 72);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(110, 17);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "IsUseColorPalette";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Enabled = false;
            this.checkBox3.Location = new System.Drawing.Point(171, 95);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(131, 17);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.Text = "IsOffScreenRendering";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(171, 136);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "SpriteAtlas";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Param 1: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(234, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Param 2: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(307, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Param 3: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(168, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "ColorPaletteNum: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(168, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Param 1:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(234, 237);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Param 2:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(307, 237);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Param 3:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(168, 214);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "ColorPalette:";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(171, 255);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 292);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(171, 563);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 91);
            this.button1.TabIndex = 20;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(261, 563);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 91);
            this.button2.TabIndex = 21;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(366, 590);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(120, 64);
            this.checkedListBox1.TabIndex = 0;
            this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(363, 563);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "VisibleAnims:";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(518, 609);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(683, 45);
            this.trackBar1.TabIndex = 23;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(515, 590);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Frame: 0";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(417, 49);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(69, 40);
            this.button3.TabIndex = 25;
            this.button3.Text = "Save Screenshot";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(417, 120);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(69, 47);
            this.button4.TabIndex = 26;
            this.button4.Text = "Save GIF";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(417, 197);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(69, 47);
            this.button5.TabIndex = 27;
            this.button5.Text = "Save webp";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            this.button6.Location = new System.Drawing.Point(340, 49);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(69, 47);
            this.button6.TabIndex = 28;
            this.button6.Text = "Update With Screenshot";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(305, 135);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(135, 20);
            this.checkBox4.TabIndex = 28;
            this.checkBox4.Text = "DS Transparency";
            this.checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox4_Changed;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 669);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.listBox4);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.checkBox4);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsAAI2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsAAIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsAJToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsDSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsAAIC1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsAAIC2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spritePartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem animToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem animKeyframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setBackgroundImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetBackgroundImageToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem setGreenScreenToolStripMenuItem;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.CheckBox checkBox4;
    }
}

