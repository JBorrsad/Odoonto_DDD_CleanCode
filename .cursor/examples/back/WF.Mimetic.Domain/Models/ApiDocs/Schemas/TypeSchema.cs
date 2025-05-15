namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;
public class TypeSchema : Schema
{
    public string Type { get; private set; }

    protected TypeSchema(string type)
    {
        Type = type;
    }
}