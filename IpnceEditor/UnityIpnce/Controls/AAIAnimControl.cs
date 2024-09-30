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
    internal class AAIAnimControl : ElementControl
    {
        AAIAnim obj;

        public AAIAnimControl(GroupBox box) : base(box) { }
        public AAIAnimControl(GroupBox box, AAIAnim anim) : base(box) { obj = anim; }
        public void SetAnim(AAIAnim anim) { obj = anim; }

        public override void SetEditedObject(object o)
        {
            obj = (AAIAnim)o;
        }

        public override void SetAllControls()
        {
            LoadParamsAnim();
        }

        private void LoadParamsAnim() //adding controls to a groupbox for editing anim
        {
            Label tfs = new Label();
            tfs.Text = "TotalFrameSize:";
            tfs.Width = 150;
            tfs.Location = new Point(10, 53);
            groupBox1.Controls.Add(tfs);
            TextBox tbtfs = new TextBox();
            tbtfs.Location = new Point(160, 50);
            tbtfs.Text = "" + obj.TotalFrameSize;
            groupBox1.Controls.Add(tbtfs);
            Label rf = new Label();
            rf.Text = "RestartFrame:";
            rf.Width = 150;
            rf.Location = new Point(10, 83);
            groupBox1.Controls.Add(rf);
            TextBox tbrf = new TextBox();
            tbrf.Location = new Point(160, 80);
            tbrf.Text = "" + obj.RestartFrame;
            groupBox1.Controls.Add(tbrf);
            Label dstX = new Label();
            dstX.Text = "DestX:";
            dstX.Width = 150;
            dstX.Location = new Point(10, 113);
            groupBox1.Controls.Add(dstX);
            TextBox tbX = new TextBox();
            tbX.Location = new Point(160, 110);
            tbX.Text = "" + obj.DestX;
            groupBox1.Controls.Add(tbX);
            Label dstY = new Label();
            dstY.Text = "DestY:";
            dstY.Width = 150;
            dstY.Location = new Point(10, 143);
            groupBox1.Controls.Add(dstY);
            TextBox tbY = new TextBox();
            tbY.Location = new Point(160, 140);
            tbY.Text = "" + obj.DestY;
            groupBox1.Controls.Add(tbY);
            Label flag = new Label();
            flag.Text = "Flag:";
            flag.Width = 150;
            flag.Location = new Point(10, 173);
            groupBox1.Controls.Add(flag);
            TextBox tbflag = new TextBox();
            tbflag.Location = new Point(160, 170);
            tbflag.Text = "" + obj.Flag;
            groupBox1.Controls.Add(tbflag);
            tbtfs.TextChanged += TotalFrameSize;
            tbrf.TextChanged += RestartFrame;
            tbX.TextChanged += AnimDestX;
            tbY.TextChanged += AnimDestY;
            tbflag.TextChanged += Flag;
        }

        //Setting anim data

        public void TotalFrameSize(object sender, EventArgs args)
        {
            obj.TotalFrameSize = GetInt(sender);
        }

        public void RestartFrame(object sender, EventArgs args)
        {
            obj.RestartFrame = GetInt(sender);
        }
        public void Flag(object sender, EventArgs args)
        {
            obj.Flag = GetInt(sender);
        }

        public void AnimDestX(object sender, EventArgs args)
        {
            obj.DestX = GetFloat(sender);
        }

        public void AnimDestY(object sender, EventArgs args)
        {
            obj.DestY = GetFloat(sender);
        }
    }
}
