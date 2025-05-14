namespace WF.Mimetic.Domain.Models.Nodes;

using System;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Serializers;

public class Serializer : Node
{
    public static Serializer Default => new Serializer();

    public SerializationType SerializationType { get; private set; }

    public override NodeType Type => NodeType.Serializer;

    private Serializer() : base(Guid.Empty, Guid.Empty)
    {
        SerializationType = SerializationType.None;
    }

    private Serializer(Guid id, Guid flowId) : base(id, flowId)
    {
        SerializationType = SerializationType.None;
    }

    internal static Serializer Create(Guid id, Flow flow)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The serializer id cant be empty.");
        }

        if (flow is null)
        {
            throw new WrongOperationException("The flow cant be null.");
        }

        return new Serializer(id, flow.Id);
    }

    public void SetSerializationType(SerializationType serializationType)
    {
        SerializationType = serializationType;
    }

    public override Task<string> Run(IRulesEngine rulesEngine, string msg)
    {
        if (msg is null)
        {
            throw new WrongOperationException("The msg cant be null.");
        }

        ISerializer serializer = CreateSerializer();

        string msgValue = Moustacher.GetValueFromJson(msg, "msg");

        if (!serializer.TrySerialize(msgValue, out string result))
        {
            throw new InvalidValueException("It is not possible to serilize the message.");
        }

        if(SerializationType == SerializationType.Json)
        {
            return Task.FromResult(Moustacher.SetJsonToJson(result, "msg", msg));
        }

        return Task.FromResult(Moustacher.SetStringToJson(result, "msg", msg));
    }

    private ISerializer CreateSerializer()
    {
        return SerializationType switch
        {
            SerializationType.Json => new JsonSerializer(),
            SerializationType.Xml => new XmlSerializer(),
            _ => throw new InvalidValueException("The serialization type is not valid.")
        };
    }
}
