using System;

namespace Abp.Dependency
{
    using System.Web;

    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Context;
    using Castle.MicroKernel.Lifestyle;

    [Serializable]
    public class PerWebRequestLifestyleManager : ScopedLifestyleManager
    {
        private readonly ILifestyleManager fallback1;
        private readonly ILifestyleManager fallback2;

        public PerWebRequestLifestyleManager()
            : base(new WebRequestScopeAccessor())
        {
            this.fallback1 = new ScopedLifestyleManager();
            this.fallback2 = new TransientLifestyleManager();
        }

        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            var current = HttpContext.Current;
            HttpRequest request;
            try
            {
                request = current.Request;
            }
            catch (HttpException ex)
            {
                request = null;
            }

            if (null == current || current.ApplicationInstance == null || request == null)
            {
                // if a scope is defined we'll use that scope
                var currentScope = Castle.MicroKernel.Lifestyle.Scoped.CallContextLifetimeScope.ObtainCurrentScope();
                if (currentScope != null)
                    return this.fallback1.Resolve(context, releasePolicy);
                else
                    // Fall back to transient behavior if not in web context or scope context.
                    return this.fallback2.Resolve(context, releasePolicy);
            }
            else
            {
                return base.Resolve(context, releasePolicy);
            }
        }

        public override void Dispose()
        {
            this.fallback1.Dispose();
            this.fallback2.Dispose();

            base.Dispose();
        }

        public override void Init(IComponentActivator componentActivator, IKernel kernel, ComponentModel model)
        {
            base.Init(componentActivator, kernel, model);

            this.fallback1.Init(componentActivator, kernel, model);
            this.fallback2.Init(componentActivator, kernel, model);
        }
    }
}
