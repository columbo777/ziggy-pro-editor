using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;


namespace ProUpgradeEditor.Common
{

    public class SettingMgr
    {
        XmlDocument xml = null;
        XmlDocument defaultxml = null;
        bool loadSaved = true;
        XmlNode root;
        XmlNode defaultroot;

        public XmlNode XMLRoot { get { return root; } }
        string cache_executingFolder;
        string executingFolder
        {
            get
            {
                if (cache_executingFolder == null)
                {
                    cache_executingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }
                return cache_executingFolder;
            }
        }

        string currentExecutingFolder;

        string settingFileName;
        public string SettingFileName
        {
            get
            {
                if (string.IsNullOrEmpty(settingFileName))
                {
                    settingFileName = defaultSettingFileName;
                }
                return settingFileName;
            }
            set { settingFileName = value; }
        }

        string defaultSettingFileName = "settings.xml";
        public string DefaultSettingFileName
        {
            get
            {
                return defaultSettingFileName;
            }
        }

        public string SettingFilePath
        {
            get
            {
                if (currentExecutingFolder == null)
                {
                    return Path.Combine(executingFolder, DefaultSettingFileName);
                }
                return Path.Combine(currentExecutingFolder, SettingFileName);
            }
            set
            {
                try
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        settingFileName = Path.GetFileName(value);
                        currentExecutingFolder = Path.GetDirectoryName(value);
                        xml = null;
                        root = null;

                    }
                }
                catch
                {
                    settingFileName = defaultSettingFileName;
                    currentExecutingFolder = executingFolder;
                    xml = defaultxml;
                    root = defaultroot;
                }
            }
        }

        public string DefaultSettingFilePath
        {
            get { return Path.Combine(executingFolder, DefaultSettingFileName); }
        }

        public SettingMgr()
        {

        }

        public void LoadSettings()
        {
            if (xml == null)
            {
                xml = new XmlDocument();
            }
            if (defaultxml == null)
            {
                defaultxml = new XmlDocument();

                if (File.Exists(DefaultSettingFilePath))
                {
                    try
                    {
                        defaultxml.Load(DefaultSettingFilePath);
                        defaultroot = XMLUtil.GetNode(defaultxml, "root");

                    }
                    catch
                    {
                        defaultroot = null;
                    }

                    if (defaultroot == null)
                    {
                        defaultxml = new XmlDocument();
                        defaultroot = XMLUtil.AddNode(defaultxml, "root");
                    }
                }
                else
                {
                    defaultroot = XMLUtil.AddNode(defaultxml, "root");

                }
            }



            try
            {
                if (SettingFilePath == DefaultSettingFilePath)
                {
                    xml = defaultxml;
                    root = defaultroot;

                    if (loadSaved)
                    {
                        var lastFile = GetDefaultSettingValue("lastSettingFile");
                        if (!string.IsNullOrEmpty(lastFile) &&
                            File.Exists(lastFile) &&
                            lastFile != DefaultSettingFilePath)
                        {
                            SettingFilePath = lastFile;
                            if (File.Exists(SettingFilePath))
                            {
                                try
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.Load(SettingFilePath);
                                    this.xml = doc;
                                    root = XMLUtil.GetNode(xml, "root");
                                    if (root == null)
                                        xml = null;
                                }
                                catch { }
                            }
                        }

                        loadSaved = false;
                    }
                }
                else
                {
                    if (root == null)
                    {
                        if (File.Exists(SettingFilePath))
                        {
                            try
                            {
                                XmlDocument x = new XmlDocument();
                                x.Load(SettingFilePath);
                                this.xml = x;
                                root = XMLUtil.GetNode(xml, "root");
                                if (root == null)
                                    xml = null;
                            }
                            catch { }
                        }
                    }
                }
            }
            catch
            {
                xml = defaultxml;
                root = defaultroot;
            }

        }

        string getValuePath(string name)
        {
            return string.Format("config[@name='{0}']/@value", name);
        }

        public void SetValue(string name, int value)
        {
            var valuePath = getValuePath(name);
            if (XMLUtil.NodeExists(root, valuePath))
            {
                XMLUtil.SetNodeValue(root, valuePath, value.ToString());
            }
            else
            {
                if (root != null)
                {
                    var cfg = XMLUtil.AddNode(root, "config");
                    XMLUtil.AddAttribute(cfg, "name", name);
                    XMLUtil.AddAttribute(cfg, "value", value.ToString());
                }
            }
        }

        public void SetValue(string name, string value)
        {
            var valuePath = getValuePath(name);

            if (XMLUtil.NodeExists(root, valuePath))
            {
                XMLUtil.SetNodeValue(root, valuePath, value);
            }
            else
            {
                if (root != null)
                {
                    var cfg = XMLUtil.AddNode(root, "config");
                    XMLUtil.AddAttribute(cfg, "name", name);
                    XMLUtil.AddAttribute(cfg, "value", value);
                }
            }
        }

        public void SetDefaultSettingValue(string name, string value)
        {
            var valuePath = getValuePath(name);
            if (XMLUtil.NodeExists(defaultroot, valuePath))
            {
                XMLUtil.SetNodeValue(defaultroot, valuePath, value);
            }
            else
            {
                var cfg = XMLUtil.AddNode(defaultroot, "config");
                XMLUtil.AddAttribute(cfg, "name", name);
                XMLUtil.AddAttribute(cfg, "value", value);
            }
        }
        public void SetValue(string name, bool value)
        {
            SetValue(name, value.ToString());
        }
        public string GetValue(string name)
        {
            var valuePath = getValuePath(name);
            if (XMLUtil.NodeExists(root, valuePath) == false)
            {
                valuePath = getValuePath("Util_" + name);
                if (XMLUtil.NodeExists(root, valuePath) == false)
                {
                    return string.Empty;
                }
            }
            return XMLUtil.GetNodeValue(root, valuePath);
        }
        public string GetDefaultSettingValue(string name)
        {
            var valuePath = getValuePath(name);
            return XMLUtil.GetNodeValue(defaultroot, valuePath);
        }
        public int GetValueInt(string name, int defaultValue)
        {
            var i = defaultValue;

            var valuePath = getValuePath(name);
            if (XMLUtil.NodeExists(root, valuePath) == false)
            {
                valuePath = getValuePath("Util_" + name);
                if (XMLUtil.NodeExists(root, valuePath) == false)
                {
                    return defaultValue;
                }
            }
            
            var r = XMLUtil.GetNodeValue(root, valuePath);
            if (string.IsNullOrEmpty(r) || int.TryParse(r, out i) == false)
            {
                i = defaultValue;
            }
            return i;

        }
        public string GetValue(string name, string defaultValue)
        {
            var valuePath = getValuePath(name);
            if (XMLUtil.NodeExists(root, valuePath) == false)
            {
                return defaultValue;
            }
            else
            {
                return XMLUtil.GetNodeValue(root, valuePath);
            }
        }
        public bool HasSetting(string name)
        {
            return XMLUtil.NodeExists(root, name);
        }
        public System.Drawing.Color GetValue(string name, System.Drawing.Color defaultColor)
        {
            var valuePath = getValuePath(name);
            if (XMLUtil.NodeExists(root, valuePath) == false)
            {
                return defaultColor;
            }
            else
            {
                var val = XMLUtil.GetNodeValue(root, valuePath);

                int i;
                if (string.IsNullOrEmpty(val) || int.TryParse(val, out i) == false)
                {
                    return defaultColor;
                }
                else
                {
                    return System.Drawing.Color.FromArgb(i);
                }
            }
        }
        public bool GetValueBool(string name, bool defaultValue)
        {
            var valuePath = getValuePath(name);
            
            if (XMLUtil.NodeExists(root, valuePath) == false)
            {
                valuePath = getValuePath("Util_" + name);
                if (XMLUtil.NodeExists(root, valuePath) == false)
                {
                    return defaultValue;
                }
            }
            var v = XMLUtil.GetNodeValue(root, valuePath);
            bool bret;
            if (string.IsNullOrEmpty(v) || bool.TryParse(v, out bret) == false)
                return defaultValue;
            else
                return bret;
        }
        public void SaveConfig()
        {
            if (defaultxml != null && xml != null)
            {
                if (string.IsNullOrEmpty(SettingFilePath) == false &&
                    SettingFilePath != DefaultSettingFilePath &&
                    xml != defaultxml)
                {
                    try
                    {
                        xml.Save(SettingFilePath);
                        SetDefaultSettingValue("lastSettingFile", SettingFilePath);
                    }
                    catch { }

                }
                else
                {
                    SetDefaultSettingValue("lastSettingFile", "");
                }
                defaultxml.Save(DefaultSettingFilePath);
            }

        }
    }

}
