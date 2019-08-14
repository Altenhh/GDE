using GDE.App.Main.Colors;
using GDE.App.Main.UI;
using GDAPI.Application;
using GDAPI.Utilities.Objects.GeometryDash;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using System;
using static System.Threading.Tasks.TaskStatus;

namespace GDE.App.Main.Screens.Menu.Components
{
    public class Toolbar : Container
    {
        private bool gottenSongMetadata;
        private Database database;

        public SpriteText LevelName, SongName;

        public Bindable<Level> Level = new Bindable<Level>();

        public Action Edit;
        public Action Delete;

        public Toolbar()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = GDEColors.FromHex("161616")
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        LevelName = new SpriteText
                        {
                            Margin = new MarginPadding(5),
                            Text = "No level selected",
                            TextSize = 30
                        },
                        SongName = new SpriteText
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            Margin = new MarginPadding(5),
                            Colour = GDEColors.FromHex("666666"),
                            TextSize = 25,
                        }
                    }
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new GDEButton
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Margin = new MarginPadding(5),
                            Width = 80,
                            BackgroundColour = GDEColors.FromHex("c6262e"),
                            Text = "Delete",
                            Action = () => Delete?.Invoke()
                        },
                        new GDEButton
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Margin = new MarginPadding(5),
                            Width = 80,
                            BackgroundColour = GDEColors.FromHex("242424"),
                            Text = "Edit",
                            Action = () => Edit?.Invoke()
                        }
                    }
                }
            };

            Level.ValueChanged += OnChanged;
        }

        [BackgroundDependencyLoader]
        private void load(DatabaseCollection databases)
        {
            database = databases[0];
        }

        private void OnChanged(ValueChangedEvent<Level> value)
        {
            LevelName.Text = value.NewValue?.Name ?? "No level selected";
            gottenSongMetadata = false;
        }

        protected override void Update()
        {
            // Since song metadata display works in the same way as the level card; logic has to be shared to avoid this ugly code copy-paste
            if (!gottenSongMetadata)
                if (!(gottenSongMetadata = Level.Value == null))
                {
                    SongMetadata metadata = null;
                    if (gottenSongMetadata = database != null && database.GetSongMetadataStatus >= RanToCompletion)
                        metadata = Level.Value.GetSongMetadata(database.SongMetadataInformation);
                    SongName.Text = metadata != null ? $"{metadata.Artist} - {metadata.Title}" : "Song information unavailable";
                }
                else
                    SongName.Text = null;
        }
    }
}