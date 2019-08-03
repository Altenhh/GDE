using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using System;
using System.Linq;

namespace GDE.App.Main.UI
{
    public class GDEBreadcrumbNavigationTextBox : TextBox
    {
        private static Color4 BlackTransparent = new Color4(0, 0, 0, 0);

        public string Separator = @"\";

        public GDEBreadcrumbNavigation<string> BreadcrumbNavigation;

        public Predicate<string> AllowChange;

        public event Action<string> OnTextChanged;

        public BindableList<string> Items
        {
            get => BreadcrumbNavigation.Items;
            set => BreadcrumbNavigation.Items.BindTo(value);
        }

        public GDEBreadcrumbNavigationTextBox()
            : base()
        {
            CornerRadius = GDEBreadcrumbNavigation<string>.DefaultCornerRadius;

            AddInternal(BreadcrumbNavigation = new GDEBreadcrumbNavigation<string>
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
            });

            TextContainer.Colour = BlackTransparent;

            OnCommit += HandleOnCommit;
        }

        private void HandleOnCommit(TextBox sender, bool newValue)
        {
            UpdateBreadcrumbs();
        }

        protected override void OnFocus(FocusEvent e)
        {
            BreadcrumbNavigation.FadeTo(0, 200, Easing.OutQuint);
            if (BreadcrumbNavigation.Items.Count > 0)
                Text = BreadcrumbNavigation.Items.Aggregate(AggregateStrings);
            TextContainer.FadeColour(Color4.White, 200, Easing.InQuint);
            base.OnFocus(e);
        }
        protected override void OnFocusLost(FocusLostEvent e)
        {
            BreadcrumbNavigation.FadeTo(1, 200, Easing.InQuint);
            Text = "";
            TextContainer.FadeColour(BlackTransparent, 200, Easing.OutQuint);
            base.OnFocusLost(e);
        }

        private void UpdateBreadcrumbs()
        {
            if (AllowChange?.Invoke(Text) ?? true)
            {
                BreadcrumbNavigation.Items.Clear();
                BreadcrumbNavigation.Items.AddRange(Text.Split(Separator).ToList());
                OnTextChanged?.Invoke(Text);
            }
            Text = "";
        }

        private string AggregateStrings(string left, string right) => $"{left}{Separator}{right}";
    }
}
