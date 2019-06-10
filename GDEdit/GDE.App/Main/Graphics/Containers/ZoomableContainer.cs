using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace GDE.App.Main.Graphics.Containers
{
    public class ZoomableContainer : Container
    {
        private readonly Container zoomedContent;

        public float CurrentZoom = 1;
        public float MaxZoom = 40;
        public float MinZoom = 0.05f;

        protected override Container<Drawable> Content => zoomedContent;

        public ZoomableContainer()
        {
            base.Content.Add(zoomedContent = new Container
            {
                RelativeSizeAxes = Axes.Both
            });
        }
    }
}