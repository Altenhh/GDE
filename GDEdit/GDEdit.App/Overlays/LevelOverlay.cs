using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace GDEdit.App.Overlays
{
    public class LevelOverlay : FocusedOverlayContainer
    {
        public LevelOverlay(GDAPI.Objects.GeometryDash.General.Level level)
        {
            RelativeSizeAxes = Axes.Both;
            Width = 0.85f;
            Height = 0.85f;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Masking = true;

            CornerRadius = 3;

            EdgeEffect = new EdgeEffectParameters
            {
                Colour = Color4.Black.Opacity(0),
                Type = EdgeEffectType.Shadow,
                Radius = 10,
                Hollow = true
            };
            
            Children = new Drawable[]
            {
                
            };
        }
        
        public override void Show()
        {
            if (State.Value == Visibility.Visible)
            {
                // re-trigger the state changed so we can potentially surface to front
                State.TriggerChange();
            }
            else
                base.Show();
        }

        protected override void PopIn()
        {
            base.PopIn();
            FadeEdgeEffectTo(0.4f, 200, Easing.Out);
            this.FadeIn(200, Easing.Out);
        }

        protected override void PopOut()
        {
            base.PopOut();
            FadeEdgeEffectTo(0f, 200, Easing.In);
            this.FadeOut(200, Easing.In);
        }
    }
}