using System.Text.Json;
using Core.Common.Lib.MessageBroker.Messaging.Exceptions;
using Core.Common.Lib.MessageBroker.Messaging.Interfaces;

namespace Core.Common.Lib.MessageBroker.Messaging.Common;

public static class MessageSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true  // mesmas options do Deserializer
    };

    public static string Serialize<T>(T message) where T : IMessage
    {
        try
        {
            return JsonSerializer.Serialize(message, Options)
                ?? throw new MessageSerializerException(nameof(message), null);
        }
        catch (MessageSerializerException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new MessageSerializerException(nameof(message), ex);
        }
    }
}