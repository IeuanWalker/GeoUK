namespace GeoUK.Ellipsoids
{
    public class Hayford1909 : Ellipsoid
    {
        //WGS constants
        private const double C_SEMI_MAJOR_AXIS = 6378388;
        private const double C_SEMI_MINOR_AXIS = 6356911.946;
        public Hayford1909()
            : base(C_SEMI_MAJOR_AXIS, C_SEMI_MINOR_AXIS)
        {
        }
    }
}