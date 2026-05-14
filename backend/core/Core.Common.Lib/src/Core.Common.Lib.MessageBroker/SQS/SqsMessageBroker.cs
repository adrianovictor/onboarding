using Core.Common.Lib.MessageBroker.SQS.Common;
using Core.Common.Lib.MessageBroker.SQS.Interfaces;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Core.Common.Lib.MessageBroker.SQS;

public class SqsMessageBroker(IAmazonSQS sqs) : ISqsMessageBroker
{
    public async Task DeleteMessageAsync(string source, string receiptHandle)
    {
        await sqs.DeleteMessageAsync(source, receiptHandle);
    }

    public async Task<SqsMessageWrapper?> ReceiveMessageAsync(string source)
    {
        var response = await sqs.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = source,
            MaxNumberOfMessages = 1,
            WaitTimeSeconds = 10
        });

        var message = response.Messages?.FirstOrDefault();

        if (message is null)
            return null;

        return new SqsMessageWrapper
        {
            Body = message.Body,
            ReceiptHandle = message.ReceiptHandle
        };
    }

    public async Task SendMessageAsync(string detination, string body)
    {
        await sqs.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = detination,
            MessageBody = body
        });
    }
}
