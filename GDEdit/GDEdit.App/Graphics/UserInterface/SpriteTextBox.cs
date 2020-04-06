using osuTK.Graphics;

namespace GDEdit.App.Graphics.UserInterface
{
    public class SpriteTextBox : GDETextBox
    {
        public SpriteTextBox()
        {
            BackgroundUnfocused = Color4.Transparent;
            BackgroundFocused = GDEColour.Gray20;
            BackgroundCommit = GDEColour.Gray25;
        }
        
        protected override Color4 SelectionColour => GDEColour.Gray40;
    }
}