using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Lock
{
    /// <summary>
    /// For use in an Async lock
    /// http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266983.aspx
    /// </summary>
    public class AsyncSemaphore
    {
        private static readonly Task completed = Task.FromResult(true);
        private readonly Queue<TaskCompletionSource<bool>> _waiters = new Queue<TaskCompletionSource<bool>>();
        private int _currentCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSemaphore"/> class.
        /// </summary>
        /// <param name="initialCount">The initial allowed lock count.</param>
        public AsyncSemaphore(int initialCount)
        {
            if (initialCount < 0)
            {
                throw new ArgumentOutOfRangeException("initialCount");
            }

            this._currentCount = initialCount;
        }

        /// <summary>
        /// Returns a task that has been synchronized in this semaphor.
        /// </summary>
        /// <returns></returns>
        public Task WaitAsync()
        {
            lock (this._waiters)
            {
                if (this._currentCount > 0)
                {
                    this._currentCount--;
                    return completed;
                }
                else
                {
                    var waiter = new TaskCompletionSource<bool>();
                    this._waiters.Enqueue(waiter);
                    return waiter.Task;
                }
            }
        }

        /// <summary>
        /// Releases the next synchronized task lock.
        /// </summary>
        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;
            lock (this._waiters)
            {
                if (this._waiters.Count > 0)
                {
                    toRelease = this._waiters.Dequeue();
                }
                else
                {
                    this._currentCount++;
                }
            }

            if (toRelease != null)
            {
                toRelease.SetResult(true);
            }
        }
    }
}