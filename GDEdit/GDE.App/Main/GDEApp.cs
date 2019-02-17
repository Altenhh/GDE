﻿using GDE.App.Main.Screens.Menu;
using GDE.App.Main.Screens;
using GDE.App.Main.Toasts;
using GDE.App.Main.Tools;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;
using System;
using osu.Framework.Input.Events;
using osuTK.Input;
using osu.Framework.Input.Bindings;
using GDE.App.Main.Containers;

namespace GDE.App.Main
{
    public class GDEApp : GDEAppBase
    {
        private MainScreen screen;
        private ToastNotification notification;

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
                        screen = new MainScreen(),
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
                    }
                }
            };

            new RavenLogger(this);
        }

        protected override bool ExceptionHandler(Exception arg)
        {
            notification.text.Text = $"An error has occurred, Please report this to the devs. (Err: {arg.Message})";
            notification.ToggleVisibility();

            return base.ExceptionHandler(arg);
        }
    }
}
