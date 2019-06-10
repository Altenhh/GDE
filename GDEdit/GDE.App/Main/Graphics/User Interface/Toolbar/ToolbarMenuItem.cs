using System;
using osu.Framework.Graphics.UserInterface;

namespace GDE.App.Main.Graphics.UserInterface.Toolbar
{
    public class ToolbarMenuItem : MenuItem
    {
        public readonly MenuItemType Type;

        public ToolbarMenuItem(string text, MenuItemType type = MenuItemType.Standard)
            : base(text)
        {
            Type = type;
        }

        public ToolbarMenuItem(string text, MenuItemType type, Action action)
            : base(text, action)
        {
            Type = type;
        }
    }
}
