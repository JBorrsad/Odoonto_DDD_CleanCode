namespace WF.Mimetic.Domain.Models.ApiDocs;

using System;
using System.Collections.Generic;
using WF.Mimetic.Domain.Models.ApiDocs.Schemas;

public class Response
{
    private readonly Dictionary<string, Schema> _content;

    public string Description { get; private set; }
    public IReadOnlyDictionary<string, Schema> Content => _content;

    private Response()
    {
        Description = string.Empty;
        _content = new Dictionary<string, Schema>();
    }

    internal static Response Create()
    {
        return new Response();
    }

    public Response SetDescription(string description)
    {
        Description = description ?? string.Empty;
        return this;
    }

    public Response AddContent(string contentType, Type type)
    {
        Schema schema = Schema.Create(type);

        _content.Add(contentType, schema);
        return this;
    }
}