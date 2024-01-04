namespace SimpleBus;

public interface ITransportMessages
{
    void Register(string destinationEndpointName, string messageType, Func<string, Task> receiver);
    Task<IAsyncDisposable> StartReceiving();
    void Send(string destinationEndpointName, string messageType, string message);
}
