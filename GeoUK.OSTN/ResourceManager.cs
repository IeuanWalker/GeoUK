using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GeoUK.OSTN
{
    internal static class ResourceManager
    {
        private static Dictionary<int, OstnDataRecord> _ostn02Data;
        public static Dictionary<int, OstnDataRecord> Ostn02Data => _ostn02Data ?? (_ostn02Data = RetrieveEmbeddedOSTN(OstnVersionEnum.OSTN02));
        private static Dictionary<int, OstnDataRecord> _ostn15Data;

        public static Dictionary<int, OstnDataRecord> Ostn15Data => _ostn15Data ?? (_ostn15Data = RetrieveEmbeddedOSTN(OstnVersionEnum.OSTN15));

        /// <summary>
        /// Loads the OSTN data into memory.
        /// </summary>
        /// <param name="ostnVersion">If not provided, it will load both OSTN02 and OSTN15 data.</param>
        public static void LoadResources(OstnVersionEnum? ostnVersion = null)
        {
            if (!ostnVersion.HasValue || ostnVersion.Value == OstnVersionEnum.OSTN02)
                _ostn02Data = RetrieveEmbeddedOSTN(OstnVersionEnum.OSTN02);

            if (!ostnVersion.HasValue || ostnVersion.Value == OstnVersionEnum.OSTN15)
                _ostn15Data = RetrieveEmbeddedOSTN(OstnVersionEnum.OSTN15);
        }

        /// <summary>
        /// Gets the embedded OSTN data
        /// </summary>
        /// <param name="ostnVersion"></param>
        /// <returns></returns>
        private static Dictionary<int, OstnDataRecord> RetrieveEmbeddedOSTN(OstnVersionEnum ostnVersion)
        {
            Stream stream;
            switch (ostnVersion)
            {
                case OstnVersionEnum.OSTN02:
                    stream = GetEmbeddedResourceStream(typeof(Transform).GetTypeInfo().Assembly, "OSTN02_OSGM02_GB.txt");
                    break;

                case OstnVersionEnum.OSTN15:
                    stream = GetEmbeddedResourceStream(typeof(Transform).GetTypeInfo().Assembly, "OSTN15_OSGM15_DataFile.txt");
                    break;

                default:
                    throw new NotImplementedException();
            }

            Dictionary<int, OstnDataRecord> data = new Dictionary<int, OstnDataRecord>();
            using (StreamReader reader = new StreamReader(stream))
            {
                // Skipping the header row
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] values = line.Split(',');
                    OstnDataRecord record = new OstnDataRecord
                    {
                        Point_ID = int.Parse(values[0]),
                        ETRS89_Easting = double.Parse(values[1]),
                        ETRS89_Northing = double.Parse(values[2]),
                        ETRS89_OSGB36_EShift = double.Parse(values[3]),
                        ETRS89_OSGB36_NShift = double.Parse(values[4]),
                        ETRS89_ODN_HeightShift = double.Parse(values[5]),
                        Height_Datum_Flag = double.Parse(values[6]),
                    };
                    data[record.Point_ID] = record;
                }
            }

            return data;
        }

        /// <summary>
        /// Attempts to find and return the given resource from within the specified assembly.
        /// </summary>
        /// <returns>The embedded resource stream.</returns>
        /// <param name="assembly">Assembly.</param>
        /// <param name="resourceFileName">Resource file name.</param>
        private static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
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