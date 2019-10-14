using System.Globalization;
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
using osuTK.Input;

namespace GDE.App.Main.UI
{
    public class GDENumberTextBox : Container
    {
        private float threshold = 1;

        public BindableDouble Bindable;

        [BackgroundDependencyLoader]
        private void load()
        {
            if (Bindable == null)
                Bindable = new BindableDouble();
            
            NumberedTextBox textBox;
            AutoSizeAxes = Axes.Y;

            var arrowLeft = new ArrowButton(Bindable, true);
            var arrowRight = new ArrowButton(Bindable);
            
            Add(new FillFlowContainer
            {
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(-3, 0),
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new Drawable[]
                {
                    arrowLeft,
                    textBox = new NumberedTextBox
                    {
                        Height = 20,
                        RelativeSizeAxes = Axes.X,
                        Text = Bindable.Value.ToString(),
                        Depth = -1
                    },
                    arrowRight
                }
            });

            textBox.OnCommit += (sender, text) =>
            {
                double.TryParse(sender.Text, NumberStyles.Any, null, out double numValue);

                if (numValue <= Bindable.MaxValue || numValue >= Bindable.MinValue)
                    arrowLeft.Value.Value = arrowRight.Value.Value = Bindable.Value = numValue;
                else
                    Bindable.TriggerChange();
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
                    // there was an attempt
                    var newValue = val.NewValue;
                    
                    bool onMax = newValue + 1 > value.MaxValue;
                    bool onMin = newValue - 1 < value.MinValue;

                    if ((flipped && onMax) || onMin)
                        SpriteText.Colour = GDEColors.FromHex("595959");
                    else
                        SpriteText.Colour = Color4.White;
                };

                Action = () =>
                {
                    var newValue = Value.Value += flipped ? -threshold : threshold;

                    if (newValue < value.MaxValue || newValue > value.MinValue)
                        value.Value = Value.Value = newValue;
                    else
                    {
                        Value.TriggerChange();
                        value.TriggerChange();
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
            protected override bool CanAddCharacter(char character) => char.IsNumber(character) || character == '.' || character == '-';

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