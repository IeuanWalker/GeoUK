using GeoUK.Coordinates;
using GeoUK.OSTN.Tests.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace GeoUK.OSTN.Tests
{
    public class TransformTests
    {
        [Fact]
        public void Etrs89ToOsgb_OSTN15_Test()
        {
            string inputFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFiles_OSGM15_OSTN15/OSTN15_OSGM15_TestInput_ETRStoOSGB.txt");
            string outputFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFiles_OSGM15_OSTN15/OSTN15_OSGM15_TestOutput_ETRStoOSGB.txt");

            var inputData = new List<DataPoint>();
            var outputData = new Dictionary<string, DataPoint>();

            using (var inputFile = new StreamReader(inputFileName))
            {
                string line;
                while ((line = inputFile.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line) && line.StartsWith("TP"))
                    {
                        var values = line.Split(',');
                        var point = new DataPoint
                        {
                            PointID = values[0],
                            X = double.Parse(values[1]),
                            Y = double.Parse(values[2]),
                            Height = double.Parse(values[3])
                        };
                        inputData.Add(point);
                    }
                }
            }

            using (var outputFile = new StreamReader(outputFileName))
            {
                string line;
                while ((line = outputFile.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line) && line.StartsWith("TP"))
                    {
                        var values = line.Split(',');
                        var point = new DataPoint
                        {
                            PointID = values[0],
                            X = double.Parse(values[1]),
                            Y = double.Parse(values[2]),
                            Height = double.Parse(values[3])
                        };
                        outputData[point.PointID] = point;
                    }
                }
            }

            foreach (var dataPoint in inputData)
            {
                var transformation = Transform.Etrs89ToOsgb(new LatitudeLongitude(dataPoint.X, dataPoint.Y, dataPoint.Height), OstnVersionEnum.OSTN15);

                // Comparing values with a precision of 3 decimals, as they are given in the output file.
                var latitudesEqual = outputData[dataPoint.PointID].X
                    .IsApproximatelyEqualTo(transformation.Easting, 0.001);
                var longitudesEqual = outputData[dataPoint.PointID].Y
                    .IsApproximatelyEqualTo(transformation.Northing, 0.001);
                var heightsEqual = outputData[dataPoint.PointID].Height
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

            var inputData = new List<DataPoint>();
            var outputData = new Dictionary<string, DataPoint>();

            using (var inputFile = new StreamReader(inputFileName))
            {
                string line;
                while ((line = inputFile.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line) && line.StartsWith("TP"))
                    {
                        var values = line.Split(',');
                        var point = new DataPoint
                        {
                            PointID = values[0],
                            X = double.Parse(values[1]),
                            Y = double.Parse(values[2]),
                            Height = double.Parse(values[3])
                        };
                        inputData.Add(point);
                    }
                }
            }

            using (var outputFile = new StreamReader(outputFileName))
            {
                string line;
                while ((line = outputFile.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line) && line.StartsWith("TP"))
                    {
                        var values = line.Split(',');
                        if (values[1] == "RESULT")
                        {
                            var point = new DataPoint
                            {
                                PointID = values[0],
                                X = double.Parse(values[2]),
                                Y = double.Parse(values[3]),
                                Height = double.Parse(values[4])
                            };
                            outputData[point.PointID] = point;
                        }
                    }
                }
            }

            foreach (var dataPoint in inputData)
            {
                var transformation = Transform.OsgbToEtrs89(new Osgb36(dataPoint.X, dataPoint.Y), OstnVersionEnum.OSTN15);

                // Comparing values with a precision of 3 decimals, as they are given in the output file.
                var latitudesEqual = outputData[dataPoint.PointID].X
                    .IsApproximatelyEqualTo(transformation.Latitude, 0.0000000001);
                var longitudesEqual = outputData[dataPoint.PointID].Y
                    .IsApproximatelyEqualTo(transformation.Longitude, 0.0000000001);

                // Comparing heights with a precision of 4 decimals, as they are given in the output file
                // Not implemented
                //var heightsEqual = outputData[dataPoint.PointID].Height
                //    .IsApproximatelyEqualTo(transformation.ElipsoidalHeight, 0.0001);

                Assert.True(latitudesEqual);
                Assert.True(longitudesEqual);
                // Not implemented
                //Assert.True(heightsEqual);
            }
        }
    }
}