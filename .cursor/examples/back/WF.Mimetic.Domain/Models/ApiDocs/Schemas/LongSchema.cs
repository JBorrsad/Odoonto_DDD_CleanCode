namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;

public class LongSchema : FormatSchema
{
    private LongSchema() : base("integer", "int64")
    {
    }

    internal static LongSchema GetSchema(Type type)
    {
        if (typeof(long) != type)
        {
            return null;
        }

        return new LongSchema();
    }
}
