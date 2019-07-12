using System;
using GeoUK.Coordinates;

namespace DemoProject.Examples
{
    public static class OSMapReference
    {
        public static void Examples()
        {
            Console.WriteLine("-- OS Map reference --");

            // Convert to Osgb36 coordinates by creating a new object passing
            // in the EastingNorthing object to the constructor.
            EastingNorthing eastingNorthing = new EastingNorthing(319267, 175189);

            Console.WriteLine("INPUT");
            Console.WriteLine($"Easting: {eastingNorthing.Easting}");
            Console.WriteLine($"Northing: {eastingNorthing.Northing}");

            Osgb36 osgb36EN = new Osgb36(eastingNorthing);
            string mapReference = osgb36EN.MapReference;

            Console.WriteLine("OUTPUT");
            Console.WriteLine($"Map reference: {mapReference}");
            Console.WriteLine();
        }
    }
}
