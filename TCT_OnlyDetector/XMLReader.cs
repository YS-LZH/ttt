/*
 * Project: DCBCT Software: tACQ
 * Company: TCT
 * Author:  Irene Kuan
 * Created: Aug. 2012
 * Notes:   This was created to read XML file parameters.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
namespace TCT_OnlyDetector
{
    class XMLReader
    {
        /// <summary>This function is used to read XML file and get single element's value and return as a string</summary>
        /// <param name="ElementName">The element name the ReadXMLFile function will get</param>
        /// <param name="FileName">The XML FileName the ReadXMLFile function will get</param>
        /// <returns>The value the ReadXMLFile function read from XML file and return</returns>
        public static string ReadXMLFile(string TagName, string ElementName, string FileName)
        {
            string value = "";
            try
            {
                XmlTextReader reader = new XmlTextReader(FileName);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(reader);
                // xmldoc.Load(FileName);
                XmlNodeList parentNode = xmldoc.SelectNodes(TagName).Item(0).ChildNodes;
                foreach (XmlNode isbn in parentNode)
                {
                    if (isbn.LocalName == ElementName)
                    {
                        value = isbn.InnerText;
                        break;
                    }
                }
                xmldoc = null;
                reader.Close();
                return value;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return value;
            }
        }
        public static string ReadXMLFile(string TagName, string ElementName, XmlDocument xmldoc)
        {
            string value = "";
            try
            {
                //XmlTextReader reader = new XmlTextReader(FileName);
                //XmlDocument xmldoc = new XmlDocument();
                //xmldoc.Load(reader);
                XmlNodeList parentNode = xmldoc.SelectNodes(TagName).Item(0).ChildNodes;
                foreach (XmlNode isbn in parentNode)
                {
                    if (isbn.LocalName == ElementName)
                    {
                        value = isbn.InnerText;
                        break;
                    }
                }
                // xmldoc = null;
                // reader.Close();
                return value;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return value;
            }
        }
    }
}
