using GDEdit.App.Graphics.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;

namespace GDEdit.App.Overlays
{
    public class LevelOverlay : FocusedOverlayContainer
    {
        public LevelOverlay(GDAPI.Objects.GeometryDash.General.Level level)
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Width = 0.85f;
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
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            Child = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    #region Header
                    new Container
                    {
                        Name = "Header",
                        RelativeSizeAxes = Axes.X,
                        Height = 125,
                        Masking = true,
                        MaskingSmoothness = 1,
                        Children = new Drawable[]
                        {
                            new Sprite
                            {
                                RelativeSizeAxes = Axes.Both,
                                FillMode = FillMode.Fill,
                                Texture = store.Get("https://pbs.twimg.com/media/ENFcYM_UYAY5yV5?format=jpg&name=orig"),
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Padding = new MarginPadding
                                {
                                    Vertical = 30,
                                    Horizontal = 70
                                },
                                Children = new Drawable[]
                                {
                                    new IconButton
                                    {
                                        Icon = FontAwesome.Solid.Times,
                                        IconScale = new Vector2(0.5f),
                                        Action = ToggleVisibility,
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.Centre,
                                    }
                                }
                            }
                        }
                    },
                    #endregion
                }
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