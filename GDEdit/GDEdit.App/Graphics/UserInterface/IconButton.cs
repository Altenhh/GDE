using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace GDEdit.App.Graphics.UserInterface
{
    public class IconButton : GDEAnimatedButton
    {
        public const float DEFAULT_BUTTON_SIZE = 30;

        private readonly SpriteIcon icon;

        private Color4? iconColour;

        private Color4? iconHoverColour;

        public IconButton()
        {
            Size = new Vector2(DEFAULT_BUTTON_SIZE);

            Add(icon = new SpriteIcon
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Size = new Vector2(18)
            });
        }

        /// <summary>The icon colour. This does not affect <see cref="Drawable.Colour">Colour</see>.</summary>
        public Color4 IconColour
        {
            get => iconColour ?? Color4.White;
            set
            {
                iconColour = value;
                icon.Colour = value;
            }
        }

        /// <summary>The icon colour while the <see cref="IconButton" /> is hovered.</summary>
        public Color4 IconHoverColour
        {
            get => iconHoverColour ?? IconColour;
            set => iconHoverColour = value;
        }

        /// <summary>The icon.</summary>
        public IconUsage Icon
        {
            get => icon.Icon;
            set => icon.Icon = value;
        }

        /// <summary>The icon scale. This does not affect <see cref="Drawable.Scale">Scale</see>.</summary>
        public Vector2 IconScale
        {
            get => icon.Scale;
            set => icon.Scale = value;
        }

        protected override bool OnHover(HoverEvent e)
        {
            icon.FadeColour(IconHoverColour, 500, Easing.OutQuint);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            icon.FadeColour(IconColour, 500, Easing.OutQuint);
            base.OnHoverLost(e);
        }
    }
}