using GeoUK.Coordinates;

namespace GeoUK.OSTN
{
    public class Shifts
    {
        public double Se { get; set; }
        public double Sn { get; set; }
        public double Sg { get; set; }

        public Osgb36GeoidDatum GeoidDatum { get; set; }
    }
}