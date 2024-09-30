using IpnceEditor.NDS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.Interfaces;
using IpnceEditor.NDS.ObjectManagers;

namespace IpnceEditor.NDS.Controls
{
    public class OAMControl : ElementControl
    {
        Nitro_OAM obj;
        NDSCellManager manager;
        TextBox rotscatb;
        TextBox indextb;
        ComboBox fobo;

        public static string[] forms = new string[]
        {
            "8x8",
            "16x16",
            "32x32",
            "64x64",
            "16x8",
            "32x8",
            "32x16",
            "64x32",
            "8x16",
            "8x32",
            "16x32",
            "32x64"
        };

        public OAMControl(GroupBox box) : base(box) { }
        public OAMControl(GroupBox box, NDSCellManager man) : base(box) { manager = man; }
        public OAMControl(GroupBox box, Nitro_OAM o) : base(box) { obj = o; }
        public void SetOAM(Nitro_OAM o) { obj = o; }

        public override void SetEditedObject(object o)
        {
            obj = (Nitro_OAM)o;
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
            tbdX.Text = "" + obj.x;
            groupBox1.Controls.Add(tbdX);
            Label dsY = new Label();
            dsY.Text = "DestY:";
            dsY.Width = 150;
            dsY.Location = new Point(10, 53);
            groupBox1.Controls.Add(dsY);
            TextBox tbdY = new TextBox();
            tbdY.Location = new Point(160, 50);
            tbdY.Text = "" + obj.y;
            groupBox1.Controls.Add(tbdY);
            Label sx = new Label();
            sx.Text = "Src:";
            sx.Width = 150;
            sx.Location = new Point(10, 83);
            groupBox1.Controls.Add(sx);
            TextBox tbsx = new TextBox();
            tbsx.Location = new Point(160, 80);
            tbsx.Text = "" + obj.cch;
            groupBox1.Controls.Add(tbsx);
            Label sy = new Label();
            sy.Text = "Rotsca:";
            sy.Width = 150;
            sy.Location = new Point(10, 113);
            groupBox1.Controls.Add(sy);
            TextBox tbsy = new TextBox();
            tbsy.Location = new Point(160, 110);
            tbsy.Text = "" + obj.rotsca;
            groupBox1.Controls.Add(tbsy);
            Label wid = new Label();
            wid.Text = "Form:";
            wid.Width = 150;
            wid.Location = new Point(10, 143);
            groupBox1.Controls.Add(wid);
            ComboBox formbox = new ComboBox();
            formbox.Location = new Point(160, 143);
            formbox.Items.AddRange(forms);
            formbox.SelectedIndex = obj.shape * 4 + obj.size;
            formbox.Text = (string)formbox.Items[obj.shape * 4 + obj.size];
            groupBox1.Controls.Add(formbox);
            Button addtoch = new Button();
            addtoch.Location = new Point(160, 170);
            addtoch.Width = 150;
            addtoch.Text = "Add blank tile";
            groupBox1.Controls.Add(addtoch);
            Label flg = new Label();
            flg.Text = "Mode:";
            flg.Width = 150;
            flg.Location = new Point(10, 203);
            groupBox1.Controls.Add(flg);
            TextBox tbfg = new TextBox();
            tbfg.Location = new Point(160, 200);
            tbfg.Text = "" + obj.mode;
            groupBox1.Controls.Add(tbfg);
            Label pri = new Label();
            pri.Text = "Priority:";
            pri.Width = 150;
            pri.Location = new Point(10, 233);
            groupBox1.Controls.Add(pri);
            TextBox tbpr = new TextBox();
            tbpr.Location = new Point(160, 230);
            tbpr.Text = "" + obj.prio;
            groupBox1.Controls.Add(tbpr);
            Label cpn = new Label();
            cpn.Text = "ColorPlteNum:";
            cpn.Width = 150;
            cpn.Location = new Point(10, 263);
            groupBox1.Controls.Add(cpn);
            TextBox tbcpn = new TextBox();
            tbcpn.Location = new Point(160, 260);
            tbcpn.Text = "" + obj.pal;
            groupBox1.Controls.Add(tbcpn);
            tbdX.TextChanged += OamX;
            tbdY.TextChanged += OamY;
            tbsx.TextChanged += OamSrc;
            indextb = tbsx;
            tbsy.TextChanged += OamRotsca;
            rotscatb = tbsy;
            formbox.SelectedIndexChanged += ChangeForm;
            fobo = formbox;
            addtoch.Click += AddToChar;
            tbfg.TextChanged += OamMode;
            tbpr.TextChanged += OamPrio;
            tbcpn.TextChanged += OamPal;
        }

        public void OamX(object sender, EventArgs args)
        {
            obj.x = (short)GetInt(sender);
            if (obj.x < 0 && obj.rotsca % 2 == 0)
            {
                obj.rotsca++;
                rotscatb.Text = "" + obj.rotsca;
            }
            else if (obj.x > 0 && obj.rotsca % 2 != 0)
            {
                obj.rotsca--;
                rotscatb.Text = "" + obj.rotsca;
            }
            UpdatePartView();
        }

        public void OamY(object sender, EventArgs args)
        {
            obj.y = (short)GetInt(sender);
            UpdatePartView();
        }

        public void OamSrc(object sender, EventArgs args)
        {
            obj.cch = (ushort)GetInt(sender);
            UpdatePartView();
        }

        public void OamRotsca(object sender, EventArgs args)
        {
            obj.rotsca = (byte)GetInt(sender);
            UpdatePartView();
        }

        public void ChangeForm(object sender, EventArgs args)
        {
            int form = ((ComboBox)sender).SelectedIndex;
            obj.SetNewForm(form);
            UpdatePartView();
            indextb.Text = "" + 0;
        }

        public void OamMode(object sender, EventArgs args)
        {
            obj.mode = (byte)GetInt(sender);
            UpdatePartView();
        }

        public void OamPrio(object sender, EventArgs args)
        {
            obj.prio = (byte)GetInt(sender);
            UpdatePartView();
        }

        public void OamPal(object sender, EventArgs args)
        {
            obj.pal = (byte)GetInt(sender);
            UpdatePartView();
        }

        public void AddToChar(object sender, EventArgs args)
        {
            int form = fobo.SelectedIndex;
            int[] sizes = new int[] { 1, 4, 16, 64, 2, 4, 8, 32, 2, 4, 8, 32 };
            int size = sizes[form] * 64;
            int index = manager.GetCharLen() / (obj.col8bit ? 2 : 4);
            manager.AddToChar(size);
            MessageBox.Show("" + manager.GetCharLen() / (obj.col8bit ? 2 : 4));
            indextb.Text = "" + index;
            UpdatePartView();
        }


        public void UpdatePartView()
        {
            manager.ShowPart();
        }
    }
}
