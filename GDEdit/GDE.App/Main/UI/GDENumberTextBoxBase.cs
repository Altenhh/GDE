using GDE.App.Main.Colors;
using GDE.App.Main.UI.Graphics;
using GDE.App.Main.UI.Shadowed;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System;

namespace GDE.App.Main.UI
{
    public abstract class GDENumberTextBoxBase<T> : Container, IHasCurrentValue<T>
        where T : struct, IComparable<T>, IConvertible, IEquatable<T>
    {
        private BindableBool mixedValue = new BindableBool();

        protected bool IsMixedCommonValue
        {
            get => mixedValue.Value;
            set => mixedValue.Value = value;
        }

        protected BindableNumber<T> Threshold;
        protected T ThresholdValue => Threshold.Value;

        protected abstract T NormalStep { get; }
        protected abstract T SmallStep { get; }
        protected abstract T TinyStep { get; }

        protected virtual bool AllowDecimalNumbers => true;

        public BindableNumber<T> Bindable { get; private set; }

        public T Value
        {
            get => Bindable.Value;
            set => Bindable.Value = value;
        }

        public Bindable<T> Current
        {
            get => Bindable;
            set => Bindable.BindTo(value);
        }

        public event Action<ValueChangedEvent<T>> ValueChanged;

        public GDENumberTextBoxBase(T value = default)
        {
            AutoSizeAxes = Axes.Y;

            Bindable = InitializeBindable();

            Threshold = InitializeBindable();
            Threshold.Value = NormalStep;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var arrowLeft = GetArrowButton(Bindable, Threshold, mixedValue, true);
            var arrowRight = GetArrowButton(Bindable, Threshold, mixedValue, false);
            var textBox = new NumberedTextBox(mixedValue, AllowDecimalNumbers)
            {
                Height = 20,
                RelativeSizeAxes = Axes.X,
                Text = Bindable.Value.ToString(),
                Depth = -1
            };

            Add(new FillFlowContainer
            {
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(-3, 0),
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new Drawable[]
                {
                    arrowLeft,
                    textBox,
                    arrowRight,
                }
            });

            textBox.OnCommit += CommitText;

            Bindable.ValueChanged += HandleValueChanged;
            Bindable.TriggerChange();

            void HandleValueChanged(ValueChangedEvent<T> value)
            {
                textBox.Text = value.NewValue.ToString();
                ValueChanged?.Invoke(value);
            }
        }

        protected abstract BindableNumber<T> InitializeBindable();
        protected abstract ArrowButton GetArrowButton(BindableNumber<T> bindable, BindableNumber<T> thresholdValue, BindableBool mixedValue, bool flipped);
        protected abstract void CommitText(TextBox sender, bool newText);
        protected abstract void AdjustValue(bool isReversed);
        
        protected override bool OnScroll(ScrollEvent e)
        {
            bool isReversed = e.ScrollDelta.Y < 0;

            AdjustValue(isReversed);
            return base.OnScroll(e);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            UpdateThreshold(e);
            return base.OnKeyDown(e);
        }
        protected override bool OnKeyUp(KeyUpEvent e)
        {
            UpdateThreshold(e);
            return base.OnKeyUp(e);
        }

        private void UpdateThreshold(KeyboardEvent e)
        {
            if (e.ControlPressed)
                Threshold.Value = TinyStep;
            else if (e.ShiftPressed)
                Threshold.Value = SmallStep;
            else
                Threshold.Value = NormalStep;
        }

        protected abstract class ArrowButton : GDEButton, ISupportsMixedValue
        {
            private BindableBool mixedValueBindable;
            private BindableNumber<T> threshold;

            protected bool Flipped;
            protected T ThresholdValue => threshold.Value;

            public BindableNumber<T> Value;

            public bool IsMixedCommonValue
            {
                get => mixedValueBindable.Value;
                set => mixedValueBindable.Value = value;
            }

            public ArrowButton(BindableNumber<T> value, BindableNumber<T> thresholdValue, BindableBool mixedValue, bool flipped = false)
            {
                Text = (Flipped = flipped) ? "<" : ">";
                FadeToNormalColors(0);
                Size = new Vector2(20);

                Value = InitializeBindable();
                Value.BindTo(value);

                threshold = InitializeBindable();
                threshold.BindTo(thresholdValue);

                mixedValueBindable = new BindableBool { BindTarget = mixedValue };
                mixedValueBindable.ValueChanged += ((ISupportsMixedValue)this).UpdateColors;

                value.ValueChanged += val =>
                {
                    var newValue = val.NewValue;

                    bool onMax = IsNewValueAboveMax(newValue);
                    bool onMin = IsNewValueAboveMax(newValue);

                    if (Enabled.Value = (flipped && onMax) || onMin)
                        SpriteText.Colour = GDEColors.FromHex("595959");
                    else
                        SpriteText.Colour = Color4.White;
                };

                Action = OnClick;
            }

            public void FadeToMixedColors(double duration = 500) => FadeColor(nameof(BackgroundColour), "663333", duration);
            public void FadeToNormalColors(double duration = 500) => FadeColor(nameof(BackgroundColour), "333333", duration);

            protected abstract BindableNumber<T> InitializeBindable();
            protected abstract void OnClick();
            protected abstract bool IsNewValueAboveMax(T newValue);
            protected abstract bool IsNewValueBelowMin(T newValue);

            private void FadeColor(string propertyName, string newColorHex, double duration = 500) => this.TransformTo(propertyName, GDEColors.FromHex(newColorHex), duration, Easing.OutQuint);
        }

        private class NumberedTextBox : ShadowedTextBox, ISupportsMixedValue
        {
            private BindableBool mixedValueBindable;

            protected override bool CanAddCharacter(char character) => char.IsNumber(character) || (AllowDecimalNumbers && character == '.') || character == '-';

            public bool AllowDecimalNumbers;

            public bool IsMixedCommonValue
            {
                get => mixedValueBindable.Value;
                set => mixedValueBindable.Value = value;
            }

            public NumberedTextBox(BindableBool mixedValue, bool allowDecimalNumbers = true)
            {
                CornerRadius = 0;

                AllowDecimalNumbers = allowDecimalNumbers;
                FadeToNormalColors(0);

                mixedValueBindable = new BindableBool { BindTarget = mixedValue };
                mixedValueBindable.ValueChanged += ((ISupportsMixedValue)this).UpdateColors;
            }

            public void FadeToMixedColors(double duration = 500)
            {
                FadeColor(nameof(BackgroundUnfocused), "4C2626", duration);
                FadeColor(nameof(BackgroundFocused), "603030", duration);
                FadeColor(nameof(BackgroundCommit), "804040", duration);
            }
            public void FadeToNormalColors(double duration = 500)
            {
                FadeColor(nameof(BackgroundUnfocused), "262626", duration);
                FadeColor(nameof(BackgroundFocused), "303030", duration);
                FadeColor(nameof(BackgroundCommit), "404040", duration);
            }

            protected override Drawable GetDrawableCharacter(char c) => new SpriteText
            {
                Text = c.ToString(),
                Font = GDEFont.GetFont(Typeface.Digitall, CalculatedTextSize),
            };

            private void FadeColor(string propertyName, string newColorHex, double duration = 500) => this.TransformTo(propertyName, GDEColors.FromHex(newColorHex), duration, Easing.OutQuint);
        }
    }
}