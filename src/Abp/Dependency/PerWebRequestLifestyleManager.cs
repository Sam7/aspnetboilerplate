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
        private readonly TransientLifestyleManager fallback;

        public PerWebRequestLifestyleManager()
            : base(new WebRequestScopeAccessor())
        {
            fallback = new TransientLifestyleManager();
        }

        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            HttpContext current = HttpContext.Current;

            if (null == current || current.ApplicationInstance == null)
            {
                // Fall back to transient behavior if not in web context.
                return fallback.Resolve(context, releasePolicy);
            }
            else
            {
                return base.Resolve(context, releasePolicy);
            }
        }

        public override void Dispose()
        {
            fallback.Dispose();

            base.Dispose();
        }

        public override void Init(IComponentActivator componentActivator, IKernel kernel, ComponentModel model)
        {
            base.Init(componentActivator, kernel, model);

            fallback.Init(componentActivator, kernel, model);
        }
    }
}
