using GDAPI.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Colors;
using GDE.App.Main.Panels.Object;
using GDE.App.Main.Panels.Object.Content;
using GDE.App.Main.Panels.Object.Content.PropertyEditorTabContents;
using GDE.App.Main.Panels.Object.Tabs;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osuTK;

namespace GDE.App.Main.Panels
{
    public class ObjectPropertyEditor : Panel
    {
        private PropertyEditorTabContent content;
        private PropertyEditorHeader header;
        private PropertyEditorFooter footer;
        private PropertyEditorTabControl tabControl;
        private TestFillFlowContainer fillFlow;

        private BindableBool deltaModeBindable;

        protected override string Name => "Object Property Editor";

        public Bindable<LevelObjectCollection> SelectedObjects;

        public ObjectPropertyEditor(LevelObjectCollection objects)
        {
            AutoSizeAxes = Axes.Both;
            SelectedObjects = new Bindable<LevelObjectCollection>(objects);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
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
                                    fillFlow = new TestFillFlowContainer
                                    {
                                        Direction = FillDirection.Vertical,
                                        AutoSizeAxes = Axes.Both,
                                        Padding = new MarginPadding
                                        {
                                            Vertical = 5,
                                            Horizontal = 10
                                        },
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                Size = new Vector2(20, 50),
                                            },
                                            header = new PropertyEditorHeader(SelectedObjects),
                                            content = new PropertyEditorGeneralTabContent(SelectedObjects.Value, deltaModeBindable),
                                            footer = new PropertyEditorFooter(SelectedObjects),
                                        }
                                    }
                                }
                            },
                        }
                    }
                };
            };

            SelectedObjects.TriggerChange();
        }

        private class TestFillFlowContainer : FillFlowContainer
        {
            public override void Add(Drawable drawable)
            {
                base.Add(drawable);
                Console.WriteLine($"Added a drawable ({nameof(drawable)})");
            }
        }
        
        protected override void LoadComplete()
        {
            base.LoadComplete();

            fillFlow.Add(new Box
            {
                Size = new Vector2(20, 50)
            });
            
            fillFlow.AddRange(new Drawable[]
            {
                header = new PropertyEditorHeader(SelectedObjects),
                content = new PropertyEditorGeneralTabContent(SelectedObjects.Value, deltaModeBindable),
                footer = new PropertyEditorFooter(SelectedObjects),
            });

            var list = new List<PropertyEditorTab>
            {
                //TODO: Import custom icons to use
                new PropertyEditorTab
                {
                    Icon = FontAwesome.Regular.Square,
                    Tab = PropertyEditorTabType.General
                }
            };

            foreach (var item in list)
                tabControl.AddItem(item);

            tabControl.Current.Value = list.FirstOrDefault();
            tabControl.Current.ValueChanged += value =>
            {
                Console.WriteLine(value.NewValue.Tab.ToString());
                //fillFlow.WithChildren(GetContent(value.NewValue.Tab));
            };
            
            tabControl.Current.TriggerChange();

            deltaModeBindable = new BindableBool { BindTarget = header.DeltaMode };
        }

        private Drawable[] GetContent(PropertyEditorTabType type)
        {
            switch (type)
            {
                case PropertyEditorTabType.Other:
                    return new Drawable[]
                    {
                        // ...
                    };
                case PropertyEditorTabType.Special:
                    return new Drawable[]
                    {
                        // ...
                    };
                case PropertyEditorTabType.General:
                default:
                    return new Drawable[]
                    {
                         header = new PropertyEditorHeader(SelectedObjects),
                         content = new PropertyEditorGeneralTabContent(SelectedObjects.Value, deltaModeBindable),
                         footer = new PropertyEditorFooter(SelectedObjects),
                    };
            }
        }
    }
}
