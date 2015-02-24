using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SACS.BusinessLayer.BusinessLogic.Application;
using SACS.Implementation;

namespace SACS.BusinessLayer.BusinessLogic.Loader
{
    /// <summary>
    /// Class that helps with initializing the actual AppDomain object
    /// </summary>
    [Serializable]
    internal class DomainInitializer : MarshalByRefObject
    {
        /// <summary>
        /// Gets the type of the entry.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EntryPointNotFoundException">Thrown when implementation of service entry could not be found.</exception>
        public virtual string GetEntryType(string assemblyPath)
        {
            // create an instance of itself in an app domain, so that the assembly can be loaded into it, the type found, then unloaded.
            // this allows us to unload the assembly, because the only way to unload an assembly is by unloading the app domain.
            AppDomain tempDomain = AppDomain.CreateDomain("DomainInitializer-" + Guid.NewGuid(), null, AppDomain.CurrentDomain.SetupInformation);

            Type thisType = typeof(DomainInitializer);
            DomainInitializer newInitializer = (DomainInitializer)tempDomain.CreateInstanceAndUnwrap(thisType.Assembly.FullName, thisType.FullName);
            string entryType = newInitializer.FindEntryType(assemblyPath);
            AppDomain.Unload(tempDomain);

            return entryType;
        }

        /// <summary>
        /// Finds the entry type for the service app. this should ideally be called in a separate app domain because it loads the assembly.
        /// </summary>
        /// <param name="assemblyPath">the path to the base assembly.</param>
        /// <returns></returns>
        private string FindEntryType(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);

            var targetType = typeof(ServiceAppBase);
            var types = assembly.GetTypes()
                .Where(p => p.IsClass && !p.IsAbstract && p.IsSubclassOf(targetType));

            int count = types.Count();

            if (count != 1)
            {
                throw new EntryPointNotFoundException(string.Format("Could not find class, or found more than one to load as a service app. Count: {0}", types.Count()));
            }

            return types.First().FullName;
        }
    }
}
