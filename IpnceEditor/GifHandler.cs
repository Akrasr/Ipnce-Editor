using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnimatedGif;
using System.Drawing;
using System.Threading.Tasks;

namespace IpnceEditor
{
    class GifHandler
    {
        const int del = 32;
        public static async void CreateGif(List<Image> images, List<int> delays, string filename)
        {
            Form2 form = new Form2();
            form.Show();
            ProgressBar pb = form.GetPB();
            Label lbl = form.GetLabel();
            pb.Minimum = 0;
            pb.Maximum = images.Count();
            using (var gif = AnimatedGif.AnimatedGif.Create(filename, del))
            {
                //MessageBox.Show("Gif started");
                for (int i = 0; i < images.Count; i++)
                {
                    if (delays[i + 1] == 1)
                        delays[i + 1] = 2;
                    gif.AddFrame(images[i], delay: delays[i + 1] * del, quality: GifQuality.Bit8);
                    //gif.AddFrame(images[i], delay: del, quality: GifQuality.Bit8);
                    await Task.Delay(10);
                    int ind = i + 1;
                    pb.Value = ind;
                    lbl.Text = "Images Added: " + ind;
                }

            }
            form.Close();
        }
    }
}
