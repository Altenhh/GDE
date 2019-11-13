using osu.Framework.Graphics.Sprites;

namespace GDE.App.Main.Panels.Object.Tabs
{
    public class PropertyEditorTab
    {
        public IconUsage Icon { get; set; }
        public TabEnumeration Tab { get; set; }
    }

    public enum TabEnumeration
    {
        General,
        Special,
        Other
    }
}