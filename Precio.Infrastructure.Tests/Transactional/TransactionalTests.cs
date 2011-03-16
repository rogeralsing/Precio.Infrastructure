using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Precio.Transactional.Tests
{
    [TestClass]
    public class TransactionalTests
    {
        [TestMethod]
        public void can_wait_for_transaction_committed()
        {
            bool eventFired = false;

            using (var scope = new TransactionScope())
            {
                CurrentTransaction.Committed(() => eventFired = true);                
                Assert.IsFalse(eventFired);
                scope.Complete();
            }
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void can_wait_for_transaction_complete()
        {
            bool eventFired = false;

            using (var scope = new TransactionScope())
            {
                CurrentTransaction.Complete(() => eventFired = true); 
                Assert.IsFalse(eventFired);
                scope.Complete();
            }
            //should fire on commit
            Assert.IsTrue(eventFired);

            eventFired = false;

            using (var scope = new TransactionScope())
            {
                CurrentTransaction.Complete(() => eventFired = true); 
                Assert.IsFalse(eventFired);
            }
            //and also on rollback
            Assert.IsTrue(eventFired);
        }

        //[TestMethod]
        //public void can_wait_for_transaction_committing()
        //{
        //    bool eventFired = false;

        //    using (var scope = new TransactionScope())
        //    {
        //        WaitFor.TransactionCommitting = () => eventFired = true;
        //        Assert.IsFalse(eventFired);
        //        scope.Complete();
        //    }
        //    Assert.IsTrue(eventFired);
        //}

        [TestMethod]
        public void can_wait_for_transaction_rollback()
        {
            bool eventFired = false;

            using (var scope = new TransactionScope())
            {
                CurrentTransaction.Aborted(() => eventFired = true); 
                Assert.IsFalse(eventFired);
            }
            Assert.IsTrue(eventFired);
        }
    }
}