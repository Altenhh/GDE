using GDAPI.Enumerations.GeometryDash;
using GDE.App.Main.Panels.Object.ValueComponents;

namespace GDE.App.Main.Panels.Object.Components
{
    public class PropertyEditorZLayerGroup : PropertyEditorSingleDeltransformableValueGroup<ZLayerInputBox, ZLayer>
    {
        public PropertyEditorZLayerGroup(string groupTitle, ObjectProperty propertyID) : base(groupTitle, propertyID) { }
    }
}