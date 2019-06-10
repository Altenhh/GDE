using GDE.App.Main.Colors;
using GDE.App.Main.Levels;
using GDE.App.Main.Objects;
using GDE.App.Main.Graphics.UserInterface;
using GDEdit.Application;
using GDEdit.Application.Editor;
using GDEdit.Utilities.Objects.GeometryDash;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GDE.App.Main.Screens.Edit.Components
{
    public class EditorTools : Container
    {
        [Resolved]
        private Editor editor { get; set; }

        private Button addObject;
        private Button deleteSelectedObjects;
        private ObjectAdditionPanel panel;
        private Database database;
        private Level level => database.UserLevels[0];

        public int CurrentSelectedObjectID => panel.SelectedObjectID;
        public readonly BindableBool AbleToPlaceBlock = new BindableBool();

        [BackgroundDependencyLoader]
        private void load(DatabaseCollection databases)
        {
            database = databases[0];
        }

        public EditorTools(LevelPreview level, Camera camera)
        {
            Children = new Drawable[]
            {
                panel = new ObjectAdditionPanel(camera)
                {
                    Size = new Vector2(335, 557),
                    Position = new Vector2(DrawWidth + 10, DrawHeight / 2)
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    CornerRadius = 15,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = GDEColors.FromHex("1f1f1f"),
                        },
                    }
                },
                new FillFlowContainer()
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White,
                    Padding = new MarginPadding(15),
                    Children = new Drawable[]
                    {
                        addObject = new Button
                        {
                            Action = panel.ToggleVisibility,
                            Text = "Add Object",
                            BackgroundColour = GDEColors.FromHex("2f2f2f"),
                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(1f, 30)
                        },
                        deleteSelectedObjects = new Button
                        {
                            Action = () => 
                            {
                                //Always defaults to 0, so fix that
                                foreach (var o in ObjectBase.DrawableSelectedObjects)
                                {
                                    //Hide it for now
                                    o.Hide();
                                }

                                ObjectBase.DrawableSelectedObjects.Clear();
                                editor?.DeselectAll();
                            },
                            Text = "Delete Selected Objects",
                            BackgroundColour = GDEColors.FromHex("2f2f2f"),
                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(1f, 30)
                        }
                    }
                }
            };

            AbleToPlaceBlock.BindTo(panel.AbleToPlace);
        }
    }
}
