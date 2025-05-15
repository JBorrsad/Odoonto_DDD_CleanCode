namespace WF.Mimetic.Domain.Models.Serializers;

using Newtonsoft.Json;
using System.Xml;
using WF.Mimetic.Domain.Core.Models.Exceptions;

internal class XmlSerializer : ISerializer
{
    public SerializationType Type => SerializationType.Xml;

    public XmlSerializer()
    {
    }

    public bool TrySerialize(string msg, out string result)
    {
        if (msg is null)
        {
            throw new WrongOperationException("The msg cant be null.");
        }

        if (TrySerializeFromXml(msg, out result))
        {
            return true;
        }

        /*if (TrySerializeFromJson(msg, out result))
        {
            return true;
        }*/

        return TrySerializeFromJsonWithRootName(msg, out result);
    }

    private bool TrySerializeFromJson(string msg, out string result)
    {
        try
        {
            XmlDocument xml = JsonConvert.DeserializeXmlNode(msg);
            result = xml.OuterXml;
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    private bool TrySerializeFromJsonWithRootName(string msg, out string result)
    {
        try
        {
            XmlDocument xml = JsonConvert.DeserializeXmlNode(msg, "root");
            result = xml.OuterXml;
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    private bool TrySerializeFromXml(string msg, out string result)
    {
        try
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(msg);
            result = xml.OuterXml;
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
}
