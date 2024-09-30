using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IpnceEditor.Interfaces
{
    public abstract class ElementControl
    {
        protected GroupBox groupBox1;

        public ElementControl(GroupBox groupBox)
        {
            groupBox1 = groupBox;
        }
        public abstract void SetEditedObject(object o);
        public abstract void SetAllControls();

        string[] nums = new string[] { "-", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "," };

        public void FloatSave(object sender)
        {
            TextBox tb = (TextBox)sender; //a function for deleting wrong symbols in text box while parsing to float
            string res = "";
            bool f = false;
            bool p = false;
            for (int i = 0; i < tb.Text.Length; i++)
            {
                if (("" + tb.Text.ToCharArray()[i]) == nums[0] && i != 0)
                {
                    continue;
                }
                if (("" + tb.Text.ToCharArray()[i]) == nums[11])
                {
                    if (p)
                        continue;
                    else
                        p = true;
                }
                if (nums.Contains("" + tb.Text.ToCharArray()[i]))
                {
                    res += tb.Text.ToCharArray()[i];
                }
                else
                    f = true;
            }
            int pos = tb.SelectionStart;
            if (f) pos--;
            tb.Text = res;
            tb.Select(pos, 0);
        }

        public float GetFloat(object sender)
        {
            FloatSave(sender); //parsing float
            string txt = ((TextBox)sender).Text;
            txt = txt.Replace("-,", "-0,");
            if (txt == "-")
            {
                return 0;
            }
            if (txt.Length == 0)
                return 0;
            if ("" + txt.ToCharArray()[0] == nums[11])
            {
                txt = "0" + txt;
            }
            if ("" + txt.ToCharArray()[txt.Length - 1] == nums[11])
            {
                txt = txt.Remove(txt.Length - 1, 1);
            }
            float res = float.Parse(txt);
            return res;
        }

        public void IntSave(object sender) //a function for deleting wrong symbols in text box while parsing to int
        {
            string[] ints = new string[11];
            for (int i = 0; i < 11; i++)
            {
                ints[i] = nums[i];
            }
            TextBox tb = (TextBox)sender;
            string res = "";
            bool f = false;
            for (int i = 0; i < tb.Text.Length; i++)
            {
                if (("" + tb.Text.ToCharArray()[i]) == nums[0] && i != 0)
                {
                    continue;
                }
                if (ints.Contains("" + tb.Text.ToCharArray()[i]))
                {
                    res += tb.Text.ToCharArray()[i];
                }
                else
                    f = true;
            }
            int pos = tb.SelectionStart;
            if (f) pos--;
            tb.Text = res;
            tb.Select(pos, 0);
        }

        public int GetInt(object sender) //parsing int
        {
            IntSave(sender);
            if (((TextBox)sender).Text == "-")
            {
                return 0;
            }
            if (((TextBox)sender).Text.Length == 0)
            {
                return 0;
            }
            return Int32.Parse(((TextBox)sender).Text);
        }

        public byte GetByte(object sender) // parsing to byte
        {
            int res = GetInt(sender);
            if (res < 0)
            {
                ((TextBox)sender).Text = "0";
                return 0;
            }
            else if (res > 255)
            {
                ((TextBox)sender).Text = "255";
                return 255;
            }
            else
                return (byte)res;
        }
    }
}
