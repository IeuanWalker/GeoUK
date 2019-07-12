using System;
using GeoUK.Coordinates;

namespace DemoProject.Examples
{
    public static class ObtainingGreaterAccuracy
    {
        public static void Example()
        {
            Console.WriteLine("-- Obtaining Greater Accuracy --");

            LatitudeLongitude latLong = new LatitudeLongitude(51.469886, -3.1636964, 108.05);

            Console.WriteLine("INPUT");
            Console.WriteLine($"Latitude: {latLong.Latitude}");
            Console.WriteLine($"Longitude: {latLong.Longitude}");
            Console.WriteLine($"Ellipsoidal Height: {latLong.EllipsoidalHeight}");

            Osgb36 bng = GeoUK.OSTN.Transform.Etrs89ToOsgb(latLong);

            Console.WriteLine("OUTPUT");
            Console.WriteLine($"Map reference: {bng.MapReference}");
            Console.WriteLine($"Easting: {bng.Easting}");
            Console.WriteLine($"Northing: {bng.Northing}");
            Console.WriteLine($"Height: {bng.Height}");
            Console.WriteLine();
        }
    }
}
