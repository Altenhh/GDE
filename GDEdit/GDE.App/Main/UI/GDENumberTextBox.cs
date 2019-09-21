using System.Globalization;
using GDE.App.Main.Colors;
using GDE.App.Main.UI.Graphics;
using GDE.App.Main.UI.Shadowed;
using JetBrains.Annotations;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace GDE.App.Main.UI
{
    public class GDENumberTextBox : Container
    {
        private float threshold = 1;

        public BindableDouble Bindable;

        public GDENumberTextBox(float defaultValue = 0)
        {
            NumberedTextBox textBox;
            Bindable = new BindableDouble(defaultValue);
            AutoSizeAxes = Axes.Y;
            
            Add(new FillFlowContainer
            {
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(-3, 0),
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new Drawable[]
                {
                    new ArrowButton(Bindable, true),
                    textBox = new NumberedTextBox
                    {
                        Height = 20,
                        RelativeSizeAxes = Axes.X,
                        Text = Bindable.Value.ToString(),
                        Depth = -1
                    },
                    new ArrowButton(Bindable)
                }
            });

            textBox.OnCommit += (sender, text) =>
            {
                double.TryParse(sender.Text, NumberStyles.Currency, null, out double numValue);

                if (numValue < Bindable.MaxValue || numValue > Bindable.MinValue)
                    Bindable.Value = numValue;
                else
                    Bindable.TriggerChange();
            };
            
            Bindable.ValueChanged += text => { textBox.Text = text.NewValue.ToString(); };
            
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
            switch (e.Key)
            {
                case Key.ShiftLeft:
                    threshold = 0.5f;
                    break;
                case Key.ControlLeft:
                    threshold = 0.25f;
                    break;
            }

            return base.OnKeyDown(e);
        }

        protected override bool OnKeyUp(KeyUpEvent e)
        {
            //Returns to normal
            threshold = 1;
            return base.OnKeyUp(e);
        }

        private class ArrowButton : GDEButton
        {
            private float threshold = 1;
            
            public ArrowButton([NotNull] BindableDouble Value, bool Flipped = false)
            {
                Text = Flipped ? "<" : ">";
                BackgroundColour = GDEColors.FromHex("333333");
                Height = 20;
                Width = 20;

                Action = () =>
                {
                    var newValue = Value.Value += Flipped ? -threshold : threshold;
                    if (newValue < Value.MaxValue || newValue > Value.MinValue)
                        Value.Value = newValue;
                    else
                    {
                        Value.TriggerChange();
                        SpriteText.Colour = GDEColors.FromHex("595959");
                    }
                };
            }

            protected override bool OnKeyDown(KeyDownEvent e)
            {
                switch (e.Key)
                {
                    case Key.ShiftLeft:
                        threshold = 0.5f;
                        break;
                    case Key.ControlLeft:
                        threshold = 0.25f;
                        break;
                }

                return base.OnKeyDown(e);
            }

            protected override bool OnKeyUp(KeyUpEvent e)
            {
                //Returns to normal
                threshold = 1;
                return base.OnKeyUp(e);
            }
        }

        private class NumberedTextBox : ShadowedTextBox
        {
            protected override bool CanAddCharacter(char character) =>
                char.IsNumber(character) || character == '.' || character == '-';

            public NumberedTextBox()
            {
                BackgroundUnfocused = GDEColors.FromHex("262626");
                BackgroundFocused = GDEColors.FromHex("303030");
                BackgroundCommit = GDEColors.FromHex("404040");
                CornerRadius = 0;
            }

            protected override Drawable GetDrawableCharacter(char c) => new SpriteText
            {
                Text = c.ToString(),
                Font = GDEFont.GetFont(Typeface.Digitall, CalculatedTextSize),
            };
        }
    }
}