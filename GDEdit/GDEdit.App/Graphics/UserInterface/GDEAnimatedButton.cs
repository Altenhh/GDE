using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace GDEdit.App.Graphics.UserInterface
{
    public class GDEAnimatedButton : ClickableContainer
    {
        private readonly Container content;
        private readonly Box hover;

        /// <summary>The colour that should be flashed when the <see cref="GDEAnimatedButton" /> is clicked.</summary>
        protected Color4 FlashColour = GDEColour.Gray30;
        
        /// <summary>The background colour that will stay constant.</summary>
        protected Color4 BackgroundColour = GDEColour.Gray10;

        private Color4 hoverColour = GDEColour.Gray15;

        /// <summary>The background colour of the <see cref="GDEAnimatedButton" /> while it is hovered.</summary>
        protected Color4 HoverColour
        {
            get => hoverColour;
            set
            {
                hoverColour = value;
                hover.Colour = value;
            }
        }

        public GDEAnimatedButton()
        {
            Add(content = new CircularContainer
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                EdgeEffect = new EdgeEffectParameters
                {
                    Colour = Color4.Black.Opacity(0.04f),
                    Type = EdgeEffectType.Shadow,
                    Radius = 5
                },
                Children = new Drawable[]
                {
                    new Box
                    {
                        Colour = BackgroundColour,
                        RelativeSizeAxes = Axes.Both
                    },
                    hover = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = HoverColour,
                        Alpha = 0
                    },
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (AutoSizeAxes != Axes.None)
            {
                content.RelativeSizeAxes = (Axes.Both & ~AutoSizeAxes);
                content.AutoSizeAxes = AutoSizeAxes;
            }
            
            Enabled.BindValueChanged(enabled => this.FadeColour(enabled.NewValue ? Color4.White : GDEColour.Gray60, 200, Easing.OutQuint), true);
        }
        
        protected override bool OnHover(HoverEvent e)
        {
            hover.FadeIn(200, Easing.Out);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            hover.FadeOut(200, Easing.Out);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            hover.FlashColour(FlashColour, 200, Easing.Out);
            return base.OnClick(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Content.ScaleTo(0.9f, 200, Easing.Out);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            Content.ScaleTo(1, 200, Easing.Out);
            base.OnMouseUp(e);
        }
    }
}