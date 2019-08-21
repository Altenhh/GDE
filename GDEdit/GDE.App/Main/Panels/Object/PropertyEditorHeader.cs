using GDAPI.Utilities.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Objects;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GDE.App.Main.Panels.Object
{
    public class PropertyEditorHeader : Container
    {
        private ObjectBase drawableObject;

        private Bindable<string> name = new Bindable<string>();
        private BindableBool deltaMode = new BindableBool();

        public Bindable<GeneralObject> Object;

        public PropertyEditorHeader()
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
                    Direction = FillDirection.Horizontal,
                    AutoSizeAxes = Axes.Both,
                    Spacing = new Vector2(5, 0),
                    Children = new Drawable[]
                    {
                        drawableObject = new ObjectBase(Object.Value)
                        {
                            Size = new Vector2(50),
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            // Resets values to avoid causing problems
                            Position = new Vector2(0),
                            Scale = new Vector2(1)
                        },
                        new FillFlowContainer
                        {
                            /*new EditableTextBox
                            {
                                LabelText = "Name",
                                Bindable = name
                            }*/
                        }
                    }
                },
                new EditableCheckboxFlipped
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    LabelText = "δ Mode",
                    Bindable = deltaMode
                }
            });
        }
    }
}
