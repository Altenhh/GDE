using GDAPI.Enumerations.GeometryDash;
using GDAPI.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Colors;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using System;

namespace GDE.App.Main.Panels.Object.Components
{
    public abstract class PropertyEditorSingleValueGroup<TDrawable, TValue> : PropertyEditorGroup<TDrawable, TValue>
        where TDrawable : Drawable, IHasCurrentValue<TValue>, new()
    {
        private bool triggerChangesEnabled = false;

        protected LabelledPropertyItemContainer ItemContainer;

        public event Action<TValue> PropertyValueChanged;

        public readonly ObjectProperty PropertyID;

        public PropertyEditorSingleValueGroup(string groupTitle, ObjectProperty propertyID)
            : base(groupTitle)
        {
            PropertyID = propertyID;
        }

        public void ChangeValueSafely(TValue newValue)
        {
            triggerChangesEnabled = false;
            ItemContainer.Value = newValue;
            triggerChangesEnabled = true;
            ResetColor();
        }
        public void DisplayMixedCommonValue()
        {
            ChangeValueSafely(default);
            ChangeToMixedValueColor();
        }

        public override void UpdateValues(LevelObjectCollection levelObjects)
        {
            bool common = levelObjects.TryGetCommonPropertyWithID(PropertyID, out TValue value);
            if (common)
                ChangeValueSafely(value);
            else
                DisplayMixedCommonValue();
        }

        protected override Drawable[] GetExtraDrawables()
        {
            return new Drawable[]
            {
                ItemContainer = GetLabelledContainer(TriggerChange)
            };
        }

        private void ChangeToMixedValueColor() => ItemContainer.Colour = GDEColors.FromHex("604040");
        private void ResetColor() => ItemContainer.Colour = GDEColors.FromHex("C08080");

        private void TriggerChange(ValueChangedEvent<TValue> v)
        {
            if (triggerChangesEnabled)
                PropertyValueChanged(v.NewValue);
        }
    }
}