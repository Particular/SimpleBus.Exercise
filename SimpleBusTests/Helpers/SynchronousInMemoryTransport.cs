namespace SimpleBusTests.Helpers;

public sealed class SynchronousInMemoryTransport : ITransportMessages
{
    private readonly List<Registration> registrations = [];

    public void Register(string destinationEndpointName, string messageType, Func<string, Task> receiver) =>
        registrations.Add(new Registration(destinationEndpointName, messageType, receiver));

    public Task<IAsyncDisposable> StartReceiving() => Task.FromResult<IAsyncDisposable>(new AsyncDisposable());

    public void Send(string destinationEndpointName, string messageType, string message)
    {
        var matchingRegistrations = registrations
            .Where(registration => registration.DestinationEndpointName == destinationEndpointName && registration.MessageType == messageType)
            .ToList();

        if (matchingRegistrations.Count == 0)
        {
            throw new InvalidOperationException($"The '{destinationEndpointName}' endpoint does not receive messages of type '{messageType}'.");
        }

        foreach (var registration in registrations)
        {
            registration.Receiver(message);
        }
    }

    private record Registration(string DestinationEndpointName, string MessageType, Func<string, Task> Receiver);

    private class AsyncDisposable : IAsyncDisposable
    {
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
