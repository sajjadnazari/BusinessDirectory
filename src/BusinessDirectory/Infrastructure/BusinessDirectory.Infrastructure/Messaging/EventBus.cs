using BusinessDirectory.Application.Interfaces;
using MassTransit;

namespace BusinessDirectory.Infrastructure.Messaging
{
    internal sealed class EventBus(IPublishEndpoint publishEndpoint) : IMessageBus
    {
        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            await publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
