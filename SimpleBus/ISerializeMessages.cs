namespace SimpleBus;

public interface ISerializeMessages
{
    string Serialize<TMessage>(TMessage message);

    TMessage Deserialize<TMessage>(string message);
}
