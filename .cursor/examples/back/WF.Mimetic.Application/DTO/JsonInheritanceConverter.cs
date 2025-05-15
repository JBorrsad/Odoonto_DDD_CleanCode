namespace WF.Mimetic.Application.DTO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

public class JsonInheritanceConverter : JsonConverter
{
    internal static readonly string DefaultDiscriminatorName = "discriminator";

    private readonly string _discriminatorName;

    [ThreadStatic]
    private static bool _isReading;

    [ThreadStatic]
    private static bool _isWriting;

    public JsonInheritanceConverter()
    {
        _discriminatorName = DefaultDiscriminatorName;
    }

    public JsonInheritanceConverter(string discriminatorName)
    {
        _discriminatorName = discriminatorName;
    }

    public string DiscriminatorName { get { return _discriminatorName; } }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        try
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            _isWriting = true;
#pragma warning restore S2696 // Instance members should not write to "static" fields

            var jObject = JObject.FromObject(value, serializer);

            if (!jObject.TryGetValue(_discriminatorName, out JToken _))
            {
                jObject.AddFirst(new JProperty(_discriminatorName, GetSubtypeDiscriminator(value.GetType())));
            }

            writer.WriteToken(jObject.CreateReader());
        }
        finally
        {
            _isWriting = false;
        }
    }

    public override bool CanWrite
    {
        get
        {
            if (_isWriting)
            {
#pragma warning disable S2696 // Instance members should not write to "static" fields
                _isWriting = false;
#pragma warning restore S2696 // Instance members should not write to "static" fields
                return false;
            }
            return true;
        }
    }

    public override bool CanRead
    {
        get
        {
            if (_isReading)
            {
#pragma warning disable S2696 // Instance members should not write to "static" fields
                _isReading = false;
#pragma warning restore S2696 // Instance members should not write to "static" fields
                return false;
            }
            return true;
        }
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jObject = serializer.Deserialize<JObject>(reader);
        if (jObject == null)
        {
            return null;
        }

        var discriminatorValue = jObject.GetValue(_discriminatorName);
        var discriminator = discriminatorValue != null ? Extensions.Value<string>(discriminatorValue) : null;
        var subtype = GetObjectSubtype(objectType, discriminator);

        var objectContract = serializer.ContractResolver.ResolveContract(subtype) as JsonObjectContract;
        if (objectContract == null || objectContract.Properties.All(p => p.PropertyName != _discriminatorName))
        {
            jObject.Remove(_discriminatorName);
        }

        try
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            _isReading = true;
#pragma warning restore S2696 // Instance members should not write to "static" fields
            return serializer.Deserialize(jObject.CreateReader(), subtype);
        }
        finally
        {
            _isReading = false;
        }
    }

    private Type GetObjectSubtype(Type objectType, string discriminator)
    {
        foreach (var attribute in CustomAttributeExtensions.GetCustomAttributes<JsonInheritanceAttribute>(IntrospectionExtensions.GetTypeInfo(objectType), true))
        {
            if (attribute.Key == discriminator)
            {
                return attribute.Type;
            }
        }

        return objectType;
    }

    private string GetSubtypeDiscriminator(Type objectType)
    {
        foreach (var attribute in CustomAttributeExtensions.GetCustomAttributes<JsonInheritanceAttribute>(IntrospectionExtensions.GetTypeInfo(objectType), true))
        {
            if (attribute.Type == objectType)
            {
                return attribute.Key;
            }
        }

        return objectType.Name;
    }
}
