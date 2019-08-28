using GDE.App.Main.Colors;
using GDE.App.Main.Panels.Object.Tabs;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace GDE.App.Main.Panels.Object
{
    public class PropertyEditorTabControl : TabControl<PropertyEditorTab>
    {
        //We dont want to handle any dropdown.
        protected override Dropdown<PropertyEditorTab> CreateDropdown() => null;

        protected override TabItem<PropertyEditorTab> CreateTabItem(PropertyEditorTab value) => new PETabItem(value);

        public PropertyEditorTabControl()
        {
            TabContainer.Spacing = new Vector2(0, 10);
            TabContainer.Direction = FillDirection.Vertical;
            TabContainer.AllowMultiline = true;
            
            //Background
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Y,
                Width = 30,
                Colour = GDEColors.FromHex("2B2B2B")
            });
        }

        private class PETabItem : TabItem<PropertyEditorTab>
        {
            private readonly SpriteIcon icon;
            private readonly Container container;
            private readonly Box background;

            public PETabItem(PropertyEditorTab value) 
                : base(value)
            {
                AutoSizeAxes = Axes.Both;
                AlwaysPresent = true;
                
                Children = new Drawable[]
                {
                    container = new Container()
                    {
                        AutoSizeAxes = Axes.Both,
                        X = 10,
                        Masking = true,
                        CornerRadius = 5,
                        EdgeEffect = new EdgeEffectParameters
                        {
                          Radius  = 1,
                          Colour = GDEColors.FromHex("1F1F1F"),
                          Type = EdgeEffectType.Shadow,
                          Hollow = true,
                          Offset = new Vector2(0, 1)
                        },
                        Children = new Drawable[]
                        {
                            background = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Size = new Vector2(1.5f, 1),
                                Colour = GDEColors.FromHex("333"),
                            },
                            new Container
                            {
                                AutoSizeAxes = Axes.Both,
                                Padding = new MarginPadding(5),
                                Children = new Drawable[]
                                {
                                    icon = new SpriteIcon
                                    {
                                        Icon = Value.Icon,
                                        Size = new Vector2(15),
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft
                                    }
                                }
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
                container.MoveToX(10, 500, Easing.OutExpo);
                container.TweenEdgeEffectTo(new EdgeEffectParameters
                {
                    Radius = 1,
                    Colour = GDEColors.FromHex("1F1F1F"),
                    Type = EdgeEffectType.Shadow,
                    Offset = new Vector2(0, 1)
                }, 500, Easing.OutExpo);

                background.FadeColour(GDEColors.FromHex("333"), 500, Easing.OutExpo);
                icon.ResizeTo(15, 500, Easing.OutExpo);
            }
        }
    }
}