using System;

namespace LinkedMink.Base.Extensions
{
    public static class DoubleExtensions
    {
        public static void ToByteArray(this double value, byte[] data, int startIndex)
        {
            var valueAsBits = BitConverter.DoubleToInt64Bits(value);

            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 8; i++)
                    data[i + startIndex] = (byte)(valueAsBits >> (i * 8) & OneByte);
            }
            else
            {
                for (int i = 7; i >= 0; i--)
                    data[i + startIndex] = (byte)(valueAsBits >> (i * 8) & OneByte);
            }
        }

        private const byte OneByte = 0xFF;
    }
}
