using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SACS.Common.Lock
{
    /// <summary>
    /// The AsyncLock class for adding synchronization to Task (which normal
    /// locks are incapable of doing)
    /// http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266988.aspx
    /// </summary>
    public class AsyncLock
    {
        private readonly AsyncSemaphore _semaphore;
        private readonly Task<Releaser> _releaser;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLock"/> class.
        /// </summary>
        public AsyncLock()
        {
            this._semaphore = new AsyncSemaphore(1);
            this._releaser = Task.FromResult(new Releaser(this));
        }

        /// <summary>
        /// Syncronizes a task.
        /// </summary>
        /// <returns></returns>
        public Task<Releaser> LockAsync()
        {
            var wait = this._semaphore.WaitAsync();
            return wait.IsCompleted ?
                this._releaser :
                wait.ContinueWith(
                    (_, state) => new Releaser((AsyncLock)state),
                    this,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
        }

        /// <summary>
        /// The struct which represents a lock to release.
        /// </summary>
        public struct Releaser : IDisposable
        {
            private readonly AsyncLock _toRelease;

            /// <summary>
            /// Initializes a new instance of the <see cref="Releaser"/> struct.
            /// </summary>
            /// <param name="toRelease">The async lock to release.</param>
            internal Releaser(AsyncLock toRelease)
            {
                this._toRelease = toRelease;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (this._toRelease != null)
                {
                    this._toRelease._semaphore.Release();
                }
            }
        }
    }
}