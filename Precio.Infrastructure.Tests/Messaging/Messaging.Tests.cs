using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Precio.Messaging.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MessagingTests
    {
        [TestMethod]
        [TestCategory("Messaging")]
        public void can_register_multiple_handlers_for_message_type()
        {
            IMessageBus bus = new MessageBus();
            TestMessage receivedMessage1 = null;
            TestMessage receivedMessage2 = null;

            bus.Subscribe<TestMessage>(MessageHandlerType.Synchronous, message => receivedMessage1 = message,
                                             false);
            bus.Subscribe<TestMessage>(MessageHandlerType.Synchronous, message => receivedMessage2 = message,
                                             false);

            var sentMessage = new TestMessage
                                  {
                                      Text = "Hello bus"
                                  };

            bus.Send(sentMessage);

            Assert.AreSame(sentMessage, receivedMessage1);
            Assert.AreSame(sentMessage, receivedMessage2);
        }

        [TestMethod]
        [TestCategory("Messaging")]
        public void can_send_and_receive_async_message()
        {
            var trigger = new AutoResetEvent(false);
            IMessageBus bus = new MessageBus();
            TestMessage receivedMessage = null;

            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            int handlerThreadId = 0;
            bus.Subscribe<TestMessage>(MessageHandlerType.Asynchronous, message =>
                                                                                  {
                                                                                      handlerThreadId =
                                                                                          Thread.CurrentThread.
                                                                                              ManagedThreadId;
                                                                                      receivedMessage = message;
                                                                                      trigger.Set();
                                                                                  }, false);

            var sentMessage = new TestMessage
                                  {
                                      Text = "Hello bus"
                                  };

            bus.Send(sentMessage);
            trigger.WaitOne(1000);
            //ensure we got the message
            Assert.AreSame(sentMessage, receivedMessage);

            //ensure we didn't handle it in the main thread
            Assert.AreNotEqual(currentThreadId, handlerThreadId);
        }

        [TestMethod]
        [TestCategory("Messaging")]
        public void can_send_and_receive_sync_message()
        {
            IMessageBus bus = new MessageBus();
            TestMessage receivedMessage = null;

            bus.Subscribe<TestMessage>(MessageHandlerType.Synchronous, message => receivedMessage = message, false);

            var sentMessage = new TestMessage
                                  {
                                      Text = "Hello bus"
                                  };

            bus.Send(sentMessage);

            Assert.AreSame(sentMessage, receivedMessage);
        }

        [TestMethod]
        [TestCategory("Messaging")]
        public void can_send_FailedMessage_when_messagehandler_throws()
        {
            IMessageBus bus = new MessageBus();

            FailedMessage receivedFailedMessage = null;
            bus.Subscribe<TestMessage>(MessageHandlerType.Synchronous,
                                             message => { throw new Exception("This should throw"); }, true);
            bus.Subscribe<FailedMessage>(MessageHandlerType.Synchronous,
                                               message => receivedFailedMessage = message, false);

            var sentMessage = new TestMessage
                                  {
                                      Text = "Hello bus"
                                  };

            bus.Send(sentMessage);

            Assert.IsNotNull(receivedFailedMessage);
        }

        [TestMethod]
        [TestCategory("Messaging")]
        public void can_process_messages_via_RX()
        {
            var trigger = new AutoResetEvent(false);
            IMessageBus bus = new MessageBus();

            string receivedMessage = null;
            IObservable<string> query = bus.MessageSubject
                .Delay(new TimeSpan(0, 0, 1))
                .Select(m => m as TestMessage)
                .Where(m => m != null)
                .Select(m => m.Text)
                .Where(s => s == "Hello bus");

            query.Subscribe(m =>
                                {
                                    receivedMessage = m;
                                    trigger.Set();
                                });

            var sentMessage1 = new TestMessage
                                   {
                                       Text = "First message"
                                   };
            var sentMessage2 = new TestMessage
                                   {
                                       Text = "Hello bus"
                                   };

            bus.Send(sentMessage1);
            bus.Send(sentMessage2);

            trigger.WaitOne(3000);

            Assert.AreEqual(sentMessage2.Text, receivedMessage);
        }
    }
}