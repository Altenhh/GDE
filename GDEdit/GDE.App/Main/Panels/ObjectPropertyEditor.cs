using GDAPI.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Colors;
using GDE.App.Main.Panels.Object;
using GDE.App.Main.Panels.Object.Content;
using GDE.App.Main.Panels.Object.Tabs;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GDE.App.Main.Panels
{
    public class ObjectPropertyEditor : Panel
    {
        private CompositeDrawable content;
        private PropertyEditorHeader header;
        private PropertyEditorFooter footer;
        private PropertyEditorTabControl tabControl;
        private FillFlowContainer fillFlow;

        protected override string Name => "Object Property Editor";

        public Bindable<LevelObjectCollection> SelectedObjects;

        public ObjectPropertyEditor(LevelObjectCollection objects)
        {
            AutoSizeAxes = Axes.Both;
            SelectedObjects = new Bindable<LevelObjectCollection>(objects);

            SelectedObjects.ValueChanged += o =>
            {
                Children = new Drawable[]
                {
                    new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Padding = new MarginPadding
                        {
                            Top = 30,
                        },
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = GDEColors.FromHex("202020")
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
                                    fillFlow = new FillFlowContainer
                                    {
                                        Direction = FillDirection.Vertical,
                                        AutoSizeAxes = Axes.Both,
                                        Padding = new MarginPadding
                                        {
                                            Vertical = 5,
                                            Horizontal = 10
                                        },
                                    }
                                }
                            },
                        }
                    }
                };

                fillFlow.Children = new Drawable[]
                {
                    header = new PropertyEditorHeader
                    {
                        Objects = SelectedObjects
                    },
                    content = new ObjectContent(SelectedObjects.Value),
                    footer = new PropertyEditorFooter
                    {
                        Objects = SelectedObjects
                    },
                };
            };

            SelectedObjects.TriggerChange();
        }
        protected override void LoadComplete()
        {
            base.LoadComplete();

            var list = new List<PropertyEditorTab>()
            {
                //TODO: Import custom icons to use
                new PropertyEditorTab
                {
                    Icon = FontAwesome.Regular.Square,
                    Tab = TabEnumeration.General
                }
            };

            foreach (var item in list)
                tabControl.AddItem(item);

            tabControl.Current.Value = list.FirstOrDefault();
            tabControl.Current.ValueChanged += value =>
            {
                Console.WriteLine(value.NewValue.Tab.ToString());
                switch (value.NewValue.Tab)
                {
                    case TabEnumeration.General:
                        fillFlow.Children = new Drawable[]
                        {
                            header = new PropertyEditorHeader(SelectedObjects),
                            content = new ObjectContent(SelectedObjects.Value),
                            footer = new PropertyEditorFooter(SelectedObjects),
                        };
                        break;
                }
            };
        }
    }
}
