using System;
using System.Collections.Generic;

namespace Precio.Messaging
{
    public interface IMessageBus : IMessageSink
    {
        Subject<IMessage> MessageSubject { get; }
        void Subscribe<T>(MessageHandlerType handlerType, Action<T> messageHandler, bool catchAndSendExceptions);
        IObservable<T> AsObservable<T>() where T : IMessage;
    }
}