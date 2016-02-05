using System;
using System.Collections.Generic;
using System.Linq;

namespace Abp.Transactions
{
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    public class RetryScope
    {
        protected readonly HashSet<Type> RetryExceptions;
        protected readonly TimeSpan DefaultCoefficient;
        protected readonly ConcurrentBag<Exception> ExceptionsEncountered = new ConcurrentBag<Exception>();
        protected readonly int MaxRetryCount;
        protected readonly TimeSpan MaxDelay;
        protected readonly Random Random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retryExceptions"></param>
        /// <param name="maxRetryCount"></param>
        /// <param name="defaultCoefficient"></param>
        /// <param name="maxDelay"></param>
        public RetryScope(HashSet<Type> retryExceptions, int maxRetryCount = 2, TimeSpan? defaultCoefficient = null, TimeSpan? maxDelay = null)
        {
            this.RetryExceptions = retryExceptions;
            this.MaxRetryCount = maxRetryCount;
            this.DefaultCoefficient = defaultCoefficient ?? TimeSpan.FromMilliseconds(500);
            this.MaxDelay = maxDelay ?? TimeSpan.FromSeconds(20.0);
        }

        public bool Execute(Action action)
        {
            TimeSpan? nextDelay;
            while (true)
            {
                try
                {
                    action();
                    return true;
                }
                catch (Exception ex)
                {
                    if (this.ShouldRetry(ex))
                    {
                        nextDelay = this.GetNextDelay(ex);
                        if (!nextDelay.HasValue)
                            return false;
                    }
                    else
                        throw;
                }
                if (nextDelay.GetValueOrDefault() > TimeSpan.Zero)
                    Thread.Sleep(nextDelay.Value);
                else
                    break;
            }
            throw new InvalidOperationException(string.Format("NegativeDelay {0}", nextDelay));
        }

        public async Task<bool> ExecuteAsync(Func<Task> action, CancellationToken cancellationToken)
        {
            TimeSpan? nextDelay;
            while (true)
            {
                try
                {
                    await action();
                    return true;
                }
                catch (Exception ex)
                {
                    if (this.ShouldRetry(ex))
                    {
                        nextDelay = this.GetNextDelay(ex);
                        if (!nextDelay.HasValue)
                            return false;
                    }
                    else
                        throw;
                }
                if (nextDelay.GetValueOrDefault() > TimeSpan.Zero)
                    await Task.Delay(nextDelay.Value, cancellationToken);
                else
                    break;
            }
            throw new InvalidOperationException(string.Format("NegativeDelay {0}", nextDelay));
        }

        protected virtual bool ShouldRetry(Exception ex)
        {
            return this.RetryExceptions.Any(x => x.IsInstanceOfType(ex));
        }

        protected internal virtual TimeSpan? GetNextDelay(Exception lastException)
        {
            this.ExceptionsEncountered.Add(lastException);
            var num1 = this.ExceptionsEncountered.Count - 1;
            if (num1 >= this.MaxRetryCount)
                return new TimeSpan?();
            var num2 = (Math.Pow(2.0, num1 + 1) - 1.0) * (1.0 + this.Random.NextDouble() * 0.1);
            return TimeSpan.FromMilliseconds(Math.Min(this.DefaultCoefficient.TotalMilliseconds * num2, this.MaxDelay.TotalMilliseconds));
        }
    }
}
