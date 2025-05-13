namespace WF.Mimetic.Domain.Models.Routes;

using Microsoft.AspNetCore.Routing.Template;
using System;
using WF.Mimetic.Domain.Core.Models;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.Nodes;

public class Route : Cachelog
{
    public Method Method { get; private set; }
    public string Uri { get; private set; }
    public string TemplateText { get; private set; }

    public Route(Guid id, Method method, string uri) : base(id)
    {
        Method = method;
        Uri = uri;
        TemplateText = TemplateParser.Parse(Uri).TemplateText;
    }

    public void SetUri(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            throw new WrongOperationException("The route uri cant be empty/null.");
        }

        Uri = uri;
        TemplateText = TemplateParser.Parse(Uri).TemplateText;
    }

    public void SetMethod(Method method)
    {
        if (Method.None.Equals(method))
        {
            throw new WrongOperationException("The route method cant be none.");
        }

        Method = method;
    }
}
