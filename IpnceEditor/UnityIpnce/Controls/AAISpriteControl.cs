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
    public class AAISpriteControl : ElementControl
    {
        AAISprite obj;

        public AAISpriteControl(GroupBox box) : base(box) { }
        public AAISpriteControl(GroupBox box, AAISprite sp) : base(box) { obj = sp; }
        public void SetSprite(AAISprite sprite) { obj = sprite; }

        public override void SetAllControls()
        {
            LoadParamsSprite();
        }

        public override void SetEditedObject(object o)
        {
            obj = (AAISprite)o;
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
            tbX.TextChanged += SpriteX;
            tbY.TextChanged += SpriteY;
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
