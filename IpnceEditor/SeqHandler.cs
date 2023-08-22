using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IpnceEditor
{
    class SeqHandler
    {
        public static async void CreateSeq(List<Image> images, List<int> delays, string filename)
        {
            Form2 form = new Form2();
            if (!Directory.Exists("tmp")) //creating tmp directory
                Directory.CreateDirectory("tmp");
            form.Show();
            ProgressBar pb = form.GetPB();
            Label lbl = form.GetLabel();
            pb.Minimum = 0;
            pb.Maximum = images.Count();
            int delmom = 1;
            for (int i = 0; i < images.Count; i++) //Saving images to folder
            {
                for (int j = 0; j < delays[i + 1]; j++)
                    images[i].Save("tmp\\frame" + FD(delmom + j) + ".png");
                delmom += delays[i + 1];
                pb.Value = i + 1;
                int ind = i + 1;
                pb.Value = ind;
                lbl.Text = "Images Added: " + ind;
                await Task.Delay(10);
            }
            lbl.Text = "Converting to webp";
            var proc = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = "webpconv.bat",
                Arguments = filename,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            };
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = proc;
            p.Start();
            p.WaitForExit();
            lbl.Text = "Deleting directory";
            Directory.Delete("tmp", true);
            if (File.Exists(filename))
                File.Delete(filename);
            File.Move("tmp.webp", filename);
            form.Close();
        }

        private static string FD(int num)
        {
            string t = "" + num;
            while (t.Length < 4)
                t = "0" + t;
            return t;
        }
    }
}
