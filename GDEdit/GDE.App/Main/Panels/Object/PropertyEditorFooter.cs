using GDAPI.Utilities.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Objects;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GDE.App.Main.Panels.Object
{
    public class PropertyEditorFooter : Container
    {
        public Bindable<GeneralObject> Object;

        public PropertyEditorFooter()
        {
            Object = new Bindable<GeneralObject>();

            AutoSizeAxes = Axes.Y;
            Width = 800;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRange(new Drawable[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Spacing = new Vector2(5),
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(5),
                            Children = new Drawable[]
                            {
                                new EditableCheckbox
                                {
                                    LabelText = "Flipped Horizontally"
                                },
                                new EditableCheckbox
                                {
                                    LabelText = "Flipped Vertically"
                                },
                                new EditableCheckbox
                                {
                                    LabelText = "Glow"
                                },
                            }
                        },
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(5),
                            Children = new Drawable[]
                            {
                                new EditableCheckbox
                                {
                                    LabelText = "Don't Fade"
                                },
                                new EditableCheckbox
                                {
                                    LabelText = "Don't Enter"
                                },
                                new EditableCheckbox
                                {
                                    LabelText = "Group Parent"
                                },
                            }
                        },
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(5),
                            Children = new Drawable[]
                            {
                                new EditableCheckbox
                                {
                                    LabelText = "High Detail"
                                },
                            }
                        },
                    }
                }
            });
        }
    }
}
