using System;
using System.Transactions;

namespace Precio.Transactional
{
    public static partial class CurrentTransaction
    {
        public static void Complete(Action handler)
        {
            Transaction transaction = Transaction.Current;
            if (transaction == null)
            {
                throw new InvalidOperationException("No active transaction in scope");
            }

            //ignore transaction status
            transaction.TransactionCompleted += (s, e) => handler();
        }
    }
}