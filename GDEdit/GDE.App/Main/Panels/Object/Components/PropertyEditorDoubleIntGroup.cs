using GDAPI.Enumerations.GeometryDash;
using GDE.App.Main.UI;

namespace GDE.App.Main.Panels.Object.Components
{
    public class PropertyEditorDoubleIntGroup : PropertyEditorDoubleValueGroup<GDEIntNumberTextBox, int>
    {
        public PropertyEditorDoubleIntGroup(string groupTitle, string firstPropertyTitle, string secondPropertyTitle, ObjectProperty firstPropertyID, ObjectProperty secondPropertyID)
            : base(groupTitle, firstPropertyTitle, secondPropertyTitle, firstPropertyID, secondPropertyID) { }
    }
}