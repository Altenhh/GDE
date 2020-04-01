using osu.Framework.Extensions.Color4Extensions;
using osuTK;
using osuTK.Graphics;

namespace GDEdit.App.Graphics
{
    public static class GDEColour
    {
        public static Color4 Gray(float amt) => new Color4(amt, amt, amt, 1f);
        public static Color4 Gray(byte amt) => new Color4(amt, amt, amt, 255);

        public static Color4 GetLightness(float lightness) => Color4.FromHsl(new Vector4(0, 0, lightness, 1));

        public static readonly Color4 Gray0 = GetLightness(0);
        public static readonly Color4 Gray05 = GetLightness(0.05f);
        public static readonly Color4 Gray10 = GetLightness(0.1f);
        public static readonly Color4 Gray15 = GetLightness(0.15f);
        public static readonly Color4 Gray20 = GetLightness(0.2f);
        public static readonly Color4 Gray25 = GetLightness(0.25f);
        public static readonly Color4 Gray30 = GetLightness(0.3f);
        public static readonly Color4 Gray35 = GetLightness(0.35f);
        public static readonly Color4 Gray40 = GetLightness(0.4f);
        public static readonly Color4 Gray45 = GetLightness(0.45f);
        public static readonly Color4 Gray50 = GetLightness(0.5f);
        public static readonly Color4 Gray55 = GetLightness(0.55f);
        public static readonly Color4 Gray60 = GetLightness(0.6f);
        public static readonly Color4 Gray65 = GetLightness(0.65f);
        public static readonly Color4 Gray70 = GetLightness(0.7f);
        public static readonly Color4 Gray75 = GetLightness(0.75f);
        public static readonly Color4 Gray80 = GetLightness(0.8f);
        public static readonly Color4 Gray85 = GetLightness(0.85f);
        public static readonly Color4 Gray90 = GetLightness(0.9f);
        public static readonly Color4 Gray95 = GetLightness(0.95f);
        public static readonly Color4 Gray100 = GetLightness(1f);
    }
}