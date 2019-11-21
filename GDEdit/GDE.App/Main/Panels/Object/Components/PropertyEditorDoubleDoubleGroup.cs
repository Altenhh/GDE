using GDAPI.Enumerations.GeometryDash;
using GDE.App.Main.UI;

namespace GDE.App.Main.Panels.Object.Components
{
    public class PropertyEditorDoubleDoubleGroup : PropertyEditorDoubleValueGroup<GDENumberTextBox, double>
    {
        public PropertyEditorDoubleDoubleGroup(string groupTitle, string firstPropertyTitle, string secondPropertyTitle, ObjectProperty firstPropertyID, ObjectProperty secondPropertyID)
            : base(groupTitle, firstPropertyTitle, secondPropertyTitle, firstPropertyID, secondPropertyID) { }
    }
}