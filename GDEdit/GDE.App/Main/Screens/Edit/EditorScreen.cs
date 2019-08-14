using DiscordRPC;
using GDE.App.Main.Colors;
using GDE.App.Main.Levels;
using GDE.App.Main.Objects;
using GDE.App.Main.Screens.Edit.Components;
using GDE.App.Main.Screens.Edit.Components.Menu;
using GDE.App.Main.Tools;
using GDE.App.Main.UI;
using GDE.App.Main.UI.FileDialogComponents;
using GDAPI.Application;
using GDAPI.Application.Editor;
using GDAPI.Utilities.Objects.GeometryDash;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using System.Collections.Generic;

namespace GDE.App.Main.Screens.Edit
{
    public class EditorScreen : Screen
    {
        private TextureStore texStore;
        private Sprite background;
        private int i;

        private Database database;
        private LevelPreview preview;
        private Level level => database.UserLevels[i];

        private Grid grid;
        private Camera camera;
        private EditorTools tools;

        private IDMigrationPanel IDMigrationPanel;

        public readonly Bindable<OpenFileDialog> OpenFileDialogBindable = new Bindable<OpenFileDialog>();
        public readonly Bindable<SaveFileDialog> SaveFileDialogBindable = new Bindable<SaveFileDialog>();

        public readonly Editor Editor;

        [BackgroundDependencyLoader]
        private void load(DatabaseCollection databases, TextureStore ts)
        {
            database = databases[0];

            texStore = ts;
            background.Texture = texStore.Get("Backgrounds/game_bg_01_001-uhd.png");

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            AddMenuItems();
        }

        private void AddMenuItems()
        {
            EditorMenuBar menuBar;

            var fileMenuItems = new List<MenuItem>
            {
                new EditorMenuItem("Save", Save, MenuItemType.Highlighted),
                new EditorMenuItem("Save & Exit", SaveAndExit, MenuItemType.Standard),
                new EditorMenuItemSpacer(),
                new EditorMenuItem("Exit", this.Exit, MenuItemType.Destructive),
            };
            var editMenuItems = new List<MenuItem>
            {
                // Undo, redo
            };
            var macrosMenuItems = new List<MenuItem>
            {
                new EditorMenuItem("Migrate IDs", IDMigrationPanel.ToggleVisibility, MenuItemType.Standard),
            };

            AddInternal(new Container
            {
                Name = "Top bar",
                RelativeSizeAxes = Axes.X,
                Height = 40,
                Child = menuBar = new EditorMenuBar
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    Items = new[]
                    {
                        new MenuItem("File")
                        {
                            Items = fileMenuItems
                        },
                        new MenuItem("Edit")
                        {
                            Items = editMenuItems
                        },
                        new MenuItem("Macros")
                        {
                            Items = macrosMenuItems
                        },
                    }
                }
            });
        }

        public EditorScreen(int index, Level level)
        {
            RelativeSizeAxes = Axes.Both;

            Editor = new Editor(level);
            // TODO: Inject editor into dependencies to work with the other things

            RPC.UpdatePresence(Editor.Level.Name, "Editing a level", new Assets
            {
                LargeImageKey = "gde",
                LargeImageText = "GD Edit"
            });

            i = index;

            AddRangeInternal(new Drawable[]
            {
                background = new Sprite
                {
                    Origin = Anchor.BottomLeft,
                    Anchor = Anchor.BottomLeft,
                    Depth = float.MaxValue,
                    Colour = GDEColors.FromHex("4f4f4f"),
                    Size = new Vector2(2048, 2048)
                },
                camera = new Camera(Editor)
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        grid = new Grid
                        {
                            Size = new Vector2(1.1f), //Doubles the size
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre
                        },
                        preview = new LevelPreview(this, index)
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre
                        },
                    }
                },
                tools = new EditorTools(preview, camera)
                {
                    Size = new Vector2(150, 300),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft
                },
                OpenFileDialogBindable.Value = new OpenFileDialog
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.8f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Depth = -10
                },
                SaveFileDialogBindable.Value = new SaveFileDialog
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.8f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Depth = -10
                },
                IDMigrationPanel = new IDMigrationPanel(this)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    LockDrag = true,
                },
            });
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (tools.AbleToPlaceBlock.Value)
            {
                var cloned = camera.GetClonedGhostObjectLevelObject();
                Editor.AddObject(cloned);
                preview.Add(new ObjectBase(cloned));
                return true;
            }
            else
                return false;
        }

        protected override bool OnDrag(DragEvent e)
        {
            foreach (var child in camera.Children)
            {
                child.Position += e.Delta;
            }

            return base.OnDrag(e);
        }

        private void SaveAndExit()
        {
            Save();
            this.Exit();
        }
        private void Save() => Editor.Save(database, i);
    }
}
