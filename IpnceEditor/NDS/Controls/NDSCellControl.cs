using IpnceEditor.NDS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.Interfaces;

namespace IpnceEditor.NDS.Controls
{
    internal class NDSCellControl : ElementControl
    {
        Nitro_Cell obj;

        public NDSCellControl(GroupBox box) : base(box) { }
        public NDSCellControl(GroupBox box, Nitro_Cell sp) : base(box) { obj = sp; }
        public void SetSprite(Nitro_Cell sprite) { obj = sprite; }

        public override void SetAllControls()
        {
            LoadParamsSprite();
        }

        public override void SetEditedObject(object o)
        {
            obj = (Nitro_Cell)o;
        }

        private void LoadParamsSprite()
        {
        }
    }
}
