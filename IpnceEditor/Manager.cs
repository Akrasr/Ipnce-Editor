using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using IpnceEditor.Interfaces;
using IpnceEditor.NDS.ObjectManagers;
using IpnceEditor.UnityIpnce.ObjectManagers;

namespace IpnceEditor
{
    class Manager
    {
        public string ipncePath;
        public string atlasPath;
        public string palettePath;
        public bool flipped;
        public static bool AAI1 = false;
        public NitroObjectManager objmanager;
        public AnimationType animationtype;

        public Manager() { }

        public Manager(string path)
        {
            animationtype = GetAnimationType(path);
            objmanager = GetObjectManager(path);
            objmanager.GetNeededFiles();
        }

        public NitroObjectManager GetObjectManager(string path)
        {
            switch (animationtype)
            {
                case AnimationType.Ipnce:
                    return new HDIpnceManager(path);
                case AnimationType.NitroCel:
                    return new NDSCellManager(path);
                case AnimationType.AAIIpnce:
                    return new AAIIpnceManager(path);
                case AnimationType.AJIpnce:
                    return new AJIpnceManager(path);
                case AnimationType.CollectionAAI1Ipnce:
                    return new CollectionAAI1Manager(path);
                case AnimationType.CollectionAAI2Ipnce:
                    return new CollectionIpnceManager(path);
                default:
                    return new NDSCellManager(path);
            }
        }

        public AnimationType GetAnimationType(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    int magic = br.ReadInt32();
                    if (magic == 0)
                    {
                        br.BaseStream.Position += 0x10;
                        int classpath = br.ReadInt32();
                        if (classpath == 0xbf)
                        {
                            return AnimationType.AAIIpnce;
                        }
                        else if (classpath == 0x6b)
                        {
                            return AnimationType.AJIpnce;
                        }
                        else if (classpath == 0x73a544e8)
                        {
                            return AnimationType.CollectionAAI1Ipnce;
                        }
                        else if (classpath == 0x6c415d67)
                        {
                            return AnimationType.CollectionAAI2Ipnce;
                        }
                        return AnimationType.Ipnce;
                    }
                    else return AnimationType.NitroCel;
                }
            }
        }

        public void SetIpnce(string _path)
        {
            animationtype = GetAnimationType(_path);
            objmanager = GetObjectManager(_path);
            objmanager.GetNeededFiles();
        }

        public bool HDCheck()
        {
            return objmanager.HDCheck();
        }

        public void Save()
        {
            objmanager.Save();
        }

        public void SaveAs()
        {
            objmanager.SaveAs();
        }

        public void SaveAsCollectionAAI1Ipnce()
        {
            objmanager.SaveAsCollectionAAI1Ipnce();
        }

        public void SaveAsCollectionIpnce()
        {
            objmanager.SaveAsCollectionIpnce();
        }

        public void SaveAsAAI2()
        {
            objmanager.SaveAsAAI2();
        }

        public void SaveAsAAI()
        {
            objmanager.SaveAsAAI();
        }

        public void SaveAsAJ()
        {
            objmanager.SaveAsAJ();
        }

        public void SaveAsDS()
        {
            objmanager.SaveAsDS();
        }

        //getting items for listboxes in form

        public string GetIpnceName()
        {
            return objmanager.GetIpnceName();
        }

        public string[] GetAtlasList()
        {
            return objmanager.GetAtlasList();
        }
        public string[] GetSpriteList()
        {
            return objmanager.GetSpriteList();
        }
        public string[] GetAnimList()
        {
            return objmanager.GetAnimList();
        }

        public string[] GetSpritePartsList(int ind)
        {
            return objmanager.GetSpritePartsList(ind);
        }

        public string[] GetAnimKeyList(int ind)
        {
            return objmanager.GetAnimKeyList(ind);
        }

        public bool GetUsePalette()
        {
            return objmanager.GetUsePalette();
        }

        public bool GetOffScreenRendering()
        {
            return objmanager.GetOffScreenRendering();
        }

        public int GetColorPaletteNum()
        {
            return objmanager.GetColorPaletteNum();
        }

        //finding an amount of frames for selected animations

        public int MaxFrames(int[] inds)
        {
            return objmanager.MaxFrames(inds);
        }

        public void AddSprite()
        {
            objmanager.AddSprite();
        }

        public void AddAnim()
        {
            objmanager.AddAnim();
        }

        public void AddSpriteParts(int ind)
        {
            objmanager.AddSpriteParts(ind);
        }

        public void AddAnimKeyframe(int ind)
        {
            objmanager.AddAnimKeyFrame(ind);
        }

        public NitroImageManager GetImageManager()
        {
            return objmanager.GetImageManager();
        }

        public void MakeControls(int ind)
        {
            objmanager.MakeControls(ind);
        }

        public void SetSpriteIndex(int ind)
        {
            objmanager.spriteIndex = ind;
        }

        public void SetSpritePartIndex(int ind)
        {
            objmanager.spritePartIndex = ind;
        }

        public void SetAnimIndex(int ind)
        {
            objmanager.animIndex = ind;
        }

        public void SetAnimFrameIndex(int ind)
        {
            objmanager.animFrameIndex = ind;
        }

        public void SetGroupBox(GroupBox box)
        {
            objmanager.SetGroupBox(box);
        }
    }
}
