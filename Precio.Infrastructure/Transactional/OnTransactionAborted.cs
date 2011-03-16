using System;
using System.Transactions;

namespace Precio.Transactional
{
    public static partial class CurrentTransaction
    {
        public static void Aborted(Action handler)
        {
            Transaction transaction = Transaction.Current;
            if (transaction == null)
            {
                throw new InvalidOperationException("No active transaction in scope");
            }

            transaction.TransactionCompleted += (s, e) =>
                                                    {
                                                        if (e.Transaction.TransactionInformation.Status ==
                                                            TransactionStatus.Aborted)
                                                        {
                                                            handler();
                                                        }
                                                    };
        }
    }
}