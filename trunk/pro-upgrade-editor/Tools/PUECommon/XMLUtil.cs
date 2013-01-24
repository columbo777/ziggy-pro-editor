using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProUpgradeEditor.Common
{
    public class XMLUtil
    {
        public static XmlDocument LoadXml(string xml)
        {
            var ret = new XmlDocument();
            try
            {
                ret.LoadXml(xml);
            }
            catch { }
            return ret;
        }
        public static string GetNodeValue(XmlNode rootNode)
        {
            string ret = string.Empty;
            if (rootNode != null)
            {
                var n = rootNode;
                if (n != null)
                {
                    if (n.NodeType == XmlNodeType.Attribute)
                    {
                        ret = n.Value ?? string.Empty;
                    }
                    else
                    {
                        ret = n.InnerText ?? string.Empty;
                    }
                }
            }
            return ret;
        }
        public static string GetNodeValue(XmlNode rootNode, string xpath)
        {
            string ret = string.Empty;
            if (rootNode != null)
            {
                var n = rootNode.SelectSingleNode(xpath);
                if (n != null)
                {
                    if (n.NodeType == XmlNodeType.Attribute)
                    {
                        ret = n.Value ?? string.Empty;
                    }
                    else
                    {
                        ret = n.InnerText ?? string.Empty;
                    }
                }
            }
            return ret;
        }

        public static string GetNodeValue(XmlNode rootNode, string xpath, string defaultValue)
        {
            string ret = defaultValue;
            if (rootNode != null)
            {
                var n = rootNode.SelectSingleNode(xpath);
                if (n != null)
                {
                    if (n.NodeType == XmlNodeType.Attribute)
                    {
                        ret = n.Value ?? defaultValue;
                    }
                    else
                    {
                        ret = n.InnerText ?? defaultValue;
                    }
                }
                else
                {
                    ret = defaultValue;
                }
            }
            return ret;
        }

        public static int GetNodeValueInt(XmlNode rootNode, string xpath, int defaultValue = Int32.MinValue)
        {
            int ret = defaultValue;
            if (rootNode != null)
            {
                var n = rootNode.SelectSingleNode(xpath);
                if (n != null)
                {
                    if (n.NodeType == XmlNodeType.Attribute)
                    {
                        if (string.IsNullOrEmpty(n.Value) || !int.TryParse(n.Value, out ret))
                        {
                            ret = defaultValue;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(n.InnerText) || !int.TryParse(n.InnerText, out ret))
                        {
                            ret = defaultValue;
                        }
                    }
                }
                else
                {
                    ret = defaultValue;
                }
            }
            return ret;
        }

        public static bool GetNodeValueBool(XmlNode rootNode, string xpath, bool defaultValue = false)
        {
            var val = GetNodeValue(rootNode, xpath);
            bool ret = defaultValue;
            if (string.IsNullOrEmpty(val) ||
                bool.TryParse(val, out ret) == false)
            {
                ret = defaultValue;
            }
            return ret;
        }

        public static XmlNode GetNode(XmlNode rootNode, string xpath)
        {
            if (rootNode != null)
            {
                return rootNode.SelectSingleNode(xpath);
            }
            return null;
        }
        public static string SetNodeValue(XmlNode n, string value)
        {
            string ret = string.Empty;
            if (n != null)
            {
                if (n.NodeType == XmlNodeType.Attribute)
                {
                    n.Value = value;
                }
                else
                {
                    n.InnerText = value;
                }

            }
            return ret;
        }
        public static string SetNodeValue(XmlNode rootNode, string xpath, string value)
        {
            string ret = string.Empty;
            if (rootNode != null)
            {
                var n = rootNode.SelectSingleNode(xpath);
                if (n != null)
                {
                    if (n.NodeType == XmlNodeType.Attribute)
                    {
                        n.Value = value;
                    }
                    else
                    {
                        n.InnerText = value;
                    }
                }
            }
            return ret;
        }

        public static bool NodeExists(XmlNode rootNode, string xpath)
        {
            if (rootNode == null)
                return false;
            return (rootNode.SelectSingleNode(xpath) == null ? false : true);
        }

        public static XmlNode AddNode(XmlNode parentNode, string name)
        {
            if (parentNode == null)
                return null;
            XmlNode doc = parentNode.OwnerDocument;
            if (doc == null)
                doc = parentNode;

            XmlNode n = ((XmlDocument)doc).CreateElement(name);
            return parentNode.AppendChild(n);
        }

        public static XmlAttribute AddAttribute(XmlNode parentNode, string name, string value)
        {
            if (parentNode.Attributes[name] == null)
            {
                XmlAttribute n = parentNode.OwnerDocument.CreateAttribute(name);
                n.Value = value;
                return parentNode.Attributes.Append(n);
            }
            else
            {
                parentNode.Attributes[name].Value = value;
                return parentNode.Attributes[name];
            }

        }

        public static List<XmlNode> GetNodeList(XmlNode rootNode, string xpath)
        {
            var ret = new List<XmlNode>();
            if (rootNode != null)
            {
                var nodes = rootNode.SelectNodes(xpath);
                if (nodes != null)
                {
                    for (int x = 0; x < nodes.Count; x++)
                    {
                        ret.Add(nodes[x]);
                    }
                }
            }
            return ret;
        }


    }
}
