namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ObjectSchema : TypeSchema
{
    private readonly Dictionary<string, Schema> _properties;

    public IReadOnlyDictionary<string, Schema> Properties => _properties;
    public bool AdditionalProperties { get; private set; }

    private ObjectSchema() : base("object")
    {
        AdditionalProperties = false;
        _properties = new Dictionary<string, Schema>();
    }

    internal static ObjectSchema GetSchema(Type type)
    {
        if (!typeof(object).IsAssignableFrom(type) || typeof(IEnumerable).IsAssignableFrom(type))
        {
            return null;
        }

        ObjectSchema objectSchema = new ObjectSchema();

        PropertyInfo[] propertyInfos = type.GetProperties();

        foreach(PropertyInfo propertyInfo in propertyInfos)
        {
            Schema propertySchema = Schema.Create(propertyInfo.PropertyType);
            objectSchema._properties.Add(propertyInfo.Name, propertySchema);
        }

        return objectSchema;
    }

    public ObjectSchema HasAdditionalProperties()
    {
        AdditionalProperties = true;
        return this;
    }
}
