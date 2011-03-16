using System;
using System.Transactions;

namespace Precio.Transactional
{
    public static partial class CurrentTransaction
    {
        public static void Committing(Action handler)
        {
            Transaction transaction = Transaction.Current;
            if (transaction == null)
            {
                throw new InvalidOperationException("No active transaction in scope");
            }

            var x = new OnTransactionCommittingHandler(handler);
            transaction.EnlistVolatile(x, EnlistmentOptions.None);
        }
    }

    public class OnTransactionCommittingHandler : IEnlistmentNotification
    {
        private readonly Action action;

        public OnTransactionCommittingHandler(Action action)
        {
            this.action = action;
        }

        #region IEnlistmentNotification Members

        public void Commit(Enlistment enlistment)
        {
            action();
        }

        public void InDoubt(Enlistment enlistment)
        {
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
        }

        public void Rollback(Enlistment enlistment)
        {
        }

        #endregion
    }
}