using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using GDE.App.Main.Graphics;

namespace GDE.App.Main.Overlays.ObjectEditor
{
    public abstract class EditableItem<T> : Container
    {
        protected abstract Drawable CreateControl();

        protected Drawable Control { get; }

        private IHasCurrentValue<T> controlWithCurrent => Control as IHasCurrentValue<T>;

        protected override Container<Drawable> Content => FlowContent;

        protected readonly FillFlowContainer FlowContent;

        protected virtual FillDirection FillFlowDirection => FillDirection.Full;

        protected virtual bool AppearBeforeText => false;

        private SpriteText text;

        public virtual string LabelText
        {
            get => text?.Text ?? string.Empty;
            set
            {
                if (text == null)
                {
                    // construct lazily for cases where the label is not needed (may be provided by the Control).
                    Add(text = new SpriteText { Font = GDEFont.GetFont(size:25), Position = new Vector2(0, -5) });
                    FlowContent.SetLayoutPosition(text, AppearBeforeText ? 1 : -1);
                }

                text.Text = value;
            }
        }

        private Bindable<T> bindable;

        public virtual Bindable<T> Bindable
        {
            get => bindable;
            set
            {
                bindable = value;
                controlWithCurrent?.Current.BindTo(bindable);
            }
        }

        protected EditableItem()
        {
            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;

            InternalChildren = new Drawable[]
            {
                FlowContent = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillFlowDirection,
                    Padding = new MarginPadding { Left = 10 },
                    Child = Control = CreateControl()
                },
            };
        }
    }
}
