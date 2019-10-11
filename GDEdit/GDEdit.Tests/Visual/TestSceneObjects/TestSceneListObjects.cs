using GDEdit.App.Level;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;

namespace GDEdit.Tests.Visual.TestSceneObjects
{
    public class TestSceneListObjects : TestScene
    {
        [Resolved]
        private TextureStore Textures { get; set; }
        
        protected override void LoadComplete()
        {
            base.LoadComplete();
            
            FillFlowContainer container;
            
            Add(new BasicScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = container = new FillFlowContainer
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes     = Axes.Y,
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