using System;

namespace Core.Common.Lib.MessageBroker.Messaging.Exceptions;

public class MessageSerializerException(string rawMessage, Exception? innerException)
    : Exception($"Não foi possível deserializer a mensagem:  \"{rawMessage}\".", innerException)
{
    public string RawMessage { get; } = rawMessage;
}
