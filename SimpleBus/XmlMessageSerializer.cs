using System.Xml.Serialization;

namespace SimpleBus;

public sealed class XmlMessageSerializer : ISerializeMessages
{
    public string Serialize<TMessage>(TMessage message)
    {
        var serializer = new XmlSerializer(typeof(TMessage));
        using var writer = new StringWriter();
        serializer.Serialize(writer, message);

        return writer.ToString();
    }

    public TMessage Deserialize<TMessage>(string message)
    {
        var serializer = new XmlSerializer(typeof(TMessage));
        using var reader = new StringReader(message);

        var deserialized =
            serializer.Deserialize(reader)
            ?? throw new InvalidOperationException("The message deserialized as a null value.");

        return (TMessage)deserialized;
    }
}
