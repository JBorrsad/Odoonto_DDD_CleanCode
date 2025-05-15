namespace WF.Mimetic.Domain.Models.Serializers;

internal interface ISerializer
{
    SerializationType Type { get; }
    bool TrySerialize(string msg, out string result);
}
