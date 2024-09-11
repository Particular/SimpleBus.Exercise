namespace SimpleBus;

public sealed class Endpoint
{
    private readonly string name;
    private readonly ISerializeMessages serializer;
    private readonly ITransportMessages transport;

    public Endpoint(string name, ITransportMessages transport, ISerializeMessages serializer)
    {
        this.name = name;
        this.transport = transport;
        this.serializer = serializer;
    }

    public void Register<TMessage>(Func<TMessage, Task> receiver)
    {
        var type = typeof(TMessage).FullName ?? throw new InvalidOperationException("Message type full name is null.");
        this.transport.Register(this.name, type, message => receiver(this.serializer.Deserialize<TMessage>(message)));
    }

    public void Send<TMessage>(string destinationEndpointName, TMessage message)
    {
        var type = typeof(TMessage).FullName ?? throw new InvalidOperationException("Message type full name is null.");
        this.transport.Send(destinationEndpointName, type, this.serializer.Serialize(message));
    }

    public Task<IAsyncDisposable> StartReceiving() => this.transport.StartReceiving();
}
