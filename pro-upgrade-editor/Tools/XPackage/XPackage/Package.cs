using System;
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

        internal PackageFile(FileEntry file, bool readData = true, Package pk = null)
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

        internal PackageFolder(FolderEntry entry, bool readData = true, Package pk = null)
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

        internal Package(STFSPackage pk, bool readData = true, DJsIO dj = null)
        {
            this.package = pk;
            this.dj = dj;

            ReadData(readData, this);
        }

        void ReadData(bool readData = true, Package pk = null)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension">pass extension as ".ext|.ext2"</param>
        /// <returns></returns>
        public IEnumerable<PackageFile> GetFilesByExtension(string extension)
        {
            var ret = new List<PackageFile>();
            GetSubFiles(rootFolder, extension, ref ret);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="extension">pass extension as ".ext|.ext2"</param>
        /// <returns></returns>
        public IEnumerable<PackageFile> GetFiles(string folder, string extension)
        {
            var ret = new List<PackageFile>();
            if (folder == null)
            {
                return GetFilesByExtension(extension);
            }
            else
            {
                var f = rootFolder.Folders.SingleOrDefault(x => string.Compare(x.Name, folder, StringComparison.OrdinalIgnoreCase) == 0);
                if (f != null)
                {
                    foreach (var ex in extension.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        ret.AddRange(f.Files.Where(x => x.Name.EndsWith(ex, StringComparison.OrdinalIgnoreCase)));
                    }
                }
            }
            return ret;
        }

        public IEnumerable<PackageFile> GetFiles(string folder)
        {
            var ret = new List<PackageFile>();
            var f = rootFolder.Folders.SingleOrDefault(x => string.Compare(x.Name, folder, StringComparison.OrdinalIgnoreCase) == 0);
            if (f != null)
            {
                ret.AddRange(f.Files);
            }
            return ret;
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

        public static Package Load(string fileName, bool readData = false)
        {
            Package ret = null;
            try
            {

                DJsIO dj = new DJsIO(fileName, DJFileMode.Open, true);
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

        static byte[] GetBytes(char[] str)
        {
            return str.Select(x => (byte)x).ToArray();
        }

        public class CreateProConfig
        {
            public string displayTitle;
            public string description;
            public string guitarDifficulty;
            public string bassDifficulty;
            public string song_id;
            public string songShortName;
            public string proMidiFileName;
            public byte[] midFileContents;
            public byte[] existingCONFile;
        }
        public static byte[] CreateRB3Pro(CreateProConfig config)
        {
            STFSPackage pk = null;
            bool loadedExisting = false;
            if (config.existingCONFile != null)
            {
                try
                {
                    pk = LoadSTFS(config.existingCONFile);
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

                pk.Header.Description = config.description;
                pk.Header.Title_Display = config.displayTitle;

                var folder = pk.GetFolder("songs_upgrades");
                if (loadedExisting)
                {


                    var files = folder.GetSubFiles();

                    bool foundDTA = false;
                    bool foundMID = false;
                    foreach (var f in files)
                    {

                        if (string.Compare(f.Name, "upgrades.dta", StringComparison.OrdinalIgnoreCase) == 0)
                        {

                            string upgradeFile = Encoding.ASCII.GetString(Resources.upgrades);

                            upgradeFile = upgradeFile.Replace("##songshortname##", config.songShortName);
                            upgradeFile = upgradeFile.Replace("##profilename##", config.proMidiFileName);
                            upgradeFile = upgradeFile.Replace("##songid##", config.song_id);
                            upgradeFile = upgradeFile.Replace("##guitardifficulty##", config.guitarDifficulty);
                            upgradeFile = upgradeFile.Replace("##bassdifficulty##", config.bassDifficulty);

                            f.Replace(Encoding.ASCII.GetBytes(upgradeFile));
                            foundDTA = true;
                        }
                        else if (string.Compare(f.Name, config.proMidiFileName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            f.Replace(config.midFileContents);
                            foundMID = true;
                        }
                    }
                    loadedOK = foundDTA && foundMID;

                    if (!loadedOK)
                    {
                        pk.CloseIO();
                        config.existingCONFile = null;
                        return CreateRB3Pro(config);
                    }
                }
                else
                {
                    foreach (var f in folder.GetSubFiles())
                    {
                        if (f.Name.EndsWith(".mid", StringComparison.OrdinalIgnoreCase) ||
                            f.Name.EndsWith(".midi", StringComparison.OrdinalIgnoreCase))
                        {
                            f.Name = config.proMidiFileName;

                            f.Replace(config.midFileContents);
                        }
                        else if (f.Name.EndsWith(".dta", StringComparison.OrdinalIgnoreCase))
                        {
                            string upgradeFile = Encoding.ASCII.GetString(Resources.upgrades);

                            f.Name = "upgrades.dta";

                            upgradeFile = upgradeFile.Replace("##songshortname##", config.songShortName);
                            upgradeFile = upgradeFile.Replace("##profilename##", config.proMidiFileName);
                            upgradeFile = upgradeFile.Replace("##songid##", config.song_id);
                            upgradeFile = upgradeFile.Replace("##guitardifficulty##", config.guitarDifficulty);
                            upgradeFile = upgradeFile.Replace("##bassdifficulty##", config.bassDifficulty);

                            f.Replace(Encoding.ASCII.GetBytes(upgradeFile));
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
