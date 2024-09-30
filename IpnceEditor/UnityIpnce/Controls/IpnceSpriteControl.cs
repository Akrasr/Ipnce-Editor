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
    public class IpnceSpriteControl : ElementControl
    {
        Sprite obj;

        public IpnceSpriteControl(GroupBox box) : base(box) { }
        public IpnceSpriteControl(GroupBox box, Sprite sp) : base(box) { obj = sp; }
        public void SetSprite(Sprite sprite) { obj = sprite; }

        public override void SetAllControls()
        {
            LoadParamsSprite();
        }

        public override void SetEditedObject(object o)
        {
            obj = (Sprite)o;
        }
        private void LoadParamsSprite()
        {
            Label dstX = new Label();
            dstX.Text = "DestX:";
            dstX.Width = 40;
            dstX.Location = new Point(10, 23);
            groupBox1.Controls.Add(dstX);
            TextBox tbX = new TextBox();
            tbX.Location = new Point(160, 20);
            tbX.Text = "" + obj.DestX;
            groupBox1.Controls.Add(tbX);
            Label dstY = new Label();
            dstY.Text = "DestY:";
            dstY.Width = 40;
            dstY.Location = new Point(10, 53);
            groupBox1.Controls.Add(dstY);
            TextBox tbY = new TextBox();
            tbY.Location = new Point(160, 50);
            tbY.Text = "" + obj.DestY;
            groupBox1.Controls.Add(tbY);
            CheckBox cb = new CheckBox();
            cb.Text = "IsBlend";
            cb.Location = new Point(10, 80);
            cb.Checked = obj.IsBlend;
            groupBox1.Controls.Add(cb);
            tbX.TextChanged += SpriteX;
            tbY.TextChanged += SpriteY;
            cb.CheckedChanged += IsBlendChanged;
        }

        public void IsBlendChanged(object sender, EventArgs args)
        {
            obj.IsBlend = ((CheckBox)sender).Checked;
        }

        public void SpriteX(object sender, EventArgs args)
        {
            obj.DestX = GetFloat(sender);
        }
        public void SpriteY(object sender, EventArgs args)
        {
            obj.DestY = GetFloat(sender);
        }
    }
}
