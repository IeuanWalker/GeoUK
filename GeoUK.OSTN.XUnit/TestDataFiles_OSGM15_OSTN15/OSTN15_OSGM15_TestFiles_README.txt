Four files are supplied to test implementations of the OSTN15/OSGM15 transformation in Great Briatin.

All files are comma delimited with a header line on line #1.

Two input files:
1.  OSTN15_OSGM15_TestInput_ETRStoOSGB.txt
A selection of points to test the transformation from ETRS89 (latitude, longitude and height) to OSGB36 (eastings, northings and height).
2.  OSTN15_OSGM15_TestInput_OSGBtoETRS.txt
A selection of points to test the transformation from OSGB36 (eastings, northings and height) to ETRS89 (latitude, longitude and height).

Two output files corresponding to the above inputs:
1.  OSTN15_OSGM15_TestOutput_ETRStoOSGB.txt
Results of OSTN15_OSGM15_TestInput_ETRStoOSGB.txt passed through OSTN15/OSGM15.  The intermediate values  from the bilinear interpolation are also given.
2.  OSTN15_OSGM15_TestOutput_OSGBtoETRS.txt
Results of OSTN15_OSGM15_TestInput_OSGBtoETRS.txt passed through OSTN15/OSGM15.  The intermediate values  from the bilinear interpolation are also given.  The iterative steps are also given so each result usually consists of 4 lines - 3 iterative steps plus the final ETRS89 output.  Results from different points are seperated by a blank line.
