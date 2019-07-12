using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace GeoUK.OSTN
{
    internal static class ResourceManager
    {
        private static Dictionary<int, OstnDataRecord> _ostn02Data;
        public static Dictionary<int, OstnDataRecord> Ostn02Data
        {
            get
            {
                if (_ostn02Data == null)
                    _ostn02Data = RetrieveEmbeddedOSTN(OstnVersionEnum.OSTN02);
                return _ostn02Data;
            }
        }

        private static Dictionary<int, OstnDataRecord> _ostn15Data;
        public static Dictionary<int, OstnDataRecord> Ostn15Data
        {
            get
            {
                if (_ostn15Data == null)
                    _ostn15Data = RetrieveEmbeddedOSTN(OstnVersionEnum.OSTN15);
                return _ostn15Data;
            }
        }

        /// <summary>
        /// Loads the OSTN data into memory.
        /// </summary>
        /// <param name="ostnVersion">If not provided, it will load both OSTN02 and OSTN15 data.</param>
        public static void LoadResources(OstnVersionEnum? ostnVersion = null)
        {
            if(!ostnVersion.HasValue || ostnVersion.Value == OstnVersionEnum.OSTN02)
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
                    stream = ResourceManager.GetEmbeddedResourceStream(typeof(Transform).GetTypeInfo().Assembly, "OSTN02_OSGM02_GB.txt");
                    break;
                case OstnVersionEnum.OSTN15:
                    stream = ResourceManager.GetEmbeddedResourceStream(typeof(Transform).GetTypeInfo().Assembly, "OSTN15_OSGM15_DataFile.txt");
                    break;
                default:
                    throw new NotImplementedException();
            }

            var data = new Dictionary<int, OstnDataRecord>();
            using (var reader = new StreamReader(stream))
            {
                // Skipping the header row
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        var values = line.Split(',');
                        var record = new OstnDataRecord
                        {
                            Point_ID = Int32.Parse(values[0]),
                            ETRS89_Easting = Double.Parse(values[1]),
                            ETRS89_Northing = Double.Parse(values[2]),
                            ETRS89_OSGB36_EShift = Double.Parse(values[3]),
                            ETRS89_OSGB36_NShift = Double.Parse(values[4]),
                            ETRS89_ODN_HeightShift = Double.Parse(values[5]),
                            Height_Datum_Flag = Double.Parse(values[6]),
                        };
                        data[record.Point_ID] = record;
                    }
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
        private static Stream GetEmbeddedResourceStream (Assembly assembly, string resourceFileName)
		{
			var resourceNames = assembly.GetManifestResourceNames ();

			var resourcePaths = resourceNames
				.Where (x => x.EndsWith (resourceFileName, StringComparison.CurrentCultureIgnoreCase))
				.ToArray ();

			if (!resourcePaths.Any ()) {
				throw new Exception (String.Format ("Resource ending with {0} not found.", resourceFileName));
			}

			if (resourcePaths.Count () > 1) {
				throw new Exception (String.Format ("Multiple resources ending with {0} found: {1}{2}", resourceFileName, Environment.NewLine, String.Join (Environment.NewLine, resourcePaths)));
			}

			return assembly.GetManifestResourceStream (resourcePaths.Single ());
		}
    }
}
