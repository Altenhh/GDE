using GDE.App.Main.UI.Shadowed;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GDE.App.Main.Panels.Object
{
    public class EditableCheckboxFlipped : EditableItem<bool>
    {
        protected override bool AppearBeforeText => false;

        protected override FillDirection FillFlowDirection => FillDirection.Full;

        protected override Drawable CreateControl() => new ShadowedCheckbox
        {
            Margin = new MarginPadding { Top = 2, Left = 5 },
            Size = new Vector2(25)
        };
    }
}
