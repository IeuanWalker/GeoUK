using System;
using GeoUK;
using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;
using Convert = GeoUK.Convert;

namespace DemoProject.Examples
{
    public static class EastingNorthingToLatitudeLongitude
    {
        public static void Example()
        {
            Console.WriteLine("-- Easting/ Nothing to Latitude Longitude --");
            // Given an easting and northing in metres (see text)
            const double easting = 319267;
            const double northing = 175189;

            Console.WriteLine("INPUT");
            Console.WriteLine($"Easting: {easting}");
            Console.WriteLine($"Northing: {northing}");

            // Convert to Cartesian
            Cartesian cartesian = Convert.ToCartesian(new Airy1830(),
                new BritishNationalGrid(),
                new EastingNorthing(
                    easting,
                    northing));

            Cartesian wgsCartesian = Transform.OSBB36ToEtrs89(cartesian); //ETRS89 is effectively WGS84

            LatitudeLongitude wgsLatLong = Convert.ToLatitudeLongitude(new Wgs84(), wgsCartesian);

            Console.WriteLine("OUTPUT");
            Console.WriteLine($"Latitude: {wgsLatLong.Latitude}");
            Console.WriteLine($"Longitude: {wgsLatLong.Longitude}");
            Console.WriteLine();
        }
    }
}
