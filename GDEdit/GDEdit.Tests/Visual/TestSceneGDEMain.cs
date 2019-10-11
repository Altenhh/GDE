using GDEdit.App;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Platform;
using osu.Framework.Testing;
using osuTK.Graphics;

namespace GDEdit.Tests.Visual
{
    public class TestSceneGDEMain : TestScene
    {
        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            GDEMain game = new GDEMain();
            game.SetHost(host);
            
            Children = new Drawable[]
                       {
                           new Box
                           {
                               RelativeSizeAxes = Axes.Both,
                               Colour           = Color4.Black,
                           },
                           game
                       };
        }
    }
}