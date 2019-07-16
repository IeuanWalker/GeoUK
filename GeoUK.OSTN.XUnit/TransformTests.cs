using System;
using System.Collections.Generic;
using System.IO;
using GeoUK.Coordinates;
using GeoUK.OSTN.XUnit.Models;
using Xunit;

namespace GeoUK.OSTN.XUnit
{
    public class TransformTests
    {
        [Fact]
        public void Etrs89ToOsgb_OSTN15_Test()
        {
            string inputFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFiles_OSGM15_OSTN15/OSTN15_OSGM15_TestInput_ETRStoOSGB.txt");
            string outputFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFiles_OSGM15_OSTN15/OSTN15_OSGM15_TestOutput_ETRStoOSGB.txt");

            List<DataPoint> inputData = new List<DataPoint>();
            Dictionary<string, DataPoint> outputData = new Dictionary<string, DataPoint>();

            using (StreamReader inputFile = new StreamReader(inputFileName))
            {
                string line;
                while ((line = inputFile.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || !line.StartsWith("TP")) continue;

                    string[] values = line.Split(',');
                    DataPoint point = new DataPoint
                    {
                        PointID = values[0],
                        X = double.Parse(values[1]),
                        Y = double.Parse(values[2]),
                        Height = double.Parse(values[3])
                    };
                    inputData.Add(point);
                }
            }

            using (StreamReader outputFile = new StreamReader(outputFileName))
            {
                string line;
                while ((line = outputFile.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || !line.StartsWith("TP")) continue;

                    string[] values = line.Split(',');
                    DataPoint point = new DataPoint
                    {
                        PointID = values[0],
                        X = double.Parse(values[1]),
                        Y = double.Parse(values[2]),
                        Height = double.Parse(values[3])
                    };
                    outputData[point.PointID] = point;
                }
            }

            foreach (DataPoint dataPoint in inputData)
            {
                Osgb36 transformation = Transform.Etrs89ToOsgb(new LatitudeLongitude(dataPoint.X, dataPoint.Y, dataPoint.Height));

                // Comparing values with a precision of 3 decimals, as they are given in the output file.
                bool latitudesEqual = outputData[dataPoint.PointID].X
                    .IsApproximatelyEqualTo(transformation.Easting, 0.001);
                bool longitudesEqual = outputData[dataPoint.PointID].Y
                    .IsApproximatelyEqualTo(transformation.Northing, 0.001);
                bool heightsEqual = outputData[dataPoint.PointID].Height
                    .IsApproximatelyEqualTo(transformation.Height, 0.001);

                Assert.True(latitudesEqual);
                Assert.True(longitudesEqual);
                Assert.True(heightsEqual);
            }
        }

        [Fact]
        public void OsgbToEtrs89_OSTN15_Test()
        {
            string inputFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFiles_OSGM15_OSTN15/OSTN15_OSGM15_TestInput_OSGBtoETRS.txt");
            string outputFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFiles_OSGM15_OSTN15/OSTN15_OSGM15_TestOutput_OSGBtoETRS.txt");

            List<DataPoint> inputData = new List<DataPoint>();
            Dictionary<string, DataPoint> outputData = new Dictionary<string, DataPoint>();

            using (StreamReader inputFile = new StreamReader(inputFileName))
            {
                string line;
                while ((line = inputFile.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || !line.StartsWith("TP")) continue;

                    string[] values = line.Split(',');
                    DataPoint point = new DataPoint
                    {
                        PointID = values[0],
                        X = double.Parse(values[1]),
                        Y = double.Parse(values[2]),
                        Height = double.Parse(values[3])
                    };
                    inputData.Add(point);
                }
            }

            using (StreamReader outputFile = new StreamReader(outputFileName))
            {
                string line;
                while ((line = outputFile.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || !line.StartsWith("TP")) continue;

                    string[] values = line.Split(',');
                    if (values[1] != "RESULT") continue;

                    DataPoint point = new DataPoint
                    {
                        PointID = values[0],
                        X = double.Parse(values[2]),
                        Y = double.Parse(values[3]),
                        Height = double.Parse(values[4])
                    };
                    outputData[point.PointID] = point;
                }
            }

            foreach (DataPoint dataPoint in inputData)
            {
                LatitudeLongitude transformation = Transform.OsgbToEtrs89(new Osgb36(dataPoint.X, dataPoint.Y));

                // Comparing values with a precision of 3 decimals, as they are given in the output file.
                bool latitudesEqual = outputData[dataPoint.PointID].X
                    .IsApproximatelyEqualTo(transformation.Latitude, 0.0000000001);
                bool longitudesEqual = outputData[dataPoint.PointID].Y
                    .IsApproximatelyEqualTo(transformation.Longitude, 0.0000000001);

                Assert.True(latitudesEqual);
                Assert.True(longitudesEqual);
            }
        }
    }
}