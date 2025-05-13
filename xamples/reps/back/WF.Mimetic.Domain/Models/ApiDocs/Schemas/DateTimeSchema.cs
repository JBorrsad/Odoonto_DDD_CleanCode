namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;

using System;

public class DateTimeSchema : FormatSchema
{
    private DateTimeSchema() : base("string", "date-time")
    {
    }

    internal static DateTimeSchema GetSchema(Type type)
    {
        if (typeof(DateTime) != type)
        {
            return null;
        }

        return new DateTimeSchema();
    }
}
