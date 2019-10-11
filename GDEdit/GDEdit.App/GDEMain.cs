using GDEdit.App.Screens.Menu;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Screens;

namespace GDEdit.App
{
    public class GDEMain : GDEBase
    {
        private ScreenStack screenStack;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            
            Add(screenStack = new ScreenStack
                              {
                                  RelativeSizeAxes = Axes.Both
                              });

            screenStack.ScreenExited += screenExited;
            screenStack.ScreenPushed += screenPushed;
            
            screenStack.Push(new MainMenu());
        }
        
        private void screenPushed(IScreen lastScreen, IScreen newScreen)
        {
            Logger.Log($"Screen changed → {newScreen}");
        }

        private void screenExited(IScreen lastScreen, IScreen newScreen)
        {
            Logger.Log($"Screen changed ← {newScreen}");

            if (newScreen == null)
                Exit();
        }
    }
}