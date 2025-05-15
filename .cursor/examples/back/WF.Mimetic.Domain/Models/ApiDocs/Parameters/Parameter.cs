namespace WF.Mimetic.Domain.Models.ApiDocs.Parameters;

using WF.Mimetic.Domain.Models.ApiDocs.Schemas;

public abstract class Parameter
{
    public string Name { get; private set; }
    public string In { get; private set; }
    public bool Required { get; private set; }
    public Schema Schema { get; private set; }

    protected Parameter(string name, string location, Schema schema)
    {
        Name = name;
        In = location;
        Schema = schema;
        Required = false;
    }

    public Parameter IsRequired()
    {
        Required = true;
        return this;
    }
}