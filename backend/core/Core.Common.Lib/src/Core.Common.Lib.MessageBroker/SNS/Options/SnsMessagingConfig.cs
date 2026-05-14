namespace Core.Common.Lib.MessageBroker.SNS.Options;

public class SnsMessagingConfig
{
    public Dictionary<string, SnsTopicConfig> Topics { get; set; } = [];
}
