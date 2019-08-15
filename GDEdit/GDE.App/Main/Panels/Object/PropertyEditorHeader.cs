using GDAPI.Utilities.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Objects;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GDE.App.Main.Panels.Object
{
    public class PropertyEditorHeader : Container
    {
        public PropertyEditorHeader(GeneralObject Object)
        {
            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;

            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    AutoSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new ObjectBase(Object),
                        new FillFlowContainer
                        {

                        }
                    }
                }
            };
        }
    }
}
