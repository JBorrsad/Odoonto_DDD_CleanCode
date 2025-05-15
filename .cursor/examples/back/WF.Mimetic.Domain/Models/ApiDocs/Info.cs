namespace WF.Mimetic.Domain.Models.ApiDocs;

using WF.Mimetic.Domain.Core.Models.Exceptions;

public class Info
{
    public string Title { get; private set; }
    public string Version { get; private set; }

    private Info(string title, string version)
    {
        Title = title;
        Version = version;
    }

    internal static Info Create(string title, string version)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new WrongOperationException("The title cant be null/empty.");
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new WrongOperationException("The version cant be null/empty.");
        }

        return new Info(title, version);
    }
}
