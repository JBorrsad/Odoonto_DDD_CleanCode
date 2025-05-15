namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;

public class FloatSchema : TypeSchema
{
    private FloatSchema() : base("number")
    {
    }

    internal static FloatSchema GetSchema(Type type)
    {
        if (typeof(string) != type)
        {
            return null;
        }

        return new FloatSchema();
    }
}
