using System;
using GDAPI.Application;
using GDAPI.Objects.GeometryDash.General;
using GDEdit.App.Graphics;
using GDEdit.App.Graphics.UserInterface;
using GDEdit.App.Overlays.Level;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace GDEdit.App.Overlays
{
    public class LevelOverlay : FocusedOverlayContainer
    {
        private readonly Database database;
        private readonly GDAPI.Objects.GeometryDash.General.Level level;
        private readonly SongMetadata songMetadata;

        public LevelOverlay(GDAPI.Objects.GeometryDash.General.Level level, Database database)
        {
            this.level = level;
            this.database = database;
            level.InitializeLoadingLevelString();

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

            songMetadata = level.GetSongMetadata(database.SongMetadataInformation);
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            TextFlowContainer songTextFlow;
            OnClickTextFlow descriptionTextFlow;
            FillFlowContainer descriptionContainer;

            // TODO: Split this out onto multiple classes.
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
                                Origin = Anchor.Centre
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
                                        Origin = Anchor.Centre
                                    }
                                }
                            }
                        }
                    },
                    #endregion

                    #region Title
                    new Container
                    {
                        Name = "Title",
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Masking = true,
                        MaskingSmoothness = 1,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                Colour = GDEColour.Gray15,
                                RelativeSizeAxes = Axes.Both
                            },
                            new Box
                            {
                                Colour = Color4.FromHsl(new Vector4(292 / 360f, 1, 0.7f, 1)),
                                RelativeSizeAxes = Axes.X,
                                Height = 3,
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre
                            },
                            new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Vertical,
                                Padding = new MarginPadding
                                {
                                    Horizontal = 70,
                                    Vertical = 20
                                },
                                Children = new Drawable[]
                                {
                                    new SpriteTextBox
                                    {
                                        Text = level.Name,
                                        Font = new FontUsage("Torus", 24, "SemiBold"),
                                        Height = 34,
                                        RelativeSizeAxes = Axes.X,
                                        OnCommit = ChangeTitle
                                    },
                                    songTextFlow = new TextFlowContainer
                                    {
                                        // a bit of a cheat here, but do i care? no.
                                        RelativeSizeAxes = Axes.X,
                                        Height = 20,
                                        TextAnchor = Anchor.BottomLeft
                                    }
                                }
                            }
                        }
                    },
                    #endregion

                    #region Components
                    new Container
                    {
                        Name = "Components",
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                Colour = GDEColour.Gray10,
                                RelativeSizeAxes = Axes.Both
                            },
                            new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Vertical,
                                Spacing = new Vector2(0, 10),
                                Padding = new MarginPadding
                                {
                                    Horizontal = 70,
                                    Top = 20,
                                    Bottom = 10
                                },
                                Children = new Drawable[]
                                {
                                    new FillFlowContainer
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Direction = FillDirection.Horizontal,
                                        Spacing = new Vector2(20, 0),
                                        Children = new Drawable[]
                                        {
                                            new LevelPill(FontAwesome.Solid.Star, "0.00")
                                            {
                                                BorderColour = Color4.FromHsl(new Vector4(292 / 360f, 1, 0.7f, 1)),
                                                BorderThickness = 2
                                            },
                                            new LevelPill(FontAwesome.Solid.Clock, level.TimeLength.ToString(@"m\:ss"))
                                        }
                                    },
                                    new ObjectDensity(level.LevelObjects)
                                    {
                                        LowColour = Color4Extensions.FromHex(@"1F8EAD"),
                                        MidColour = Color4Extensions.FromHex(@"52B1E0"),
                                        HighColour = Color4Extensions.FromHex(@"66CCFF"),
                                        DefColour = Color4Extensions.FromHex(@"2E3538")
                                    },
                                    descriptionContainer = new FillFlowContainer
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Direction = FillDirection.Vertical,
                                        Spacing = new Vector2(0, 5),
                                        Children = new Drawable[]
                                        {
                                            new SpriteText
                                            {
                                                Text = "Description",
                                                Font = new FontUsage("Torus", 18, "SemiBold")
                                            },
                                            descriptionTextFlow = new OnClickTextFlow
                                            {
                                                RelativeSizeAxes = Axes.X,
                                                AutoSizeAxes = Axes.Y
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    #endregion

                    #region Buttons
                    new Container
                    {
                        Name = "Action Buttons",
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                Colour = GDEColour.Gray15,
                                RelativeSizeAxes = Axes.Both
                            },
                            new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Spacing = new Vector2(10, 0),
                                Padding = new MarginPadding
                                {
                                    Horizontal = 70,
                                    Vertical = 10
                                },
                                Children = new Drawable[]
                                {
                                    new GDEButton
                                    {
                                        Text = "Delete Level",
                                        BackgroundColour = Color4.FromHsl(new Vector4(0, 1, 0.65f, 1)),
                                        Anchor = Anchor.BottomRight,
                                        Origin = Anchor.BottomRight,
                                        Width = 128.5f,
                                        Height = 30,
                                        Enabled = { Value = true }
                                    },
                                    new GDEButton
                                    {
                                        Text = "Edit Level",
                                        BackgroundColour = Color4.FromHsl(new Vector4(292 / 360f, 1, 0.65f, 1)),
                                        Anchor = Anchor.BottomRight,
                                        Origin = Anchor.BottomRight,
                                        Width = 114,
                                        Height = 30,
                                        Enabled = { Value = true }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            };

            void ChangeTitle(TextBox textBox, bool hasNewText)
            {
                level.Name = textBox.Text;
                database.WriteLevelData();
            }

            songTextFlow.AddText(songMetadata.Title, t =>
            {
                t.Font = new FontUsage("Torus", 18, "SemiBold");
                t.Colour = GDEColour.Gray80;
            });

            songTextFlow.AddText(" by " + songMetadata.Artist, t =>
            {
                t.Font = new FontUsage("Torus", 14, "SemiBold");
                t.Colour = GDEColour.Gray70;
            });

            if (string.IsNullOrEmpty(level.Description))
            {
                descriptionTextFlow.AddText("This level does not have a description set. ",
                                            t => { t.Font = new FontUsage(size: 14); });

                descriptionTextFlow.AddText("Click here ", t => { t.Font = new FontUsage("Torus", 14, "SemiBold"); });
                descriptionTextFlow.AddText("to add a description!", t => { t.Font = new FontUsage(size: 14); });
            }
            else
            {
                descriptionTextFlow.AddText(level.Description, t => { t.Font = new FontUsage(size: 14); });
            }

            descriptionTextFlow.OnClickEvent += e =>
            {
                descriptionTextFlow.Expire();

                SpriteTextBox textBox;

                descriptionContainer.Add(textBox = new SpriteTextBox
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 18,
                    Text = level.Description
                });

                textBox.OnCommit += (text, hasNewText) =>
                {
                    level.Description = text.Text;
                    database.WriteLevelData();
                };
            };
        }

        public override void Show()
        {
            if (State.Value == Visibility.Visible)

                // re-trigger the state changed so we can potentially surface to front
                State.TriggerChange();
            else
                base.Show();
        }

        protected override void PopIn()
        {
            base.PopIn();

            FadeEdgeEffectTo(0.4f, 100, Easing.Out);
            this.FadeIn(100, Easing.Out);
            this.ScaleTo(1, 100, Easing.Out);
        }

        protected override void PopOut()
        {
            base.PopOut();

            FadeEdgeEffectTo(0f, 100, Easing.In);
            this.FadeOut(100, Easing.In);
            this.ScaleTo(0.99f, 100, Easing.InQuint);
        }

        public class OnClickTextFlow : TextFlowContainer
        {
            public Action<ClickEvent> OnClickEvent;

            protected override bool OnClick(ClickEvent e)
            {
                OnClickEvent.Invoke(e);

                return base.OnClick(e);
            }
        }
    }
}