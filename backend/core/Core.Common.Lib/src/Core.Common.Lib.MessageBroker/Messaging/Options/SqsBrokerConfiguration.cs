namespace Core.Common.Lib.MessageBroker.Messaging.Options;

public class SqsBrokerConfiguration
{
    public string Region { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public AwsCredentialsConfiguration Credentials { get; set; } = new();
}
