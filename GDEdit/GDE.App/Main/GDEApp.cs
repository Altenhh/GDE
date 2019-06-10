using GDE.App.Main.Colors;
using GDE.App.Main.Graphics.Containers;
using GDE.App.Main.Screens.Menu;
using GDE.App.Main.Toasts;
using GDE.App.Main.Tools;
using GDE.App.Updater;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GDE.App.Main
{
    public class GDEApp : GDEAppBase
    {
        private ToastNotification notification;

        private readonly List<OverlayContainer> overlays = new List<OverlayContainer>();

        private readonly List<OverlayContainer> visibleBlockingOverlays = new List<OverlayContainer>();

        private Container screenContainer;

        private void updateBlockingOverlayFade() =>
            screenContainer.FadeColour(visibleBlockingOverlays.Any() ? GDEColors.Gray(0.5f) : Color4.White, 500, Easing.OutQuint);

        public void AddBlockingOverlay(OverlayContainer overlay)
        {
            if (!visibleBlockingOverlays.Contains(overlay))
                visibleBlockingOverlays.Add(overlay);
            updateBlockingOverlayFade();
        }

        public void RemoveBlockingOverlay(OverlayContainer overlay)
        {
            visibleBlockingOverlays.Remove(overlay);
            updateBlockingOverlayFade();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new GlobalActionContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new ScreenStack(new MainScreen())
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                    }   
                },
                notification = new ToastNotification
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Size = new Vector2(600, 30),
                    Margin = new MarginPadding
                    {
                        Bottom = 5
                    }
                }
            };

            new RavenLogger(this);
        }

        protected override void LoadComplete()
        {
            if (RuntimeInfo.OS == RuntimeInfo.Platform.Windows)
                Add(new SquirrelUpdateManager());

            base.LoadComplete();
        }

        protected override bool ExceptionHandler(Exception arg)
        {
            //fuck my life
            //notification.text.Text = $"An error has occurred, Please report this to the devs. (Err: {arg.Message})";
            //notification.ToggleVisibility();

            return base.ExceptionHandler(arg);
        }
    }
}
