using GDAPI.Enumerations.GeometryDash;
using GDE.App.Main.UI;
using osu.Framework.Bindables;

namespace GDE.App.Main.Panels.Object.Components
{
    public class PropertyEditorSingleIntGroup : PropertyEditorSingleValueGroup<GDEIntNumberTextBox, int>, IDeltable
    {
        public BindableBool DeltaModeBindable { get; set; }

        public PropertyEditorSingleIntGroup(string groupTitle, ObjectProperty propertyID) : base(groupTitle, propertyID) { }
    }
}