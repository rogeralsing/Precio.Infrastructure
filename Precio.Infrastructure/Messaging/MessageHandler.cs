using System;

namespace Precio.Messaging
{
    public class MessageHandler<T>
    {
        public Action<T> Handler { get; set; }

        public MessageHandlerType Type { get; set; }
    }
}