namespace WF.Mimetic.Domain.Models.ApiDocs.Parameters;

using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.ApiDocs.Schemas;


public class PathParameter : Parameter
{
    private PathParameter(string name, Schema schema) : base(name, "path", schema)
    {
    }

    internal static PathParameter Create(string name, Type type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new WrongOperationException("The parameter name cant be null/empty.");
        }

        Schema schema = Schema.Create(type);

        PathParameter parameter = new PathParameter(name, schema);
        parameter.IsRequired();

        return parameter;
    }
}
