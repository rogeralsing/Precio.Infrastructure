using System;
using System.Collections.Generic;
using System.Linq;

namespace Precio.Messaging
{
    public class MessageBus : IMessageBus
    {
        private readonly Dictionary<Type, IList<object>> handlers = new Dictionary<Type, IList<object>>();
        private readonly Subject<IMessage> messageSubject = new Subject<IMessage>();

        #region IMessageBus Members

        public Subject<IMessage> MessageSubject
        {
            get { return messageSubject; }
        }

        public void Subscribe<T>(MessageHandlerType handlerType, Action<T> messageHandler,
                                       bool catchAndSendExceptions)
        {
            if (catchAndSendExceptions)
            {
                messageHandler = WrapActionWithErrorHandling(messageHandler);
            }

            if (!handlers.ContainsKey(typeof (T)))
            {
                handlers.Add(typeof (T), new List<object>());
            }

            IList<object> handlersForType = handlers[typeof (T)];

            var handler = new MessageHandler<T>
                              {
                                  Type = handlerType,
                                  Handler = messageHandler,
                              };
            handlersForType.Add(handler);
        }

        public void Send<T>(T message) where T : class, IMessage
        {
            messageSubject.OnNext(message);

            IEnumerable<MessageHandler<T>> handldersForType = GetHandlers<T>();
            foreach (var handlerForType in handldersForType)
            {
                switch (handlerForType.Type)
                {
                    case MessageHandlerType.Synchronous:
                        handlerForType.Handler(message);
                        break;
                    case MessageHandlerType.Asynchronous:
                        handlerForType.Handler.BeginInvoke(message, null, null);
                        break;
                }
            }
        }

        public IObservable<T> AsObservable<T>() where T : IMessage
        {
            return MessageSubject
                .Where(m => m is T)
                .Select(m => (T) m);
        }

        #endregion

        private IEnumerable<MessageHandler<T>> GetHandlers<T>()
        {
            if (handlers.ContainsKey(typeof (T)))
            {
                IList<object> handlersForType = handlers[typeof (T)];
                return handlersForType.Cast<MessageHandler<T>>();
            }

            return new List<MessageHandler<T>>();
        }

        private Action<T> WrapActionWithErrorHandling<T>(Action<T> action)
        {
            Action<T> wrappedAction = message =>
                                          {
                                              try
                                              {
                                                  action(message);
                                              }
                                              catch (Exception x)
                                              {
                                                  var failedMessage = new FailedMessage
                                                                          {
                                                                              MessageFailureException = x
                                                                          };
                                                  Send(failedMessage);
                                              }
                                          };
            return wrappedAction;
        }
    }
}