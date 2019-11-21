using GDAPI.Enumerations.GeometryDash;
using GDE.App.Main.UI;
using osu.Framework.Graphics;
using System;

namespace GDE.App.Main.Panels.Object.Components
{
    public class PropertyEditorSingleDoubleGroup : PropertyEditorSingleValueGroup<GDENumberTextBox, double>
    {
        public PropertyEditorSingleDoubleGroup(string groupTitle, ObjectProperty propertyID) : base(groupTitle, propertyID) { }
    }
}