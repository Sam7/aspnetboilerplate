using System;

namespace Abp.EntityFramework.Uow
{
    using System.Data.Entity.Infrastructure;
    using System.Linq.Expressions;

    using Abp.Transactions;

    using Castle.Core.Logging;

    /// <summary>
    /// Since Azure only gurantees it's SLA if one uses the SqlAzureExecutionStrategy but is not compatible with transactions,
    /// this class will keep retrying to connect to the Sqlserver until either the query was successful or the maxRetryCount or the maxDelay are hit.
    /// </summary>
    public class SqlRetryScope : RetryScope
    {
        private readonly Func<Exception, bool> shouldRetryOn;

        public SqlRetryScope(int maxRetryCount = 5, TimeSpan? defaultCoefficient = null, TimeSpan? maxDelay = null, ILogger logger = null)
            : base(null, maxRetryCount, defaultCoefficient, maxDelay, logger)
        {
            // Since SqlAzureRetriableExceptionDetector is internal only, we have to compile it manually here
            var sqlAzureRetriableExceptionDetectorType = Type.GetType("System.Data.Entity.SqlServer.SqlAzureRetriableExceptionDetector, EntityFramework.SqlServer");
            var shouldRetryOnMethod = sqlAzureRetriableExceptionDetectorType.GetMethod("ShouldRetryOn", new[] { typeof(Exception) });

            var variable = Expression.Parameter(typeof(Exception), "x");
            Expression exprLeft = Expression.Call(shouldRetryOnMethod, variable);
            this.shouldRetryOn = Expression.Lambda<Func<Exception, bool>>(exprLeft, variable).Compile();
        }

        protected override bool ShouldRetry(Exception ex)
        {
            return DbExecutionStrategy.UnwrapAndHandleException(ex, this.shouldRetryOn);
        }
    }
}
