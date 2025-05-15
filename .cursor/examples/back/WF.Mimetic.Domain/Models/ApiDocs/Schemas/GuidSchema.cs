namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;

public class GuidSchema : FormatSchema
{
    private GuidSchema() : base("string", "uuid")
    {
    }

    internal static GuidSchema GetSchema(Type type)
    {
        if (typeof(Guid) != type)
        {
            return null;
        }

        return new GuidSchema();
    }
}
