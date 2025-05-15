namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;

public class IntSchema : FormatSchema
{
    private IntSchema() : base("integer", "int32")
    {
    }

    internal static IntSchema GetSchema(Type type)
    {
        if (typeof(int) != type)
        {
            return null;
        }

        return new IntSchema();
    }
}
