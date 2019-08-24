using GDE.App.Main.Colors;
using GDE.App.Main.Panels.Tabs;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GDE.App.Main.Panels
{
    public class PropertyEditorTabControl : TabControl<IPETab>
    {
        //We dont want to handle any dropdown.
        protected override Dropdown<IPETab> CreateDropdown() => null;

        protected override TabItem<IPETab> CreateTabItem(IPETab value) => new PETabItem(value);

        public PropertyEditorTabControl()
        {
            TabContainer.Masking = false;
            TabContainer.Spacing = new Vector2(0, 25);
            
            //Background
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Y,
                Width = 30,
                Colour = GDEColors.FromHex("2B2B2B")
            });
        }

        private class PETabItem : TabItem<IPETab>
        {
            private readonly SpriteIcon icon;
            private readonly Container container;
            private readonly Box background;

            public PETabItem(IPETab value) 
                : base(value)
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;
                
                Children = new Drawable[]
                {
                    container = new Container()
                    {
                        Padding = new MarginPadding(10),
                        X = 10,
                        Masking = true,
                        CornerRadius = 5,
                        EdgeEffect = new EdgeEffectParameters
                        {
                          Radius  = 1,
                          Colour = GDEColors.FromHex("1F1F1F"),
                          Type = EdgeEffectType.Shadow,
                          Offset = new Vector2(0, 1)
                        },
                        Children = new Drawable[]
                        {
                            background = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = GDEColors.FromHex("333")
                            },
                            icon = new SpriteIcon
                            {
                                Icon = Value.Icon,
                                Size = new Vector2(15),
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre
                            }
                        }
                    }
                };
            }


            protected override void OnActivated()
            {
                container.MoveToX(5, 500, Easing.OutExpo);
                container.TweenEdgeEffectTo(new EdgeEffectParameters
                {
                    Radius = 2,
                    Colour = GDEColors.FromHex("1F1F1F"),
                    Type = EdgeEffectType.Shadow,
                    Offset = new Vector2(0, 2)
                }, 500, Easing.OutExpo);

                background.FadeColour(GDEColors.FromHex("3D3D3D"), 500, Easing.OutExpo);
                icon.ResizeTo(25, 500, Easing.OutExpo);
            }

            protected override void OnDeactivated()
            {
                container.MoveToX(10, 1000, Easing.InOutSine);
                container.TweenEdgeEffectTo(new EdgeEffectParameters
                {
                    Radius = 1,
                    Colour = GDEColors.FromHex("1F1F1F"),
                    Type = EdgeEffectType.Shadow,
                    Offset = new Vector2(0, 1)
                }, 1000, Easing.InOutSine);

                background.FadeColour(GDEColors.FromHex("333"), 1000, Easing.InOutSine);
                icon.ResizeTo(15, 1000, Easing.InOutSine);
            }
        }
    }
}