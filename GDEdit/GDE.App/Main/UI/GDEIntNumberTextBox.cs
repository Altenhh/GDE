using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;

namespace GDE.App.Main.UI
{
    public class GDEIntNumberTextBox : GDENumberTextBoxBase<int>
    {
        protected override int NormalStep => 5;
        protected override int SmallStep => 2;
        protected override int TinyStep => 1;

        protected override bool AllowDecimalNumbers => false;

        public GDEIntNumberTextBox() : base() { }
        public GDEIntNumberTextBox(int value) : base(value) { }

        protected override void AdjustValue(bool isReversed) => Bindable.Value += isReversed ? -ThresholdValue : ThresholdValue;
        protected override void CommitText(TextBox sender, bool newText)
        {
            bool success = int.TryParse(sender.Text, out int numValue);

            if (success && (numValue <= Bindable.MaxValue || numValue >= Bindable.MinValue))
                Bindable.Value = numValue;
            else
                sender.Text = Bindable.Value.ToString();
        }
        protected override ArrowButton GetArrowButton(BindableNumber<int> bindable, BindableNumber<int> thresholdValue, BindableBool mixedValue, bool flipped) => new IntArrowButton(bindable, thresholdValue, mixedValue, flipped);
        protected override BindableNumber<int> InitializeBindable() => new BindableInt();

        private class IntArrowButton : ArrowButton
        {
            public IntArrowButton(BindableNumber<int> value, BindableNumber<int> thresholdValue, BindableBool mixedValue, bool flipped = false)
                : base(value, thresholdValue, mixedValue, flipped) { }

            protected override BindableNumber<int> InitializeBindable() => new BindableInt();
            protected override bool IsNewValueAboveMax(int newValue) => newValue + 1 > Value.MaxValue;
            protected override bool IsNewValueBelowMin(int newValue) => newValue - 1 < Value.MinValue;
            protected override void OnClick() => Value.Value += Flipped ? -ThresholdValue : ThresholdValue;
        }
    }
}