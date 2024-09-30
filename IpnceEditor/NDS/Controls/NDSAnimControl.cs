using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.NDS;
using IpnceEditor.Interfaces;

namespace IpnceEditor.NDS.Controls
{
    internal class NDSAnimControl : ElementControl
    {
        DSSeq obj;

        public NDSAnimControl(GroupBox box) : base(box) { }
        public NDSAnimControl(GroupBox box, DSSeq anim) : base(box) { obj = anim; }
        public void SetAnim(DSSeq anim) { obj = anim; }

        public override void SetEditedObject(object o)
        {
            obj = (DSSeq)o;
        }

        public override void SetAllControls()
        {
            LoadParamsAnim();
        }

        private void LoadParamsAnim() //adding controls to a groupbox for editing anim
        {
            Label kes = new Label();
            kes.Text = "First frame:";
            kes.Width = 150;
            kes.Location = new Point(10, 23);
            groupBox1.Controls.Add(kes);
            TextBox tbkes = new TextBox();
            tbkes.Location = new Point(160, 20);
            tbkes.Text = "" + obj.firstframe;
            groupBox1.Controls.Add(tbkes);
            Label tfs = new Label();
            tfs.Text = "Frame type:";
            tfs.Width = 150;
            tfs.Location = new Point(10, 53);
            groupBox1.Controls.Add(tfs);
            TextBox tbtfs = new TextBox();
            tbtfs.Location = new Point(160, 50);
            tbtfs.Text = "" + obj.frametype;
            groupBox1.Controls.Add(tbtfs);
            Label rf = new Label();
            rf.Text = "Seq type:";
            rf.Width = 150;
            rf.Location = new Point(10, 83);
            groupBox1.Controls.Add(rf);
            TextBox tbrf = new TextBox();
            tbrf.Location = new Point(160, 80);
            tbrf.Text = "" + obj.seqtype;
            groupBox1.Controls.Add(tbrf);
            Label dstX = new Label();
            dstX.Text = "Seq mode:";
            dstX.Width = 150;
            dstX.Location = new Point(10, 113);
            groupBox1.Controls.Add(dstX);
            TextBox tbX = new TextBox();
            tbX.Location = new Point(160, 110);
            tbX.Text = "" + obj.seqmode;
            groupBox1.Controls.Add(tbX);
            tbkes.TextChanged += FirstFram;
            tbtfs.TextChanged += FramType;
            tbrf.TextChanged += SeqType;
            tbX.TextChanged += SeqMode;
        }

        public void FirstFram(object sender, EventArgs args)
        {
            obj.firstframe = (ushort)GetInt(sender);
        }

        public void FramType(object sender, EventArgs args)
        {
            obj.ChangeTypes((ushort)GetInt(sender));
        }

        public void SeqType(object sender, EventArgs args)
        {
            obj.seqtype = (ushort)GetInt(sender);
        }

        public void SeqMode(object sender, EventArgs args)
        {
            obj.seqmode = (ushort)GetInt(sender);
        }
    }
}
