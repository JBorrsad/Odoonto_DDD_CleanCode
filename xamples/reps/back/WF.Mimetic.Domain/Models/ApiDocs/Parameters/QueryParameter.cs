namespace WF.Mimetic.Domain.Models.ApiDocs.Parameters;

using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.ApiDocs.Schemas;

public class QueryParameter : Parameter
{
    private QueryParameter(string name, Schema schema) : base(name, "query", schema)
    {
    }

    internal static QueryParameter Create(string name, Type type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new WrongOperationException("The parameter name cant be null/empty.");
        }

        Schema schema = Schema.Create(type);

        return new QueryParameter(name, schema);
    }
}
