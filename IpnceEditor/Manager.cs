using System;
using System.IO;

namespace IpnceEditor
{
    class Manager
    {
        public Ipnce ipnce;
        public string ipncePath;
        public string atlasPath;
        public string palettePath;
        public bool flipped;
        public byte[] header;
        public IpnceAdapter adapt;

        public Manager()
        {
            this.ipnce = new Ipnce();
        }

        public void SetIpnce(string _path)
        {
            this.ipncePath = _path;
            this.Load();
        }

        public string GetIpnce()
        {
            return this.ipncePath;
        }

        public void Load()
        {
            byte[] bytes = File.ReadAllBytes(this.ipncePath); //reading header
            int name = 4 + bytes[28] - bytes[28] % 4;
            if (bytes[28] % 4 == 0)
                name -= 4;
            byte[] data = new byte[bytes.Length - 32 - name];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = bytes[32 + name + i];
            }
            header = new byte[32 + name];
            for (int i = 0; i < header.Length; i++)
            {
                header[i] = bytes[i];
            }
            this.adapt = MakeAdapter(bytes[20]); //choosing what ipnce this is
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                this.ipnce = adapt.Load(br);
                bool cor = true;
                try
                {
                    int d = br.ReadInt32();
                    cor = false;
                } catch { }
                if (!cor)
                {
                    throw new Exception("Invalid File");
                }
            }
        }

        public IpnceAdapter MakeAdapter(byte id)
        {
            if (id == 191)
            {
                flipped = true;
                return new AAIAdapter();
            }
            else if (id == 244)
            {
                flipped = false;
                return new AAI2Adapter();
            }
            else if (id == 107)
            {
                flipped = true;
                return new AJAdapter();
            }
            else
                throw new Exception("Invalid ID");
        }

        public void Save()
        {
            SaveAs(this.ipncePath);
        }

        public void SaveAs(string _path)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                bw.Write(header);
                adapt.Save(ipnce, bw);
            }
        }

        public void SaveAsAAI2(string _path)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                byte r = header[20];
                header[20] = 244;
                bw.Write(header);
                header[20] = r;
                new AAI2Adapter().Save(ipnce, bw);
            }
        }

        public void SaveAsAAI(string _path)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                byte r = header[20];
                header[20] = 191;
                bw.Write(header);
                header[20] = r;
                new AAIAdapter().Save(ipnce, bw);
            }
        }

        public void SaveAsAJ(string _path)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                byte r = header[20];
                header[20] = 107;
                bw.Write(header);
                header[20] = r;
                new AJAdapter().Save(ipnce, bw);
            }
        }

        //getting items for listboxes in form

        public string[] GetAtlasList()
        {
            string[] res = new string[ipnce.SpriteAtlas.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "SpriteAtlas[" + i + "]";
            }
            return res;
        }
        public string[] GetSpriteList()
        {
            string[] res = new string[ipnce.SpriteList.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "SpriteList[" + i + "]";
            }
            return res;
        }
        public string[] GetAnimList()
        {
            string[] res = new string[ipnce.AnimList.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "AnimList[" + i + "]";
            }
            return res;
        }

        public string[] GetSpritePartsList(int ind)
        {
            string[] res = new string[ipnce.SpriteList[ind].Parts.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "Parts[" + i + "]";
            }
            return res;
        }

        public string[] GetAnimKeyList(int ind)
        {
            string[] res = new string[ipnce.AnimList[ind].KeyFrames.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = "KeyFrames[" + i + "]";
            }
            return res;
        }

        //finding an amount of frames for selected animations

        public int MaxFrames(int[] inds)
        {
            int max = 0;
            for (int i = 0; i < inds.Length; i++)
            {
                if (ipnce.AnimList[inds[i]].TotalFrameSize > max)
                    max = ipnce.AnimList[inds[i]].TotalFrameSize;
            }
            return max;
        }
    }
}
