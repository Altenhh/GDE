using osu.Framework.Graphics.Sprites;

namespace GDE.App.Main.Panels.Tabs
{
    public interface IPETab
    {
        IconUsage Icon { get; set; }

        //TODO: Have a design for this(?)
        string Title { get; set; }
    }
}