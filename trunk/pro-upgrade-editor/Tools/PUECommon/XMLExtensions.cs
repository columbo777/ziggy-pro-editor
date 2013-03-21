using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;

namespace ProUpgradeEditor.Common
{
    public static class XmlExtension
    {
        public static XmlDocument ToXmlDocument(this string str, string rootName = "")
        {
            if (!str.IsEmpty())
            {
                if (!string.IsNullOrEmpty(rootName))
                {
                    var doc = new XmlDocument();
                    var root = XMLUtil.AddNode(doc.DocumentElement, rootName);
                    root.SetValue(str);
                    return doc;
                }
                else
                {
                    return XMLUtil.LoadXml(str);
                }
            }
            return null;
        }

        public static XmlDocument GetXMLDocument(this XmlNode node)
        {
            XmlDocument ret = null;
            if (node != null)
            {
                if (node.OwnerDocument == null)
                {
                    ret = node as XmlDocument;
                }
                else
                {
                    ret = node.OwnerDocument;
                }
            }
            return ret;
        }


        public static XmlAttribute AddAttribute(this XmlNode node, string name, string value)
        {
            return XMLUtil.AddAttribute(node, name, value);
        }

        public static XmlNode GetNode(this XmlNode node, string name) 
        { 
            return XMLUtil.GetNode(node, name); 
        }


        public static XmlNode AddNode(this XmlNode node, string name)
        {
            return XMLUtil.AddNode(node, name);
        }

        public static XmlNode AddNodes(this XmlNode node, IEnumerable<XmlNode> nodes)
        {
            if (nodes != null && node != null)
            {
                nodes.ToList().ForEach(x => node.AppendChild(x));
            }
            return node;
        }
        public static XmlNode AddNodes(this XmlNode node, IEnumerable<string> nodeValues, string nodeName)
        {
            if (nodeValues != null && node != null && !string.IsNullOrEmpty(nodeName))
            {
                nodeValues.ToList().ForEach(x => XMLUtil.AddNode(node, nodeName).SetValue(x));
            }
            return node;
        }
        public static IEnumerable<XmlNode> GetChildren(this XmlNode node)
        {
            var ret = new List<XmlNode>();
            if (node != null && node.ChildNodes != null && node.ChildNodes.Count > 0)
            {
                for (int x = 0; x < node.ChildNodes.Count; x++)
                {
                    XmlNode n = node.ChildNodes[x];
                    ret.Add(n);
                }
            }
            return ret;
        }
        public static IEnumerable<XmlAttribute> GetAttributes(this XmlNode node)
        {
            var ret = new List<XmlAttribute>();
            if (node != null && node.Attributes != null && node.Attributes.Count > 0)
            {
                for (int x = 0; x < node.Attributes.Count; x++)
                {
                    XmlAttribute n = node.Attributes[x];
                    ret.Add(n);
                }
            }
            return ret;
        }
        public static XmlNode GetNode(this XmlAttribute attrib)
        {
            return ((attrib != null) ? (attrib.OwnerElement as XmlNode) : null);
        }

        public static XmlNode GetNode(this XmlDocument doc, string xpath)
        {
            return XMLUtil.GetNode(doc, xpath);
        }
        public static XmlNode SetValue(this XmlNode node, string xpath, string value)
        {
            XMLUtil.SetNodeValue(node, xpath, value);
            return node;
        }
        public static XmlNode SetValue(this XmlNode node, string value)
        {
            XMLUtil.SetNodeValue(node, value);
            return node;
        }
        public static string GetOuterXML(this XmlDocument doc)
        {
            var ret = string.Empty;
            if (doc != null)
            {
                ret = doc.OuterXml;
            }
            return ret ?? "";
        }

    }

}
