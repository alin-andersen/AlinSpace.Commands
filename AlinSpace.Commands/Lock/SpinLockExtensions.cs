using System;
using System.Threading;

namespace AlinSpace.Commands
{
    public static class SpinLockExtensions
    {
        public static void Execute(this SpinLock spinlock, Action execute)
        {
            bool lockTaken = false;

            try
            {
                // Lock spinlock.
                spinlock.Enter(ref lockTaken);

                execute();
            }
            finally
            {
                // Unlock spinlock.
                if (lockTaken)
                {
                    spinlock.Exit();
                }
            }
        }

        public static T Execute<T>(this SpinLock spinlock, Func<T> execute)
        {
            bool lockTaken = false;

            try
            {
                // Lock spinlock.
                spinlock.Enter(ref lockTaken);

                return execute();
            }
            finally
            {
                // Unlock spinlock.
                if (lockTaken)
                {
                    spinlock.Exit();
                }
            }
        }
    }
}
