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
        public FileEntry FileEntry { get; internal set; }
        public PackageFolder Folder { get; internal set; }

        public Package Package { get; internal set; }

        public string Name { get { return FileEntry.Name; } set { FileEntry.Name = value; } }

        byte[] data;
        public byte[] Data
        {
            get
            {

                if (data == null)
                {

                    try
                    {
                        Package.package.xIO.OpenAgain();
                        FileEntry.xPackage = Package.package;
                        data = FileEntry.ExtractBytes(true);
                        Package.package.xIO.Close();
                    }
                    catch
                    {
                       
                    }
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        public PackageFile(PackageFolder folder, FileEntry file, bool readData = true, Package pk = null)
        {
            this.FileEntry = file;
            this.Folder = folder;
            this.Package = pk;

           
                this.data = file.ExtractBytes(readData);
           
        }
    }
    public class PackageFolder
    {
        public string Name
        {
            get { return FolderEntry.Name; }
        }

        public PackageFolder Parent { get; internal set; }

        public IEnumerable<PackageFolder> GetChildFolderByName(string name, bool recursive)
        {
            var ret = new List<PackageFolder>();
            
            foreach (var folder in Folders)
            {
                if (string.Compare(folder.Name, name, true) == 0)
                    ret.Add(folder);

                if (recursive && folder.Folders.Any())
                {
                    ret.AddRange(folder.GetChildFolderByName(name, recursive));
                }
            }
            return ret;
        }
        

        public FolderEntry FolderEntry{get;internal set;}
        public List<PackageFile> Files;
        public List<PackageFolder> Folders;
        public Package Package { get; internal set; }

        internal PackageFolder(PackageFolder parent, FolderEntry entry, bool readData = true, Package pk = null)
        {
            this.FolderEntry = entry;
            Parent = parent;
            Package = pk;
            Files = new List<PackageFile>();
            Folders = new List<PackageFolder>();

            foreach (var f in entry.GetSubFiles())
            {
                Files.Add(new PackageFile(this, f, readData, pk));
            }

            foreach (var f in entry.GetSubFolders())
            {
                Folders.Add(new PackageFolder(this, f, readData, pk));
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
            rootFolder = new PackageFolder(null, package.RootDirectory, readData, pk);
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
        void GetSubFilesByName(PackageFolder folder, string fileName, ref List<PackageFile> ret)
        {
            
            foreach (var file in folder.Files)
            {
                
                if (string.Compare(file.Name,fileName,true)==0)
                {
                    ret.Add(file);
                }
                
            }

            foreach (var subf in folder.Folders)
            {
                GetSubFilesByName(subf, fileName, ref ret);
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
        public IEnumerable<PackageFile> GetFilesByName(string fileName)
        {
            var ret = new List<PackageFile>();
            GetSubFilesByName(rootFolder, fileName, ref ret);
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
        public static Package Load(byte[] fileBytes, bool loadBytes=false)
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
            public CreateProConfig()
            {
                TitleID = 45410914;

            }
            public string displayTitle;
            public string description;
            public string guitarDifficulty;
            public string bassDifficulty;
            public string song_id;
            public string songShortName;
            public string proMidiFileName;
            public byte[] midFileContents;
            public byte[] existingCONFile;
            public uint TitleID;
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
                
                pk.Header.TitleID = config.TitleID;
                //pk.Header.MediaID = 4FC9256F;

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

        public static byte[] RebuildPackage(Package package)
        {
            byte[] ret = null;
            try
            {

                if (package.dj == null || (package.dj != null && package.dj.OpenAgain()))
                {
                    try
                    {
                        ret = package.package.RebuildPackageInMemory(
                            new RSAParams(new DJsIO(Resources.KV, true)));
                    }
                    finally
                    {
                        package.package.CloseIO();
                    }
                }
            }
            catch { }
            return ret;
        }

        public static byte[] AddFileToFolder(Package package, PackageFolder folder, string fileName, byte[] fileContents)
        {
            byte[] ret = null;
            try
            {

                if (package.dj == null || (package.dj != null && package.dj.OpenAgain()))
                {
                    try
                    {
                        if (package.package.CanWrite == false)
                            package.package.OpenAgain();

                        if (package.package.CanWrite)
                        {
                            if (package.package.AddFileToFolder(folder.FolderEntry, fileName, fileContents))
                            {
                                ret = package.package.RebuildPackageInMemory(
                                    new RSAParams(new DJsIO(Resources.KV, true)));
                            }
                        }
                    }
                    finally
                    {
                        package.package.CloseIO();
                    }
                }
            }
            catch { }
            return ret;
        }

        public static byte[] RebuildPackageInMemory(Package package)
        {
            byte[] ret = null;
            try
            {

                if (package.dj == null || (package.dj != null && package.dj.OpenAgain()))
                {
                    try
                    {
                        ret = package.package.RebuildPackageInMemory(
                            new RSAParams(new DJsIO(Resources.KV, true)));
                    }
                    finally
                    {
                        package.package.CloseIO();
                    }
                }
            }
            catch { }
            return ret;
        }
    }



}
