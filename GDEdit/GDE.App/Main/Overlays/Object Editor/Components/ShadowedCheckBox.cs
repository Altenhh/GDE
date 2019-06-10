using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using GDE.App.Main.Colors;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Shapes;

namespace GDE.App.Main.Overlays.ObjectEditor.Components
{
    public class ShadowedCheckbox : Checkbox
    {
        private SpriteIcon icon;
        private Box background;

        public ShadowedCheckbox()
        {
            CornerRadius = 5;
            Masking = true;

            EdgeEffect = new EdgeEffectParameters
            {
                Colour = GDEColors.FromHex("141414").Opacity(0.25f),
                Type = EdgeEffectType.Shadow,
                Offset = new Vector2(0, 0.5f),
                Roundness = 5,
                Radius = 1f,
            };

            AddRange(new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = GDEColors.FromHex("2D2D2D")
                },
                icon = new SpriteIcon
                {
                    Icon = FontAwesome.Solid.Check,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Scale = new Vector2(0),
                }
            });

            Current.ValueChanged += updateCheck;
        }

        private void updateCheck(ValueChangedEvent<bool> obj)
        {
            if (obj.NewValue)
            {
                icon.ScaleTo(0.7f, 200, Easing.OutExpo);
                background.FadeColour(GDEColors.FromHex("333"), 200, Easing.OutExpo);
            }
            else
            {
                icon.ScaleTo(0, 200, Easing.OutExpo);
                background.FadeColour(GDEColors.FromHex("2D2D2D"), 200, Easing.OutExpo);
            }
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
