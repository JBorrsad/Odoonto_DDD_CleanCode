namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;

public class ShortSchema : FormatSchema
{
    private ShortSchema() : base("integer", "int16")
    {
    }

    internal static ShortSchema GetSchema(Type type)
    {
        if (typeof(short) != type)
        {
            return null;
        }

        return new ShortSchema();
    }
}
