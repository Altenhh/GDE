using GDE.App.Main.Colors;
using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using System;

namespace GDE.App.Main.UI
{
    public class ToggleableValueButton<TValue> : GDEButton, IHasCurrentValue<TValue>
        where TValue : struct
    {
        private Bindable<TValue> current;

        private TValue[] values;
        private int valueIndex;

        public Bindable<TValue> Current
        {
            get => current;
            set => current.BindTo(value);
        }

        public TValue Value => values[valueIndex];
        public bool Toggled => valueIndex == 1;

        public event Action<ValueChangedEvent<TValue>> ValueChanged
        {
            add => current.ValueChanged += value;
            remove => current.ValueChanged -= value;
        }

        public ToggleableValueButton(TValue valueOff, TValue valueOn)
            : base()
        {
            values = new TValue[2] { valueOff, valueOn };
            current = new Bindable<TValue>(valueOff);
        }

        protected override bool OnClick(ClickEvent e)
        {
            valueIndex = (valueIndex + 1) % 2;
            current.Value = values[valueIndex];
            return base.OnClick(e);
        }
    }
}
