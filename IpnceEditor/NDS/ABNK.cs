using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IpnceEditor.NDS
{
    public class ABNK
    {
        const uint magic = 0x41424e4b;
        public uint size;
        ushort animCount;
        public ushort total_frames;
        uint unk1;
        uint frame_ref_start;
        uint frame_data_start;
        public DSSeq[] dSSeqs;

        public ABNK()
        {
            unk1 = 0x18;
            dSSeqs = new DSSeq[0];
        }

        public ABNK(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader reader)
        {
            uint m = reader.ReadUInt32();
            if (m != magic)
            {
                throw new Exception("ABNK magic is not present");
            }
            size = reader.ReadUInt32();
            animCount = reader.ReadUInt16();
            total_frames = reader.ReadUInt16();
            unk1 = reader.ReadUInt32();
            frame_ref_start = reader.ReadUInt32();
            frame_data_start = reader.ReadUInt32();
            reader.ReadUInt64(); // padding
            dSSeqs = new DSSeq[animCount];
            for (int i = 0; i < animCount; i++)
            {
                dSSeqs[i] = new DSSeq(reader);
            }
            for (int i = 0; i < animCount; i++)
                dSSeqs[i].LoadKeyFrames(reader, 0x18 + frame_ref_start, 0x18 + frame_data_start);
            reader.BaseStream.Position = 0x10 + size;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(magic);
            bw.Write(size);
            bw.Write(animCount);
            bw.Write(total_frames);
            bw.Write(unk1);
            bw.Write(frame_ref_start);
            bw.Write(frame_data_start);
            bw.Write((ulong)0);
            foreach (DSSeq seq in dSSeqs)
                seq.Save(bw);
            foreach (DSSeq seq in dSSeqs)
                seq.SaveKeyFrames(bw);
            while (bw.BaseStream.Position < frame_data_start + 0x10)
            {
                bw.Write((byte)0);
            }
            foreach (DSSeq seq in dSSeqs)
                seq.SaveKeyFramesFrames(bw);
        }

        public void Adjust()
        {
            animCount = (ushort)dSSeqs.Length;
            List<DSKeyFrame> checkedFrames = new List<DSKeyFrame>();
            foreach (DSSeq seq in dSSeqs)
            {
                seq.SetEquals(checkedFrames);
            }

            uint poi = 0;
            uint frame_poi = 0;
            foreach (DSSeq seq in dSSeqs)
            {
                poi = seq.SetPointers(poi);
                seq.framecount = (ushort)seq.keyframes.Length;
                seq.frame_addr = frame_poi;
                frame_poi += (uint)(8 * seq.keyframes.Length);
            }
            frame_ref_start = (uint)(0x18 + dSSeqs.Length * 0x10);
            frame_data_start = (uint)(frame_ref_start + frame_poi);
            size = frame_data_start + 0x8 + poi;
        }

        public void AddAnim()
        {
            List<DSSeq> seqs = new List<DSSeq>();
            seqs.AddRange(dSSeqs);
            DSSeq s = DSSeq.CreateBlank();
            seqs.Add(s);
            dSSeqs = seqs.ToArray();
            animCount++;
        }

        public void AddAnim(DSSeq s)
        {
            List<DSSeq> seqs = new List<DSSeq>();
            seqs.AddRange(dSSeqs);
            seqs.Add(s);
            dSSeqs = seqs.ToArray();
            animCount++;
        }

        public void AddAnimKeyFrame(int ind)
        {
            DSSeq seq = dSSeqs[ind];
            seq.AddKeyFrame();
        }
    }

    public class DSSeq
    {
        public ushort framecount;
        public ushort firstframe;
        public ushort frametype;
        public ushort seqtype;
        public uint seqmode;
        public uint frame_addr;
        public DSKeyFrame[] keyframes;

        public DSSeq() { keyframes = new DSKeyFrame[0]; }

        public static DSSeq CreateBlank()
        {
            DSSeq seq = new DSSeq();
            seq.framecount = 0;
            seq.firstframe = 0;
            seq.frametype = 0;
            seq.seqtype = 0;
            seq.seqmode = 0;
            //seq.AddKeyFrame();
            return seq;
        }

        public DSSeq(BinaryReader br)
        {
            Load(br);
        }

        public void Load(BinaryReader reader)
        {
            framecount = reader.ReadUInt16();
            firstframe = reader.ReadUInt16();
            frametype = reader.ReadUInt16();
            seqtype = reader.ReadUInt16();
            seqmode = reader.ReadUInt32();
            frame_addr = reader.ReadUInt32();
        }

        public void LoadKeyFrames(BinaryReader reader, uint keyFramesStart, uint FramesStart)
        {
            keyframes = new DSKeyFrame[framecount];
            long oldpos = reader.BaseStream.Position;
            reader.BaseStream.Position = keyFramesStart + frame_addr;
            for (int i = 0; i < framecount; i++)
            {
                keyframes[i] = new DSKeyFrame(reader, frametype);
                keyframes[i].LoadFrame(reader, FramesStart);
            }
        }

        public int GetDuration()
        {
            int dur = 0;
            foreach (DSKeyFrame keyframe in keyframes)
            {
                keyframe.start = dur;
                dur += keyframe.duration;
            }
            return dur;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(framecount);
            bw.Write(firstframe);
            bw.Write(frametype);
            bw.Write(seqtype);
            bw.Write(seqmode);
            bw.Write(frame_addr);
        }

        public void SaveKeyFrames(BinaryWriter bw)
        {
            foreach (DSKeyFrame keyframe in keyframes)
                keyframe.Save(bw);
        }

        public void SaveKeyFramesFrames(BinaryWriter bw)
        {
            foreach (DSKeyFrame keyframe in keyframes)
                keyframe.SaveFrame(bw);
            //bw.Write((ushort)0xcccc);
        }

        public void SetEquals(List<DSKeyFrame> frames)
        {
            foreach (DSKeyFrame frame in keyframes)
            {
                foreach (DSKeyFrame checkequ in frames)
                {
                    if (checkequ.Equ(frame))
                    {
                        frame.equalToOther = true;
                        frame.theequalone = checkequ;
                        break;
                    }
                }
                frames.Add(frame);
            }
        }

        public void ChangeTypes(ushort newType)
        {
            foreach (DSKeyFrame keyframe in keyframes)
                keyframe.ChangeType(newType);
        }

        public void AddKeyFrame()
        {
            List<DSKeyFrame> frs = new List<DSKeyFrame>();
            frs.AddRange(keyframes);
            DSKeyFrame fr = DSKeyFrame.CreateBlank(frametype);
            frs.Add(fr);
            keyframes = frs.ToArray();
            GetDuration();
            framecount++;
        }

        public void AddKeyFrame(DSKeyFrame fr)
        {
            List<DSKeyFrame> frs = new List<DSKeyFrame>();
            frs.AddRange(keyframes);
            frs.Add(fr);
            keyframes = frs.ToArray();
            GetDuration();
            framecount++;
        }


        public int GetSize()
        {

            switch (frametype)
            {
                case 0:
                    return 2 * keyframes.Length + 2;
                case 1:
                    return 16 * keyframes.Length;
                case 2:
                    return 8 * keyframes.Length;
            }
            return 0;
        }


        public uint SetPointers(uint poi)
        {
            foreach(DSKeyFrame keyframe in keyframes)
            {
                if (keyframe.equalToOther)
                {
                    keyframe.poi = keyframe.theequalone.poi;
                    continue;
                }
                keyframe.poi = poi;
                switch (frametype)
                {
                    case 0:
                        poi += 2;
                        break;
                    case 1:
                        poi += 16;
                        break;
                    case 2:
                        poi += 8;
                        break;
                }
            }
            if (frametype == 0)
                poi += 2;
            return poi;
        }
    }

    public class DSKeyFrame
    {
        public uint poi;
        public ushort duration;
        public int start;
        public ushort type;
        public ushort unk;
        public IFrame fr;
        public bool equalToOther = false;
        public DSKeyFrame theequalone;

        public DSKeyFrame() { }

        public static DSKeyFrame CreateBlank(ushort type)
        {
            DSKeyFrame fr = new DSKeyFrame();
            fr.type = type;
            fr.duration = 0;
            fr.unk = 0xbeef;
            switch (type)
            {
                case 0:
                    fr.fr = new Frame();
                    break;
                case 1:
                    fr.fr = new Frame1();
                    break;
                case 2:
                    fr.fr = new Frame2();
                    break;
            }
            return fr;
        }
        public DSKeyFrame(BinaryReader br, ushort type)
        {
            Load(br);
            this.type = type;
        }

        public void Load(BinaryReader reader)
        {
            poi = reader.ReadUInt32();
            duration = reader.ReadUInt16();
            unk = reader.ReadUInt16();
        }

        public void LoadFrame(BinaryReader br, uint startFrames)
        {
            long oldpos = br.BaseStream.Position;
            br.BaseStream.Position = startFrames + poi;
            switch (type)
            {
                case 0:
                    fr = new Frame(br);
                    break;
                case 1:
                    fr = new Frame1(br);
                    break;
                case 2:
                    fr = new Frame2(br);
                    break;
            }
            br.BaseStream.Position = oldpos;
        }

        public void SetIndex(int ind)
        {
            fr.index = (ushort)ind;
        }

        public void SetRotZ(int z)
        {
            fr.rotZ = (ushort)z;
        }

        public void SetX(int x)
        {
            fr.px = (short)x;
        }

        public void SetY(int y)
        {
            fr.py = (short)y;
        }

        public void SetSX(float x)
        {
            fr.sx = (int)(x * 4096);
        }

        public void SetSY(float y)
        {
            fr.sy = (int)(y * 4096);
        }

        public int GetX()
        {
            return fr.px;
        }

        public float GetSX()
        {
            return (float)(fr.sx) / 4096.0f;
        }

        public float GetSY()
        {
            return (float)(fr.sy) / 4096.0f;
        }

        public int GetY()
        {
            return fr.py;
        }

        public int GetIndex()
        {
            return fr.index;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(poi);
            bw.Write(duration);
            bw.Write(unk);
        }

        public void SaveFrame(BinaryWriter bw)
        {
            if (equalToOther)
                return;
            fr.Save(bw);
        }

        public bool Equ(DSKeyFrame other)
        {
            if (type != other.type)
                return false;
            return fr.Equ(other.fr);
        }

        public uint GetFrameSize()
        {
            switch (type)
            {
                case 0:
                    return 4;
                case 1:
                    return 16;
                case 2:
                    return 8;
            }
            return 0;
        }

        public void ChangeType(ushort newType)
        {
            switch (newType)
            {
                case 0:
                    fr = fr.ToFrame();
                    break;
                case 1:
                    fr = fr.ToFrame1();
                    break;
                case 2:
                    fr = fr.ToFrame2();
                    break;
            }
            type = newType;
        }
    }

    public abstract class IFrame
    {
        public ushort index;
        public ushort rotZ;
        public int sx;
        public int sy;
        public short px;
        public short py;
        public ushort padding;

        protected IFrame()
        {
            sx = 4096;
            sy = 4096;
            px = 0;
            py = 0;
            rotZ = 0;
        }

        public abstract void Load(BinaryReader reader);

        public abstract void Save(BinaryWriter bw);

        public Frame ToFrame()
        {
            Frame fr = new Frame();
            fr.padding = 0xcccc;
            fr.index = index;
            return fr;
        }

        public Frame1 ToFrame1()
        {
            Frame1 fr = new Frame1();
            fr.index = index;
            fr.py = py;
            fr.px = px;
            fr.sx = 4096;
            fr.sy = 4096;
            fr.rotZ = 0;
            return fr;
        }

        public Frame2 ToFrame2()
        {
            Frame2 fr = new Frame2();
            fr.padding = 0xBEEF;
            fr.index = index;
            fr.py = py;
            fr.px = px;
            return fr;
        }

        public bool Equ(IFrame other)
        {
            return other.rotZ == rotZ && other.index == index && other.px == px && other.py == py && other.sx == sx && other.sy == sy;
        }
    }

    public class Frame : IFrame
    {

        public Frame() : base()
        {
            index = 0;
            padding = 0xcccc;
        }
        public Frame(BinaryReader br) : base()
        {
            Load(br);
        }

        public override void Load(BinaryReader reader)
        {
            index = reader.ReadUInt16();
            padding = reader.ReadUInt16();
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(index);
            bw.Write(padding);
        }
    }

    public class Frame1 : IFrame
    {
        public Frame1() : base() { }
        public Frame1(BinaryReader br) : base()
        {
            Load(br);
        }

        public override void Load(BinaryReader reader)
        {
            index = reader.ReadUInt16();
            rotZ = reader.ReadUInt16();
            sx = reader.ReadInt32();
            sy = reader.ReadInt32();
            px = reader.ReadInt16();
            py = reader.ReadInt16();
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(index);
            bw.Write(rotZ);
            bw.Write(sx);
            bw.Write(sy);
            bw.Write(px);
            bw.Write(py);
        }
    }

    public class Frame2 : IFrame
    {

        public Frame2() : base() 
        {
            padding = 0xBEEF;
        }
        public Frame2(BinaryReader br) : base()
        {
            Load(br);
        }

        public override void Load(BinaryReader reader)
        {
            index = reader.ReadUInt16();
            padding = reader.ReadUInt16();
            px = reader.ReadInt16();
            py = reader.ReadInt16();
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(index);
            bw.Write(padding);
            bw.Write(px);
            bw.Write(py);
        }
    }
}
