﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using X360.STFS;
using X360.IO;
using X360.FATX;

namespace XPackage
{

    public class PackageFile
    {
        FileEntry file;
        
        Package package;
        
        public string Name { get { return file.Name; } set { file.Name = value; } }

        byte[] data;
        public byte[] Data 
        { 
            get 
            {
                
                if (data == null)
                {

                    try
                    {
                        package.package.xIO.OpenAgain();
                        file.xPackage = package.package;
                        data = file.ExtractBytes();
                        package.package.xIO.Close();
                    }
                    catch 
                    {
                        package = new Package(package.package,
                            true);
                        data = file.ExtractBytes();
                    }
                }
                return data; 
            } 
        }

        internal PackageFile(FileEntry file, bool readData=true, Package pk=null)
        {
            this.file = file;
            this.package = pk;
            if (readData)
            {
                
                this.data = file.ExtractBytes();
                
            }
        }
    }
    public class PackageFolder
    {
        public string Name
        {
            get { return folder.Name; }
        }

        FolderEntry folder;
        public List<PackageFile> Files;
        public List<PackageFolder> Folders;

        internal PackageFolder(FolderEntry entry, bool readData=true, Package pk=null)
        {
            this.folder = entry;

            Files = new List<PackageFile>();
            Folders = new List<PackageFolder>();

            foreach (var f in entry.GetSubFiles())
            {
                Files.Add(new PackageFile(f, readData, pk));
            }

            foreach (var f in entry.GetSubFolders())
            {
                Folders.Add(new PackageFolder(f, readData, pk));
            }
        }
    }
    public class Package
    {
        public STFSPackage package;
        PackageFolder rootFolder;
        public DJsIO dj;

        public string Name
        {
            get { return package.Header.Description; }
        }

        internal Package(STFSPackage pk, bool readData=true, DJsIO dj=null)
        {
            this.package = pk;
            this.dj = dj;

            ReadData(readData, this);
        }

        void ReadData(bool readData=true, Package pk=null)
        {
            rootFolder = new PackageFolder(package.RootDirectory, readData, pk);
        }

        public PackageFolder RootFolder
        {
            get { return rootFolder; }
        }

        public PackageFile GetFile(string folder, string fileName)
        {
            var f = rootFolder.Folders.SingleOrDefault(x => string.Compare(x.Name, folder, StringComparison.OrdinalIgnoreCase) == 0);
            if (f != null)
            {
                return f.Files.SingleOrDefault(x => string.Compare(x.Name, fileName, StringComparison.OrdinalIgnoreCase) == 0);
            }
            return null;
        }

        void GetSubFiles(PackageFolder folder, string extension, ref List<PackageFile> ret)
        {
            var exts = extension.Split('|');
            foreach (var file in folder.Files)
            {
                foreach (var ex in exts)
                {
                    if (file.Name.EndsWith(ex, StringComparison.OrdinalIgnoreCase))
                    {
                        ret.Add(file);
                    }
                }
            }

            foreach (var subf in folder.Folders)
            {
                GetSubFiles(subf, extension, ref ret);
            }

        }

        public PackageFile[] GetFilesByExtension(string extension)
        {
            List<PackageFile> ret = new List<PackageFile>();
            GetSubFiles(rootFolder, extension, ref ret);
            return ret.ToArray();
        }

        public PackageFile[] GetFiles(string folder, string extension)
        {
            if (folder == null)
            {
                return GetFilesByExtension(extension);
            }
            else
            {
                var f = rootFolder.Folders.SingleOrDefault(x => string.Compare(x.Name, folder, StringComparison.OrdinalIgnoreCase) == 0);
                if (f != null)
                {
                    var exs = extension.Split('|');
                    foreach (var ex in exs)
                    {
                        return f.Files.Where(x => x.Name.EndsWith(ex, StringComparison.OrdinalIgnoreCase) == true).ToArray();
                    }
                }
            }
            return null;
        }

        public PackageFile[] GetFiles(string folder)
        {
            var f = rootFolder.Folders.SingleOrDefault(x => string.Compare(x.Name, folder, StringComparison.OrdinalIgnoreCase) == 0);
            if (f != null)
            {
                return f.Files.ToArray();
            }
            return null;
        }

        public static byte[] ReadFileBytes(string fileName)
        {
            byte[] buffer = null;
            if (File.Exists(fileName))
            {
                buffer = File.ReadAllBytes(fileName);
                
            }
            return buffer;
        }
        public static Package Load(byte[] fileBytes)
        {
            Package ret = null;
            try
            {
                STFSPackage pk = null;
                try
                {
                    pk = new STFSPackage(fileBytes, null);
                }
                catch
                {
                    if (pk != null)
                        pk.CloseIO();
                }
                if (pk != null)
                {
                    if (pk.ParseSuccess == true)
                    {
                        ret = new Package(pk);

                    }
                    pk.CloseIO();
                }

            }
            catch { }
            return ret;
        }

        public static Package Load(string fileName, bool readData=false)
        {
            Package ret = null;
            try
            {
                
                DJsIO dj = new DJsIO(fileName,DJFileMode.Open, true);
                STFSPackage pk = null;
                try
                {
                    
                    pk = new STFSPackage(dj, null);
                }
                catch
                {
                    if (pk != null)
                        pk.CloseIO();
                }
                if (pk != null)
                {
                    if (pk.ParseSuccess == true)
                    {
                        ret = new Package(pk, readData);
                        
                    }
                    pk.CloseIO();
                }

                
            }
            catch { }
            return ret;
        }
        public static Package Load(DJsIO dj, bool readData = false)
        {
            Package ret = null;
            try
            {

                STFSPackage pk = null;
                try
                {

                    pk = new STFSPackage(dj, null);
                }
                catch
                {
                    if (pk != null)
                        pk.CloseIO();
                }
                if (pk != null)
                {
                    if (pk.ParseSuccess == true)
                    {
                        ret = new Package(pk, readData, dj);

                    }
                    pk.CloseIO();
                }


            }
            catch { }
            return ret;
        }
        

        static STFSPackage LoadSTFS(string fileName)
        {
            return LoadSTFS(ReadFileBytes(fileName));
        }

        static STFSPackage LoadSTFS(byte[] data)
        {
            DJsIO dj = new DJsIO(data, true);
            var ret = new STFSPackage(dj, null);
            if (ret.ParseSuccess == false)
                ret = null;
            return ret;
        }

        public static byte[] GetSongIDList()
        {
            return Resources.song_ids;
        }

        public static byte[] CreateRB3Pro(
            string description,
            string guitarDifficulty,
            string bassDifficulty,
            string song_id,
            string songShortName,
            string proMidiFileName, byte[] midFileContents,
            byte[] existingCONFile)
        {


            STFSPackage pk = null;
            bool loadedExisting = false;
            if(existingCONFile != null)
            {
                try
                {
                    
                    pk = LoadSTFS(existingCONFile);
                }
                catch { }
                if (pk == null)
                {
                    pk = LoadSTFS(Resources.proconfile);
                }
                else
                {
                    loadedExisting = true;
                }
            }
            else
            {
                pk = LoadSTFS(Resources.proconfile);
            }
            bool loadedOK = false;
            if (pk != null)
            {
                
                //pk.Header.ContentImage = Resources.rockband;
                //pk.Header.PackageImage = Resources.rockband2;

                pk.Header.Description = description;
                var folder = pk.GetFolder("songs_upgrades");
                if (loadedExisting)
                {
                    

                    var files = folder.GetSubFiles();

                    bool foundDTA = false;
                    bool foundMID = false;
                    foreach (var f in files)
                    {
                        
                        if (string.Compare(f.Name,"upgrades.dta", StringComparison.OrdinalIgnoreCase)==0)
                        {

                            string upgradeFile = Encoding.ASCII.GetString(Resources.upgrades);

                            upgradeFile = upgradeFile.Replace("##songshortname##", songShortName);
                            upgradeFile = upgradeFile.Replace("##profilename##", proMidiFileName);
                            upgradeFile = upgradeFile.Replace("##songid##", song_id);
                            upgradeFile = upgradeFile.Replace("##guitardifficulty##", guitarDifficulty);
                            upgradeFile = upgradeFile.Replace("##bassdifficulty##", bassDifficulty);
                            
                            f.Replace(Encoding.ASCII.GetBytes(upgradeFile));
                            foundDTA = true;
                        }
                        else if (string.Compare(f.Name, proMidiFileName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            f.Replace(midFileContents);
                            foundMID = true;
                        }
                    }
                    loadedOK = foundDTA && foundMID;

                    if (!loadedOK)
                    {
                        pk.CloseIO();
                        return CreateRB3Pro(description, guitarDifficulty, bassDifficulty, song_id, songShortName, proMidiFileName,
                            midFileContents, null);
                    }
                }
                else
                {
                    foreach (var f in folder.GetSubFiles())
                    {
                        if (f.Name.EndsWith(".mid", StringComparison.OrdinalIgnoreCase))
                        {
                            f.Name = proMidiFileName;

                            f.Replace(midFileContents);
                        }
                        else if (f.Name.EndsWith(".dta", StringComparison.OrdinalIgnoreCase))
                        {
                            string upgradeFile = Encoding.UTF8.GetString(Resources.upgrades);

                            f.Name = "upgrades.dta";

                            upgradeFile = upgradeFile.Replace("##songshortname##", songShortName);
                            upgradeFile = upgradeFile.Replace("##profilename##", proMidiFileName);
                            upgradeFile = upgradeFile.Replace("##songid##", song_id);
                            upgradeFile = upgradeFile.Replace("##guitardifficulty##", guitarDifficulty);
                            upgradeFile = upgradeFile.Replace("##bassdifficulty##", bassDifficulty);

                            f.Replace(Encoding.UTF8.GetBytes(upgradeFile));
                        }
                    }
                }
            }


            var bytes = pk.RebuildPackageInMemory(
                new RSAParams(new DJsIO(Resources.KV, true)));

            pk.CloseIO();

            return bytes;
        }


    }



}
