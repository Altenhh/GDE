using GDAPI.Objects.GeometryDash.General;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

//wtf is this bullshittery.
using lvl = GDAPI.Objects.GeometryDash.General.Level;

namespace GDEdit.App.Screens.Menu
{
    public class LevelCard : CompositeDrawable
    {
        public lvl Level { get; }

        public readonly Bindable<CarouselItemState> State =
            new Bindable<CarouselItemState>(CarouselItemState.NotSelected);

        public bool Visible => State.Value != CarouselItemState.Collapsed;

        private Box background;
        private Box accent;

        public LevelCard(lvl lvlpleasekthx)
        {
            Level = lvlpleasekthx;
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Masking = true;
            CornerRadius = 4;

            BorderColour = Color4Extensions.FromHex("5182A7");
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Colour = Color4.Black.Opacity(0.25f),
                Radius = 15,
                Roundness = 4,
                Offset = new Vector2(0, 4)
            };

            TextFlowContainer description;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    Colour = Color4Extensions.FromHex("262626"),
                    RelativeSizeAxes = Axes.Both
                },
                accent = new Box
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 5,
                    Colour = Color4Extensions.FromHex("404040")
                },
                new FillFlowContainer
                {
                    Padding = new MarginPadding(10),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 8),
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Font = new FontUsage("Roboto", 18, "Bold"),
                            Text = Level.Name,
                            RelativeSizeAxes = Axes.X,
                            Colour = Color4Extensions.FromHex("E5E5E5")
                        },
                        description = new TextFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Colour = Color4Extensions.FromHex("E5E5E5")
                        },
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Spacing = new Vector2(8, 0),
                            Children = new Drawable[]
                            {
                                new SpriteIcon
                                {
                                    Size = new Vector2(15),
                                    Icon = FontAwesome.Solid.Music,
                                    Colour = Color4Extensions.FromHex("4182A7")
                                },
                                new SpriteText
                                {
                                    Text = Level.GetSongMetadata(new SongMetadataCollection()).Artist + " - " + Level.GetSongMetadata(new SongMetadataCollection()).Title,
                                    Colour = Color4Extensions.FromHex("4182A7"),
                                    Font = new FontUsage(size: 14),
                                }
                            }
                        }
                    }
                }
            };

            description.AddText(Level.Description, t => { t.Font = new FontUsage(size: 14); });

            State.ValueChanged += updateState;
        }

        private void updateState(ValueChangedEvent<CarouselItemState> state)
        {
            if (state.NewValue == CarouselItemState.Selected)
            {
                accent.FadeColour(Color4Extensions.FromHex("4182A7"), 100);
                BorderThickness = 3;
            }
            else
            {
                accent.FadeColour(Color4Extensions.FromHex("404040"), 100);
                BorderThickness = 0;
            }
        }

        protected override bool OnHover(HoverEvent e)
        {
            background.FadeColour(Color4Extensions.FromHex("333"), 100);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            background.FadeColour(Color4Extensions.FromHex("262626"), 100);
            base.OnHoverLost(e);
        }
    }
    
    public enum CarouselItemState
    {
        Collapsed,
        NotSelected,
        Selected,
    }
}