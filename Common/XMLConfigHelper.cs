using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Common
{
    public class XMLConfigHelper
    {
        /// <summary>
        /// XML序列化与反序列化类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Reader<T>
        {
            /// <summary>
            /// XML转对象
            /// </summary>
            /// <param name="xmlPath">XML的地址</param>
            /// <returns>返回对象</returns>
            public T ReadXML(string xmlPath)
            {
                T t;
                XmlSerializer myXmlSerializer = new XmlSerializer(typeof(T));
                FileStream myFileStream = new FileStream(xmlPath, FileMode.Open);
                t = (T)myXmlSerializer.Deserialize(myFileStream);
                myFileStream.Close();
                return t;

            }
            /// <summary>
            /// 对象转XML
            /// </summary>
            /// <param name="xmlPath">Xml地址</param>
            /// <param name="t">类型</param>
            /// <returns>ture为转化成功，否则为失败</returns>
            public bool ObjectToXML(string xmlPath, T t)
            {
                XmlDocument doc = new XmlDocument();
                if (!File.Exists(xmlPath))
                {
                    doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
                    XmlElement root = doc.CreateElement(typeof(T).Name);
                    doc.AppendChild(root);
                }
                XmlSerializer mySerializer = new XmlSerializer(typeof(T));
                StreamWriter myWriter = new StreamWriter(xmlPath);
                mySerializer.Serialize(myWriter, t);
                myWriter.Close();
                doc.Save(xmlPath);
                return true;
            }
        }
    }
}
