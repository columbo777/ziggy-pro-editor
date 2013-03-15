﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Drawing;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using X360.FATX;
using X360.Other;
using X360.STFS;
using X360.SVOD;
using System.Threading;

namespace X360.IO
{
    public class HookedStream : DJsIO
    {
        public override byte[] ReadBytes(int xSize)
        {
            return base.ReadBytes(xSize);
        }

        public override byte[] ReadStream()
        {
            return base.ReadStream();
        }

        public override void Write(byte[] xIn)
        {
            base.Write(xIn);
        }
    }
}

#region Extensions
namespace X360.IO.FATXExtensions
{

    public class ByteCounter
    {
        UInt64 readCount = 0;
        UInt64 writeCount = 0;

        public void AddBytesRead(int count)
        {
            readCount += (ulong)count;
        }

        public void AddBytesWritten(int count)
        {
            writeCount += (ulong)count;
        }
    }



    /// <summary>
    /// Class to hold IO extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns the amount of data in the last block
        /// </summary>
        public static int BlockRemainderFATX(this DJsIO y, FATXPartition xPartition)
        {

            if (y.IOType == DataType.Drive)
                return 0;
            if (y.Length == 0)
                return 0;
            return (int)(((y.Length - 1) % xPartition.xBlockSize) + 1);

        }

        /// <summary>
        /// Returns the count of blocks this file contains
        /// </summary>
        public static uint BlockCountFATX(this DJsIO y, FATXPartition xPartition)
        {

            {
                if (y.IOType == DataType.Drive)
                    return 0;
                if (y.Length == 0)
                    return 0;
                return (uint)(((y.Length - 1) / xPartition.xBlockSize) + 1);
            }
            //throw x
        }
    }
}

namespace X360.IO.STFSExtensions
{
    /// <summary>
    /// Class to hold IO extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns the count of blocks this file contains
        /// </summary>
        public static uint BlockCountSTFS(this DJsIO y)
        {

            {
                if (y.IOType == DataType.Drive)
                    return 0;
                if (y.Length == 0)
                    return 0;
                return (uint)(((y.Length - 1) / 0x1000) + 1);
            }
            //throw x
        }

        /// <summary>
        /// Returns the amount of data in the last block
        /// </summary>
        public static int BlockRemainderSTFS(this DJsIO y)
        {

            {
                if (y.IOType == DataType.Drive)
                    return 0;
                if (y.Length == 0)
                    return 0;
                if (y.Length > 0xFFFFFFFF)
                    return 0;
                return (int)(y.Length % 0x1000);
            }
            //throw x
        }
    }
}

namespace X360.IO.SearchExtensions
{
    /// <summary>
    /// Class to hold IO extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Searches the stream for a set of bytes
        /// </summary>
        /// <param name="y"></param>
        /// <param name="xData"></param>
        /// <param name="xStopWhenFound"></param>
        /// <returns></returns>
        public static long[] SearchBinary(this DJsIO y, byte[] xData, bool xStopWhenFound)
        {

            byte[] xbuff;
            List<long> xReturn = new List<long>();
            for (long i = y.Position; i < y.Length; i++)
            {
                if (i + xData.Length > y.Length)
                    break;
                y.Position = i;
                xbuff = y.ReadBytes(xData.Length);
                if (BitConverter.ToString(xData) == BitConverter.ToString(xbuff))
                {
                    xReturn.Add(i);
                    if (xStopWhenFound)
                        break;
                }
            }
            return xReturn.ToArray();

        }

        /// <summary>
        /// Searching the stream for an ASCII string
        /// </summary>
        /// <param name="y"></param>
        /// <param name="xData"></param>
        /// <param name="xStopWhenFound"></param>
        /// <returns></returns>
        public static long[] SearchASCII(this DJsIO y, string xData, bool xStopWhenFound)
        {
            byte[] xbuff = Encoding.ASCII.GetBytes(xData);
            return SearchBinary(y, xbuff, xStopWhenFound);
        }

        /// <summary>
        /// Searches the file for a Unicode string, Endian based on stream's Endian
        /// </summary>
        /// <param name="y"></param>
        /// <param name="xData"></param>
        /// <param name="xStopWhenFound"></param>
        /// <returns></returns>
        public static long[] SearchUnicode(this DJsIO y, string xData, bool xStopWhenFound)
        {
            byte[] xbuff;
            if (y.IsBigEndian)
                xbuff = Encoding.BigEndianUnicode.GetBytes(xData);
            else
                xbuff = Encoding.Unicode.GetBytes(xData);
            return SearchBinary(y, xbuff, xStopWhenFound);
        }
    }
}

namespace X360.IO.ExtraExtensions
{
    /// <summary>
    /// Class to hold IO extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Attempts to return an Image from this stream
        /// </summary>
        /// <returns></returns>
        public static Image ImageFromStream(this DJsIO y)
        {
            { return Image.FromStream((Stream)y.xStream); }

        }

        /// <summary>
        /// Inserts bytes as needed
        /// </summary>
        /// <param name="xLocation"></param>
        /// <param name="xAddSize"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool InsertBlankData(this DJsIO y, long xLocation, int xAddSize)
        {
            if (y.IOType == DataType.Drive)
                return false;
            //try
            {
                long xLOBefore = y.Length;
                long xShiftCt = xLOBefore - xLocation;
                for (int i = 0; i < xShiftCt; i++)
                {
                    y.Position = xLOBefore - i - 1;
                    byte x = y.ReadByte();
                    y.Position = (xShiftCt - i) + xLocation + xAddSize;
                    y.Write(x);
                }
                y.Position = xLocation;
                for (int i = 0; i < xAddSize; i++)
                    y.Write((byte)0);
                return true;
            }
            //catch { return false; }
        }
    }
}
#endregion

namespace X360.IO
{
    #region Enums

    /// <summary>
    /// String type
    /// </summary>
    public enum StringForm : byte
    {
        /// <summary>
        /// ACSII String
        /// </summary>
        ASCII = 1,
        /// <summary>
        /// Unicode String
        /// </summary>
        Unicode = 2
    }

    /// <summary>
    /// File Type
    /// </summary>
    public enum DJFileMode : byte
    {
        /// <summary>
        /// Create a file
        /// </summary>
        Create,
        /// <summary>
        /// Open a file
        /// </summary>
        Open
    }

    /// <summary>
    /// Pad direction
    /// </summary>
    public enum PadLocale : byte
    {
        /// <summary>
        /// Pad to the left
        /// </summary>
        Left,
        /// <summary>
        /// Pad to the right
        /// </summary>
        Right
    }

    /// <summary>
    /// Generic pad types
    /// </summary>
    public static class PadType
    {
        /// <summary>
        /// Null pad
        /// </summary>
        public const char Null = (char)0;
        /// <summary>
        /// Space pad
        /// </summary>
        public const char Space = (char)0x20;
    }

    /// <summary>
    /// How to read  a string
    /// </summary>
    public enum StringRead : byte
    {
        /// <summary>
        /// Defined length
        /// </summary>
        Defined,
        /// <summary>
        /// Read to null
        /// </summary>
        ToNull,
        /// <summary>
        /// Reads a string with a byte size in front of it
        /// </summary>
        PrecedingLength
    }

    /// <summary>
    /// IO Type
    /// </summary>
    public enum DataType : byte
    {
        /// <summary>
        /// No specific type
        /// </summary>
        None,
        FatXFile,
        /// <summary>
        /// Memory IO
        /// </summary>
        Memory,
        /// <summary>
        /// File IO
        /// </summary>
        File,
        /// <summary>
        /// Device IO
        /// </summary>
        Drive,
        /// <summary>
        /// 
        /// </summary>
        Real,
        /// <summary>
        /// Contains multiple IO's
        /// </summary>
        MultiFile,
        /// <summary>
        /// Some other IO
        /// </summary>
        Other
    }

    enum RealType : byte { None, STFS, FATX, SVOD }
    #endregion

    /// <summary>
    /// IO Exceptions
    /// </summary>
    //[DebuggerStepThrough]
    public static class IOExcepts
    {
        //[CompilerGenerated]
        static readonly Exception xAccessError = new Exception("Underlining source not accessed");
        //[CompilerGenerated]
        static readonly Exception xDirectoryErr = new Exception("Invalid Directory index");
        //[CompilerGenerated]
        static readonly Exception xPosition = new Exception("Cannot move position past file size");
        //[CompilerGenerated]
        static readonly Exception xIndex = new Exception("Out of bounds indexer");
        //[CompilerGenerated]
        static readonly Exception xNoExist = new Exception("Path does not exist");
        //[CompilerGenerated]
        static readonly Exception xCreateError = new Exception("Unable to create path or file");
        //[CompilerGenerated]
        static readonly Exception xMultiAccessError = new Exception("Unable to access a file");

        /// <summary>
        /// Not accessed
        /// </summary>
        public static Exception AccessError { get { return xAccessError; } }
        /// <summary>
        /// Invalid directory index
        /// </summary>
        public static Exception DirectError { get { return xDirectoryErr; } }
        /// <summary>
        /// Position error
        /// </summary>
        public static Exception PositionError { get { return xPosition; } }
        /// <summary>
        /// Index out of bounds
        /// </summary>
        public static Exception Index { get { return xIndex; } }
        /// <summary>
        /// Path unexistant
        /// </summary>
        public static Exception DoesntExist { get { return xNoExist; } }
        /// <summary>
        /// Cannot create path or file
        /// </summary>
        public static Exception CreateError { get { return xCreateError; } }
        /// <summary>
        /// Returns this when one of the files in a multifile IO could not be accessed
        /// </summary>
        public static Exception MultiAccessError { get { return xMultiAccessError; } }
    }

    /// <summary>
    /// Object for generic IO
    /// </summary>
    //[DebuggerStepThrough]
    public class DJsIO
    {
        #region Variables
        /// <summary>
        /// Bool if the Stream is properly accessed
        /// </summary>
        public bool Accessed { get { return GetAccessed(); } }
        /// <summary>
        /// The name of the actual file
        /// </summary>
        public string FileNameShort
        {
            get
            {
                if (txtidx == null || txtidx[0] == -1)
                    return null;
                return xFile.Substring(txtidx[0] + 1);
            }
        }
        /// <summary>
        /// The file path of this file
        /// </summary>
        public string FilePath
        {
            get
            {
                if (txtidx == null || txtidx[0] == -1)
                    return null;
                return xFile.Substring(0, (txtidx[0] + 1));
            }
        }
        /// <summary>
        /// Returns the full file location (applicable only to File Stream)
        /// </summary>
        public string FileNameLong { get { return xFile; } }
        /// <summary>
        /// The Extension of the File (File Stream only)
        /// </summary>
        public string FileExtension
        {
            get
            {
                if (txtidx == null || txtidx[0] == -1)
                    return null;
                if (txtidx[1] < txtidx[0])
                    return "";
                return xFile.Substring(txtidx[1] + 1);
            }
        }
        /// <summary>
        /// Select what Endian to Read/Write in
        /// </summary>
        //[CompilerGenerated]
        public bool IsBigEndian { get; set; }
        /// <summary>
        /// The type of this instance
        /// </summary>
        //[CompilerGenerated]
        public DataType IOType { get { return xThisData; } }
        /// <summary>
        /// This type of IO
        /// </summary>
        //[CompilerGenerated]
        public DataType xThisData = DataType.None;
        /// <summary>
        /// The file name of which the stream is referenced to
        /// </summary>
        //[CompilerGenerated]
        public string xFile = "Null"; // Long file location (File Stream applicable only)
        /// <summary>
        /// The stream of the instance
        /// </summary>
        //[CompilerGenerated]
        public Stream xStream = null; // Stream
        //[CompilerGenerated]
        int[] txtidx = null;
        #endregion

        #region StreamSetting
        /// <summary>
        /// Sets the strings of a file like string
        /// </summary>
        public void XSetStrings()
        {
            if (xThisData != DataType.File)
                return;
            txtidx = new int[2];
            txtidx[0] = xFile.LastIndexOfAny(new char[] { '/', '\\' });
            txtidx[1] = xFile.LastIndexOf('.');
        }

        // Sets the File Stream (if applicable)
        public void XSetStream(DJFileMode xftype)
        {
            if (xftype == DJFileMode.Open && !string.IsNullOrEmpty(this.xFile))
            {
                xStream = new FileStream(this.xFile, FileMode.Open);
                xThisData = DataType.File;
            }
            else if (xftype == DJFileMode.Create && !string.IsNullOrEmpty(this.xFile))
            {
                xStream = new FileStream(this.xFile, FileMode.Create);
                xThisData = DataType.File;
            }
            else
            {
                xStream = new MemoryStream();
                xThisData = DataType.Memory;
            }
        }

        /// <summary>
        /// Create instance of IO directly to a file with file mode type
        /// </summary>
        /// <param name="xFileIn"></param>
        /// <param name="xType"></param>
        /// <param name="BigEndian"></param>
        public DJsIO(string xFileIn, DJFileMode xType, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            xFile = xFileIn;
            XSetStream(xType);
        }


        /// <summary>
        /// Creates an instance of IO for byte array's.
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public DJsIO(byte[] xIn, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            //try
            {
                MemoryStream xMS = new MemoryStream();
                xStream = xMS;
                xThisData = DataType.Memory;
                xStream.Write(xIn, 0, xIn.Length);
                Position = 0;
            }
            /*catch
            {
                { xStream.Close(); }
                catch { }
                xStream = null;
            }*/
        }

        public DJsIO(FATXFileEntry xIn, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            //try
            {

                xThisData = DataType.FatXFile;

                Position = 0;
            }

        }

        /// <summary>
        /// Creates a stream on a byte array to a specified size
        /// </summary>
        /// <param name="ArraySize"></param>
        /// <param name="BigEndian"></param>
        public DJsIO(long ArraySize, bool BigEndian)
        {
            //try
            {
                IsBigEndian = BigEndian;
                byte[] Buffer = new byte[ArraySize];
                MemoryStream xMS = new MemoryStream(Buffer);
                xStream = xMS;
                xThisData = DataType.Memory;
            }
            /*catch
            {
                { xStream.Close(); }
                catch { }
                xStream = null;
            }*/
        }

        /// <summary>
        /// Makes a temporary file stream
        /// </summary>
        /// <param name="BigEndian"></param>
        public DJsIO(bool BigEndian)
            : this()
        {
            IsBigEndian = BigEndian;

            //xFile = string.Copy(VariousFunctions.GetTempFileLocale());
            //XSetStream(DJFileMode.Create);
        }
        public DJsIO()
        {
            IsBigEndian = true;
            xStream = new MemoryStream();
            xThisData = DataType.Memory;
            //xFile = string.Copy(VariousFunctions.GetTempFileLocale());
            //XSetStream(DJFileMode.Create);
        }

        public byte[] GetBytes()
        {

            var startPos = xStream.Position;
            xStream.Position = 0;


            var b = new byte[xStream.Length];
            xStream.Read(b, 0, b.Length);
            xStream.Position = startPos;


            xStream.Position = startPos;

            return b;
        }

        #endregion

        #region Reading
        /// <summary>
        /// Read a set amount of bytes
        /// </summary>
        /// <param name="xSize"></param>
        /// <returns></returns>
        public virtual byte[] ReadBytes(int xSize)
        {

            var xbuff = new byte[xSize];
            if (xStream.CanRead)
            {
                xStream.Read(xbuff, 0, xSize);
            }
            return xbuff;
        }

        public virtual byte[] unbufferedread(int xSize)
        {
            byte[] xbuff = new byte[xSize];
            //xStream.Read(xbuff, 0, xSize);
            xbuff = this.ReadBytes(xSize);

            return xbuff;

        }

        /// <summary>
        /// Reads a signed 16-bit (2 bytes) integer
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            //{
            return ReadInt16(IsBigEndian);
            //}//throw x
        }

        /// <summary>
        /// Reads a signed 16-bit (2 bytes) integer in specified endian 
        /// </summary>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public short ReadInt16(bool BigEndian)
        {
            //            try
            {
                byte[] buff = ReadBytes(2);
                return BitConv.ToInt16(buff, BigEndian);
            }
            //          //throw x
        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 24-bit integer (3 bytes)
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt24()
        {
            //{ 
            return ReadUInt24(IsBigEndian);
            //}//throw x
        }

        /// <summary>
        /// Reads an unsigned 24-bit (3 byte) integer in specified mode
        /// </summary>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public uint ReadUInt24(bool BigEndian)
        {
            //            try
            {
                byte[] xData = ReadBytes(3);
                if (BigEndian)
                    xData.EndianConvert();
                return ((uint)xData[2] << 16 | (uint)xData[1] << 8 |
                    (uint)xData[0]);
            }
            // //throw x
        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 40-bit integer (5 bytes)
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt40()
        {
            //{ 
            return ReadUInt40(IsBigEndian);
            //}//throw x
        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 40-bit integer (5 bytes) in a specified Endian
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt40(bool BigEndian)
        {

            byte[] xData = ReadBytes(5);
            if (BigEndian)
                xData.EndianConvert();
            return ((ulong)xData[4] << 32 | (ulong)xData[3] << 24 |
                (ulong)xData[2] << 16 | (ulong)xData[1] << 8 |
                (ulong)xData[0]);

        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 48-bit integer (6 bytes)
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt48()
        {
            return ReadUInt48(IsBigEndian);

        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 48-bit integer (6 bytes)
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt48(bool BigEndian)
        {

            byte[] xData = ReadBytes(6);
            if (BigEndian)
                xData.EndianConvert();
            return ((ulong)xData[5] << 40 | (ulong)xData[4] << 32 |
                (ulong)xData[3] << 24 | (ulong)xData[2] << 16 |
                (ulong)xData[1] << 8 | (ulong)xData[0]);

        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 56-bit integer (7 bytes)
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt56()
        {
            { return ReadUInt56(IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads an unsinged 56 bit Int in specified mode
        /// </summary>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public ulong ReadUInt56(bool BigEndian)
        {

            {
                byte[] xData = ReadBytes(7);
                if (BigEndian)
                    xData.EndianConvert();
                return ((ulong)xData[6] << 48 | (ulong)xData[5] << 40 |
                    (ulong)xData[4] << 32 | (ulong)xData[3] << 24 |
                    (ulong)xData[2] << 16 | (ulong)xData[1] << 8 |
                    (ulong)xData[0]);
            }
            //throw x
        }

        /// <summary>
        /// Reads a 32-bit (4 bytes) signed integer
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            { return ReadInt32(IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Read a 32-bit (4 bytes) signed integer to a specified endian
        /// </summary>
        /// <returns></returns>
        public int ReadInt32(bool BigEndian)
        {

            {
                byte[] buff = ReadBytes(4);
                return BitConv.ToInt32(buff, BigEndian);
            }
            //throw x
        }

        /// <summary>
        /// Reads a 64-bit (8 bytes) integer
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            { return ReadInt64(IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a 64-bit (8 bytes) integer to a specified endian
        /// </summary>
        /// <returns></returns>
        public long ReadInt64(bool BigEndian)
        {

            {
                byte[] buff = ReadBytes(8);
                return BitConv.ToInt64(buff, BigEndian);
            }
            //throw x
        }

        /// <summary>
        /// Reads a 8-bit (1 byte) integer
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            //{ return ReadBytes(1)[0]; }
            //catch (Exception xerror) { throw xerror; }
            return (byte)ReadBytes(1)[0];
        }

        /// <summary>
        /// Reads a Signed 8 bit (1 byte) integer
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();

        }

        /// <summary>
        /// Reads a Single
        /// </summary>
        /// <returns></returns>
        public float ReadSingle()
        {
            { return ReadSingle(IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a Single in a specified endian
        /// </summary>
        /// <returns></returns>
        public float ReadSingle(bool BigEndian)
        {

            {
                byte[] buff = ReadBytes(4);
                return BitConv.ToSingle(buff, BigEndian);
            }
            //throw x
        }

        /// <summary>
        /// Reads a Double
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            { return ReadDouble(IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a Double in a specified endian
        /// </summary>
        /// <returns></returns>
        public double ReadDouble(bool BigEndian)
        {

            {
                byte[] buff = ReadBytes(8);
                return BitConv.ToDouble(buff, BigEndian);
            }
            //throw x
        }

        /// <summary>
        /// Reads a 16-bit (2 bytes) integer
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            { return ReadUInt16(IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads an unsigned 16-bit (2 bytes) integer in a specified endian
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16(bool BigEndian)
        {

            {
                byte[] buff = ReadBytes(2);
                return BitConv.ToUInt16(buff, BigEndian);
            }
            //throw x
        }

        /// <summary>
        /// Reads an unsigned 32-bit (4 bytes) integer
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            { return ReadUInt32(IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads an unsigned 32-bit (4 bytes) integer to a specified endian
        /// </summary>
        /// <returns></returns>

        public uint ReadUInt32(bool BigEndian)
        {

            {
                byte[] buff = ReadBytes(4);
                return BitConv.ToUInt32(buff, BigEndian);
            }
            //throw x
        }

        /// <summary>
        /// Reads an unsigned 64-bit (8 bytes) integer
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            { return ReadUInt64(IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads an unsigned 64-bit (8 bytes) integer to a specified endian
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64(bool BigEndian)
        {

            {
                byte[] buff = ReadBytes(8);
                return BitConv.ToUInt64(buff, BigEndian);
            }
            //throw x
        }

        /// <summary>
        /// Reads a bit bool (0 no, 1 yes)
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            { return (ReadByte() & 1) == 1; }
            //throw x
        }

        /// <summary>
        /// Reads an ASCII line
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            { return ReadLine(StringForm.ASCII, 0xA, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads an ASCII line to a specified line break
        /// </summary>
        /// <param name="BreakType"></param>
        /// <returns></returns>
        public string ReadLine(byte BreakType)
        {
            { return ReadLine(StringForm.ASCII, BreakType, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a line from a specified string format
        /// </summary>
        /// <param name="xType"></param>
        /// <returns></returns>
        public string ReadLine(StringForm xType)
        {
            { return ReadLine(xType, 0xA, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a line of a specific string format and endian
        /// </summary>
        /// <param name="xType"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadLine(StringForm xType, bool BigEndian)
        {
            { return ReadLine(xType, 0xA, BigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a line from a specific string format, line break, and endian
        /// </summary>
        /// <param name="xType"></param>
        /// <param name="BreakIndicator"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadLine(StringForm xType, short BreakIndicator, bool BigEndian)
        {

            {
                List<byte> buffer = new List<byte>();
                if (xType == StringForm.Unicode)
                {
                    if (Position >= Length - 1)
                        return "";
                    byte[] buff = ReadBytes(2);
                    while (buff[0] != BreakIndicator)
                    {
                        try
                        {
                            buffer.AddRange(buff);
                            buff = ReadBytes(2);
                        }
                        catch { break; }
                    }
                    if (BigEndian)
                        return Encoding.BigEndianUnicode.GetString(buffer.ToArray());
                    else
                        return Encoding.Unicode.GetString(buffer.ToArray());
                }
                else
                {
                    if (Position >= Length)
                        return "";
                    byte buff = ReadByte();
                    while (buff != (byte)BreakIndicator)
                    {
                        try
                        {
                            buffer.Add(buff);
                            buff = ReadByte();
                        }
                        catch { break; }
                    }
                    return Encoding.ASCII.GetString(buffer.ToArray()).Replace(PadType.Null.ToString(), "");
                }
            }
            //throw x
        }

        /// <summary>
        /// Reads a ToNull String
        /// </summary>
        /// <param name="xStringType"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType)
        {
            { return ReadString(xStringType, 0, StringRead.ToNull, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a string to null wif specified endian
        /// </summary>
        /// <param name="xStringType"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, bool BigEndian)
        {
            { return ReadString(xStringType, 0, StringRead.ToNull, BigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a string
        /// </summary>
        /// <param name="xStringSize"></param>
        /// <param name="xStringType"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, int xStringSize)
        {
            { return ReadString(xStringType, xStringSize, StringRead.Defined, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a string
        /// </summary>
        /// <param name="xStringSize"></param>
        /// <param name="xStringType"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, int xStringSize, bool BigEndian)
        {
            { return ReadString(xStringType, xStringSize, StringRead.Defined, BigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a string with special circumstance capability
        /// </summary>
        /// <param name="xStringSize"></param>
        /// <param name="xStringType"></param>
        /// <param name="xRead"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, int xStringSize, StringRead xRead)
        {
            { return ReadString(xStringType, xStringSize, xRead, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Reads a string with special circumstance capability
        /// </summary>
        /// <param name="xStringSize"></param>
        /// <param name="xStringType"></param>
        /// <param name="xRead"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, int xStringSize, StringRead xRead, bool BigEndian)
        {
            if (!Enum.IsDefined(typeof(StringForm), xStringType) || !Enum.IsDefined(typeof(StringRead), xRead))
                throw new Exception("Invalid parameters");
            //            try
            {
                if (xRead == StringRead.ToNull)
                {
                    if (Position >= Length - 1)
                        return "";
                    List<byte> buffer = new List<byte>();
                    if (xStringType == StringForm.Unicode)
                    {
                        byte[] buff = ReadBytes(2);
                        int i = 0;
                        while (xStringSize == 0 || i < xStringSize)
                        {
                            if (buff[0] == 0 && buff[1] == 0)
                                break;
                            //try
                            {
                                buffer.AddRange(buff);
                                buff = ReadBytes(2);
                            }
                            //catch { break; }
                        }
                        if (BigEndian)
                            return Encoding.BigEndianUnicode.GetString(buffer.ToArray()).Replace(PadType.Null.ToString(), "");
                        else
                            return Encoding.Unicode.GetString(buffer.ToArray()).Replace(PadType.Null.ToString(), "");
                    }
                    else
                    {
                        if (Position >= Length)
                            return "";
                        byte buff = ReadByte();
                        int i = 0;
                        while (xStringSize == 0 || i < xStringSize)
                        {
                            if (buff == 0)
                                break;
                            //try
                            {
                                buffer.Add(buff);
                                buff = ReadByte();
                            }
                            //catch { break; }
                            i++;
                        }
                        return Encoding.ASCII.GetString(buffer.ToArray()).Replace(PadType.Null.ToString(), "");
                    }
                }
                else if (xRead == StringRead.PrecedingLength)
                {
                    List<byte> Buffer = new List<byte>();
                    byte len = ReadByte();
                    for (int i = 0; i < len; i++)
                    {
                        if (xStringType == StringForm.ASCII)
                            Buffer.AddRange(ReadBytes(2));
                        else
                            Buffer.Add(ReadByte());
                    }
                    if (xStringType == StringForm.ASCII)
                        return Encoding.ASCII.GetString(Buffer.ToArray()).Replace(PadType.Null.ToString(), "");
                    else
                    {
                        if (BigEndian)
                            return Encoding.BigEndianUnicode.GetString(Buffer.ToArray()).Replace(PadType.Null.ToString(), "");
                        else
                            return Encoding.Unicode.GetString(Buffer.ToArray()).Replace(PadType.Null.ToString(), "");
                    }
                }
                else
                {
                    byte[] buff = ReadBytes((byte)xStringType * xStringSize);
                    if (xStringType == StringForm.ASCII)
                        return Encoding.ASCII.GetString(buff).Replace(PadType.Null.ToString(), "");
                    else
                    {
                        if (BigEndian)
                            return Encoding.BigEndianUnicode.GetString(buff).Replace(PadType.Null.ToString(), "");
                        else
                            return Encoding.Unicode.GetString(buff).Replace(PadType.Null.ToString(), "");
                    }
                }

            }
            // //throw x
        }

        /// <summary>
        /// Reads bytes and returns it as a Hexadecimal String
        /// </summary>
        /// <param name="xLength"></param>
        /// <returns></returns>
        public string ReadHexString(int xLength)
        {
            { return ReadBytes(xLength).HexString(); }
            //throw x
        }

        /// <summary>
        /// Reads a File Time Stamp (8 bytes for time)
        /// </summary>
        /// <returns></returns>
        public DateTime ReadFileTime()
        {
            { return DateTime.FromFileTime(ReadInt64()); }
            //throw x
        }

        /// <summary>
        /// WARNING: Only use on small files, will throw Exception if not enough memory
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ReadStream()
        {
            long posbefore = Position;
            Position = 0;
            var ret = ReadBytes((int)Length);

            Position = posbefore;

            return ret;
        }

        //            try
        /* {
             byte[] xReturn = new byte[Length];
             long posbefore = Position;
             Position = 0;
             for (long i = 0; i < xReturn.Length; i++)
                 xReturn[i] = ReadByte();
             Position = posbefore;
             return xReturn;
         }*/
        ////throw x

        #endregion

        #region Writing
        /// <summary>
        /// Writes a Byte Array
        /// </summary>
        /// <param name="xIn"></param>
        public virtual void Write(byte[] xIn)
        {
            // try
            {
                if (xThisData == DataType.Real)
                {
                    foreach (byte x in xIn)
                        Write(x);
                }
                else if (xThisData == DataType.Memory)
                {
                    //                    try
                    {
                        //foreach (byte x in xIn)
                        //   xStream.WriteByte(x);

                        xStream.Write(xIn, 0, (int)xIn.Length);

                        /*var p = xStream.Position + xIn.Length;
                        if (p > xStream.Length)
                        {
                            long pos = xStream.Position;
                            var ms = new MemoryStream((int)p);
                            xStream.Seek(0, SeekOrigin.Begin);

                            var bw = new BinaryWriter(ms);
                            
                            var br = new BinaryReader(xStream);
                            bw.Write(br.ReadBytes((int)xStream.Length));
                        
                        
                            var ox = xStream;
                            xStream = ms;
                            xStream.Position = pos;
                            ox.Close();
                        }
                        xStream.Write(xIn, 0, xIn.Length);*/
                    }
                    /*catch (Exception)
                    {
                        System.Diagnostics.Debug.WriteLine("fail");
                    }*/
                }
                else if (xThisData != DataType.Drive)
                {
                    
                    xStream.Write(xIn, 0, xIn.Length);
                }
            }
            // //throw x
        }

        public void unbufferedwrite(byte[] xIn)
        {
            { xStream.Write(xIn, 0, xIn.Length); }
            //throw x
        }

        /// <summary>
        /// Writes a signed 16-bit (2 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(short xIn)
        {
            //{ 
            Write(BitConv.GetBytes(xIn, IsBigEndian));
            //}//throw x
        }

        /// <summary>
        /// Writes a short in specified Endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(short xIn, bool BigEndian)
        {
            { Write(BitConv.GetBytes(xIn, BigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes a signed 32-bit (4 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(int xIn)
        {
            { Write(BitConv.GetBytes(xIn, IsBigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes a signed 32-bit (4 byte) integer to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(int xIn, bool BigEndian)
        {
            { Write(BitConv.GetBytes(xIn, BigEndian)); }

        }

        /// <summary>
        /// Writes a signed 64-bit (8 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(long xIn)
        {
            { Write(BitConv.GetBytes(xIn, IsBigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes a signed 64-bit (8 byte) integer to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(long xIn, bool BigEndian)
        {
            { Write(BitConv.GetBytes(xIn, BigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes an unsigned 16-bit (2 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(ushort xIn)
        {
            { Write(BitConv.GetBytes(xIn, IsBigEndian)); }
            //throw x
        }

        /// <summary>
        /// Write an unsigned 16-bit (2 bytes) integer in a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(ushort xIn, bool BigEndian)
        {
            { Write(BitConv.GetBytes(xIn, BigEndian)); }
            //throw x
        }

        /// <summary>
        /// Write an unsigned 32-bit (4 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(uint xIn)
        {
            { Write(BitConv.GetBytes(xIn, IsBigEndian)); }
            //throw x
        }

        /// <summary>
        /// Write an unsigned 32-bit (4 bytes) integer in a specifed endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(uint xIn, bool BigEndian)
        {
            { Write(BitConv.GetBytes(xIn, BigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes an unsigned 64-bit (8 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(ulong xIn)
        {
            { Write(BitConv.GetBytes(xIn, IsBigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes an unsigned 64-bit (8 bytes) integer to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(ulong xIn, bool BigEndian)
        {
            { Write(BitConv.GetBytes(xIn, BigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes a single/float
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(float xIn)
        {
            { Write(BitConv.GetBytes(xIn, IsBigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes a single/float to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(float xIn, bool BigEndian)
        {
            { Write(BitConv.GetBytes(xIn, BigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes a double
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(double xIn)
        {
            { Write(BitConv.GetBytes(xIn, IsBigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes a double to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(double xIn, bool BigEndian)
        {
            { Write(BitConv.GetBytes(xIn, BigEndian)); }
            //throw x
        }

        /// <summary>
        /// Writes a Signed Byte
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(sbyte xIn) { Write((byte)xIn); }

        /// <summary>
        /// Writes a bool byte (1 yes, 0 no)
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(bool xIn)
        {
            { Write((byte)(xIn ? 1 : 0)); }
            //throw x
        }

        /// <summary>
        /// Writes 24-bit (3 byte) integers
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteUInt24(uint xIn)
        {
            { WriteUInt24(xIn, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Writes 24-bit (3 byte) integers to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void WriteUInt24(uint xIn, bool BigEndian)
        {

            {
                List<byte> xList = BitConv.GetBytes(xIn, false).ToList<byte>();
                xList.RemoveAt(3);
                if (BigEndian)
                    xList.Reverse();
                Write(xList.ToArray());
            }
            //throw x
        }

        /// <summary>
        /// Writes 40-bit (5 byte) integers
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteUInt40(ulong xIn)
        {
            { WriteUInt40(xIn, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Writes 40-bit (5 bytes) integers to specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void WriteUInt40(ulong xIn, bool BigEndian)
        {

            {
                List<byte> xList = BitConv.GetBytes(xIn, false).ToList<byte>();
                xList.RemoveAt(5);
                xList.RemoveAt(5);
                xList.RemoveAt(5);
                if (BigEndian)
                    xList.Reverse();
                Write(xList.ToArray());
            }
            //throw x
        }

        /// <summary>
        /// Writes a 48-bit (6 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteUInt48(ulong xIn)
        {
            { WriteUInt48(xIn, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Writes 48-bit (6 bytes) integer to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void WriteUInt48(ulong xIn, bool BigEndian)
        {

            {
                List<byte> xList = BitConv.GetBytes(xIn, false).ToList<byte>();
                xList.RemoveAt(6);
                xList.RemoveAt(6);
                if (BigEndian)
                    xList.Reverse();
                Write(xList.ToArray());
            }
            //throw x
        }

        /// <summary>
        /// Writes a 56-bit (7 byte) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteUInt56(ulong xIn)
        {
            { WriteUInt56(xIn, IsBigEndian); }
            //throw x
        }

        /// <summary>
        /// Writes 56-bit (7 byte) integers to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void WriteUInt56(ulong xIn, bool BigEndian)
        {

            {
                List<byte> xList = BitConv.GetBytes(xIn, BigEndian).ToList<byte>();
                xList.RemoveAt(7);
                if (BigEndian)
                    xList.Reverse();
                Write(xList.ToArray());
            }
            //throw x
        }

        /// <summary>
        /// Writes an ASCII string
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(string xIn)
        {
            Write(Encoding.ASCII.GetBytes(xIn.ToCharArray()));
        }

        /// <summary>
        /// Writes a specified type of string
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="xType"></param>
        public void Write(string xIn, StringForm xType)
        {

            if (xType == StringForm.ASCII)
                Write(xIn);
            else
            {
                if (IsBigEndian)
                    Write(Encoding.BigEndianUnicode.GetBytes(xIn.ToCharArray()));
                else
                    Write(Encoding.Unicode.GetBytes(xIn.ToCharArray()));
            }

        }

        /// <summary>
        /// Pads a string and writes it
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="xType"></param>
        /// <param name="xDesiredSize"></param>
        /// <param name="xPadLocale"></param>
        /// <param name="PadChar"></param>
        public void Write(string xIn, StringForm xType, int xDesiredSize, PadLocale xPadLocale, char PadChar)
        {
            if (!Enum.IsDefined(typeof(StringForm), xType) || !Enum.IsDefined(typeof(PadLocale), xPadLocale))
                throw new Exception("Invalid Parameters");

            if (xPadLocale == PadLocale.Right)
            {
                xIn = xIn.PadRight(xDesiredSize, PadChar);
            }
            else
            {
                xIn = xIn.PadLeft(xDesiredSize, PadChar);
            }

            if (xType == StringForm.ASCII)
            {
                Write(Encoding.ASCII.GetBytes(xIn.ToCharArray()));
            }
            else if (xType == StringForm.Unicode)
            {
                if (IsBigEndian)
                {
                    Write(Encoding.BigEndianUnicode.GetBytes(xIn.ToCharArray()));
                }

                else
                {
                    Write(Encoding.Unicode.GetBytes(xIn.ToCharArray()));
                }
            }


        }

        /// <summary>
        /// Attempts to write a hexadecimal string
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteHexString(string xIn)
        {
            Write(xIn.HexToBytes());
        }

        /// <summary>
        /// Writes a byte (8-bits)
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(byte xIn)
        {
            Write(new byte[] { xIn });
        }

        /// <summary>
        /// Writes a File Time TimeStamp
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteFileTime(DateTime xIn)
        {
            Write(xIn.ToFileTime());
        }

        /// <summary>
        /// Flushes the stream and writes the pending data (if any)
        /// </summary>
        public virtual void Flush()
        {

            if (xThisData == DataType.File)
                xStream.Flush();

            if (xThisData == DataType.Memory)
                xStream.Flush();

        }
        #endregion

        #region Misc
        /// <summary>
        /// Returns the stream length
        /// </summary>
        public virtual long Length
        {
            get { return xStream.Length; }
        }

        /// <summary>
        /// Gets a text based size for users
        /// </summary>
        public string LengthFriendly
        {
            get
            {
                return VariousFunctions.GetFriendlySize(Length);
            }
        }

        /// <summary>
        /// Returns the stream position
        /// </summary>
        public virtual long Position
        {
            get
            {
                return xStream.Position;
            }
            set
            {
                if (xStream != null && xStream.CanSeek)
                {
                    if (value != xStream.Position)
                        xStream.Position = value;
                }
            }
        }

        /// <summary>
        /// Closes the stream
        /// </summary>
        public virtual bool Close()
        {
            if (Accessed)
            {
                //{ 
                xStream.Close();
                //catch { return false; }
            }
            return true;
        }

        /// <summary>
        /// Disposes of the stream
        /// </summary>
        /// <returns></returns>
        public virtual bool Dispose()
        {
            return Dispose(false);
        }

        public bool Dispose(bool DeleteFile)
        {
            if (!Close())
                return false;
            if (xThisData != DataType.Real)
            {
                xStream.Dispose();
                //catch { return false; }
                if (xThisData == DataType.File && DeleteFile)
                    VariousFunctions.DeleteFile(FileNameLong);
            }
            xFile = null;
            txtidx = null;
            return true;
        }

        /// <summary>
        /// Reopens the stream
        /// </summary>
        /// <returns></returns>
        
        public virtual bool OpenAgain()
        {
            
            if (!Accessed)
            {
                if (xThisData == DataType.Real)
                    return false;
                else if (xThisData == DataType.File)
                    XSetStream(DJFileMode.Open);
                else if (xThisData == DataType.Memory)
                    XSetStream(DJFileMode.Open);
                else if (xThisData == DataType.Drive)
                {

                }
            }
            return Accessed;
        }

        /// <summary>
        /// Returns the stream
        /// </summary>
        /// <returns></returns>
        public virtual Stream GrabStream() { if (xThisData == DataType.Real) return null; return xStream; }

        /// <summary>
        /// Sets the length of the stream (not always applicable)
        /// </summary>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public virtual bool SetLength(long xLen)
        {
            if (xThisData == DataType.Memory)
            {
                xStream.SetLength(xLen);
                return true;
            }
            else if (xThisData == DataType.File)
            {
                ((FileStream)xStream).SetLength(xLen);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a bool if instance is valid
        /// </summary>
        /// <returns></returns>
        public virtual bool GetAccessed() { return xStream.CanRead && xStream.CanWrite; }
        #endregion
    }

    /// <summary>
    /// IO to handle Drives
    /// </summary>
    //[DebuggerStepThrough]
    public sealed class DriveIO : DJsIO
    {

        public Drive xDrive;

        void driveset(ref Drive xIn, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            xIn.MakeHandle();
            xStream = new FileStream(xIn.Handle, FileAccess.ReadWrite);
            xThisData = DataType.Drive;
            xDrive = xIn;
        }

        /// <summary>
        /// Accesses a stream from a Drive
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public DriveIO(Drive xIn, bool BigEndian)
        {
            driveset(ref xIn, BigEndian);
        }

        /// <summary>
        /// Accesses a stream from a Drive
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public DriveIO(ref Drive xIn, bool BigEndian)
        {
            driveset(ref xIn, BigEndian);
        }

        /// <summary>
        /// Writes bytes to the stream
        /// </summary>
        /// <param name="xIn"></param>
        public override void Write(byte[] xIn)
        {
            // Have to align to sector, my buffer system
            // Get position
            long pos = xStream.Position;

            // Gets bytes away from beginning of sector
            int leftover = (int)(pos % xDrive.Geometry.BytesPerSector);

            // Set Position to sector beginning
            if (leftover != 0)
                Position = pos - leftover;

            // Get size of our buffer relative to starting sector
            int size = xIn.Length + leftover;

            int count = (int)((((size - 1) / xDrive.Geometry.BytesPerSector) + 1));

            // For some reason, my IO doesn't want to write just one sector
            if (count == 1)
                count++;

            // Read the sectors needed to write
            size = (int)(count * xDrive.Geometry.BytesPerSector);

            byte[] buffer = new byte[size];

            xStream.Read(buffer, 0, size);

            Array.Copy(xIn, 0, buffer, leftover, xIn.Length);

            // Go back to original position (automatically aligns to sector start even when it says it isn't v.v)
            Position = pos - leftover;

            xStream.Write(buffer, 0, buffer.Length);

            xStream.Position = pos + xIn.Length;
        }

        /// <summary>
        /// Reads bytes to the stream
        /// </summary>
        /// <param name="xSize"></param>
        /// <returns></returns>
        public override byte[] ReadBytes(int xSize)
        {
            long pos = xStream.Position;

            int leftover = (int)(pos % xDrive.Geometry.BytesPerSector);

            if (leftover != 0)
                Position = pos - leftover;

            int size =
                (int)(((((xSize + leftover) - 1) / xDrive.Geometry.BytesPerSector) + 1)
                * xDrive.Geometry.BytesPerSector);

            byte[] xbuff = new byte[size];

            xStream.Read(xbuff, 0, size);

            xStream.Position = pos + xSize;

            return xbuff.BytePiece(leftover, xSize);
        }

        /// <summary>
        /// Grabs the length of the stream
        /// </summary>
        public override long Length { get { return xDrive.Geometry.DiskSize; } }

        /// <summary>
        /// DOES NOT WORK IN THIS CLASS
        /// </summary>
        public override void Flush() { return; }

        /// <summary>
        /// Tries to open stream again
        /// </summary>
        /// <returns></returns>
        public override bool OpenAgain()
        {
            xDrive.MakeHandle();
            driveset(ref xDrive, IsBigEndian);
            return xDrive.Accessed;
        }

        /// <summary>
        /// Closes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            xDrive.Handle.Close();
            return xDrive.Handle.IsClosed;
        }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override byte[] ReadStream() { return null; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public override bool SetLength(long xLen) { return false; }
    }

    /// <summary>
    /// STFS Stream
    /// </summary>
    //[DebuggerStepThrough]
    public sealed class STFSStreamIO : DJsIO
    {
        //[CompilerGenerated]
        public FileEntry xFileEnt;
        //[CompilerGenerated]
        public STFSPackage xRef { get { return xFileEnt.xPackage; } }
        //[CompilerGenerated]
        int pos = 0;
        //[CompilerGenerated]
        int idx = 0;
        //[CompilerGenerated]
        bool accessed = false;

        DJsIO xIO { get { return xRef.xIO; } }

        /// <summary>
        /// Creates a Real Time STFS File Stream
        /// </summary>
        /// <param name="Instance"></param>
        /// <param name="BigEndian"></param>
        public STFSStreamIO(FileEntry Instance, bool BigEndian)
        {
            xFileEnt = Instance;
            if (xRef.xActive)
                throw IOExcepts.AccessError;
            xFile = Instance.GetPath();
            XSetStrings();
            xThisData = DataType.Real;
            IsBigEndian = BigEndian;
            accessed = true;
            Position = 0;
        }

        /// <summary>
        /// Reads bytes from a stream
        /// </summary>
        /// <param name="xSize"></param>
        /// <returns></returns>
        public override byte[] ReadBytes(int xSize)
        {
            byte[] buff = new byte[xSize];
            for (long i = 0; i < xSize; i++)
            {
                buff[i] = (byte)xIO.ReadByte();
                Position = pos + 1;
            }
            return buff;
        }

        /// <summary>
        /// Writes a byte array to the stream
        /// </summary>
        /// <param name="xIn"></param>
        public override void Write(byte[] xIn)
        {
            if (pos + xIn.Length >= xFileEnt.Size)
                throw IOExcepts.PositionError;
            foreach (byte x in xIn)
                xIO.Write(x);
        }

        /// <summary>
        /// Closes the STFS stream
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            xRef.xActive = false;
            accessed = false;
            return true;
        }

        /// <summary>
        /// Sets the position of the stream
        /// </summary>
        public override long Position
        {
            get { return pos; }
            set
            {
                if (xRef == null)
                    return;
                if (value == pos)
                    return;
                int indx = (int)(value / 0x1000);
                if (xFileEnt.xBlocks.Length < idx)
                    throw IOExcepts.PositionError;
                int left = (int)(value % 0x1000);
                if (((indx * 0x1000) + left) > Length)
                    throw IOExcepts.PositionError;
                if (idx != indx)
                {
                    idx = indx;
                    xRef.xIO.Position = xRef.GenerateDataOffset(xFileEnt.xBlocks[idx].ThisBlock);
                }
                xRef.xIO.Position = ((xRef.xIO.Position & 0x7FFFFFFFFFFFF000) + left);
                pos = (int)value;
            }
        }

        /// <summary>
        /// Grabs the length of the stream
        /// </summary>
        public override long Length { get { return xFileEnt.Size; } }

        /// <summary>
        /// Disposes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Dispose()
        {
            Close();
            xFileEnt = null;
            return true;
        }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override Stream GrabStream() { return null; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public override bool SetLength(long xLen) { return false; }

        /// <summary>
        /// Checks if it is opened
        /// </summary>
        /// <returns></returns>
        public override bool GetAccessed() { return accessed; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override bool OpenAgain() { return false; }
    }

    /// <summary>
    /// FATX Stream
    /// </summary>
    //[DebuggerStepThrough]
    public sealed class FATXStreamIO : DJsIO
    {
        //[CompilerGenerated]
        FATXFileEntry xEntry = null;
        //[CompilerGenerated]
        uint[] blocks;
        //[CompilerGenerated]
        long pos = long.MaxValue;
        //[CompilerGenerated]
        long pos1 = long.MaxValue;
        //[CompilerGenerated]
        bool accessed = false;

        FATXDrive xDrive { get { return Partition.xdrive; } }
        FATXPartition Partition { get { return xEntry.Partition; } }
        DJsIO xIO { get { return xDrive.xIO; } }

        /// <summary>
        /// File IO on a FATX drive
        /// </summary>
        /// <param name="Instance"></param>
        /// <param name="xBlocks"></param>
        /// <param name="BigEndian"></param>
        public FATXStreamIO(FATXFileEntry Instance, ref uint[] xBlocks, bool BigEndian)
        {
            xEntry = Instance;
            if (xDrive.xActive)
                throw IOExcepts.AccessError;
            xDrive.GetIO();
            blocks = xBlocks;
            accessed = true;
            xDrive.xActive = true;
            IsBigEndian = BigEndian;
            Position = 0;
        }

        /// <summary>
        /// Position of stream
        /// </summary>
        public override long Position
        {
            get { return pos; }
            set
            {
                if (value > xEntry.Size)
                    throw IOExcepts.PositionError;
                if (pos / Partition.xBlockSize != value / Partition.xBlockSize)
                    pos1 = Partition.BlockToOffset(blocks[value / Partition.xBlockSize]);
                xIO.Position = pos1 + (value % Partition.xBlockSize);
                pos = (int)value;
            }
        }

        /// <summary>
        /// Reads bytes from a stream
        /// </summary>
        /// <param name="xSize"></param>
        /// <returns></returns>
        public override byte[] ReadBytes(int xSize)
        {
            byte[] xReturn = new byte[xSize];
            for (int i = 0; i < xSize; i++)
            {
                xReturn[i] = (byte)xIO.ReadByte();
                Position = pos + 1;
            }
            return xReturn;
        }

        /// <summary>
        /// Writes a byte array to a stream
        /// </summary>
        /// <param name="xIn"></param>
        public override void Write(byte[] xIn)
        {
            if (Position + xIn.Length >= Length)
                throw IOExcepts.PositionError;
            foreach (byte x in xIn)
            {
                xIO.Write(x);
                Position = pos + 1;
            }
        }

        /// <summary>
        /// Closes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            accessed = false;
            xDrive.xActive = false;
            return true;
        }

        /// <summary>
        /// Disposes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Dispose()
        {
            this.Close();
            xEntry = null;
            return true;
        }

        /// <summary>
        /// Checks if the stream is open
        /// </summary>
        /// <returns></returns>
        public override bool GetAccessed() { return accessed; }

        /// <summary>
        /// Grabs the size of the stream
        /// </summary>
        public override long Length { get { return xEntry.Size; } }

        /// <summary>
        /// DOES NOT WORK FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override Stream GrabStream() { return null; }

        /// <summary>
        /// DOES NOT WORK FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override bool OpenAgain() { return false; }

        /// <summary>
        /// DOES NOT WORK FOR THIS STREAM
        /// </summary>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public override bool SetLength(long xLen) { return false; }
    }

    /// <summary>
    /// IO to handle a multifile IO to act as one file
    /// </summary>
    //[DebuggerStepThrough]
    public sealed class MultiFileIO : DJsIO
    {
        //[CompilerGenerated]
        List<DJsIO> xIOz = null;
        //[CompilerGenerated]
        long size = 0;
        //[CompilerGenerated]
        int idx = 0;

        DJsIO xIO { get { return xIOz[idx]; } }
        //[CompilerGenerated]
        long fileloc = 0;


        /// <summary>
        /// Creates an instances of multiple files interpretted as one
        /// </summary>
        /// <param name="Files">Files in order of piece number</param>
        /// <param name="BigEndian">Byte Endian</param>
        public MultiFileIO(string[] Files, bool BigEndian)
        {
            xStream = null;
            if (Files == null || Files.Length == 0)
                throw IOExcepts.MultiAccessError;
            List<DJsIO> xios = new List<DJsIO>();

            {
                foreach (string x in Files)
                {
                    xios.Add(new DJsIO(x, DJFileMode.Open, BigEndian));
                    if (!xios[xios.Count - 1].Accessed)
                        throw new Exception();
                    size += xios[xios.Count - 1].Length;
                }
                xIOz = xios;
            }

            xThisData = DataType.MultiFile;
        }

        /// <summary>
        /// Reads bytes from the stream
        /// </summary>
        /// <param name="xSize"></param>
        /// <returns></returns>
        public override byte[] ReadBytes(int xSize)
        {
            byte[] buff = new byte[xSize];

            xIO.xStream.Read(buff, 0, xSize);

            Position = fileloc + xIO.Position;

            /*for (int i = 0; i < xSize; i++)
            {
                
                buff[i] = xIO.ReadByte();
                
                Position = fileloc + xIO.Position;
            }
            */
            return buff;
        }

        /// <summary>
        /// Writes bytes to the stream
        /// </summary>
        /// <param name="xIn"></param>
        public override void Write(byte[] xIn)
        {
            Position = fileloc + xIO.Position;
            xIO.Write(xIn);

            /*foreach (byte x in xIn)
            {
                xIO.Write(x);

                Position = fileloc + xIO.Position;
            }*/
        }

        /// <summary>
        /// Gets or sets the position of the stream
        /// </summary>
        public override long Position
        {
            get { return fileloc + xIO.Position; }
            set
            {
                try
                {
                    long currOffset = 0;
                    for (int x = 0; x < xIOz.Count; x++)
                    {
                        idx = x;

                        if (value < (currOffset + xIO.Length))
                            break;

                        currOffset += xIO.Length;
                    }

                    fileloc = currOffset;
                    xIO.Position = value - fileloc;

                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Get's the length of the stream
        /// </summary>
        public override long Length { get { return size; } }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override byte[] ReadStream() { return null; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public override bool SetLength(long xLen) { return false; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override Stream GrabStream() { return null; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override bool OpenAgain() { return Accessed; }

        /// <summary>
        /// Checks if instance is valid
        /// </summary>
        /// <returns></returns>
        public override bool GetAccessed()
        {
            bool xReturn = (xIOz != null);
            if (xReturn)
            {
                foreach (DJsIO x in xIOz)
                    xReturn = xReturn & x.Accessed;
            }
            return xReturn;
        }

        /// <summary>
        /// Flushes the stream
        /// </summary>
        public override void Flush()
        {
            foreach (DJsIO x in xIOz)
                x.Flush();
        }

        /// <summary>
        /// Closes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            if (xIOz == null)
                return true;
            foreach (DJsIO x in xIOz)
                x.Dispose();
            return true;
        }

        /// <summary>
        /// Disposes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Dispose()
        {
            this.Close();
            xIOz = null;
            return true;
        }
    }

    #region Drive Essentials
    /// <summary>
    /// Object to hold Disk Geometry details
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DiskGeometry
    {
        long cylinders;
        uint mediaType;
        uint tracksPerCylinder;
        uint sectorsPerTrack;
        uint bytesPerSector;

        /// <summary>
        /// Bytes Per Sector
        /// </summary>
        public uint BytesPerSector { get { return bytesPerSector; } }
        /// <summary>
        /// Disk Size
        /// </summary>
        public long DiskSize { get { return cylinders * tracksPerCylinder * sectorsPerTrack * bytesPerSector; } }
    }

    /// <summary>
    /// Type of device
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// Physical type
        /// </summary>
        PhysicalDrive,
        /// <summary>
        /// Logical type
        /// </summary>
        LogicalDrive
    }

    /// <summary>
    /// Class for Accessing Drives
    /// </summary>
    //[DebuggerStepThrough]
    public class Drive
    {
        #region Imports
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(
        string lpFileName,
        FileAccess dwDesiredAccess,
        FileShare dwShareMode,
        IntPtr lpSecurityAttributes,
        FileMode dwCreationDisposition,
        FlagsAndAttributes dwFlagsAndAttributes,
        IntPtr hTemplateFile);

        [DllImport("kernel32.dll")]
        public static extern bool DeviceIoControl(SafeHandle hDevice, uint dwIoControlCode,
        IntPtr lpInBuffer, uint nInBufferSize, ref DiskGeometry lpOutBuffer,
        uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);
        #endregion

        //[CompilerGenerated]
        SafeFileHandle xSFH;
        //[CompilerGenerated]
        byte xIndex;
        //[CompilerGenerated]
        DiskGeometry xGeom;
        //[CompilerGenerated]
        DeviceType xType;

        byte IndexOrLetter { get { return xIndex; } }
        public SafeFileHandle Handle { get { return xSFH; } }
        /// <summary>
        /// Is accessed
        /// </summary>
        public bool Accessed { get { return (xSFH != null && !xSFH.IsInvalid); } }
        /// <summary>
        /// This geometry
        /// </summary>
        public DiskGeometry Geometry { get { return xGeom; } }
        /// <summary>
        /// This type
        /// </summary>
        public DeviceType Type { get { return xType; } }
        /// <summary>
        /// Device name
        /// </summary>
        public string DeviceName
        {
            get
            {
                if (xType == DeviceType.PhysicalDrive)
                    return (xType.ToString() + xIndex.ToString());
                return ((char)xIndex).ToString() + ":";
            }
        }

        /// <summary>
        /// Grabs a physical drive from an index
        /// </summary>
        /// <param name="index"></param>
        public Drive(byte index) { xIndex = index; xType = DeviceType.PhysicalDrive; MakeHandle(); GetGeom(); }

        /// <summary>
        /// Grabs a logical drive from a letter
        /// </summary>
        /// <param name="Letter"></param>
        public Drive(char Letter) { xIndex = (byte)Letter; xType = DeviceType.LogicalDrive; MakeHandle(); GetGeom(); }

        /// <summary>
        /// Grabs drive
        /// </summary>
        /// <param name="index"></param>
        /// <param name="xtype"></param>
        public Drive(byte index, DeviceType xtype) { xIndex = index; xType = xtype; MakeHandle(); GetGeom(); }

        /// <summary>
        /// IO Attributes
        /// </summary>
        [Flags]
        public enum FlagsAndAttributes : uint
        {
            /// <summary>
            /// Read Only
            /// </summary>
            ReadOnly = 1,
            /// <summary>
            /// Hidden
            /// </summary>
            Hidden = 2,
            /// <summary>
            /// System
            /// </summary>
            System = 4,
            /// <summary>
            /// Directory
            /// </summary>
            Directory = 0x10,
            /// <summary>
            /// Archive
            /// </summary>
            Archive = 0x20,
            /// <summary>
            /// Device
            /// </summary>
            Device = 0x40,
            /// <summary>
            /// Normal
            /// </summary>
            Normal = 0x80,
            /// <summary>
            /// Temporary
            /// </summary>
            Temporary = 0x100,
            /// <summary>
            /// Sparse File
            /// </summary>
            SparseFile = 0x200,
            /// <summary>
            /// Reparse Point
            /// </summary>
            ReparsePoint = 0x400,
            /// <summary>
            /// Compressed
            /// </summary>
            Compressed = 0x800,
            /// <summary>
            /// Offline
            /// </summary>
            Offline = 0x1000,
            /// <summary>
            /// Not Content Indexed
            /// </summary>
            NotContentIndexed = 0x2000,
            /// <summary>
            /// Encrypted
            /// </summary>
            Encrypted = 0x4000,
            /// <summary>
            /// Write through
            /// </summary>
            Write_Through = 0x80000000,
            /// <summary>
            /// Overlapped
            /// </summary>
            Overlapped = 0x40000000,
            /// <summary>
            /// No Buffering
            /// </summary>
            NoBuffering = 0x20000000,
            /// <summary>
            /// Random Access
            /// </summary>
            RandomAccess = 0x10000000,
            /// <summary>
            /// Sequential Scan
            /// </summary>
            SequentialScan = 0x8000000,
            /// <summary>
            /// Delete on close
            /// </summary>
            DeleteOnClose = 0x4000000,
            /// <summary>
            /// Backup Semantics
            /// </summary>
            BackupSemantics = 0x2000000,
            /// <summary>
            /// Posix Semantics
            /// </summary>
            PosixSemantics = 0x1000000,
            /// <summary>
            /// Open Reparse Point
            /// </summary>
            OpenReparsePoint = 0x200000,
            /// <summary>
            /// Open No Recall
            /// </summary>
            OpenNoRecall = 0x100000,
            /// <summary>
            /// First Pipe Instance
            /// </summary>
            FirstPipeInstance = 0x80000
        }

        public void GetGeom()
        {
            xGeom = new DiskGeometry();
            uint blah;
            DeviceIoControl(xSFH, 0x70000, IntPtr.Zero, 0, ref xGeom,
                (uint)Marshal.SizeOf(typeof(DiskGeometry)), out blah, IntPtr.Zero);
        }

        public void MakeHandle()
        {
            //{ 
            if (xSFH != null)
            {
                xSFH.Close();
                xSFH = null;
            }
            //}catch { }
            xSFH = CreateFile(@"\\.\" + DeviceName.ToUpper(),
                 FileAccess.ReadWrite,
                 FileShare.ReadWrite,
                 IntPtr.Zero,
                 FileMode.Open,
                 FlagsAndAttributes.Device | FlagsAndAttributes.NoBuffering |
                 FlagsAndAttributes.Write_Through,
                 IntPtr.Zero);
        }
    }
    #endregion
}
/* For odd - numbered byte integers, you cannot make your own data
 * type in C#, therefore you can't make a signed integer because you
 * can't split the values in half for negative and positive, have to
 * resort to using unsigned integers only instead */