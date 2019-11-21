using GDAPI.Enumerations.GeometryDash;
using GDE.App.Main.UI.Containers;
using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;

namespace GDE.App.Main.Panels.Object.Components
{
    public abstract class PropertyEditorSingleDeltransformableValueGroup<TDrawable, TValue> : PropertyEditorSingleValueGroup<TDrawable, TValue>, IDeltable
        where TDrawable : DeltransformableContainer, IHasCurrentValue<TValue>, new()
    {
        private BindableBool deltaModeBindable = new BindableBool();

        public BindableBool DeltaModeBindable
        {
            get => deltaModeBindable;
            set => deltaModeBindable.BindTo(value);
        }

        public PropertyEditorSingleDeltransformableValueGroup(string groupTitle, ObjectProperty propertyID)
            : base(groupTitle, propertyID)
        {
            deltaModeBindable.BindTo(ItemContainer.Item.DeltaModeBindable);
        }
    }
}