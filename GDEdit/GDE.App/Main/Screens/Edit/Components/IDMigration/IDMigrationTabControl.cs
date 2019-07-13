﻿using GDEdit.Application.Editor;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace GDE.App.Main.Screens.Edit.Components.IDMigration
{
    public class IDMigrationTabControl : Container
    {
        private IDMigrationTabItem[] tabItems = new IDMigrationTabItem[4];
        private IDMigrationTabItem currentTab;

        private FillFlowContainer itemsContainer;

        public event Action<IDMigrationMode> TabSelected;

        public IDMigrationTabControl()
            : base()
        {
            for (int i = 0; i < 4; i++)
            {
                tabItems[i] = new IDMigrationTabItem((IDMigrationMode)i)
                {
                    // WHY THE FUCK DOES THIS NOT WORK?
                    Y = 20
                };
                tabItems[i].TabSelected += HandleTabSelected;
            }
            currentTab = this[IDMigrationMode.Groups];
            currentTab.Selected = true;

            RelativeSizeAxes = Axes.X;
            Height = 32;

            Children = new Drawable[]
            {
                itemsContainer = new FillFlowContainer
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Left = 10 },
                    Children = tabItems,
                }
            };
        }

        private IDMigrationTabItem this[IDMigrationMode mode] => tabItems[(int)mode];

        private void HandleTabSelected(IDMigrationMode newMode)
        {
            currentTab.MoveToOffset(new Vector2(0, -10), 500, Easing.OutQuint);
            currentTab.Selected = false;
            (currentTab = this[newMode]).MoveToOffset(new Vector2(0, 10), 500, Easing.InQuint);
            TabSelected?.Invoke(newMode);
        }
    }
}
