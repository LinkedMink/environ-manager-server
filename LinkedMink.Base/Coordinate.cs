using System;
using LinkedMink.Base.Extensions;

namespace LinkedMink.Base
{
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public byte[] Data
        {
            get
            {
                var data = new byte[16];

                Latitude.ToByteArray(data, 0);
                Longitude.ToByteArray(data, 8);

                return data;
            }
            set
            {
                if (value.Length != 16)
                    throw new ArgumentException("Coordinate.Data is 16 bytes in length");

                Latitude = BitConverter.ToDouble(value, 0);
                Longitude = BitConverter.ToDouble(value, 8);
            }
        }
    }
}
