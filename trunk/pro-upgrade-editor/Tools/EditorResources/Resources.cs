using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using EditorResources;

namespace ProUpgradeEditor
{
    public static class IconResources
    {

        public static Bitmap CDRom { get { return GetBitmap("CDRom"); } }
        public static Bitmap Task { get { return GetBitmap("Task"); } }
        public static Bitmap XBJoy { get { return GetBitmap("XBJoy"); } }
        public static Bitmap XPDrive { get { return GetBitmap("XPDrive"); } }
        public static Bitmap XPLens { get { return GetBitmap("XPLens"); } }
        public static Bitmap XPFolder { get { return GetBitmap("XPFolder"); } }
        public static Bitmap XPRecycle { get { return GetBitmap("XPRecycle"); } }
        public static Bitmap OpenFolder { get { return GetBitmap("OpenFolder"); } }

        public static Bitmap USBBlueArrow { get { return GetBitmap("USBBlueArrow"); } }
        public static Bitmap USBExcl { get { return GetBitmap("USBExcl"); } }
        public static Bitmap USBFlash { get { return GetBitmap("USBFlash"); } }
        public static Bitmap USBGreenPlus { get { return GetBitmap("USBGreenPlus"); } }
        public static Bitmap USBLogo { get { return GetBitmap("USBLogo"); } }
        public static Bitmap USBPencil { get { return GetBitmap("USBPencil"); } }
        public static Bitmap USBRedMinus { get { return GetBitmap("USBRedMinus"); } }

        public static Bitmap Partition { get { return GetBitmap("Partition"); } }

        static Bitmap GetBitmap(string name)
        {
            var bmp = PEResources.ResourceManager.GetObject(name) as Bitmap;
            if (bmp != null)
            {
                bmp.Tag = name;
            }
            return bmp;
        }

        public static byte[] GetBytes(this Bitmap bmp)
        {
            byte[] ret = null;
            var name = bmp.Tag as string;
            if (name != null)
            {

                using (var stream = PEResources.ResourceManager.GetStream(name))
                {
                    ret = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(ret, 0, (int)stream.Length);
                }
            }
            return ret;
        }
    }


}
