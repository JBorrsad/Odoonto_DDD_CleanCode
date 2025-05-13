namespace WF.Mimetic.Domain.Models.ApiDocs;

using System;
using System.Collections.Generic;
using System.Linq;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.ApiDocs.Parameters;

public class Method
{
    private readonly IList<string> _tags;
    private readonly IList<Parameter> _parameters;
    private readonly Dictionary<string, Response> _responses;

    public IEnumerable<string> Tags => _tags.AsEnumerable();
    public IEnumerable<Parameter> Parameters => _parameters.AsEnumerable();
    public IReadOnlyDictionary<string, Response> Responses => _responses;

    private Method()
    {
        _tags = new List<string>();
        _parameters = new List<Parameter>();
        _responses = new Dictionary<string, Response>();
    }

    internal static Method Create()
    {
        return new Method();
    }

    public Method AddTag(string tag)
    {
        if (_tags.Contains(tag))
        {
            return this;
        }

        _tags.Add(tag);
        return this;
    }

    public Method AddParameter(Parameter parameter)
    {
        if(parameter is null)
        {
            throw new WrongOperationException("The parameter cant be null.");
        }

        _parameters.Add(parameter);
        return this;
    }

    public Method AddResponse(string code, Response response)
    {
        if(string.IsNullOrWhiteSpace(code))
        {
            throw new WrongOperationException("The resposne code cant be null/empty.");
        }

        if (response is null)
        {
            throw new WrongOperationException("The response cant be null.");
        }

        if (_responses.ContainsKey(code))
        {
            _responses.Remove(code);
        }

        _responses.Add(code, response);
        return this;
    }
}