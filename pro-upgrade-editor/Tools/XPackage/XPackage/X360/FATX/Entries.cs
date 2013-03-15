
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;
using System.Diagnostics;
using X360.FATX;
using X360.IO;
using X360.IO.FATXExtensions;
using X360.Other;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace X360.FATX
{

    [Flags]
    public enum FATXAttribute
    {
        None = 0,
        Folder = 0x10,
        Invalid = 0xFF,
    }


    public unsafe struct FATXEntry64
    {
        //[FieldOffset(0)]// max 42, FF = end of directory 1	Size of filename (max. 42)
        public byte FileNameSize;

        //[FieldOffset(1)] //1	Attribute as on FAT
        public byte Attribute;

        //[FieldOffset(2)]//42	Filename in ASCII, padded with 0xff (not zero-terminated)
        //[MarshalAs(UnmanagedType.ByValArray,SizeConst=42)]
        public fixed byte FileName[42];

        //[FieldOffset(44)]//44	4	First cluster
        public UInt32 FirstCluster;

        //[FieldOffset(48)]//48	4	file size
        public Int32 FileSize;

        //[FieldOffset(52)]
        public Int32 ModDate;

        //[FieldOffset(56)]
        public Int32 CreateDate;

        //[FieldOffset(60)]
        public Int32 AccessedDate;


        public FATXEntry64(byte[] originalData)
            : this()
        {
            this.originalData = new byte[64];
            for (int x = 0; x < 64; x++)
            {
                this.originalData[x] = originalData[x];
            }

            var e = FATXEntry64.Read(originalData);

            {
                this = e;

            }
        }

        public static FATXEntry64 Invalid
        {
            get
            {
                return new FATXEntry64();
            }
        }

        [NonSerialized]
        public byte[] originalData;

        public static FATXEntry64 Read(byte[] b)
        {
            var ret = new FATXEntry64();

            if (b == null || b.Length < 64)
                return ret;

            ret.originalData = b;

            ret.FileNameSize = b[0];
            ret.Attribute = b[1];
            fixed (byte* p = b)
            {
                for (int x = 0; x < 42; x++)
                {
                    ret.FileName[x] = b[2 + x];
                }

                ret.FirstCluster = BitConv.ToUInt32(p, 44, true);
                ret.FileSize = BitConv.ToInt32(p, 48, true);
                ret.ModDate = BitConv.ToInt32(p, 52, true);
                ret.CreateDate = BitConv.ToInt32(p, 56, true);
                ret.AccessedDate = BitConv.ToInt32(p, 60, true);

            }
            return ret;
        }

        public bool IsValid
        {
            get
            {
                FATXAttribute attr = (FATXAttribute)Attribute;
                if (attr == FATXAttribute.Invalid)
                {
                    return false;
                }
                if (FileNameSize == 0xE5)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(FileNameStr))
                {
                    return false;
                }



                return true;
            }
        }


        public bool IsFolder
        {
            get
            {
                if (!IsValid)
                    return false;

                return (((FATXAttribute)Attribute) & FATXAttribute.Folder) == FATXAttribute.Folder;
            }
        }


        public bool IsFileNameValid
        {
            get
            {
                if (FileNameSize == 0 || FileNameSize == 0xE5 || FileNameSize == 0xFF || FileNameSize > 42)
                    return false;
                return true;
            }
        }


        public bool IsFile
        {
            get
            {
                return (IsValid && IsFileNameValid && !IsFolder);
            }
        }

        public bool IsDeleted
        {
            get
            {
                return FileNameSize == 0xE5;
            }
        }


        public string FileNameStr
        {
            get
            {
                if (FileNameSize == 0 || FileNameSize == 0xE5 ||
                    FileNameSize == 0xFF || FileNameSize > 42)
                    return null;


                var v = new byte[42];
                fixed (byte* p = FileName)
                {
                    for (int x = 0; x < 42; x++)
                    {
                        v[x] = p[x];
                        if (v[x] == 255)
                        {
                            v[x] = 0;
                        }
                    }
                }
                return Encoding.UTF8.GetString(v.ToArray()).Trim((char)0);

            }
        }

    }
    /// <summary>
    /// Generic entry for FATX
    /// </summary>
    public class FATXEntry
    {
        #region Variables
        //[CompilerGenerated]
        public byte xNLen;
        //[CompilerGenerated]
        public string xName;
        //[CompilerGenerated]
        public int xSize;
        //[CompilerGenerated]
        public uint xStartBlock;
        //[CompilerGenerated]
        public int xT1;
        //[CompilerGenerated]
        public int xT2;
        //[CompilerGenerated]
        public int xT3;
        //[CompilerGenerated]
        public bool xIsValid = false;
        //[CompilerGenerated]
        public bool xIsFolder = false;
        //[CompilerGenerated]
        FATXPartition xPart;
        //[CompilerGenerated]
        public long xOffset;
        //[CompilerGenerated]
        public FATXDrive xDrive;


        /// <summary>
        /// Entry Size
        /// </summary>
        public int Size
        {
            get
            {
                if (!xIsValid)
                    throw FATXExcepts.ValidExcept;
                return xSize;
            }
        }
        /// <summary>
        /// Entry Start Block
        /// </summary>
        public uint StartBlock
        {
            get
            {
                if (!xIsValid)
                    throw FATXExcepts.ValidExcept;
                return xStartBlock;
            }
        }
        /// <summary>
        /// Entry folder flag
        /// </summary>
        public bool IsFolder
        {
            get
            {
                if (!xIsValid)
                    throw FATXExcepts.ValidExcept;
                return xIsFolder;
            }
        }
        /// <summary>
        /// Entry name
        /// </summary>
        public string Name
        {
            get
            {
                if (!xIsValid)
                    throw FATXExcepts.ValidExcept;
                return xName;
            }
            set
            {
                if (value.Length > 0x2A)
                    value = value.Substring(0, 0x2A);
                xName = value;

                if (xNLen != 0xE5)
                    xNLen = (byte)value.Length;
            }
        }


        /// <summary>
        /// is a FATX partition
        /// </summary>
        public FATXPartition Partition { get { return xPart; } }
        #endregion

        public FATXEntry Parent { get; internal set; }
        public FATXEntry(FATXEntry parent, ref FATXEntry xEntry)
        {
            this.Parent = parent;
            //Debug.WriteLine("fatx entry ref");
            xOffset = xEntry.xOffset;
            xNLen = xEntry.xNLen;
            xName = xEntry.xName;
            xStartBlock = xEntry.xStartBlock;
            xSize = xEntry.xSize;
            xT1 = xEntry.xT1;
            xT2 = xEntry.xT2;
            xT3 = xEntry.xT3;
            xIsValid = xEntry.xIsValid;
            xIsFolder = xEntry.IsFolder;
            xPart = xEntry.xPart;
            xDrive = xEntry.xDrive;
            fatType = xEntry.fatType;
            this.xPart = xEntry.xPart;
            this.FatEntry = xEntry.FatEntry;
        }

        public FATXType fatType;
        public FATXEntry64 FatEntry;


        public FATXEntry(FATXType fatType, long Pos, byte[] xData, ref FATXDrive xdrive)
        {

            FatEntry = new FATXEntry64(xData);

            this.fatType = fatType;
            this.xDrive = xdrive;
            this.xIsFolder = FatEntry.Attribute != 0xFF && ((FatEntry.Attribute & 0x10) == 0x10);
            this.xName = FatEntry.FileNameStr;
            this.xNLen = FatEntry.FileNameSize;

            this.xStartBlock = FatEntry.FirstCluster;
            this.xSize = FatEntry.FileSize;

            this.xT1 = FatEntry.ModDate;
            this.xT2 = FatEntry.CreateDate;
            this.xT3 = FatEntry.AccessedDate;


            this.fatType = fatType;
            xDrive = xdrive;
            xOffset = Pos;
            xIsValid = this.FatEntry.IsValid;
            /*
            try
            {
                DJsIO xIO = new DJsIO(xData, true);
                xNLen = xIO.ReadByte();
                if (xNLen == 0xE5 || xNLen == 0xFF || xNLen == 0 || xNLen > 0x2A)
                    return;
                byte xatt = (byte)((xIO.ReadByte() >> 4) & 1);
                byte xLen = (byte)(xNLen & 0x3F);
                xName = xIO.ReadString(StringForm.ASCII, xLen);
                xName.IsValidXboxName();
                xIO.Position = 0x2C;
                xStartBlock = xIO.ReadUInt32();
                if (fatType == FATXType.FATX32)
                {
                    if (xStartBlock == Constants.FATX32End)
                        return;
                }else if (fatType == FATXType.FATX16)
                {
                    if (xStartBlock == Constants.FATX16End)
                        return;
                }
                xSize = xIO.ReadInt32();
                xT1 = xIO.ReadInt32();
                xT2 = xIO.ReadInt32();
                xT3 = xIO.ReadInt32();
                if (xatt == 1)
                    xIsFolder = true;
                else if (xSize == 0)
                    return;
                xIsValid = true;
            }
            catch { xIsValid = false; }*/
        }

        public FATXEntry(FATXEntry parent, string xNameIn, uint xStart, int xSizeIn, long xPosition, bool xFolder, ref FATXDrive xdrive)
        {
            Debug.WriteLine("fatx string " + xNameIn);
            int DT = TimeStamps.FatTimeInt(DateTime.Now);
            xT1 = DT;
            xT2 = DT;
            xT3 = DT;
            Name = xNameIn;
            xStartBlock = xStart;
            xSize = (xIsFolder = xFolder) ? 0 : xSizeIn;
            xOffset = xPosition;
            xIsValid = true;
            xDrive = xdrive;


        }

        public void SetAtts(FATXPartition Part)
        {
            xPart = Part;
            fatType = xPart.FatType;
        }

        public byte[] GetData()
        {
            List<byte> xArray = new List<byte>();

            xArray.Add(xNLen);
            var attr = (byte)((IsFolder ? 1 : 0) << 4);
            xArray.Add(attr);

            var name = Encoding.UTF8.GetBytes(xName);
            xArray.AddRange(name);
            var blen = 0x2A - xName.Length;
            if (blen > 0)
            {

                var b = new byte[blen];

                for (int x = 0; x < blen; x++)
                {
                    b[x] = 0xFF;
                }
                xArray.AddRange(b);
            }
            xArray.AddRange(BitConv.GetBytes(xStartBlock, true));
            xArray.AddRange(BitConv.GetBytes(xSize, true));
            xArray.AddRange(BitConv.GetBytes(xT1, true));
            xArray.AddRange(BitConv.GetBytes(xT2, true));
            xArray.AddRange(BitConv.GetBytes(xT3, true));
            var ret = xArray.ToArray();


            return ret;
        }

        public bool xWriteEntry()
        {
            try
            {
                if (xT1 == 0)
                {
                    xT1 = X360.Other.TimeStamps.FatTimeInt(DateTime.Now);
                }
                xT2 = X360.Other.TimeStamps.FatTimeInt(DateTime.Now);
                xT3 = X360.Other.TimeStamps.FatTimeInt(DateTime.Now);
                byte[] xdata = GetData();

                xDrive.GetIO();
                xDrive.xIO.Position = xOffset;
                xDrive.xIO.Write(xdata);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Writes the entry data
        /// </summary>
        /// <returns></returns>
        public bool WriteEntry()
        {
            if (xDrive.ActiveCheck())
                return false;
            return (xWriteEntry() & !(xDrive.xActive = false));
        }

    }

    /// <summary>
    /// Object to hold FATX File Entry
    /// </summary>
    public sealed class FATXFileEntry : FATXEntry
    {
        
        public FATXFileEntry(FATXFolderEntry folder, FATXEntry x)
            : base(folder, ref x) {  }

        public override string ToString()
        {
            string ret = Name;
            if (!string.IsNullOrEmpty(ret))
            {
                int i = 48 - ret.Length;
                for (int x = 0; x < i; x++)
                    ret += " ";

                var dt = TimeStamps.FatTimeDT(this.xT1);

                ret += dt.ToString();
            }
            return ret;
        }

        /// <summary>
        /// Overwrite the file
        /// </summary>
        /// <param name="FileIn"></param>
        /// <returns></returns>
        public bool Inject(string FileIn)
        {
            if (xDrive.ActiveCheck())
                return false;
            DJsIO xIOIn = null;
            try { xIOIn = new DJsIO(FileIn, DJFileMode.Open, true); }
            catch { return (xDrive.xActive = false); }
            if (xIOIn == null || !xIOIn.Accessed)
                return (xDrive.xActive = false);
            try { return xInject(xIOIn) & !(xDrive.xActive = false); }
            catch { xIOIn.Close(); return (xDrive.xActive = false); }
        }

        public bool xInject(DJsIO xIOIn)
        {
            List<uint> blocks = new List<uint>(Partition.xTable.GetBlocks(xStartBlock));
            if (blocks.Count == 0)
                throw new Exception();

            uint numBlocksInFile = xIOIn.BlockCountFATX(Partition);
            if (blocks.Count < numBlocksInFile)
            {
                uint[] blocks2 = Partition.xTable.GetNewBlockChain((uint)(numBlocksInFile - blocks.Count), 1);
                if (blocks2.Length == 0)
                    throw new Exception();
                blocks.AddRange(blocks2);
                uint[] x = blocks.ToArray();
                if (!Partition.xTable.WriteChain(ref x))
                    throw new Exception();
            }
            else if (blocks.Count > numBlocksInFile)
            {
                uint[] xUnneeded = new uint[blocks.Count - numBlocksInFile];
                for (uint i = numBlocksInFile; i < blocks.Count; i++)
                {
                    xUnneeded[(int)i] = i;
                    blocks.RemoveAt((int)i--);
                }
                if (!Partition.xTable.DeleteChain(ref xUnneeded))
                    throw new Exception();
            }
            xIOIn.Position = 0;
            xDrive.GetIO();
            foreach (uint i in blocks)
            {
                xDrive.xIO.Position = Partition.BlockToOffset(i);
                var bytes = xIOIn.ReadBytes(Partition.xBlockSize);
                xDrive.xIO.Write(bytes);
            }
            if ((xSize == 0 || (uint)(((xSize - 1) / Partition.xBlockSize) + 1) != numBlocksInFile) &&
                !Partition.WriteAllocTable())
                throw new Exception();
            xSize = (int)xIOIn.Length;
            xIOIn.Close();
            return xWriteEntry();
        }

        /// <summary>
        /// Replace the file
        /// </summary>
        /// <param name="FileIn"></param>
        /// <returns></returns>
        public bool Replace(string FileIn)
        {
            if (xDrive.ActiveCheck())
                return false;
            DJsIO xIOIn = null;
            try { xIOIn = new DJsIO(FileIn, DJFileMode.Open, true); }
            catch { return (xDrive.xActive = false); }
            if (xIOIn == null || !xIOIn.Accessed)
                return (xDrive.xActive = false);
            return xReplace(xIOIn) & !(xDrive.xActive = false);
        }

        public bool xReplace(DJsIO xIOIn)
        {
            uint bu = xStartBlock;
            int size = xSize;
            try
            {
                uint[] curblocks = Partition.xTable.GetBlocks(xStartBlock);
                var blockCount = xIOIn.BlockCountFATX(Partition);

                uint[] blocks = Partition.xTable.GetNewBlockChain(blockCount, 1);
                if (blocks.Length == 0)
                    throw new Exception();
                if (!Partition.xTable.WriteChain(ref blocks))
                    throw new Exception();
                if (!Partition.xTable.DeleteChain(ref curblocks))
                    throw new Exception();
                xIOIn.Position = 0;
                xDrive.GetIO();
                if (!Partition.WriteFile(blocks, ref xIOIn))
                    throw new Exception();
                if (!Partition.WriteAllocTable())
                    throw new Exception();

                base.xStartBlock = blocks[0];
                base.xSize = (int)xIOIn.Length;
                xIOIn.Close();
                return xWriteEntry();
            }
            catch { xIOIn.Close(); base.xStartBlock = bu; base.xSize = size; return false; }
        }

        /// <summary>
        /// Delete the file
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            if (xDrive.ActiveCheck())
                return false;
            try
            {
                uint[] blocks = Partition.xTable.GetBlocks(xStartBlock);

                if (blocks.Length == 0)
                {
                    return (xDrive.xActive = false);
                }
                if (!Partition.xTable.DeleteChain(ref blocks))
                {
                    return (xDrive.xActive = false);
                }
                if (!Partition.WriteAllocTable())
                {
                    return (xDrive.xActive = false);
                }

                xNLen = 0xE5;

                if (!xWriteEntry())
                {
                    return (xDrive.xActive = false);
                }

                return !(xDrive.xActive = false);
            }
            catch { return (xDrive.xActive = false); }
        }

        public bool xExtract(ref DJsIO xIOOut)
        {
            try
            {
                xIOOut.Position = 0;
                uint[] xChain = Partition.xTable.GetBlocks(xStartBlock);
                uint xct = (uint)(((xSize - 1) / Partition.xBlockSize) + 1);
                if (xChain.Length < xct)
                    return false;
                xDrive.GetIO();

                for (uint i = 0; i < xct - 1; i++)
                {
                    xDrive.xIO.Position = Partition.BlockToOffset(xChain[(int)i]);
                    var bytes = xDrive.xIO.ReadBytes(Partition.xBlockSize);
                    xIOOut.Write(bytes);
                }
                int xleft = (int)(((xSize - 1) % Partition.xBlockSize) + 1);
                xDrive.xIO.Position = Partition.BlockToOffset(xChain[(int)xct - 1]);
                xIOOut.Write(xDrive.xIO.ReadBytes(xleft));
                xIOOut.Flush();
                return true;
            }
            catch { return false; }
        }

        public FATXStreamIO GetFatXStream()
        {
            try
            {

                uint[] xChain = Partition.xTable.GetBlocks(xStartBlock);
                uint xct = (uint)(((xSize - 1) / Partition.xBlockSize) + 1);
                if (xChain.Length < xct)
                    return null;


                FATXStreamIO io = new FATXStreamIO(this, ref xChain, true);

                return io;

            }
            catch { return null; }
        }

        /// <summary>
        /// Extract the file
        /// </summary>
        /// <param name="OutLocation"></param>
        /// <returns></returns>
        public bool Extract(string OutLocation)
        {
            if (xDrive.ActiveCheck())
                return false;
            bool xReturn = false;
            DJsIO xIO = new DJsIO(true);
            try
            {
                xReturn = xExtract(ref xIO);
                xIO.Close();
                if (xReturn)
                    xReturn = VariousFunctions.MoveFile(xIO.FileNameLong, OutLocation);
            }
            catch
            {
                xIO.Close();
                xReturn = false;
            }
            VariousFunctions.DeleteFile(xIO.FileNameLong);
            xDrive.xActive = false;
            return xReturn;
        }

        public byte[] xExtractBytes()
        {
            byte[] ret = null;

            var xIO = new DJsIO(true);
            try
            {
                if (xExtract(ref xIO))
                {
                    xIO.Position = 0;
                    ret = xIO.GetBytes();
                }
            }
            catch
            {
                ret = null;
            }
            xIO.Close();

            return ret;
        }

        /// <summary>
        /// Grabs the STFS name of the package
        /// </summary>
        /// <returns></returns>
        public string GetSTFSName()
        {
            if (xDrive.ActiveCheck())
                return null;
            string xReturn = null;
            try
            {
                if (xSize < 0x500)
                    throw new Exception();
                xDrive.GetIO();
                uint[] blocks = Partition.xTable.GetBlocks(xStartBlock);
                if (blocks.Length == 0)
                    throw new Exception();
                xDrive.xActive = false;
                FATXStreamIO io = new FATXStreamIO(this, ref blocks, true);
                uint xBuff = io.ReadUInt32();
                if (xBuff != (uint)STFS.PackageMagic.CON &&
                    xBuff != (uint)STFS.PackageMagic.LIVE &&
                    xBuff != (uint)STFS.PackageMagic.PIRS)
                    throw new Exception();
                io.Position = 0x411;
                xReturn = io.ReadString(StringForm.Unicode, 0x80);
                io.Position = 0x340;
                byte xbase = (byte)(((io.ReadUInt32() + 0xFFF) & 0xF000) >> 0xC);
                if (io.ReadUInt32() != (uint)STFS.PackageType.Profile)
                    throw new Exception();
                io.Position = 0x379;
                if (io.ReadByte() != 0x24 || io.ReadByte() != 0)
                    throw new Exception();
                byte idx = (byte)(io.ReadByte() & 3);
                byte[] Desc = io.ReadBytes(5);
                if (idx == 0 || idx == 2)
                {
                    if (xbase != 0xA)
                        throw new Exception();
                }
                else if (idx == 1)
                {
                    if (xbase != 0xB)
                        throw new Exception();
                }
                else
                    throw new Exception();
                io.Position = 0x395;
                STFS.STFSDescriptor xDesc = new X360.STFS.STFSDescriptor(Desc, io.ReadUInt32(), io.ReadUInt32(), idx);
                int pos = (int)xDesc.GenerateDataOffset(xDesc.DirectoryBlock);
                uint block = xDesc.DirectoryBlock;
                while (pos != -1)
                {
                    for (int i = 0; i < 0x40; i++)
                    {
                        if (pos == -1)
                            break;
                        io.Position = pos + 0x28 + (0x40 * i);
                        byte nlen = (byte)(io.ReadByte() & 0x3F);
                        if (nlen > 0x28)
                            nlen = 0x28;
                        io.Position = pos + (0x40 * i);
                        if (io.ReadString(StringForm.ASCII, nlen) == "Account")
                        {
                            io.Position = pos + (0x40 * i) + 0x2F;
                            List<byte> buff = new List<byte>(io.ReadBytes(3));
                            buff.Add(0);
                            block = BitConv.ToUInt32(buff.ToArray(), false);
                            pos = -1;
                        }
                    }
                    if (pos != -1)
                    {
                        byte shift = xDesc.TopRecord.Index;
                        if (xDesc.BlockCount >= Constants.BlockLevel[1])
                        {
                            io.Position = (int)xDesc.GenerateHashOffset(block, X360.STFS.TreeLevel.L2) + 0x14 +
                                (shift << 0xC);
                            shift = (byte)((io.ReadByte() >> 6) & 1);
                        }
                        if (xDesc.BlockCount >= Constants.BlockLevel[0])
                        {
                            io.Position = (int)xDesc.GenerateHashOffset(block, X360.STFS.TreeLevel.L1) + 0x14 +
                                (xDesc.ThisType == STFS.STFSType.Type0 ? 0 : (shift << 0xC));
                            shift = (byte)((io.ReadByte() >> 6) & 1);
                        }
                        io.Position = (int)xDesc.GenerateHashOffset(block, X360.STFS.TreeLevel.L0) + 0x15 +
                                (xDesc.ThisType == STFS.STFSType.Type0 ? 0 : (shift << 0xC));
                        List<byte> xbuff = new List<byte>(io.ReadBytes(3));
                        xbuff.Reverse();
                        xbuff.Insert(0, 3);
                        block = BitConv.ToUInt32(xbuff.ToArray(), true);
                        if (block == Constants.STFSEnd)
                            pos = -1;
                    }
                }
                if (block == 0xFFFFFF)
                    throw new Exception();
                io.Position = (int)xDesc.GenerateDataOffset(block);
                byte[] databuff = io.ReadBytes(404);
                Profile.UserAccount ua = new X360.Profile.UserAccount(new DJsIO(databuff, true), X360.Profile.AccountType.Stock, false);
                if (!ua.Success)
                {
                    ua = new X360.Profile.UserAccount(new DJsIO(databuff, true), X360.Profile.AccountType.Kits, false);
                    if (!ua.Success)
                        throw new Exception();
                }
                xReturn = ua.GetGamertag();
                io.Close();
                xDrive.xActive = false;
                return xReturn;
            }
            catch { xDrive.xActive = false; return xReturn; }
        }
    }

    /// <summary>
    /// Object to hold contents of a read folder
    /// </summary>
    public sealed class FATXReadContents
    {
        //[CompilerGenerated]
        public List<FATXFolderEntry> xfolds;
        //[CompilerGenerated]
        public List<FATXFileEntry> xfiles;
        //[CompilerGenerated]
        public List<FATXPartition> xsubparts = new List<FATXPartition>();

        /// <summary>
        /// Files
        /// </summary>
        public FATXFileEntry[] Files { get { return xfiles.ToArray(); } }
        /// <summary>
        /// Folders
        /// </summary>
        public FATXFolderEntry[] Folders { get { return xfolds.ToArray(); } }
        /// <summary>
        /// Subpartitions
        /// </summary>
        public FATXPartition[] SubPartitions { get { return xsubparts.ToArray(); } }

        public FATXReadContents() { }
    }

    /// <summary>
    /// Object to hold FATX Folder
    /// </summary>
    public sealed class FATXFolderEntry : FATXEntry
    {
        public FATXFolderEntry(FATXEntry parent, FATXEntry xEntry, string path) : base(parent, ref xEntry) { this.Path = path; }

        
        public string Path;
        /// <summary>
        /// Reads the contents
        /// </summary>
        /// <returns></returns>
        public FATXReadContents Read()
        {
            if (xDrive.ActiveCheck())
                return null;
            FATXReadContents xReturn = xRead();
            xDrive.xActive = false;
            return xReturn;
        }

        public bool Delete()
        {
            if (xDrive.ActiveCheck())
                return false;

            var xReturn = xDelete();

            xDrive.xActive = false;
            return xReturn;
        }

        public bool xDelete()
        {
            bool ret = false;
            try
            {
                xDrive.GetIO();

                if (!Partition.xTable.xAllocTable.Accessed)
                {
                    return ret;
                }

                xNLen = 0xE5;

                uint[] blocks = Partition.xTable.GetBlocks(xStartBlock);

                if (blocks.Length == 0 ||
                    !Partition.xTable.DeleteChain(ref blocks) ||
                    !Partition.WriteAllocTable())
                {
                    return (xDrive.xActive = false);
                }

                if (!xWriteEntry())
                {
                    return (xDrive.xActive = false);
                }

                return !(xDrive.xActive = false);
            }
            catch { }
            return ret;
        }
        public FATXReadContents xRead()
        {
            FATXReadContents xreturn = new FATXReadContents();
            try
            {
                xDrive.GetIO();

                if (!Partition.xTable.xAllocTable.Accessed)
                {
                    return null;
                }
                List<FATXEntry> xEntries = new List<FATXEntry>();
                uint[] xBlocks = Partition.xTable.GetBlocks(xStartBlock);

                for (int i = 0; i < xBlocks.Length; i++)
                {
                    long xCurrent = Partition.BlockToOffset(xBlocks[i]);
                    if (xCurrent == -1)
                        break;

                    for (int x = 0; x < Partition.xEntryCount; x++)
                    {
                        xDrive.xIO.Position = xCurrent + (0x40 * x);
                        FATXEntry z = new FATXEntry(Partition.FatType,
                            (xCurrent + (0x40 * x)),
                            xDrive.xIO.ReadBytes(0x40),
                            ref xDrive);

                        z.SetAtts(Partition);

                        if (z.xIsValid)
                        {
                            xEntries.Add(z);
                        }
                        else
                        {
                            if (z.xNLen != 0xE5)
                                break;
                        }
                    }
                }
                xreturn.xfolds = new List<FATXFolderEntry>();
                xreturn.xfiles = new List<FATXFileEntry>();
                for (int i = 0; i < xEntries.Count; i++)
                {
                    if (xEntries[i].IsFolder)
                    {
                        if (string.Compare(xEntries[i].Name, this.Name, true) != 0)
                        {
                            var f = new FATXFolderEntry(this, xEntries[i], Path + "/" + xEntries[i].Name);

                            xreturn.xfolds.Add(f);
                        }
                    }
                    else
                    {
                        var f = new FATXFileEntry(this, xEntries[i]);

                        xreturn.xfiles.Add(f);
                    }
                }
                return xreturn;
            }
            catch { return (xreturn = null); }
        }

        /// <summary>
        /// Gets a location for a new entry
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        long GetNewEntryPos(out uint block)
        {
            block = 0;
            List<uint> xFileBlocks = new List<uint>(Partition.xTable.GetBlocks(xStartBlock));
            xDrive.GetIO();
            // Searches current allocated blocks
            for (int x = 0; x < xFileBlocks.Count; x++)
            {
                var fileBlock = xFileBlocks[x];
                long xCurOffset = Partition.BlockToOffset(fileBlock);
                for (int i = 0; i < Partition.xEntryCount; i++)
                {
                    xDrive.xIO.Position = xCurOffset + (0x40 * i);
                    byte xCheck = xDrive.xIO.ReadByte();
                    if (xCheck == 0 || xCheck > 0x2A || xCheck == 0xFF)
                    {
                        return --xDrive.xIO.Position;
                    }
                }
            }


            uint[] xBlock = Partition.xTable.GetNewBlockChain(1, 1);
            if (xBlock.Length > 0)
            {
                // Nulls out a new block and returns the start of the new block
                xDrive.xIO.Position = Partition.BlockToOffset(xBlock[0]);

                byte[] xnull = new byte[Partition.xBlockSize];
                for (int x = 0; x < xnull.Length; x++)
                {
                    xnull[x] = 0x00;
                }
                xDrive.xIO.Write(xnull);
                xFileBlocks.Add(xBlock[0]);
                block = xBlock[0];
                return Partition.BlockToOffset(xBlock[0]); // Returns the beginning of the allocated block
            }
            return -1;
        }

        /* Note: Have plans for safer and better manipulation to prevent
         * minimal block loss to human error */

        /// <summary>
        /// Adds a folder
        /// </summary>
        /// <param name="FolderName"></param>
        /// <returns></returns>
        public bool AddFolder(string FolderName)
        {
            if (!VariousFunctions.IsValidXboxName(FolderName))
                return false;

            if (xDrive.ActiveCheck())
                return false;
            try
            {
                FATXReadContents xconts = xRead();
                if (xconts == null)
                    return false;
                foreach (FATXFolderEntry x in xconts.xfolds)
                {
                    if (string.Compare(x.Name , FolderName,true)==0)
                        return (xDrive.xActive = false);
                }

                var b = new byte[Partition.xBlockSize];
                for (int x = 0; x < 4; x++)
                    b[x] = 0x00;

                for (int x = 4; x < b.Length; x++)
                {
                    b[x] = 0xFF;
                }
                DJsIO xIOIn = new DJsIO(b, true);
                uint xnew = 0;
                long xpos = GetNewEntryPos(out xnew);
                if (xpos == -1)
                    return (xDrive.xActive = false);

                var blockCount = xIOIn.BlockCountFATX(Partition);
                var xnewIndex = xnew + 1;

                uint[] blocks = Partition.xTable.GetNewBlockChain(blockCount, xnewIndex);
                if (blocks.Length == 0)
                    return (xDrive.xActive = false);

                if (!Partition.WriteFile(blocks, ref xIOIn))
                    return (xDrive.xActive = false);

                FATXEntry y = new FATXEntry(this, FolderName, blocks[0],
                    (int)xIOIn.Length, xpos, true, ref xDrive);

                //y.FatEntry = new FATXEntry64(xData);

                y.SetAtts(this.Partition);
                if (!y.xWriteEntry())
                    return (xDrive.xActive = false);

                if (xnew > 0)
                {
                    var fileblocks = Partition.xTable.GetBlocks(xStartBlock).ToList();
                    fileblocks.Add(xnew);

                    uint[] xtemp = fileblocks.ToArray();

                    if (!Partition.xTable.WriteChain(ref xtemp))
                        return (xDrive.xActive = false);
                }

                if (!Partition.xTable.WriteChain(ref blocks))
                    return (xDrive.xActive = false);

                if (Partition.WriteAllocTable())
                    return !(xDrive.xActive = false);

                return (xDrive.xActive = false);
            }
            catch { return xDrive.xActive = false; }
        }

        /// <summary>
        /// Adds a file
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="FileLocation"></param>
        /// <param name="xType"></param>
        /// <returns></returns>
        /// 
        public bool AddFile(string FileName, string FileLocation, AddType xType)
        {
            
            try
            {
                byte[] b = File.ReadAllBytes(FileLocation);

                if (b != null)
                {
                    return AddFile(FileName, b, xType);
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }

        }

        public bool AddFile(string FileName, byte[] fileData, AddType xType)
        {
            
            if (!VariousFunctions.IsValidXboxName(FileName))
                return false;

            if (xDrive.ActiveCheck())
                return false;

            DJsIO xIOIn = null;
            byte[] b = fileData;
            xIOIn = new DJsIO(b, true);

            try
            {
                FATXReadContents xconts = xRead();
                foreach (FATXFileEntry x in xconts.xfiles)
                {
                    if (string.Compare(x.Name, FileName, true) == 0)
                    {
                        bool xreturn = false;
                        if (xType == AddType.NoOverWrite)
                        {
                            xIOIn.Close();
                            return (xDrive.xActive = false);
                        }
                        else if (xType == AddType.Inject)
                        {
                            xreturn = x.xInject(xIOIn);
                        }
                        else
                        {
                            xreturn = x.xReplace(xIOIn);
                        }
                        xIOIn.Close();
                        return (xreturn & !(xDrive.xActive = false));
                    }
                }
                uint xnew = 0;
                long xpos = GetNewEntryPos(out xnew);
                if (xpos == -1)
                    return (xDrive.xActive = false);

                var count = xIOIn.BlockCountFATX(Partition);

                uint[] blocks = Partition.xTable.GetNewBlockChain(count, xnew + 1);

                if (blocks.Length == 0)
                    return (xDrive.xActive = false);

                if (!Partition.WriteFile(blocks, ref xIOIn))
                    return (xDrive.xActive = false);

                FATXEntry y = new FATXEntry(this,
                    FileName,
                    blocks[0], (int)xIOIn.Length,
                    xpos, false, ref xDrive);

                if (!y.xWriteEntry())
                    return (xDrive.xActive = false);

                if (xnew > 0)
                {
                    var filebx = Partition.xTable.GetBlocks(xStartBlock);
                    List<uint> fileblocks = new List<uint>(filebx);

                    fileblocks.Add(xnew);

                    uint[] xtemp = fileblocks.ToArray();

                    if (!Partition.xTable.WriteChain(ref xtemp))
                        return (xDrive.xActive = false);
                }

                if (!Partition.xTable.WriteChain(ref blocks))
                    return (xDrive.xActive = false);

                if (Partition.WriteAllocTable())
                    return !(xDrive.xActive = false);


                return (xDrive.xActive = false);
            }
            catch { xIOIn.Close(); return (xDrive.xActive = false); }
            
        }

        bool xExtract(string xOut, bool Sub)
        {
            if (!VariousFunctions.xCheckDirectory(xOut))
                return false;
            FATXReadContents xread = xRead();
            if (xread == null)
                return false;
            foreach (FATXFileEntry x in xread.Files)
            {
                DJsIO xIOOut = new DJsIO(xOut + "/" + x.Name, DJFileMode.Create, true);
                if (!xIOOut.Accessed)
                    continue;
                x.xExtract(ref xIOOut);
                xIOOut.Dispose();
            }
            if (!Sub)
                return true;
            foreach (FATXFolderEntry x in xread.Folders)
                x.xExtract(xOut + "/" + x.Name, Sub);
            return true;
        }

        /// <summary>
        /// Extracts a file
        /// </summary>
        /// <param name="xOutPath"></param>
        /// <param name="IncludeSubFolders"></param>
        /// <returns></returns>
        public bool Extract(string xOutPath, bool IncludeSubFolders)
        {
            if (xDrive.ActiveCheck())
                return false;
            xOutPath = xOutPath.Replace('\\', '/');
            if (xOutPath[xOutPath.Length - 1] == '/')
                xOutPath += xName;
            else
                xOutPath += "/" + xName;
            return (xExtract(xOutPath, IncludeSubFolders) &
                !(xDrive.xActive = false));
        }
    }

    /// <summary>
    /// Object to hold a FATX partition
    /// </summary>
    public sealed class FATXPartition
    {
        #region Variables
        //[CompilerGenerated]
        long xBase;
        //[CompilerGenerated]
        public int xFATSize;
        //[CompilerGenerated]
        uint SectorsPerBlock;
        //[CompilerGenerated]
        public List<FATXFolderEntry> xFolders;
        //[CompilerGenerated]
        public List<FATXFileEntry> xFiles;
        //[CompilerGenerated]
        public FATXType FatType = FATXType.None;
        //[CompilerGenerated]
        public FATXDrive xdrive;
        //[CompilerGenerated]
        public AllocationTable xTable;
        //[CompilerGenerated]
        public List<FATXPartition> xExtParts;
        //[CompilerGenerated]
        string xName;

        /// <summary>
        /// Folders
        /// </summary>
        public FATXFolderEntry[] Folders { get { return xFolders.ToArray(); } }
        /// <summary>
        /// Files in the partition
        /// </summary>
        public FATXFileEntry[] Files { get { return xFiles.ToArray(); } }
        /// <summary>
        /// Subpartitions
        /// </summary>
        public FATXPartition[] SubPartitions { get { return xExtParts.ToArray(); } }

        public long xFATLocale { get { return xBase + 0x1000; } }

        long xDataStart { get { return xBase + 0x1000 + xFATSize; } }

        public int xBlockSize { get { return (int)(SectorsPerBlock * xdrive.SectorSize); } }

        public short xEntryCount { get { return (short)(xBlockSize / 0x40); } }

        /// <summary>
        /// Valid instance
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (xFolders != null &&
                        xFiles != null &&
                        (xFolders.Count + xFiles.Count != 0));
            }
        }

        /// <summary>
        /// Partition name
        /// </summary>
        public string PartitionName { get { return xName; } }
        #endregion

        public FATXPartition(long xOffset, long xPartitionSize,
            FATXDrive xDrive,
            string xLocaleName)
        {

            xdrive = xDrive;
            xName = xLocaleName;
            xBase = xOffset;
            xDrive.GetIO();
            xDrive.xIO.IsBigEndian = true;
            xDrive.xIO.Position = xOffset;

            string fatX = Encoding.ASCII.GetString(xdrive.xIO.ReadBytes(4).Reverse().ToArray());
            if (fatX != "FATX")
                return;

            //if (xDrive.xIO.ReadUInt32() != 0x58544146)
            //  return;
            var VolumeID = xDrive.xIO.ReadUInt32(); // Partition ID (884418784)

            SectorsPerBlock = xDrive.xIO.ReadUInt32();//Cluster size in (512 byte) sectors

            uint blockct = (uint)(xPartitionSize / xBlockSize);

            if (blockct < 0xFFF5 && xLocaleName != "Content")
                FatType = FATXType.FATX16;
            else
                FatType = FATXType.FATX32;

            uint dirblock = (uint)xdrive.xIO.ReadUInt32(); //Number of FAT copies

            xFATSize = (int)(blockct * (byte)FatType);
            xFATSize += (0x1000 - (xFATSize % 0x1000));

            xTable = new AllocationTable(new DJsIO(true),
                (uint)((xPartitionSize - 0x1000 - xFATSize) / xBlockSize), FatType);

            xTable.xAllocTable.Position = 0;
            xDrive.xIO.Position = xFATLocale;
            for (int i = 0; i < xFATSize; i += 0x1000)
            {
                var blockBytes = xdrive.xIO.ReadBytes(0x1000);
                xTable.xAllocTable.Write(blockBytes);
            }

            xTable.xAllocTable.Flush();

            long DirOffset = BlockToOffset(dirblock);

            xFolders = new List<FATXFolderEntry>();
            xFiles = new List<FATXFileEntry>();
            var xEntries = new List<FATXEntry>();

            for (byte x = 0; x < xEntryCount; x++)
            {
                var offset = (DirOffset + (0x40 * x));
                xDrive.xIO.Position = offset;

                var entry64 = xdrive.xIO.ReadBytes(0x40);

                FATXEntry z = new FATXEntry(FatType, offset, entry64, ref xdrive);
                z.SetAtts(this);

                if (z.xIsValid)
                {
                    xEntries.Add(z);
                }
                else if (z.xNLen == 0xE5)
                {
                }
                else if (z.xNLen != 0xE5)
                {
                    break;
                }
            }

            foreach (FATXEntry x in xEntries)
            {
                if (x.IsFolder)
                {
                    xFolders.Add(new FATXFolderEntry(null, x, this.PartitionName + "/" + x.Name));
                }
                else
                {
                    xFiles.Add(new FATXFileEntry(null, x));
                }
            }

            xExtParts = new List<FATXPartition>();
            for (int i = 0; i < xFiles.Count; i++)
            {
                if (xFiles[i].Name.ToLower() != "extendedsystem.partition")
                    continue;

                var x = new FATXPartition(
                    BlockToOffset(xFiles[i].StartBlock),
                    xFiles[i].Size, xdrive, xFiles[i].Name);

                if (!x.IsValid)
                    continue;

                xExtParts.Add(x);
                xFiles.RemoveAt(i--);
            }
        }

        public long BlockToOffset(uint xBlock)
        {
            if (FatType == FATXType.FATX16)
            {
                ushort bl = (ushort)xBlock;
                if (bl == Constants.FATX16End || xBlock == 0 || xBlock >= xTable.BlockCount)
                {
                    return -1;
                }
                else
                {
                    var blockOffset = ((xBlock - 1) * (long)xBlockSize);

                    long ret = xDataStart + blockOffset;
                    return ret;
                }
            }
            else if (FatType == FATXType.FATX32)
            {
                if (xBlock == Constants.FATX32End || xBlock == 0 || xBlock >= xTable.BlockCount)
                {
                    return -1;
                }
                else
                {
                    var blockOffset = ((xBlock - 1) * (long)xBlockSize);

                    long ret = xDataStart + blockOffset;
                    return ret;
                }
            }
            else
            {
                return -1;
            }

        }

        public bool WriteFile(uint[] xChain, ref DJsIO xIOIn)
        {
            try
            {
                xdrive.GetIO();
                for (int i = 0; i < xChain.Length; i++)
                {
                    xdrive.xIO.Position = BlockToOffset(xChain[i]);

                    xIOIn.Position = (i * xBlockSize);
                    var b = xIOIn.ReadBytes(xBlockSize);

                    xdrive.xIO.Write(b);
                    xdrive.xIO.Flush();
                }
                return true;
            }
            catch { return false; }
        }

        public void RestoreAllocTable()
        {
            string xfn = xTable.xAllocTable.FileNameLong;
            xTable.xAllocTable.Close();
            xTable.xAllocTable = new DJsIO(xfn, DJFileMode.Create, true);
            xdrive.GetIO();
            xdrive.xIO.Position = xFATLocale;
            var bytes = xdrive.xIO.ReadBytes(xFATSize);
            xTable.xAllocTable.Write(bytes);
            xTable.xAllocTable.Flush();
        }

        public bool WriteAllocTable()
        {
            try
            {
                xdrive.GetIO();
                xdrive.xIO.Position = xFATLocale;
                var count = (((xFATSize - 1) / xdrive.SectorSize) + 1);

                for (int i = 0; i < count; i++)
                {
                    xTable.xAllocTable.Position = i * xdrive.SectorSize;
                    var b = xTable.xAllocTable.ReadBytes((int)xdrive.SectorSize);

                    xdrive.xIO.Write(b);
                    xdrive.xIO.Flush();
                }
                return true;
            }
            catch { return false; }
        }
    }

    public class AllocationTable
    {
        //[CompilerGenerated]
        public DJsIO xAllocTable;
        //[CompilerGenerated]
        public uint BlockCount;
        //[CompilerGenerated]
        FATXType PartitionType;

        public AllocationTable(DJsIO xIOIn, uint xCount, FATXType xType)
        {
            xAllocTable = xIOIn;
            BlockCount = xCount;
            PartitionType = xType;
        }

        public bool DeleteChain(ref uint[] xChain)
        {
            if (PartitionType == FATXType.None)
                return false;
            try
            {
                for (int i = 0; i < xChain.Length; i++)
                {
                    if (xChain[i] >= BlockCount || xChain[i] == 0)
                        continue;
                    for (int x = 0; x < (byte)PartitionType; x++)
                        xAllocTable.Write((byte)0);
                }
                return true;
            }
            catch { return false; }
        }

        uint GetNextBlock(uint xBlock)
        {
            if (PartitionType == FATXType.None)
                return Constants.FATX32End;

            xAllocTable.Position = (xBlock * (byte)PartitionType);
            List<byte> xList = xAllocTable.ReadBytes((byte)PartitionType).ToList();
            for (int i = (int)PartitionType; i < 4; i++)
                xList.Insert(0, 0);
            return BitConv.ToUInt32(xList.ToArray(), true);
        }

        public uint[] GetBlocks(uint xBlock)
        {
            List<uint> xReturn = new List<uint>();
            while (xBlock < BlockCount && xBlock != 0)
            {
                switch (PartitionType)
                {
                    case FATXType.FATX16:
                        if (xBlock == Constants.FATX16End)
                            return xReturn.ToArray();
                        break;
                    case FATXType.FATX32:
                        if (xBlock == Constants.FATX32End)
                            return xReturn.ToArray();
                        break;
                    default:
                        return xReturn.ToArray();
                }
                if (!xReturn.Contains(xBlock))
                    xReturn.Add(xBlock);
                else
                    break;
                xBlock = GetNextBlock(xBlock);
            }
            return xReturn.ToArray();
        }

        public bool WriteChain(ref uint[] xChain)
        {
            if (PartitionType == FATXType.None)
                return false;
            try
            {
                for (int i = 0; i < xChain.Length; i++)
                {
                    xAllocTable.Position = (xChain[i] * (byte)PartitionType);

                    bool atEnd = (i == xChain.Length - 1);

                    if (!atEnd)
                    {
                        if (PartitionType == FATXType.FATX16)
                        {
                            uint xc = xChain[i + 1];
                            ushort xcs = (ushort)xc;
                            xAllocTable.Write(xcs);
                        }
                        else
                        {
                            uint xc = xChain[i + 1];
                            xAllocTable.Write(xc);
                        }
                    }
                    else
                    {
                        if (PartitionType == FATXType.FATX16)
                        {
                            xAllocTable.Write(Constants.FATX16End);
                        }
                        else
                        {
                            xAllocTable.Write(Constants.FATX32End);
                        }
                    }
                }

                return true;
            }
            catch { return false; }
        }

        public uint[] GetNewBlockChain(uint xCount, uint xBlockStart)
        {
            List<uint> xReturn = new List<uint>();
            for (uint i = xBlockStart; i < BlockCount; i++)
            {
                if (xReturn.Count == xCount)
                    return xReturn.ToArray();
                xAllocTable.Position = ((byte)PartitionType * i);
                uint xCheck = Constants.FATX32End;
                switch (PartitionType)
                {
                    case FATXType.FATX16:
                        xCheck = xAllocTable.ReadUInt16();
                        break;

                    case FATXType.FATX32:
                        xCheck = xAllocTable.ReadUInt32();
                        break;

                    default:
                        break;
                }

                if (xCheck == 0)
                {
                    xReturn.Add(i);
                }
            }
            return new uint[0];
        }
    }
}