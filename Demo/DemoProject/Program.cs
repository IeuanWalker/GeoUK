using System;
using DemoProject.Examples;

namespace DemoProject
{
    class Program
    {
        static void Main()
        {
            EastingNorthingToLatitudeLongitude.Example();

            LatitudeLongitudeToEastingNorthing.Example();

            OSMapReference.Examples();

            ObtainingGreaterAccuracy.Example();

            Console.ReadKey();
        }
    }
}
