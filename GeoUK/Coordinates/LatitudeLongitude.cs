namespace GeoUK.Coordinates
{
	/// <summary>
	/// This immutable class represents a set of latitude/longitude/ellipsoidal height coordinates.
	/// </summary>
	public class LatitudeLongitude

	{

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="degreesLatitude"></param>
		/// <param name="degreesLongitude"></param>
		public LatitudeLongitude(double degreesLatitude, double degreesLongitude)
		{
			Latitude = degreesLatitude;
			Longitude = degreesLongitude;
			EllipsoidalHeight = 0.0;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="degreesLatitude"></param>
		/// <param name="degreesLongitude"></param>
		/// <param name="ellipsoidalHeight"></param>
		public LatitudeLongitude(double degreesLatitude, double degreesLongitude, double ellipsoidalHeight)
		{
			Latitude = degreesLatitude;
			Longitude = degreesLongitude;
			EllipsoidalHeight = ellipsoidalHeight;
		}

		/// <summary>
		/// Returns latitude in degrees.
		/// </summary>
		public double Latitude { get; }

		/// <summary>
		/// Returns longitude in degrees.
		/// </summary>
		public double Longitude { get; }

		/// <summary>
		/// returns ellipsoidal height in meters.
		/// </summary>
		public double EllipsoidalHeight { get; }
	}
}