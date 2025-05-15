namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;

public class StringSchema : TypeSchema
{
    private StringSchema() : base("string")
    {
    }

    internal static StringSchema GetSchema(Type type)
    {
        if (typeof(string) != type)
        {
            return null;
        }

        return new StringSchema();
    }
}
