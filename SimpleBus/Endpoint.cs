namespace SimpleBus;

public sealed class Endpoint(string name, ITransportMessages transport, ISerializeMessages serializer)
{
    public void Register<TMessage>(Func<TMessage, Task> receiver)
    {
        var type = typeof(TMessage).FullName ?? throw new InvalidOperationException("Message type full name is null.");
        transport.Register(name, type, message => receiver(serializer.Deserialize<TMessage>(message)));
    }

    public void Send<TMessage>(string destinationEndpointName, TMessage message)
    {
        var type = typeof(TMessage).FullName ?? throw new InvalidOperationException("Message type full name is null.");
        transport.Send(destinationEndpointName, type, serializer.Serialize(message));
    }

    public Task<IAsyncDisposable> StartReceiving() => transport.StartReceiving();
}
