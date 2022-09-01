using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// Acturus Dev Note:
/// This script is purely for internal use.
/// In other words, don't use this.
/// </summary>

namespace Arcturus.Xml.Internal
{
    public class XmlData : IXmlSerializable
    {
        public XmlData() { }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            DataReader._XmlDataInstanceRef.OnRead?.Invoke(reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            DataReader._XmlDataInstanceRef.OnWrite?.Invoke(writer);
        }
    }
}
