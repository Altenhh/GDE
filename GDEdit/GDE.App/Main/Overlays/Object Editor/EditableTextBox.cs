using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GDE.App.Main.Overlays.ObjectEditor
{
    public class EditableTextBox : EditableItem<string>
    {
        protected override FillDirection FillFlowDirection => FillDirection.Vertical;

        protected override Drawable CreateControl() => new TextBox
        {
            Margin = new MarginPadding { Top = 5 },
            Width = 100,
            Height = 20,
        };
    }
}
