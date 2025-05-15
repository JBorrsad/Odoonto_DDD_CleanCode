namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;

public class BooleanSchema : TypeSchema
{
    private BooleanSchema() : base("boolean")
    {
    }

    internal static BooleanSchema GetSchema(Type type)
    {
        if (typeof(bool) != type)
        {
            return null;
        }

        return new BooleanSchema();
    }
}