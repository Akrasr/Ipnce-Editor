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
    internal class IpnceSpritePartsControl : ElementControl
    {
        SpriteParts obj;
        NitroObjectManager manager;

        public IpnceSpritePartsControl(GroupBox box) : base(box) { }
        public IpnceSpritePartsControl(GroupBox box, NitroObjectManager man) : base(box) { manager = man; }
        public IpnceSpritePartsControl(GroupBox box, SpriteParts o) : base(box) { obj = o; }
        public void SetAnim(SpriteParts o) { obj = o; }

        public override void SetEditedObject(object o)
        {
            obj = (SpriteParts)o;
        }

        public override void SetAllControls()
        {
            LoadParamsParts();
        }

        private void LoadParamsParts() //adding controls to a groupbox for editing sprite part
        {
            Label dsX = new Label();
            dsX.Text = "DestX:";
            dsX.Width = 150;
            dsX.Location = new Point(10, 23);
            groupBox1.Controls.Add(dsX);
            TextBox tbdX = new TextBox();
            tbdX.Location = new Point(160, 20);
            tbdX.Text = "" + obj.DestX;
            groupBox1.Controls.Add(tbdX);
            Label dsY = new Label();
            dsY.Text = "DestY:";
            dsY.Width = 150;
            dsY.Location = new Point(10, 53);
            groupBox1.Controls.Add(dsY);
            TextBox tbdY = new TextBox();
            tbdY.Location = new Point(160, 50);
            tbdY.Text = "" + obj.DestY;
            groupBox1.Controls.Add(tbdY);
            Label sx = new Label();
            sx.Text = "SrcX:";
            sx.Width = 150;
            sx.Location = new Point(10, 83);
            groupBox1.Controls.Add(sx);
            TextBox tbsx = new TextBox();
            tbsx.Location = new Point(160, 80);
            tbsx.Text = "" + obj.SrcX;
            groupBox1.Controls.Add(tbsx);
            Label sy = new Label();
            sy.Text = "SrcY:";
            sy.Width = 150;
            sy.Location = new Point(10, 113);
            groupBox1.Controls.Add(sy);
            TextBox tbsy = new TextBox();
            tbsy.Location = new Point(160, 110);
            tbsy.Text = "" + obj.SrcY;
            groupBox1.Controls.Add(tbsy);
            Label wid = new Label();
            wid.Text = "Width:";
            wid.Width = 150;
            wid.Location = new Point(10, 143);
            groupBox1.Controls.Add(wid);
            TextBox tbwd = new TextBox();
            tbwd.Location = new Point(160, 140);
            tbwd.Text = "" + obj.Width;
            groupBox1.Controls.Add(tbwd);
            Label hei = new Label();
            hei.Text = "Height:";
            hei.Width = 150;
            hei.Location = new Point(10, 173);
            groupBox1.Controls.Add(hei);
            TextBox tbhg = new TextBox();
            tbhg.Location = new Point(160, 170);
            tbhg.Text = "" + obj.Height;
            groupBox1.Controls.Add(tbhg);
            Label flg = new Label();
            flg.Text = "Flag:";
            flg.Width = 150;
            flg.Location = new Point(10, 203);
            groupBox1.Controls.Add(flg);
            TextBox tbfg = new TextBox();
            tbfg.Location = new Point(160, 200);
            tbfg.Text = "" + obj.Flag;
            groupBox1.Controls.Add(tbfg);
            Label pri = new Label();
            pri.Text = "Priority:";
            pri.Width = 150;
            pri.Location = new Point(10, 233);
            groupBox1.Controls.Add(pri);
            TextBox tbpr = new TextBox();
            tbpr.Location = new Point(160, 230);
            tbpr.Text = "" + obj.Priority;
            groupBox1.Controls.Add(tbpr);
            Label cpn = new Label();
            cpn.Text = "ColorPlteNum:";
            cpn.Width = 150;
            cpn.Location = new Point(10, 263);
            groupBox1.Controls.Add(cpn);
            TextBox tbcpn = new TextBox();
            tbcpn.Location = new Point(160, 260);
            tbcpn.Text = "" + obj.ColorPlteNum;
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

        public void PartDestX(object sender, EventArgs args)
        {
            obj.DestX = GetFloat(sender);
            UpdatePartView();
        }
        public void PartDestY(object sender, EventArgs args)
        {
            obj.DestY = GetFloat(sender);
            UpdatePartView();
        }
        public void SrcX(object sender, EventArgs args)
        {
            obj.SrcX = GetFloat(sender);
            UpdatePartView();
        }
        public void SrcY(object sender, EventArgs args)
        {
            obj.SrcY = GetFloat(sender);
            UpdatePartView();
        }
        public void PartWidth(object sender, EventArgs args)
        {
            obj.Width = GetFloat(sender);
            UpdatePartView();
        }
        public void PartHeight(object sender, EventArgs args)
        {
            obj.Height = GetFloat(sender);
            UpdatePartView();
        }

        public void PartFlag(object sender, EventArgs args)
        {
            obj.Flag = GetInt(sender);
            UpdatePartView();
        }

        public void Priority(object sender, EventArgs args)
        {
            obj.Priority = GetByte(sender);
            UpdatePartView();
        }

        public void ColorPlteNum(object sender, EventArgs args)
        {
            obj.ColorPlteNum = GetByte(sender);
            UpdatePartView();
        }

        public void UpdatePartView()
        {
            manager.ShowPart();
        }
    }
}
