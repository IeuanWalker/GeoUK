![Dependabot](https://api.dependabot.com/badges/status?host=github&repo=IeuanWalker/GeoUK) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/6b7e9e6fe8844188911c8a69a2e9905a)](https://app.codacy.com/app/ieuan.walker007/GeoUK?utm_source=github.com&utm_medium=referral&utm_content=IeuanWalker/GeoUK&utm_campaign=Badge_Grade_Dashboard) [![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FIeuanWalker%2FGeoUK.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FIeuanWalker%2FGeoUK?ref=badge_shield)

This is port from [GeoUK](https://bitbucket.org/johnnewcombe/geouk/src/master/). I created this port to convert the project to .NET Standard

The original authors have created a blog post detailing how to use both NuGet packages - [Converting Latitude and Longitude to British National Grid in C#](http://www.codeproject.com/Articles/1007147/Converting-Latitude-and-Longitude-to-British-Natio). Please go and read this article to get a better understanding of the concepts used. 

> TIP: When working with locations i like to use this website - [Grid reference finder](https://gridreferencefinder.com/)


# GeoUK [![Nuget](https://img.shields.io/nuget/v/IeuanWalker.GeoUK.svg) ![Nuget](https://img.shields.io/nuget/dt/IeuanWalker.GeoUK.svg)](https://www.nuget.org/packages/IeuanWalker.GeoUK/)

[![License: LGPL v3](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](https://www.gnu.org/licenses/lgpl-3.0)

[![Build Status](https://dev.azure.com/ieuanwalker/GeoUK/_apis/build/status/IeuanWalker.GeoUK?branchName=master)](https://dev.azure.com/ieuanwalker/GeoUK/_build/latest?definitionId=7&branchName=master)




The project allows for a conversion from GPS coordinates to British National Grid and back again. The product is licensed under the [GNU Lesser General Public License (LGPL)](https://www.gnu.org/licenses/lgpl-3.0.en.html).

# GeoUK.OSTN [![Nuget](https://img.shields.io/nuget/v/IeuanWalker.GeoUK.OSTN.svg) ![Nuget](https://img.shields.io/nuget/dt/IeuanWalker.GeoUK.OSTN.svg)](https://www.nuget.org/packages/IeuanWalker.GeoUK.OSTN/)

[![License: LGPL v3](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](https://www.gnu.org/licenses/lgpl-3.0)

[![Build Status](https://dev.azure.com/ieuanwalker/GeoUK/_apis/build/status/IeuanWalker.GeoUK.OSTN?branchName=master)](https://dev.azure.com/ieuanwalker/GeoUK/_build/latest?definitionId=8&branchName=master)



The GeoUk.OSTN project, adds OSTN02 and OSTN15 transformation which provide a greater accuracy. It should be noted that the this package contains the OSGM02 geoid and OSTN02 OSTN15 transformations, as a result is fairly large, in addition, transformations will be slower than using the Helmert transformations as used in the above nuget. The product is licensed under the [GNU Lesser General Public License (LGPL)](https://www.gnu.org/licenses/lgpl-3.0.en.html).

# How to use

> Install [GeoUK](https://www.nuget.org/packages/IeuanWalker.GeoUK/)

## Convert Easting/ Northing to Latitude/ Longitude 
1.  Convert to Cartesian
```csharp
// Given an easting and northing in metres (see text)
const double easting = 651409.903;
const double northing = 313177.270;

// Convert to Cartesian
Cartesian cartesian = Convert.ToCartesian(new Airy1830(),
    new BritishNationalGrid(),
    new EastingNorthing(easting, northing));
```
2. Transform from OSBB36 datum to ETRS89 datum
```csharp
Cartesian wgsCartesian = Transform.Osgb36ToEtrs89(cartesian); //ETRS89 is effectively WGS84
```
3. Convert back to Latitude/Longitude
```csharp
LatitudeLongitude wgsLatLong = Convert.ToLatitudeLongitude(new Wgs84(), wgsCartesian);
```

## Convert Latitude/ Longitude to Easting/ Northing
```csharp
LatitudeLongitude latLong = new LatitudeLongitude(51.469886, -3.1636964);

Cartesian cartesian = Convert.ToCartesian(new Wgs84(), latLong);
Cartesian bngCartesian = Transform.Etrs89ToOsgb36(cartesian);
EastingNorthing bngEN = Convert.ToEastingNorthing(new Airy1830(), new BritishNationalGrid(), bngCartesian); 
```

## Get OS Map reference
The map references (Easting/Northing) used in Ordnance Survey maps are divided into 500km squares which are sub-divided into 100km squares. These squares are given a two letter code. The first letter represents the 500km square and the second represents the 100km square within it. A six digit map reference would look something like TL123456 where the first two characters represents the 100km square as indicated on the map with the first three digits of the six representing the easting and the last three digits representing the northing. Using this system means that a map reference is quoted as an easting/northing (in metres) from the square's origin. An EastingNorthing coordinate object, as returned from the transformation described above, can be converted to an OS map reference by using the Osgb36 class as follows:
```csharp
EastingNorthing eastingNorthing = new EastingNorthing(319267, 175189);

Osgb36 osgb36EN = new Osgb36(eastingNorthing);
string mapReference = osgb36EN.MapReference;
```
## Obtaining Greater Accuracy
In order to obtain greater accuracy when transforming ETRS89 (WGS84) coordinates to British National Grid, the Ordnance Survey Geoid Model (OSGM02) needs to be used. The OSGM02 can be thought of as a large rubber sheet covering Great Britain, Northern Island and the Republic of Ireland. Special transformations are applied to the data within the OSGM02 to transform from ETRS89 and OSGB36. For Great Britain, the transformation is called OSTN02. The OSTN02 transformations combined with the ETRS89 positions of active GPS network stations represents the official definition of OSGB36 and can give very accurate transformations.

This rubber sheet geoid is effectively a lookup table that can be used to determine Othometric (geoid) heights and, via the OSTN transformation, accurate Easting and Northing coordinates. It is worth noting that Northern Island and the Republic of Ireland use the same geoid model but with a different transformation (OSi/OSNI) which, for now at least, is outside of the scope of this article.

The GeoUK.OSTN Nuget package extends the GeoUK package to include OSGM02/OSTN0 and OSTN15 functionality and provides a simple method to make an accurate one-way transformation from ETRS89 to BNG. The package can be added to a project using the following Package Manager command. The package is dependent upon the GeoUK package and will add it as required.

> Install [GeoUK.OSTN](https://www.nuget.org/packages/IeuanWalker.GeoUK.OSTN/)

It should be noted that the `GeoUK.OSTN` package contains the OSGM02 geoid and OSTN02 OSTN15 transformations, as a result is fairly large, in addition, transformations will be slower than using the Helmert transformations as used in the examples above.

The example below converts an ETRS89 Latitude/Longitude/Ellipsoid height to BNG Easting and Northing and ODN Height to within 10 centimetres or so.

```csharp
LatitudeLongitude latLong = new LatitudeLongitude(51.469886, -3.1636964, 108.05);
Osgb36 bng = GeoUK.OSTN.Transform.Etrs89ToOsgb(latLong);
```

# License
Both NuGet packages are licenced under [GNU Lesser General Public License (LGPL)](https://www.gnu.org/licenses/lgpl-3.0.en.html). This to respect the previous authors licence. Learn more [here.](https://tldrlegal.com/license/gnu-lesser-general-public-license-v3-(lgpl-3))


## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FIeuanWalker%2FGeoUK.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FIeuanWalker%2FGeoUK?ref=badge_large)
