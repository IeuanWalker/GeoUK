using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GeoUK
{
    internal static class ResourceManager
    {
        /// <summary>
        /// Attempts to find and return the given resource from within the specified assembly.
        /// </summary>
        /// <returns>The embedded resource stream.</returns>
        /// <param name="assembly">Assembly.</param>
        /// <param name="resourceFileName">Resource file name.</param>
        public static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
        {
            string[] resourceNames = assembly.GetManifestResourceNames();

            string[] resourcePaths = resourceNames
                .Where(x => x.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();

            if (!resourcePaths.Any())
            {
                throw new Exception($"Resource ending with {resourceFileName} not found.");
            }

            if (resourcePaths.Length > 1)
            {
                throw new Exception($"Multiple resources ending with {resourceFileName} found: {Environment.NewLine}{string.Join(Environment.NewLine, resourcePaths)}");
            }

            return assembly.GetManifestResourceStream(resourcePaths.Single());
        }
    }
}