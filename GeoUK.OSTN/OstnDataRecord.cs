namespace GeoUK.OSTN
{
    public class OstnDataRecord
    {
        public int Point_ID { get; set; }
        public double ETRS89_Easting { get; set; }
        public double ETRS89_Northing { get; set; }
        public double ETRS89_OSGB36_EShift { get; set; }
        public double ETRS89_OSGB36_NShift { get; set; }
        public double ETRS89_ODN_HeightShift { get; set; }
        public double Height_Datum_Flag { get; set; }
    }
}