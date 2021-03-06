﻿using System;
using Abp.Dependency;
using Abp.Domain.Entities.Auditing;
using Abp.Runtime.Session;
using NHibernate;
using NHibernate.Type;

namespace Abp.NHibernate.Interceptors
{
    internal class AbpNHibernateInterceptor : EmptyInterceptor
    {
        private readonly IIocManager _iocManager;
        private readonly Lazy<IAbpSession> _abpSession;

        public AbpNHibernateInterceptor(IIocManager iocManager)
        {
            _iocManager = iocManager;
            _abpSession =
                new Lazy<IAbpSession>(
                    () => _iocManager.IsRegistered(typeof(IAbpSession))
                        ? _iocManager.Resolve<IAbpSession>()
                        : NullAbpSession.Instance
                    );
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            //Set CreationTime for new entity
            if (entity is IHasCreationTime)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "CreationTime")
                    {
                        state[i] = (entity as IHasCreationTime).CreationTimeUtc = DateTime.UtcNow;
                    }
                }
            }

            //Set CreatorUserId for new entity
            if (entity is ICreationAudited)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "CreatorUserId")
                    {
                        state[i] = (entity as ICreationAudited).CreatorUserId = _abpSession.Value.UserId;
                    }
                }
            }

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            //TODO@Halil: Implement this when tested well (Issue #49)
            ////Prevent changing CreationTime on update 
            //if (entity is IHasCreationTime)
            //{
            //    for (var i = 0; i < propertyNames.Length; i++)
            //    {
            //        if (propertyNames[i] == "CreationTime" && previousState[i] != currentState[i])
            //        {
            //            throw new AbpException(string.Format("Can not change CreationTime on a modified entity {0}", entity.GetType().FullName));
            //        }
            //    }
            //}

            //Prevent changing CreatorUserId on update
            //if (entity is ICreationAudited)
            //{
            //    for (var i = 0; i < propertyNames.Length; i++)
            //    {
            //        if (propertyNames[i] == "CreatorUserId" && previousState[i] != currentState[i])
            //        {
            //            throw new AbpException(string.Format("Can not change CreatorUserId on a modified entity {0}", entity.GetType().FullName));
            //        }
            //    }
            //}

            //Set modification audits
            if (entity is IModificationAudited)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "LastModificationTime")
                    {
                        currentState[i] = (entity as IModificationAudited).LastModificationTimeUtc = DateTime.UtcNow;
                    }
                    else if (propertyNames[i] == "LastModifierUserId")
                    {
                        currentState[i] = (entity as IModificationAudited).LastModifierUserId = _abpSession.Value.UserId;
                    }
                }
            }

            //Set deletion audits
            if (entity is IDeletionAudited && (entity as IDeletionAudited).IsDeleted)
            {
                //@hikalkan: Is deleted bofore? Normally, a deleted entity should not e updated later but I preferred to check it.
                var previousIsDeleted = false;
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "IsDeleted")
                    {
                        previousIsDeleted = (bool)previousState[i];
                        break;
                    }
                }

                if (!previousIsDeleted)
                {
                    for (var i = 0; i < propertyNames.Length; i++)
                    {
                        if (propertyNames[i] == "DeletionTime")
                        {
                            currentState[i] = (entity as IDeletionAudited).DeletionTimeUtc = DateTime.UtcNow;
                        }
                        else if (propertyNames[i] == "DeleterUserId")
                        {
                            currentState[i] = (entity as IDeletionAudited).DeleterUserId = _abpSession.Value.UserId;
                        }
                    }
                }
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }
    }
}
