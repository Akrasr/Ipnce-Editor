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
    public class CollectionSpriteControl : ElementControl
    {
        CollectionSprite obj;

        public CollectionSpriteControl(GroupBox box) : base(box) { }
        public CollectionSpriteControl(GroupBox box, CollectionSprite sp) : base(box) { obj = sp; }
        public void SetSprite(CollectionSprite sprite) { obj = sprite; }

        public override void SetAllControls()
        {
            LoadParamsSprite();
        }

        public override void SetEditedObject(object o)
        {
            obj = (CollectionSprite)o;
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
            tbY.TextChanged += SpriteY; Label scX = new Label();
            scX.Text = "ScaleX:";
            scX.Width = 150;
            scX.Location = new Point(10, 83);
            groupBox1.Controls.Add(scX);
            TextBox tbsX = new TextBox();
            tbsX.Location = new Point(160, 80);
            tbsX.Text = "" + obj.ScaleX;
            groupBox1.Controls.Add(tbsX);
            Label scY = new Label();
            scY.Text = "ScaleY:";
            scY.Width = 150;
            scY.Location = new Point(10, 113);
            groupBox1.Controls.Add(scY);
            TextBox tbsY = new TextBox();
            tbsY.Location = new Point(160, 110);
            tbsY.Text = "" + obj.ScaleY;
            groupBox1.Controls.Add(tbsY);
            tbsX.TextChanged += ScaleX;
            tbsY.TextChanged += ScaleY;
        }

        public void SpriteX(object sender, EventArgs args)
        {
            obj.DestX = GetFloat(sender);
        }
        public void SpriteY(object sender, EventArgs args)
        {
            obj.DestY = GetFloat(sender);
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
        }

        public void ScaleY(object sender, EventArgs args)
        {
            obj.ScaleY = GetFloat(sender);
        }
    }
}
