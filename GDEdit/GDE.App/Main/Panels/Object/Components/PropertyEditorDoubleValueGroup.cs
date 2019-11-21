using GDAPI.Enumerations.GeometryDash;
using GDAPI.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Colors;
using GDE.App.Main.UI;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using System;

namespace GDE.App.Main.Panels.Object.Components
{
    public abstract class PropertyEditorDoubleValueGroup<TDrawable, TValue> : PropertyEditorGroup<TDrawable, TValue>
        where TDrawable : Drawable, IHasCurrentValue<TValue>, new()
    {
        private bool triggerChangesEnabled = false;

        private LabelledPropertyItemContainer firstItemContainer;
        private LabelledPropertyItemContainer secondItemContainer;

        protected SpriteText FirstPropertyTitle => firstItemContainer.PropertyTitle;
        protected SpriteText SecondPropertyTitle => secondItemContainer.PropertyTitle;
        protected TDrawable FirstPropertyDrawable => firstItemContainer.Item;
        protected TDrawable SecondPropertyDrawable => secondItemContainer.Item;

        public readonly ObjectProperty FirstPropertyID;
        public readonly ObjectProperty SecondPropertyID;

        public event Action<TValue> FirstPropertyValueChanged;
        public event Action<TValue> SecondPropertyValueChanged;

        public PropertyEditorDoubleValueGroup(string groupTitle, string firstPropertyTitle, string secondPropertyTitle, ObjectProperty firstPropertyID, ObjectProperty secondPropertyID)
            : base(groupTitle)
        {
            FirstPropertyTitle.Text = firstPropertyTitle;
            SecondPropertyTitle.Text = secondPropertyTitle;

            FirstPropertyDrawable.Current.ValueChanged += a => FirstPropertyValueChanged(a.NewValue);
            SecondPropertyDrawable.Current.ValueChanged += a => SecondPropertyValueChanged(a.NewValue);

            FirstPropertyID = firstPropertyID;
            SecondPropertyID = secondPropertyID;
        }

        public void ChangeFirstValueSafely(TValue newValue) => ChangeValueSafely(newValue, firstItemContainer, ResetFirstColor);
        public void DisplayMixedCommonFirstValue() => DisplayMixedCommonValue(ChangeFirstValueSafely, ChangeToMixedFirstValueColor);

        public void ChangeSecondValueSafely(TValue newValue) => ChangeValueSafely(newValue, secondItemContainer, ResetSecondColor);
        public void DisplayMixedCommonSecondValue() => DisplayMixedCommonValue(ChangeSecondValueSafely, ChangeToMixedSecondValueColor);

        private void ChangeValueSafely(TValue newValue, LabelledPropertyItemContainer container, Action resetColor)
        {
            triggerChangesEnabled = false;
            container.Value = newValue;
            triggerChangesEnabled = true;
            resetColor();
        }
        public void DisplayMixedCommonValue(Action<TValue> changeValueSafely, Action changeToMixedValueColor)
        {
            changeValueSafely(default);
            changeToMixedValueColor();
        }

        public override void UpdateValues(LevelObjectCollection levelObjects)
        {
            UpdateValue(levelObjects, FirstPropertyID, ChangeFirstValueSafely, DisplayMixedCommonFirstValue);
            UpdateValue(levelObjects, SecondPropertyID, ChangeSecondValueSafely, DisplayMixedCommonSecondValue);
        }

        private void UpdateValue(LevelObjectCollection objects, ObjectProperty propertyID, Action<TValue> changeValueSafely, Action displayMixedCommonValue)
        {
            bool common = objects.TryGetCommonPropertyWithID(propertyID, out TValue value);
            if (common)
                changeValueSafely(value);
            else
                displayMixedCommonValue();
        }

        protected override Drawable[] GetExtraDrawables()
        {
            return new Drawable[]
            {
                firstItemContainer = GetLabelledContainer(TriggerFirstValueChange, Anchor.Centre, Anchor.Centre),
                secondItemContainer = GetLabelledContainer(TriggerSecondValueChange, Anchor.CentreRight, Anchor.CentreRight, new MarginPadding { Right = 5 }),
            };
        }

        private void ChangeToMixedFirstValueColor() => ChangeToMixedValueColor(firstItemContainer);
        private void ResetFirstColor() => ResetColor(firstItemContainer);

        private void ChangeToMixedSecondValueColor() => ChangeToMixedValueColor(secondItemContainer);
        private void ResetSecondColor() => ResetColor(secondItemContainer);

        private void ChangeToMixedValueColor(LabelledPropertyItemContainer container) => container.Colour = GDEColors.FromHex("604040");
        private void ResetColor(LabelledPropertyItemContainer container) => container.Colour = GDEColors.FromHex("C08080");

        private void TriggerFirstValueChange(ValueChangedEvent<TValue> v)
        {
            if (triggerChangesEnabled)
                FirstPropertyValueChanged(v.NewValue);
        }
        private void TriggerSecondValueChange(ValueChangedEvent<TValue> v)
        {
            if (triggerChangesEnabled)
                SecondPropertyValueChanged(v.NewValue);
        }
    }
}