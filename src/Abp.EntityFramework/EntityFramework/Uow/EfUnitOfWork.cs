using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Castle.Core.Internal;

namespace Abp.EntityFramework.Uow
{
    using System.Data.Entity.SqlServer;
    using System.Runtime.Remoting.Messaging;
    using System.Threading;

    /// <summary>
    /// Implements Unit of work for Entity Framework.
    /// </summary>
    public class EfUnitOfWork : UnitOfWorkBase
    {
        private readonly IDictionary<Type, DbContext> _activeDbContexts;
        private readonly IIocResolver _iocResolver;
        private TransactionScope _transaction;

        private static readonly SqlRetryScope SqlRetryScope = new SqlRetryScope();

        /// <summary>
        /// Creates a new <see cref="EfUnitOfWork"/>.
        /// </summary>
        public EfUnitOfWork(IIocResolver iocResolver)
        {
            this._iocResolver = iocResolver;
            this._activeDbContexts = new Dictionary<Type, DbContext>();
        }

        protected override void BeginUow()
        {
            if (this.Options.IsTransactional == true)
            {
                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = this.Options.IsolationLevel.GetValueOrDefault(IsolationLevel.ReadUncommitted),
                };

                if (this.Options.Timeout.HasValue)
                {
                    transactionOptions.Timeout = this.Options.Timeout.Value;
                }

                SqlRetryScope.Execute(() =>
                {
                    this._transaction = new TransactionScope(
                        TransactionScopeOption.Required,
                        transactionOptions,
                        this.Options.AsyncFlowOption.GetValueOrDefault(TransactionScopeAsyncFlowOption.Enabled));
                });
            }
        }

        public override void SaveChanges()
        {
            this._activeDbContexts.Values.ForEach(dbContext => SqlRetryScope.Execute(() => dbContext.SaveChanges()));
        }

        public override async Task SaveChangesAsync()
        {
            foreach (var dbContext in this._activeDbContexts.Values)
            {
                await SqlRetryScope.ExecuteAsync(() => dbContext.SaveChangesAsync(), CancellationToken.None);
            }
        }

        protected override void CompleteUow()
        {
            this.SaveChanges();
            if (this._transaction != null)
            {
                SqlRetryScope.Execute(() => this._transaction.Complete());
            }
        }

        protected override async Task CompleteUowAsync()
        {
            await this.SaveChangesAsync();
            if (this._transaction != null)
            {
                SqlRetryScope.Execute(() => this._transaction.Complete());
            }
        }

        internal TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            DbContext dbContext;
            if (!this._activeDbContexts.TryGetValue(typeof(TDbContext), out dbContext))
            {
                this._activeDbContexts[typeof(TDbContext)] = dbContext = this._iocResolver.Resolve<TDbContext>();
            }

            return (TDbContext)dbContext;
        }

        protected override void DisposeUow()
        {
            SqlRetryScope.Execute(() => { 
                this._activeDbContexts.Values.ForEach(dbContext =>
                {
                    dbContext.Dispose();
                    this._iocResolver.Release(dbContext);
                });

                if (this._transaction != null)
                {
                    this._transaction.Dispose();
                }
            });
        }
    }
}