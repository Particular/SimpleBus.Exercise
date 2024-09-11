namespace SimpleBusTests;

[TestFixture]
public class Tests
{
    [Test]
    public static async Task ReceiveMessage()
    {
        // arrange
        var transport = new SynchronousInMemoryTransport();
        var serializer = new XmlMessageSerializer();

        var sender = new Endpoint("Sender", transport, serializer);
        var receiver = new Endpoint("Receiver", transport, serializer);

        var sent = new Message { Int32Field = 123 };
        var received = default(Message);

        receiver.Register<Message>(message =>
        {
            received = message;
            return Task.CompletedTask;
        });

        // act
        await using (await receiver.StartReceiving())
        {
            sender.Send("Receiver", sent);
        }

        // assert
        Assert.That(received?.Int32Field, Is.EqualTo(sent.Int32Field));
    }
}
