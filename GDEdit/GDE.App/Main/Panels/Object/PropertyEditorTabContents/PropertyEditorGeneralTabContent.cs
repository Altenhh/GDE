using GDAPI.Enumerations.GeometryDash;
using GDAPI.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Panels.Object.Components;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace GDE.App.Main.Panels.Object.Content.PropertyEditorTabContents
{
    public class PropertyEditorGeneralTabContent : PropertyEditorTabContent
    {
        private PropertyEditorSingleIntGroup objectID;
        private PropertyEditorDoubleDoubleGroup position;
        private PropertyEditorSingleDoubleGroup rotation;
        private PropertyEditorSingleDoubleGroup scaling;
        private PropertyEditorSingleIntGroup zOrder;
        private PropertyEditorZLayerGroup zLayer;
        private PropertyEditorDoubleIntGroup editorLayers;
        private PropertyEditorSingleIntGroup linkedGroupID;

        protected override PropertyEditorGroup[] GetGroups() => new PropertyEditorGroup[]
        {
            objectID,
            position,
            rotation,
            scaling,
            zOrder,
            zLayer,
            editorLayers,
            linkedGroupID
        };

        public PropertyEditorGeneralTabContent(LevelObjectCollection objects, BindableBool deltaModeBindable) : base(objects, deltaModeBindable) { }

        protected override Drawable[] CreateContent()
        {
            objectID = new PropertyEditorSingleIntGroup("Object ID", ObjectProperty.ObjectID);
            position = new PropertyEditorDoubleDoubleGroup("Position", "X", "Y", ObjectProperty.X, ObjectProperty.Y);
            rotation = new PropertyEditorSingleDoubleGroup("Rotation", ObjectProperty.Rotation);
            scaling = new PropertyEditorSingleDoubleGroup("Scaling", ObjectProperty.Scaling);
            zOrder = new PropertyEditorSingleIntGroup("Z Order", ObjectProperty.ZOrder);
            zLayer = new PropertyEditorZLayerGroup("Z Layer", ObjectProperty.ZLayer);
            editorLayers = new PropertyEditorDoubleIntGroup("Editor Layer", "1", "2", ObjectProperty.EL1, ObjectProperty.EL2);
            linkedGroupID = new PropertyEditorSingleIntGroup("Linked Group ID", ObjectProperty.LinkedGroupID);

            objectID.PropertyValueChanged += ObjectIDChanged;
            position.FirstPropertyValueChanged += XPositionChanged;
            position.SecondPropertyValueChanged += YPositionChanged;
            rotation.PropertyValueChanged += RotationChanged;
            scaling.PropertyValueChanged += ScalingChanged;
            zOrder.PropertyValueChanged += ZOrderChanged;
            zLayer.PropertyValueChanged += ZLayerChanged;
            editorLayers.FirstPropertyValueChanged += EL1Changed;
            editorLayers.SecondPropertyValueChanged += EL2Changed;
            linkedGroupID.PropertyValueChanged += LinkedGroupIDChanged;

            var content = new Drawable[]
            {
                objectID,
                position,
                rotation,
                scaling,
                zOrder,
                zLayer,
                editorLayers,
                linkedGroupID,
            };

            return content;
        }

        private void ObjectIDChanged(int value) => SelectedObjects.CommonObjectID = value;
        private void XPositionChanged(double value) => SelectedObjects.CommonX = value;
        private void YPositionChanged(double value) => SelectedObjects.CommonY = value;
        private void RotationChanged(double value) => SelectedObjects.CommonRotation = value;
        private void ScalingChanged(double value) => SelectedObjects.CommonScaling = value;
        private void ZOrderChanged(int value) => SelectedObjects.CommonZOrder = value;
        private void ZLayerChanged(ZLayer value) => SelectedObjects.CommonZLayer = (int)value;
        private void EL1Changed(int value) => SelectedObjects.CommonEL1 = value;
        private void EL2Changed(int value) => SelectedObjects.CommonEL2 = value;
        private void LinkedGroupIDChanged(int value) => SelectedObjects.CommonLinkedGroupID = value;
    }
}