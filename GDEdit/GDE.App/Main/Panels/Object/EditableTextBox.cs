﻿using GDE.App.Main.UI.Shadowed;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GDE.App.Main.Panels.Object
{
    public class EditableTextBox : EditableItem<string>
    {
        protected override FillDirection FillFlowDirection => FillDirection.Vertical;

        protected override Drawable CreateControl() => new ShadowedTextBox
        {
            Margin = new MarginPadding { Top = 2 },
            Text = "Object",
            Width = 200,
            Height = 25,
        };
    }
}