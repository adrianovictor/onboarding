namespace Core.Common.Lib.MessageBroker.SNS.Options;

public class SnsTopicConfig
{
    public string TopicName { get; set; } = string.Empty;
    public string Protocol { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
}
