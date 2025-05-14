namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;
public class FormatSchema : TypeSchema
{
    public string Format { get; private set; }

    protected FormatSchema(string format, string type) : base(type)
    {
        Format = format;
    }
}
