using osu.Framework.Graphics.Sprites;

namespace GDE.App.Main.Graphics
{
    public class GDEFont
    {
        /// <summary>The default font size.</summary>
        public const float DEFAULT_FONT_SIZE = 30;

        /// <summary>The default font.</summary>
        public static FontUsage Default => GetFont();

        public static FontUsage Numeric => GetFont(Typeface.Digitall);

        /// <summary>Retrieves a <see cref="FontUsage"/>.</summary>
        /// <param name="typeface">The font typeface.</param>
        /// <param name="size">The size of the text in local space. For a value of 16, a single line will have a height of 16px.</param>
        /// <param name="fixedWidth">Whether all characters should be spaced the same distance apart.</param>
        /// <returns>The <see cref="FontUsage"/>.</returns>
        public static FontUsage GetFont(Typeface typeface = Typeface.Purista, float size = DEFAULT_FONT_SIZE, bool fixedWidth = false)
            => new FontUsage(GetFamilyString(typeface), size, "", false, fixedWidth);

        /// <summary>Retrieves the string representation of a <see cref="Typeface"/>.</summary>
        /// <param name="typeface">The <see cref="Typeface"/>.</param>
        /// <returns>The string representation.</returns>
        public static string GetFamilyString(Typeface typeface)
        {
            switch (typeface)
            {
                case Typeface.Purista:
                    return "Purista";

                case Typeface.Digitall:
                    return "Digitall";
            }

            return null;
        }
    }

    public enum Typeface
    {
        Purista,
        Digitall,
    }
}
