using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using Abp.Reflection;

namespace Abp.Web
{
    /// <summary>
    /// This class is used to get all assemblies in bin folder of a web application.
    /// </summary>
    public class WebAssemblyFinder : IAssemblyFinder
    {
        /// <summary>
        /// This return all assemblies in bin folder of the web application.
        /// </summary>
        /// <returns>List of assemblies</returns>
        public List<Assembly> GetAllAssemblies()
        {
            var assembliesInBinFolder = new List<Assembly>();

            var allReferencedAssemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            var dllFiles = Directory.GetFiles(HttpRuntime.AppDomainAppPath + "bin\\", "*.dll", SearchOption.TopDirectoryOnly).ToList();

            foreach (var dllFile in dllFiles)
            {
                // try loading the assembly name.
                AssemblyName dllAssemblyName;
                try
                {
                    dllAssemblyName = AssemblyName.GetAssemblyName(dllFile);
                }
                catch (System.BadImageFormatException)
                {
                    // The exception that is thrown when the file image of the DLL is invalid.
                    // those can be excluded.
                    continue;
                }

                var locatedAssembly = allReferencedAssemblies.FirstOrDefault(asm => AssemblyName.ReferenceMatchesDefinition(asm.GetName(), dllAssemblyName));
                if (locatedAssembly != null)
                {
                    assembliesInBinFolder.Add(locatedAssembly);
                }
            }

            return assembliesInBinFolder;
        }
    }
}
