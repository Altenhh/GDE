using osu.Framework.Graphics.Sprites;

namespace GDE.App.Main.Panels.Object.Tabs
{
    public class PropertyEditorTab
    {
        public IconUsage Icon { get; set; }
        public PropertyEditorTabType Tab { get; set; }
    }

    public enum PropertyEditorTabType
    {
        General,
        Special,
        Other
    }
}