using GDEdit.App.Screens.Menu;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;

namespace GDEdit.Tests.Visual
{
    public class TestSceneMainMenu : TestScene
    {
        public TestSceneMainMenu()
        {
            Add(new ScreenStack(new MainMenu()) { RelativeSizeAxes = Axes.Both });
        }
    }
}