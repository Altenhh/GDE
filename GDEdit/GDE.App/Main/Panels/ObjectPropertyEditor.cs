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

namespace GDE.App.Main.Panels
{
    public class ObjectPropertyEditor : Panel
    {
        private Container content;
        private PropertyEditorHeader header;
        //private PropertyEditorTabControl tabControl;

        protected override string Name => "Object Property Editor";

        public Bindable<GeneralObject> ObjectBindable;

        public ObjectPropertyEditor(GeneralObject Object)
        {
            AutoSizeAxes = Axes.Both;
            ObjectBindable = new Bindable<GeneralObject>(Object);

            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    AutoSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        //tabControl = new PropertyEditorTabControl(),
                        header = new PropertyEditorHeader(),
                        content = new Container()
                    }
                }
            };
        }
    }
}
