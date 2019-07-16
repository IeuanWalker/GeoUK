using System;
using DemoProject.Examples;

namespace DemoProject
{
    internal class Program
    {
        private static void Main()
        {
            EastingNorthingToLatitudeLongitude.Example();

            LatitudeLongitudeToEastingNorthing.Example();

            OSMapReference.Examples();

            ObtainingGreaterAccuracy.Example();

            Console.ReadKey();
        }
    }
}