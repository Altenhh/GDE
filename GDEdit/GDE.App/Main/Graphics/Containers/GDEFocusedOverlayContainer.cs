using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using GDE.App.Main.Overlays;

namespace GDE.App.Main.Graphics.Containers
{
    public class GDEFocusedOverlayContainer : FocusedOverlayContainer, IKeyBindingHandler<GlobalAction>
    {
        protected override bool BlockNonPositionalInput => true;

        /// <summary>Temporary to allow for overlays in the main screen content to not dim theirselves.
        /// Should be eventually replaced by dimming which is aware of the target dim container (traverse parent for certain interface type?).</summary>
        protected virtual bool DimMainContent => true;

        [Resolved]
        private GDEApp gdeApp { get; set; }

        protected readonly Bindable<OverlayActivation> OverlayActivationMode = new Bindable<OverlayActivation>(OverlayActivation.All);

        [BackgroundDependencyLoader]
        private void load()
        {
            StateChanged += onStateChanged;
        }

        /// <summary>Whether mouse input should be blocked screen-wide while this overlay is visible.
        /// Performing mouse actions outside of the valid extents will hide the overlay.</summary>
        public virtual bool BlockScreenWideMouse => BlockPositionalInput;

        // receive input outside our bounds so we can trigger a close event on ourselves.
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => BlockScreenWideMouse || base.ReceivePositionalInputAt(screenSpacePos);

        protected override bool OnClick(ClickEvent e)
        {
            if (!base.ReceivePositionalInputAt(e.ScreenSpaceMousePosition))
            {
                State = Visibility.Hidden;
                return true;
            }

            return base.OnClick(e);
        }

        public virtual bool OnPressed(GlobalAction action)
        {
            switch (action)
            {
                case GlobalAction.Back:
                    State = Visibility.Hidden;
                    return true;

                case GlobalAction.Select:
                    return true;
            }

            return false;
        }

        public bool OnReleased(GlobalAction action) => false;

        private void onStateChanged(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.Visible:
                    if (OverlayActivationMode.Value != OverlayActivation.Disabled)
                    {
                        if (BlockScreenWideMouse && DimMainContent) gdeApp?.AddBlockingOverlay(this);
                    }
                    else
                        State = Visibility.Hidden;

                    break;

                case Visibility.Hidden:
                    if (BlockScreenWideMouse) gdeApp?.RemoveBlockingOverlay(this);
                    break;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            gdeApp?.RemoveBlockingOverlay(this);
        }
    }
}
