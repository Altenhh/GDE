using GDEdit.App.Graphics;
using GDEdit.App.Graphics.UserInterface;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace GDEdit.App.Overlays.Level
{
    public class LevelPill : Container
    {
        /// <summary>
        /// The colour used for the background.
        /// </summary>
        public Color4 BackgroundColour = GDEColour.Gray20;
        
        private Color4? iconColour;

        /// <summary>
        /// The icon colour. This does not affect <see cref="Drawable.Colour">Colour</see>.
        /// </summary>
        public Color4 IconColour
        {
            get => iconColour ?? Color4.White;
            set
            {
                iconColour = value;
                icon.Colour = value;
            }
        }

        /// <summary>
        /// The icon scale. This does not affect <see cref="Drawable.Scale">Scale</see>.
        /// </summary>
        public Vector2 IconScale
        {
            get => icon.Scale;
            set => icon.Scale = value;
        }

        private readonly SpriteIcon icon;

        public LevelPill(IconUsage icon, string text)
        {
            AutoSizeAxes = Axes.Both;

            CornerRadius = 3;
            Masking = true;
            
            Children = new Drawable[]
            {
                new Box
                {
                    Colour = BackgroundColour,
                    RelativeSizeAxes = Axes.Both
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(3, 0),
                    Padding = new MarginPadding
                    {
                        Vertical = 1,
                        Horizontal = 5
                    },
                    Children = new Drawable[]
                    {
                        new SpriteIcon
                        {
                            Size = new Vector2(8),
                            Origin = Anchor.CentreLeft,
                            Anchor = Anchor.CentreLeft,
                            Icon = icon
                        },
                        new SpriteText
                        {
                            Text = text,
                            Font = new FontUsage(size: 14)
                        }
                    }
                }
            };
        }
    }
}