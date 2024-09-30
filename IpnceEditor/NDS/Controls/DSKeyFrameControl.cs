using IpnceEditor.NDS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.Interfaces;

namespace IpnceEditor.NDS.Controls
{
    public class DSKeyFrameControl : ElementControl
    {
        DSKeyFrame obj;
        NitroObjectManager manager;

        public DSKeyFrameControl(GroupBox box) : base(box) { }
        public DSKeyFrameControl(GroupBox box, NitroObjectManager man) : base(box) { manager = man; }
        public DSKeyFrameControl(GroupBox box, DSKeyFrame o) : base(box) { obj = o; }
        public void SetFrame(DSKeyFrame o) { obj = o; }

        public override void SetEditedObject(object o)
        {
            obj = (DSKeyFrame)o;
        }

        public override void SetAllControls()
        {
            LoadParamsFrame();
        }
        private void LoadParamsFrame() //adding controls to a groupbox for editing anim key frame
        {
            Label frm = new Label();
            frm.Text = "Duration:";
            frm.Width = 150;
            frm.Location = new Point(10, 23);
            groupBox1.Controls.Add(frm);
            TextBox tbfrm = new TextBox();
            tbfrm.Location = new Point(160, 20);
            tbfrm.Text = "" + obj.duration;
            groupBox1.Controls.Add(tbfrm);
            tbfrm.TextChanged += Dur;
            switch (obj.type)
            {
                case 0:
                    LoadFrame0();
                    break;
                case 1:
                    LoadFrame1();
                    break;
                case 2:
                    LoadFrame2();
                    break;
            }
        }

        public void LoadFrame0()
        {
            IFrame fr = obj.fr;
            Label ind = new Label();
            ind.Text = "Index:";
            ind.Width = 150;
            ind.Location = new Point(10, 53);
            groupBox1.Controls.Add(ind);
            TextBox tbind = new TextBox();
            tbind.Location = new Point(160, 50);
            tbind.Text = "" + fr.index;
            groupBox1.Controls.Add(tbind);
            tbind.TextChanged += Index;
        }

        public void LoadFrame1()
        {
            IFrame fr = obj.fr;
            Label ind = new Label();
            ind.Text = "Index:";
            ind.Width = 150;
            ind.Location = new Point(10, 53);
            groupBox1.Controls.Add(ind);
            TextBox tbind = new TextBox();
            tbind.Location = new Point(160, 50);
            tbind.Text = "" + fr.index;
            groupBox1.Controls.Add(tbind);
            Label scX = new Label();
            scX.Text = "ScaleX:";
            scX.Width = 150;
            scX.Location = new Point(10, 83);
            groupBox1.Controls.Add(scX);
            TextBox tbsX = new TextBox();
            tbsX.Location = new Point(160, 80);
            tbsX.Text = "" + obj.GetSX();
            groupBox1.Controls.Add(tbsX);
            Label scY = new Label();
            scY.Text = "ScaleY:";
            scY.Width = 150;
            scY.Location = new Point(10, 113);
            groupBox1.Controls.Add(scY);
            TextBox tbsY = new TextBox();
            tbsY.Location = new Point(160, 110);
            tbsY.Text = "" + obj.GetSY();
            groupBox1.Controls.Add(tbsY);
            Label trX = new Label();
            trX.Text = "TranslateX:";
            trX.Width = 150;
            trX.Location = new Point(10, 143);
            groupBox1.Controls.Add(trX);
            TextBox tbtX = new TextBox();
            tbtX.Location = new Point(160, 140);
            tbtX.Text = "" + fr.px;
            groupBox1.Controls.Add(tbtX);
            Label trY = new Label();
            trY.Text = "TranslateY:";
            trY.Width = 150;
            trY.Location = new Point(10, 173);
            groupBox1.Controls.Add(trY);
            TextBox tbtY = new TextBox();
            tbtY.Location = new Point(160, 170);
            tbtY.Text = "" + fr.py;
            groupBox1.Controls.Add(tbtY);
            Label trz = new Label();
            trz.Text = "Rotz:";
            trz.Width = 150;
            trz.Location = new Point(10, 173);
            groupBox1.Controls.Add(trz);
            TextBox tbtz = new TextBox();
            tbtz.Location = new Point(160, 170);
            tbtz.Text = "" + fr.rotZ;
            groupBox1.Controls.Add(tbtz);
            tbind.TextChanged += Index;
            tbsX.TextChanged += SX;
            tbsY.TextChanged += SY;
            tbtX.TextChanged += PX;
            tbtY.TextChanged += PY;
            tbtz.TextChanged += Rotz;
        }

        public void LoadFrame2()
        {
            IFrame fr = obj.fr;
            Label ind = new Label();
            ind.Text = "Index:";
            ind.Width = 150;
            ind.Location = new Point(10, 53);
            groupBox1.Controls.Add(ind);
            TextBox tbind = new TextBox();
            tbind.Location = new Point(160, 50);
            tbind.Text = "" + fr.index;
            groupBox1.Controls.Add(tbind);
            Label scX = new Label();
            scX.Text = "TranslateX:";
            scX.Width = 150;
            scX.Location = new Point(10, 83);
            groupBox1.Controls.Add(scX);
            TextBox tbsX = new TextBox();
            tbsX.Location = new Point(160, 80);
            tbsX.Text = "" + fr.px;
            groupBox1.Controls.Add(tbsX);
            Label scY = new Label();
            scY.Text = "TranslateY:";
            scY.Width = 150;
            scY.Location = new Point(10, 113);
            groupBox1.Controls.Add(scY);
            TextBox tbsY = new TextBox();
            tbsY.Location = new Point(160, 110);
            tbsY.Text = "" + fr.py;
            groupBox1.Controls.Add(tbsY);
            tbind.TextChanged += Index;
            tbsX.TextChanged += PX;
            tbsY.TextChanged += PY;
        }

        public void Dur(object sender, EventArgs args)
        {
            obj.duration = (ushort)GetInt(sender);
            UpdateFrameView();
        }

        public void Index(object sender, EventArgs args)
        {
            obj.SetIndex(GetInt(sender));
            UpdateFrameView();
        }

        public void PX(object sender, EventArgs args)
        {
            obj.SetX(GetInt(sender));
            UpdateFrameView();
        }

        public void PY(object sender, EventArgs args)
        {
            obj.SetY(GetInt(sender));
            UpdateFrameView();
        }

        public void SX(object sender, EventArgs args)
        {
            obj.SetSX(GetInt(sender));
            UpdateFrameView();
        }

        public void SY(object sender, EventArgs args)
        {
            obj.SetSY(GetInt(sender));
            UpdateFrameView();
        }

        public void Rotz(object sender, EventArgs args)
        {
            obj.SetRotZ(GetInt(sender));
            UpdateFrameView();
        }

        public void UpdateFrameView()
        {
            manager.ShowFrame();
        }
    }
}
