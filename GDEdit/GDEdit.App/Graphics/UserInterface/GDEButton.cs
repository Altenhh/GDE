using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace GDEdit.App.Graphics.UserInterface
{
    public class GDEButton : Button
    {
        public string Text
        {
            get => SpriteText?.Text;
            set
            {
                if (SpriteText != null)
                    SpriteText.Text = value;
            }
        }

        private Color4? backgroundColour;

        public Color4 BackgroundColour
        {
            set
            {
                backgroundColour = value;
                Background.FadeColour(value);
            }
        }
        
        protected override Container<Drawable> Content { get; }

        protected Box Hover;
        protected Box Background;
        protected SpriteText SpriteText;

        public GDEButton()
        {
            Height = 40;
            
            AddInternal(Content = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Masking = true,
                CornerRadius = 5,
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    Background = new Box
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                    },
                    Hover = new Box
                    {
                        Alpha = 0,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.White.Opacity(.1f),
                        Blending = BlendingParameters.Additive,
                        Depth = float.MinValue
                    },
                    SpriteText = CreateText(),
                }
            });
            
            Enabled.BindValueChanged(enabledChanged, true);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (backgroundColour == null)
                backgroundColour = GDEColour.Gray15;

            Enabled.ValueChanged += enabledChanged;
            Enabled.TriggerChange();
        }
        
        protected override bool OnClick(ClickEvent e)
        {
            if (Enabled.Value)
            {
                Debug.Assert(backgroundColour != null);
                Background.FlashColour(backgroundColour.Value, 200);
            }

            return base.OnClick(e);
        }
        
        protected override bool OnHover(HoverEvent e)
        {
            if (Enabled.Value)
                Hover.FadeIn(100, Easing.Out);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            Hover.FadeOut(100);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Content.ResizeHeightTo(0.9f, 100, Easing.Out);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            Content.ResizeHeightTo(1, 100, Easing.Out);
            base.OnMouseUp(e);
        }

        protected virtual SpriteText CreateText() => new SpriteText
        {
            Depth = -1,
            Shadow = true,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Font = new FontUsage("Torus", 18, "SemiBold")
        };
        
        private void enabledChanged(ValueChangedEvent<bool> e)
        {
            this.FadeColour(e.NewValue ? Color4.White : Color4.Gray, 200, Easing.OutQuint);
        }
    }
}