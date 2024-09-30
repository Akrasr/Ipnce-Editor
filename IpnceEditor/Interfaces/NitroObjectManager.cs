using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IpnceEditor.GUI;
using IpnceEditor.UnityIpnce;

namespace IpnceEditor.Interfaces
{
    public abstract class NitroObjectManager
    {
        protected string lastname;
        public string IpnceName;
        public ElementControl[] controls;
        public int spriteIndex;
        public int spritePartIndex;
        public int animIndex;
        public int animFrameIndex;
        public NitroObjectManager(string name)
        {
            lastname = name;
            Load(lastname);
        }
        public abstract void AddAnim();
        public abstract void AddAnimKeyFrame(int ind);
        public abstract void AddSprite();
        public abstract void AddSpriteParts(int ind);
        public abstract void SaveAs();
        public abstract void Load(string filename);
        public abstract string[] GetAtlasList();
        public abstract string[] GetSpriteList();
        public abstract string[] GetAnimList();
        public abstract string[] GetSpritePartsList(int ind);
        public abstract string[] GetAnimKeyList(int ind);
        public abstract int MaxFrames(int[] inds);
        public abstract void GetNeededFiles();
        public abstract NitroImageManager GetImageManager();
        public abstract bool HDCheck();
        public abstract bool GetUsePalette();
        public abstract int GetColorPaletteNum();
        public abstract bool GetOffScreenRendering();
        public abstract void SetGroupBox(GroupBox box);
        public abstract string GetIpnceName();
        public abstract void SaveAsCollectionAAI1Ipnce();
        public abstract void SaveAsCollectionIpnce();
        public abstract void SaveAsDS();
        public abstract void SaveAsAAI2();
        public abstract void SaveAsAAI();
        public abstract void SaveAsAJ();
        public abstract Texture2D GetAtlasData(int ind);
        public virtual void MakeControls(int ind)
        {
            controls[ind].SetAllControls();
        }
        public abstract void Save();
        public void ShowPart()
        {
            IpnceDrawer dw = IpnceDrawer.Instance;
            dw.UpdatePart(spriteIndex, spritePartIndex);
            //dw.DrawPart(spriteIndex, spritePartIndex);
        }

        public void ShowFrame()
        {
            IpnceDrawer dw = IpnceDrawer.Instance;
            dw.DrawCertainFrame(animIndex, animFrameIndex, dw.BGImage != null);
        }
    }
}
