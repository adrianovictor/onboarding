using System.Text.Json;
using Core.Common.Lib.MessageBroker.Messaging.Exceptions;
using Core.Common.Lib.MessageBroker.Messaging.Interfaces;

namespace Core.Common.Lib.MessageBroker.Messaging.Common;

public class MessageDeserializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static TMessage Deserialize<TMessage>(string message) where TMessage : IMessage
    {
        try
        {
            return JsonSerializer.Deserialize<TMessage>(message, Options)
                ?? throw new MessageSerializerException(message, null);
        }
        catch (MessageSerializerException) 
        { 
            throw; 
        }
        catch (Exception ex) 
        { 
            throw new MessageSerializerException(message, ex); 
        }
    }
}
