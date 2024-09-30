using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.Interfaces;

namespace IpnceEditor.UnityIpnce.Controls
{
    public class IpnceAnimFrameControl : ElementControl
    {
        AnimKeyframe obj;
        NitroObjectManager manager;

        public IpnceAnimFrameControl(GroupBox box) : base(box) { }
        public IpnceAnimFrameControl(GroupBox box, NitroObjectManager man) : base(box) { manager = man; }
        public IpnceAnimFrameControl(GroupBox box, AnimKeyframe frame) : base(box) { obj = frame; }
        public void SetAnim(AnimKeyframe frame) { obj = frame; }

        public override void SetEditedObject(object o)
        {
            obj = (AnimKeyframe)o;
        }

        public override void SetAllControls()
        {
            LoadParamsFrame();
        }
        private void LoadParamsFrame() //adding controls to a groupbox for editing anim key frame
        {
            Label frm = new Label();
            frm.Text = "Frame:";
            frm.Width = 150;
            frm.Location = new Point(10, 23);
            groupBox1.Controls.Add(frm);
            TextBox tbfrm = new TextBox();
            tbfrm.Location = new Point(160, 20);
            tbfrm.Text = "" + obj.Frame;
            groupBox1.Controls.Add(tbfrm);
            Label ind = new Label();
            ind.Text = "Index:";
            ind.Width = 150;
            ind.Location = new Point(10, 53);
            groupBox1.Controls.Add(ind);
            TextBox tbind = new TextBox();
            tbind.Location = new Point(160, 50);
            tbind.Text = "" + obj.Index;
            groupBox1.Controls.Add(tbind);
            Label rt = new Label();
            rt.Text = "Rotate:";
            rt.Width = 150;
            rt.Location = new Point(10, 83);
            groupBox1.Controls.Add(rt);
            TextBox tbrt = new TextBox();
            tbrt.Location = new Point(160, 80);
            tbrt.Text = "" + obj.Rotate;
            groupBox1.Controls.Add(tbrt);
            Label scX = new Label();
            scX.Text = "ScaleX:";
            scX.Width = 150;
            scX.Location = new Point(10, 113);
            groupBox1.Controls.Add(scX);
            TextBox tbsX = new TextBox();
            tbsX.Location = new Point(160, 110);
            tbsX.Text = "" + obj.ScaleX;
            groupBox1.Controls.Add(tbsX);
            Label scY = new Label();
            scY.Text = "ScaleY:";
            scY.Width = 150;
            scY.Location = new Point(10, 143);
            groupBox1.Controls.Add(scY);
            TextBox tbsY = new TextBox();
            tbsY.Location = new Point(160, 140);
            tbsY.Text = "" + obj.ScaleY;
            groupBox1.Controls.Add(tbsY);
            Label trX = new Label();
            trX.Text = "TranslateX:";
            trX.Width = 150;
            trX.Location = new Point(10, 173);
            groupBox1.Controls.Add(trX);
            TextBox tbtX = new TextBox();
            tbtX.Location = new Point(160, 170);
            tbtX.Text = "" + obj.TranslateX;
            groupBox1.Controls.Add(tbtX);
            Label trY = new Label();
            trY.Text = "TranslateY:";
            trY.Width = 150;
            trY.Location = new Point(10, 203);
            groupBox1.Controls.Add(trY);
            TextBox tbtY = new TextBox();
            tbtY.Location = new Point(160, 200);
            tbtY.Text = "" + obj.TranslateY;
            groupBox1.Controls.Add(tbtY);
            Label att = new Label();
            att.Text = "Attribute:";
            att.Width = 150;
            att.Location = new Point(10, 233);
            groupBox1.Controls.Add(att);
            TextBox tbatt = new TextBox();
            tbatt.Location = new Point(160, 230);
            tbatt.Text = "" + obj.Attribute;
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
            obj.Frame = GetInt(sender);
            UpdateFrameView();
        }

        public void Index(object sender, EventArgs args)
        {
            obj.Index = GetInt(sender);
            UpdateFrameView();
        }

        public void Rotate(object sender, EventArgs args)
        {
            obj.Rotate = GetFloat(sender);
            UpdateFrameView();
        }

        public void ScaleX(object sender, EventArgs args)
        {
            float sc = GetFloat(sender);
            if (sc == 0)
            {
                //id.Clear();
                return;
            }
            obj.ScaleX = sc;
            UpdateFrameView();
        }

        public void ScaleY(object sender, EventArgs args)
        {
            obj.ScaleY = GetFloat(sender);
            UpdateFrameView();
        }

        public void TranslateX(object sender, EventArgs args)
        {
            obj.TranslateX = GetFloat(sender);
            UpdateFrameView();
        }

        public void TranslateY(object sender, EventArgs args)
        {
            obj.TranslateY = GetFloat(sender);
            UpdateFrameView();
        }

        public void Attribute(object sender, EventArgs args)
        {
            obj.Attribute = (uint)(GetInt(sender));
            UpdateFrameView();
        }

        public void UpdateFrameView()
        {
            manager.ShowFrame();
        }
    }
}
