using System;
using GeoUK;
using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;
using Convert = GeoUK.Convert;

namespace DemoProject.Examples
{
    public static class LatitudeLongitudeToEastingNorthing
    {
        public static void Example()
        {
            Console.WriteLine("-- Latitude/ Longitude to Easting/ Northing --");
            LatitudeLongitude latLong = new LatitudeLongitude(51.469886, -3.1636964);

            Console.WriteLine("INPUT");
            Console.WriteLine($"Latitude: {latLong.Latitude}");
            Console.WriteLine($"Longitude: {latLong.Longitude}");

            Cartesian cartesian = Convert.ToCartesian(new Wgs84(), latLong);
            Cartesian bngCartesian = Transform.Etrs89ToOsgb36(cartesian);
            EastingNorthing bngEN = Convert.ToEastingNorthing(new Airy1830(), new BritishNationalGrid(), bngCartesian);

            Console.WriteLine("OUTPUT");
            Console.WriteLine($"Easting: {bngEN.Easting}");
            Console.WriteLine($"Northing: {bngEN.Northing}");
            Console.WriteLine();
        }
    }
}
