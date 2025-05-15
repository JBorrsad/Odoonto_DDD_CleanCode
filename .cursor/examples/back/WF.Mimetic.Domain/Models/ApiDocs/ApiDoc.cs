namespace WF.Mimetic.Domain.Models.ApiDocs;

using System.Collections.Generic;
using WF.Mimetic.Domain.Core.Models.Exceptions;

public class ApiDoc
{
    private readonly Dictionary<string, Path> _paths;

    public string Openapi { get; private set; }
    public Info Info { get; private set; }
    public IReadOnlyDictionary<string, Path> Paths => _paths;

    private ApiDoc(string openapi, Info info)
    {
        Openapi = openapi;
        Info = info;
        _paths = new Dictionary<string, Path>();
    }

    internal static ApiDoc Create(string openapi, string title, string version)
    {
        if (string.IsNullOrWhiteSpace(openapi))
        {
            throw new WrongOperationException("The openapi cant be null/empty.");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new WrongOperationException("The title cant be null/empty.");
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new WrongOperationException("The version cant be null/empty.");
        }

        Info info = Info.Create(title, version);

        return new ApiDoc(openapi, info);
    }

    public ApiDoc AddPath(string route, Path path)
    {
        if (string.IsNullOrWhiteSpace(route))
        {
            throw new WrongOperationException("The route cant be null/empty.");
        }

        _paths.Add(route, path);
        return this;
    }
}
