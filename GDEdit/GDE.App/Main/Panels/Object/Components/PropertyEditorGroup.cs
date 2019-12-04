using GDAPI.Objects.GeometryDash.LevelObjects;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using System;

namespace GDE.App.Main.Panels.Object.Components
{
    public abstract class PropertyEditorGroup : Container
    {
        protected SpriteText GroupTitle;

        public PropertyEditorGroup(string groupTitle)
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            Add(GroupTitle = new SpriteText
            {
                Text = groupTitle,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft
            });
            AddRange(GetExtraDrawables());
        }

        public abstract void UpdateValues(LevelObjectCollection levelObjects);

        protected abstract Drawable[] GetExtraDrawables();
    }

    public abstract class PropertyEditorGroup<TDrawable, TValue> : PropertyEditorGroup
        where TDrawable : Drawable, IHasCurrentValue<TValue>, new()
    {
        public PropertyEditorGroup(string groupTitle) : base(groupTitle) { }

        protected virtual TDrawable GenerateItemDrawable()
        {
            return new TDrawable
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Width = 125,
            };
        }
        protected LabelledPropertyItemContainer GetLabelledContainer(Action<ValueChangedEvent<TValue>> del)
        {
            return GetLabelledContainer(del, Anchor.Centre, Anchor.Centre);
        }
        protected LabelledPropertyItemContainer GetLabelledContainer(Action<ValueChangedEvent<TValue>> del, Anchor anchor, Anchor origin, MarginPadding padding = default)
        {
            var container = GetLabelledContainer(anchor, origin, padding);
            container.ValueChanged += del;
            return container;
        }
        protected LabelledPropertyItemContainer GetLabelledContainer() => GetLabelledContainer(Anchor.Centre, Anchor.Centre);
        protected LabelledPropertyItemContainer GetLabelledContainer(Anchor anchor, Anchor origin, MarginPadding padding = default) => new LabelledPropertyItemContainer(GroupTitle.Text, this)
        {
            RelativePositionAxes = Axes.X,
            Anchor = anchor,
            Origin = origin,
            Padding = padding
        };

        protected sealed class LabelledPropertyItemContainer : Container
        {
            public SpriteText PropertyTitle;
            public TDrawable Item;

            public Bindable<TValue> Bindable => Item.Current;
            public TValue Value
            {
                get => Bindable.Value;
                set => Bindable.Value = value;
            }

            public event Action<ValueChangedEvent<TValue>> ValueChanged
            {
                add => Bindable.ValueChanged += value;
                remove => Bindable.ValueChanged -= value;
            }

            public LabelledPropertyItemContainer(string propertyTitle, PropertyEditorGroup<TDrawable, TValue> instance)
            {
                RelativeSizeAxes = Axes.Y;
                Width = 125;

                Children = new Drawable[]
                {
                    PropertyTitle = new SpriteText
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Padding = new MarginPadding { Right = 5 },
                        Text = propertyTitle,
                    },
                    Item = instance.GenerateItemDrawable(),
                };
            }
        }
    }
}