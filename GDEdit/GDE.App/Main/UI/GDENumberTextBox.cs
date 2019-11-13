using GDE.App.Main.Colors;
using GDE.App.Main.UI.Graphics;
using GDE.App.Main.UI.Shadowed;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System.Globalization;

namespace GDE.App.Main.UI
{
    public class GDENumberTextBox : Container
    {
        private float threshold = 1;

        protected bool AllowDecimalNumbers { get; set; }

        public BindableDouble Bindable { get; private set; }

        public double Value => Bindable.Value;
        public int ValueInt => (int)Value;

        public GDENumberTextBox(bool allowDecimalNumbers = true)
        {
            AutoSizeAxes = Axes.Y;

            AllowDecimalNumbers = allowDecimalNumbers;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (Bindable == null)
                Bindable = new BindableDouble();

            var arrowLeft = new ArrowButton(Bindable, true);
            var arrowRight = new ArrowButton(Bindable);
            var textBox = new NumberedTextBox(AllowDecimalNumbers)
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

            textBox.OnCommit += (_, text) =>
            {
                double.TryParse(textBox.Text, NumberStyles.Any, null, out double numValue);

                if (numValue <= Bindable.MaxValue || numValue >= Bindable.MinValue)
                    Bindable.Value = numValue;
                else
                    textBox.Text = Bindable.Value.ToString();
            };

            Bindable.ValueChanged += text => textBox.Text = text.NewValue.ToString();
            Bindable.TriggerChange();
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            bool isReversed = e.ScrollDelta.Y < 0;

            Bindable.Value += isReversed ? -threshold : threshold;
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
                threshold = 0.25f;
            else if (e.ShiftPressed)
                threshold = 0.5f;
            else
                threshold = 1;
        }

        private class ArrowButton : GDEButton
        {
            private float threshold = 1;

            public BindableDouble Value = new BindableDouble();

            public ArrowButton(BindableDouble value, bool flipped = false)
            {
                Text = flipped ? "<" : ">";
                BackgroundColour = GDEColors.FromHex("333333");
                Height = 20;
                Width = 20;

                Value.BindTo(value);

                value.ValueChanged += val =>
                {
                    var newValue = val.NewValue;

                    bool onMax = newValue + 1 > value.MaxValue;
                    bool onMin = newValue - 1 < value.MinValue;

                    if (Enabled.Value = (flipped && onMax) || onMin)
                        SpriteText.Colour = GDEColors.FromHex("595959");
                    else
                        SpriteText.Colour = Color4.White;
                };

                Action = () =>
                {
                    var newValue = Value.Value += flipped ? -threshold : threshold;
                };
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
                    threshold = 0.25f;
                else if (e.ShiftPressed)
                    threshold = 0.5f;
                else
                    threshold = 1;
            }
        }

        private class NumberedTextBox : ShadowedTextBox
        {
            public bool AllowDecimalNumbers = true;

            protected override bool CanAddCharacter(char character) => char.IsNumber(character) || (AllowDecimalNumbers && character == '.') || character == '-';

            public NumberedTextBox(bool allowDecimalNumbers = true)
            {
                BackgroundUnfocused = GDEColors.FromHex("262626");
                BackgroundFocused = GDEColors.FromHex("303030");
                BackgroundCommit = GDEColors.FromHex("404040");
                CornerRadius = 0;
                AllowDecimalNumbers = allowDecimalNumbers;
            }

            protected override Drawable GetDrawableCharacter(char c) => new SpriteText
            {
                Text = c.ToString(),
                Font = GDEFont.GetFont(Typeface.Digitall, CalculatedTextSize),
            };
        }
    }
}