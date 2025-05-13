namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;
using System.Collections;

public class ArraySchema : TypeSchema
{
    public Schema Items { get; private set; }

    private ArraySchema(Schema items) : base("array")
    {
        Items = items;
    }

    internal static ArraySchema GetSchema(Type type)
    {
        if (!typeof(IEnumerable).IsAssignableFrom(type) || typeof(IDictionary).IsAssignableFrom(type))
        {
            return null;
        }

        if(type.IsArray)
        {
            Type subtype = type.GetElementType();
            Schema itemsSchema = Schema.Create(subtype);
            return new ArraySchema(itemsSchema);
        }

        if(type.IsGenericType)
        {
            Type subtype = type.GetGenericArguments()[0];
            Schema itemsSchema = Schema.Create(subtype);
            return new ArraySchema(itemsSchema);
        }

        return null;
    }
}
