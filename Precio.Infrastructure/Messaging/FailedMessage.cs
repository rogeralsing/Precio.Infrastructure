using System;

namespace Precio.Messaging
{
    //Stolen from Patrik Löwendahl's event aggregator
    public class FailedMessage : IMessage
    {
        public Exception MessageFailureException { get; set; }
    }

    public class FailedMessage<TFailedMessage> : FailedMessage
    {
        public TFailedMessage Message { get; set; }
    }
}