using Core.Common.Lib.MessageBroker.Messaging.Common;

namespace Core.Common.Lib.MessageBroker.Messaging.Interfaces;

public interface IMessageBroker
{
    Task SendMessageAsync(string detination, string body);
    Task<MessageWrapper?> ReceiveMessageAsync(string source);
    Task AckMessageAsync(MessageWrapper message);
    Task NackMessageAsync(MessageWrapper message);
}
