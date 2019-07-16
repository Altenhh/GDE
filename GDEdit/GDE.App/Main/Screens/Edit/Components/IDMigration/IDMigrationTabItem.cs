﻿using GDE.App.Main.Colors;
using GDEdit.Application.Editor;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System;

namespace GDE.App.Main.Screens.Edit.Components.IDMigration
{
    public class IDMigrationTabItem : Container
    {
        private static Color4 selectedBackgroundColor = GDEColors.FromHex("606060");
        private static Color4 hoveredBackgroundColor = GDEColors.FromHex("484848");
        private static Color4 backgroundColor = GDEColors.FromHex("303030");

        private Box background;

        private bool selected;

        public bool Selected
        {
            get => selected;
            set
            {
                bool old = selected;
                background.FadeColour((selected = value) ? selectedBackgroundColor : (IsHovered ? hoveredBackgroundColor : backgroundColor), 200);
                if (!old && value)
                    TabSelected?.Invoke(Mode);
            }
        }

        public readonly IDMigrationMode Mode;

        public event Action<IDMigrationMode> TabSelected;

        public IDMigrationTabItem(IDMigrationMode mode)
            : base()
        {
            Mode = mode;

            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            Size = new Vector2(80, 50);

            CornerRadius = 10;
            Masking = true;

            Children = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = backgroundColor,
                },
                new SpriteText
                {
                    Margin = new MarginPadding
                    {
                        Top = 5,
                    },
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = new FontUsage(size: 20),
                    Text = mode.ToString(),
                },
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (!Selected)
                background.FadeColour(hoveredBackgroundColor, 200);

            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!Selected)
                background.FadeColour(backgroundColor, 200);

            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            Selected = true;
            return base.OnClick(e);
        }
    }
}