using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;

namespace GDE.App.Main.UI
{
    public class GDENumberTextBox : GDENumberTextBoxBase<double>
    {
        protected override double NormalStep => 1;
        protected override double SmallStep => 0.5;
        protected override double TinyStep => 0.25;

        public GDENumberTextBox() : base() { }

        protected override void AdjustValue(bool isReversed) => Bindable.Value += isReversed ? -ThresholdValue : ThresholdValue;
        protected override void CommitText(TextBox sender, bool newText)
        {
            bool success = double.TryParse(sender.Text, out double numValue);

            if (success && (numValue <= Bindable.MaxValue || numValue >= Bindable.MinValue))
                Bindable.Value = numValue;
            else
                sender.Text = Bindable.Value.ToString();
        }
        protected override ArrowButton GetArrowButton(BindableNumber<double> bindable, BindableNumber<double> thresholdValue, BindableBool mixedValue, bool flipped) => new DoubleArrowButton(bindable, thresholdValue, mixedValue, flipped);
        protected override BindableNumber<double> InitializeBindable() => new BindableDouble();

        private class DoubleArrowButton : ArrowButton
        {
            public DoubleArrowButton(BindableNumber<double> value, BindableNumber<double> thresholdValue, BindableBool mixedValue, bool flipped = false)
                : base(value, thresholdValue, mixedValue, flipped) { }

            protected override BindableNumber<double> InitializeBindable() => new BindableDouble();
            protected override bool IsNewValueAboveMax(double newValue) => newValue + 1 > Value.MaxValue;
            protected override bool IsNewValueBelowMin(double newValue) => newValue - 1 < Value.MinValue;
            protected override void OnClick() => Value.Value += Flipped? -ThresholdValue : ThresholdValue;
        }
    }
}