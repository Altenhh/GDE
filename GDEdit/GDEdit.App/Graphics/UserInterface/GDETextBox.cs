using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace GDEdit.App.Graphics.UserInterface
{
    public class GDETextBox : BasicTextBox
    {
        protected override float LeftRightPadding => 0;

        protected override float CaretWidth => 3;
        
        public FontUsage Font { get; set; }

        protected override SpriteText CreatePlaceholder() => new SpriteText
        {
            Font = new FontUsage(size: 18),
            Colour = GDEColour.Gray(180),
        };

        public GDETextBox()
        {
            Height = 40;
            TextContainer.Height = 0.75f;
            CornerRadius = 3;
            LengthLimit = 1000;

            Current.DisabledChanged += disabled => { Alpha = disabled ? 0.3f : 1; };

            BackgroundUnfocused = GDEColour.Gray15;
            BackgroundFocused = GDEColour.Gray15;
            BackgroundCommit = GDEColour.Gray20;
        }

        protected override Color4 SelectionColour => GDEColour.Gray30;

        protected override Drawable GetDrawableCharacter(char c) => new SpriteText
        {
            Text = c.ToString(),
            Font = Font.With(size: CalculatedTextSize)
        };
    }
}