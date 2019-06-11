//abnormally huge usings yes
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.MathUtils;
using System.Linq;
using osu.Framework;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Input.Bindings;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Threading;
using osuTK.Input;
using GDE.App.Main.Levels;
using GDE.App.Main.Graphics.Containers;
using GDE.App.Main.Graphics.UserInterface;
using GDE.App.Main.Objects;
using GDE.App.Main.Panels;
using GDE.App.Main.Colors;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;
using GDEdit.Utilities.Objects.GeometryDash;
using GDEdit.Application;
using GDE.App.Main.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;

namespace GDE.App.Main.Overlays.ObjectEditor
{
    public class ObjectEditor : FocusedOverlayContainer
    {
        private ObjectBase drawableObject;
        private GeneralObject generalObject;
        private Database database;
        private Level level => database.UserLevels[i];
        private int i;

        // Bindables
        private BindableBool deltaMode = new BindableBool();
        private BindableBool flippedH = new BindableBool();
        private BindableBool flippedV = new BindableBool();
        private BindableBool glow = new BindableBool();
        private BindableBool dontFade = new BindableBool();
        private BindableBool dontEnter = new BindableBool();
        private BindableBool groupParent = new BindableBool();
        private BindableBool highDetail = new BindableBool();

        //public override bool BlockScreenWideMouse => true;

        public ObjectEditor(GeneralObject obj, int i)
        {
            generalObject = obj;
            this.i = i;

            //Bindables
            flippedH.Value = obj.FlippedHorizontally;
            flippedV.Value = obj.FlippedVertically;
            glow.Value = !obj.DisableGlow;
            dontFade.Value = obj.DontFade;
            dontEnter.Value = obj.DontEnter;
            groupParent.Value = obj.GroupParent;
            highDetail.Value = obj.HighDetail;
        }

        [BackgroundDependencyLoader]
        private void load(DatabaseCollection databases)
        {
            database = databases[0];

            AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = GDEColors.FromHex("202020")
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 40,
                    Colour = GDEColors.FromHex("2B2B2B")
                },
                new SpriteText
                {
                    Font = GDEFont.Default,
                    Text = "Object Editor",
                    Padding = new MarginPadding
                    {
                        Top = 10,
                        Left = 50
                    }
                },
                new DrawSizePreservingFillContainer
                {
                    Strategy = DrawSizePreservationStrategy.Minimum,
                    Name = "Content",
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding
                    {
                        Left = 50,
                        Right = 10,
                        Top = 60,
                        Bottom = 10
                    },
                    Children = new Drawable[]
                    {
                        new EditableCheckboxFlipped
                        {
                            LabelText = "Delta Mode",
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Bindable = deltaMode,
                        },
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 50,
                            Direction = FillDirection.Horizontal,
                            Name = "Top Row",
                            Children = new Drawable[]
                            {
                                drawableObject = new ObjectBase(generalObject)
                                {
                                    Size = new Vector2(50),
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    // Resets values so theres no problems in design
                                    Position = new Vector2(0),
                                    Scale = new Vector2(1)
                                }
                            }
                        },
                        new FillFlowContainer
                        {
                            Name = "Bottom Row",
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(10, 0),
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Children = new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 10),
                                    Anchor = Anchor.BottomLeft,
                                    Origin = Anchor.BottomLeft,
                                    Children = new Drawable[]
                                    {
                                        new EditableCheckbox
                                        {
                                            LabelText = "Flipped Horizontally",
                                            Bindable = flippedH
                                        },
                                        new EditableCheckbox
                                        {
                                            LabelText = "Flipped Vertically",
                                            Bindable = flippedV
                                        },
                                        new EditableCheckbox
                                        {
                                            LabelText = "Glow",
                                            Bindable = glow
                                        }
                                    }
                                },
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 10),
                                    Anchor = Anchor.BottomLeft,
                                    Origin = Anchor.BottomLeft,
                                    Children = new Drawable[]
                                    {
                                        new EditableCheckbox
                                        {
                                            LabelText = "Don't Fade",
                                            Bindable = dontFade
                                        },
                                        new EditableCheckbox
                                        {
                                            LabelText = "Don't Enter",
                                            Bindable = dontEnter
                                        },
                                        new EditableCheckbox
                                        {
                                            LabelText = "Group Parent",
                                            Bindable = groupParent
                                        }
                                    }
                                },
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 10),
                                    Anchor = Anchor.BottomLeft,
                                    Origin = Anchor.BottomLeft,
                                    Children = new Drawable[]
                                    {
                                        new EditableCheckbox
                                        {
                                            LabelText = "High Detail",
                                            Bindable = highDetail
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
