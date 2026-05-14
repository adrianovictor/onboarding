namespace Core.Common.Lib.MessageBroker.SNS.Interfaces;

public interface ISnsMessageBroker
{
    /// <summary>
    /// Cri
    /// </summary>
    /// <param name="topicName"></param>
    /// <returns></returns>
    Task<string> CreateTopicAsync(string topicName);
    Task PublishMessageAsync(string topicArn, string message, string? subject = null);
    Task SubscribeAsync(string topicArn, string protocol, string endpoint);
}
