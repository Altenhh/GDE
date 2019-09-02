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
using GDE.App.Main.UI.Containers;
using GDE.App.Main.UI;
using GDE.App.Main.Objects;
using GDE.App.Main.Panels;
using GDE.App.Main.Colors;
using GDAPI.Utilities.Objects.GeometryDash.LevelObjects;
using GDAPI.Utilities.Objects.GeometryDash;
using GDAPI.Application;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;
using GDE.App.Main.Panels.Object;
using GDE.App.Main.Panels.Object.Tabs;

namespace GDE.App.Main.Panels
{
    public class ObjectPropertyEditor : Panel
    {
        private Container content;
        private PropertyEditorHeader header;
        private PropertyEditorFooter footer;
        private PropertyEditorTabControl tabControl;

        protected override string Name => "Object Property Editor";

        public Bindable<GeneralObject> ObjectBindable;

        public ObjectPropertyEditor(GeneralObject Object)
        {
            AutoSizeAxes = Axes.Both;
            ObjectBindable = new Bindable<GeneralObject>(Object);

            Add(new Container
            {
                AutoSizeAxes = Axes.Both,
                Padding = new MarginPadding
                {
                    Top = 30
                },
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = GDEColors.FromHex("151515")
                    },
                    new FillFlowContainer
                    {
                        Direction = FillDirection.Horizontal,
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            tabControl = new PropertyEditorTabControl
                            {
                                RelativeSizeAxes = Axes.Y,
                                Width = 30,
                                AutoSort = true,
                            },
                            new FillFlowContainer
                            {
                                Direction = FillDirection.Vertical,
                                AutoSizeAxes = Axes.Both,
                                Padding = new MarginPadding(5),
                                Children = new Drawable[]
                                {
                                    header = new PropertyEditorHeader
                                    {
                                        Object = ObjectBindable
                                    },
                                    content = new Container(),
                                    footer = new PropertyEditorFooter
                                    {
                                        Object = ObjectBindable
                                    },
                                }
                            }
                        }
                    },
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            
            var list = new List<PropertyEditorTab>()
            {
                //TODO: Import custom icons to use
                new PropertyEditorTab
                {
                    Icon = FontAwesome.Regular.Square
                },
                new PropertyEditorTab
                {
                    Icon = FontAwesome.Solid.Star
                }
            };
            
            foreach (var item in list)
                tabControl.AddItem(item);

            tabControl.Current.Value = list.FirstOrDefault();
        }
    }
}
