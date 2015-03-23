namespace Abp.Dependency
{
    using Castle.Core;
    using Castle.MicroKernel.Facilities;

    internal class AttributeFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.ComponentModelCreated += kernel_ComponentModelCreated;
        }

        private void kernel_ComponentModelCreated(ComponentModel model)
        {
            var attributes = model.Implementation.GetCustomAttributes(typeof(LifestyleAttribute), false);
            if (attributes.Length > 0)
            {
                var attr = attributes[0] as LifestyleAttribute;
                if (null != attr)
                {
                    switch (attr.Lifestyle)
                    {
                        case LifestyleType.PerWebRequest:
                            model.CustomLifestyle = typeof(PerWebRequestLifestyleManager);
                            model.LifestyleType = LifestyleType.Custom;
                            break;
                    }
                }
            }
        }
    }
}
