using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using GDE.App.Main.Colors;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;

namespace GDE.App.Main.UI.Shadowed
{
    public class ShadowedTextBox : TextBox
    {
        public ShadowedTextBox()
        {
            BackgroundUnfocused = GDEColors.FromHex("262626");
            BackgroundFocused = GDEColors.FromHex("2B2B2B");
            BackgroundCommit = GDEColors.FromHex("808080");

            CornerRadius = 5;

            EdgeEffect = new EdgeEffectParameters
            {
                Colour = GDEColors.FromHex("141414").Opacity(0.25f),
                Type = EdgeEffectType.Shadow,
                Offset = new Vector2(0, 0.5f),
                Roundness = 5,
                Radius = 1f,
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Colour = GDEColors.FromHex("141414").Opacity(0.25f),
                Type = EdgeEffectType.Shadow,
                Offset = new Vector2(0, 1f),
                Roundness = 5,
                Radius = 2f,
            }, 200, Easing.OutExpo);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Colour = GDEColors.FromHex("141414").Opacity(0.25f),
                Type = EdgeEffectType.Shadow,
                Offset = new Vector2(0, 0.5f),
                Roundness = 5,
                Radius = 1f,
            }, 200, Easing.OutExpo);
        }
    }
}
