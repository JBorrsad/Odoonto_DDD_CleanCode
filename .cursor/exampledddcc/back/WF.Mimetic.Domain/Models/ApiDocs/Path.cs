namespace WF.Mimetic.Domain.Models.ApiDocs;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public class Path : IReadOnlyDictionary<string, Method>
{
    private readonly Dictionary<string, Method> _methods;

    private Path()
    {
        _methods = new Dictionary<string, Method>();
    }

    internal static Path Create()
    {
        return new Path();
    }

    public Method this[string key] => _methods[key];

    public IEnumerable<string> Keys => _methods.Keys;

    public IEnumerable<Method> Values => _methods.Values;

    public int Count => _methods.Count;

    public bool ContainsKey(string key)
    {
        return _methods.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, Method>> GetEnumerator()
    {
        return _methods.GetEnumerator();
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out Method value)
    {
        return _methods.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_methods).GetEnumerator();
    }

    public Path AddMethod(string name, Method method)
    {
        _methods.Add(name, method);
        return this;
    }
}