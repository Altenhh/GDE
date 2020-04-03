using GDEdit.App.Screens.Menu;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace GDEdit.App
{
    public class GDEMain : GDEBase
    {
        private ScreenStack screenStack;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(screenStack = new ScreenStack
            {
                RelativeSizeAxes = Axes.Both
            });

            screenStack.Push(new MainMenu());
        }
    }
}