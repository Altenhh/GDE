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

namespace GDE.App.Main.Overlays.ObjectEditor
{
    public class ObjEditor : GDEFocusedOverlayContainer
    {
        private ObjectBase drawableObject;
        private GeneralObject generalObject;

        private int i;

        private Database database;

        private Level level => database.UserLevels[i];

        public override bool BlockScreenWideMouse => false;

        public ObjEditor(GeneralObject obj, int i)
        {
            generalObject = obj;
            this.i = i;
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
                    Text = "Object Editor",
                    Padding = new MarginPadding
                    {
                        Top = 10,
                        Left = 50
                    }
                },
                new Container
                {
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
                }
            });
        }
    }
}
