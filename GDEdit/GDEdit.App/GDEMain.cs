using GDEdit.App.Screens.Menu;
using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace GDEdit.App
{
    public class GDEMain : GDEBase
    {
        private ScreenStack screenStack;
        
        [BackgroundDependencyLoader]
        private void load()
        {
            SpriteText debugText;
            
            Add(screenStack = new ScreenStack
            {
                RelativeSizeAxes = Axes.Both
            });
            
            Add(debugText = new SpriteText());

            screenStack.ScreenPushed += (oldScreen, newScreen) =>
            {
                if (!DebugUtils.IsDebugBuild) // returns inverted, so we need to invert it back
                {
                    var screen = newScreen.GetType();

                    debugText.Text = screen.Name;
                }
            };
            
            screenStack.Push(new MainMenu());
        }
    }
}