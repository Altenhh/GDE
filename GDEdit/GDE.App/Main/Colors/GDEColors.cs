using System;
using osuTK.Graphics;

namespace GDE.App.Main.Colors
{
    /// <summary>Provides the colors of the application and functions related to them.</summary>
    public static class GDEColors
    {
        public static Color4 Gray(float amt) => new Color4(amt, amt, amt, 1f);
        public static Color4 Gray(byte amt) => new Color4(amt, amt, amt, 255);

        /// <summary>Returns a <see cref="Color4"/> value from a hex string.</summary>
        /// <param name="hex">The hex string of the color.</param>
        public static Color4 FromHex(string hex)
        {
            if (hex[0] == '#')
                hex = hex.Substring(1);

            switch (hex.Length)
            {
                case 3:
                    return new Color4((byte)(getByte(0, 1) * 17), (byte)(getByte(1, 1) * 17), (byte)(getByte(2, 1) * 17), 255);
                case 6:
                    return new Color4(getByte(0, 2), getByte(2, 2), getByte(4, 2), 255);
                default:
                    throw new ArgumentException(@"Invalid hex string length!");
            }

            byte getByte(int n, int k) => Convert.ToByte(hex.Substring(n, k), 16);
        }

        // TODO: Add official colors here
    }
}