namespace WF.Mimetic.Domain.Models.Serializers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using WF.Mimetic.Domain.Core.Models.Exceptions;

internal class JsonSerializer : ISerializer
{
    public SerializationType Type => SerializationType.Json;

    public JsonSerializer()
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

        return TrySerializeFromJson(msg, out result);
    }

    private bool TrySerializeFromJson(string json, out string result)
    {
        try
        {
            JObject.Parse(json);
            result = json;
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    private bool TrySerializeFromXml(string json, out string result)
    {
        try
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(json);
            result = JsonConvert.SerializeXmlNode(xml);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
}
