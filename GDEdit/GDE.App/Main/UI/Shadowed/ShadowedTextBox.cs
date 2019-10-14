using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using GDE.App.Main.Colors;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;

namespace GDE.App.Main.UI.Shadowed
{
    public class ShadowedTextBox : TextBox
    {
        private static readonly Color4 edgeEffectColor = GDEColors.FromHex("141414").Opacity(0.25f);

        public ShadowedTextBox()
        {
            BackgroundUnfocused = GDEColors.FromHex("262626");
            BackgroundFocused = GDEColors.FromHex("2B2B2B");
            BackgroundCommit = GDEColors.FromHex("808080");

            CornerRadius = 5;

            EdgeEffect = GetEdgeEffectParameters(0.5f, 1);
        }

        protected override bool OnHover(HoverEvent e)
        {
            TweenEdgeEffectTo(GetEdgeEffectParameters(1, 2), 200, Easing.OutExpo);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);
            TweenEdgeEffectTo(GetEdgeEffectParameters(0.5f, 1), 200, Easing.OutExpo);
        }

        // Abstraction is not bad; copying code around gets annoying
        private static EdgeEffectParameters GetEdgeEffectParameters(float offsetY, float radius)
        {
            return new EdgeEffectParameters
            {
                Colour = edgeEffectColor,
                Type = EdgeEffectType.Shadow,
                Offset = new Vector2(0, offsetY),
                Roundness = 5,
                Radius = radius,
            };
        }
    }
}
