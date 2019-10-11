using GDEdit.App.Level;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GDEdit.App
{
    public class GDEMain : GDEBase
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();

            FillFlowContainer container;
            
            Add(new BasicScrollContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Child = container = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                            }
                });

            for (int i = 0; i < 1911; i++)
            {
                var obj = ObjectManager.GetAppropriateObject(i, Textures);
                
                if (obj != null)
                    container.Add(obj);
            }
        }
    }
}