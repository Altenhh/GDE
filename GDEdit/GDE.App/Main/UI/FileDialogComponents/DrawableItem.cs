using GDE.App.Main.Colors;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;

namespace GDE.App.Main.UI.FileDialogComponents
{
    public class DrawableItem : Container, IHasFilterTerms
    {
        private static readonly IconUsage fileIcon = FontAwesome.Regular.FileAlt;
        private static readonly IconUsage directoryIcon = FontAwesome.Regular.Folder;

        public const float DefaultHeight = 30;

        private string itemName = "";
        private ItemType type;

        private readonly BindableBool selected = new BindableBool(false);

        private Color4 selectedForegroundColor = GDEColors.FromHex("66ccff");
        private Color4 deselectedForegroundColor = Color4.White;
        private Color4 selectedBackgroundColor = GDEColors.FromHex("606060");
        private Color4 hoveredBackgroundColor = GDEColors.FromHex("404040");
        private Color4 deselectedBackgroundColor = new Color4(0, 0, 0, 0);

        private readonly Box background;
        private readonly SpriteText text;
        private readonly SpriteIcon icon;

        public Action<DrawableItem> OnDoubleClicked;
        public Action<DrawableItem> OnClicked;
        public Action<DrawableItem> OnSelected;

        public bool IsFile => ItemType == ItemType.File;
        public bool IsDirectory => ItemType == ItemType.Directory;

        public ItemType ItemType
        {
            get => type;
            set => icon.Icon = GetIcon(type = value);
        }

        public string ItemName
        {
            get => itemName;
            set => text.Text = itemName = value;
        }

        public bool Selected
        {
            get => selected.Value;
            set => selected.Value = value;
        }

        public Color4 Color
        {
            get => selectedForegroundColor;
            set => text.Colour = icon.Colour = selectedForegroundColor = value;
        }

        public IEnumerable<string> FilterTerms => new[]
        {
            itemName
        };

        public DrawableItem() : this("") { }
        public DrawableItem(string itemName, ItemType itemType = ItemType.File)
        {
            RelativeSizeAxes = Axes.X;
            Height = DefaultHeight;
            CornerRadius = 5;
            Masking = true;

            AddRange(new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = deselectedBackgroundColor,
                },
                new FillFlowContainer
                {
                    Spacing = new Vector2(5, 0),
                    Direction = FillDirection.Horizontal,
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Padding = new MarginPadding { Left = 5 },
                    Children = new Drawable[]
                    {
                        icon = new SpriteIcon
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Size = new Vector2(25),
                        },
                        text = new SpriteText
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                        }
                    }
                },
            });

            ItemName = itemName;
            ItemType = itemType;

            selected.ValueChanged += HandleSelectionChanged;
        }

        public void FlashError() => this.FlashColour(Color4.Red, 500, Easing.OutQuint);

        public void ToggleSelection() => selected.Toggle();

        public bool MatchesNameAndType(string name, ItemType type) => ItemName == name && ItemType == type;

        public string GetPathSuffix() => $@"{ItemName}{(IsDirectory ? "\\" : "")}";

        private void HandleSelectionChanged(ValueChangedEvent<bool> value)
        {
            Color4 newForegroundColor = value.NewValue ? selectedForegroundColor : deselectedForegroundColor;
            icon.FadeColour(newForegroundColor, 200);
            text.FadeColour(newForegroundColor, 200);
            background.FadeColour(value.NewValue ? selectedBackgroundColor : GetHoverColor(), 200);
            if (value.NewValue)
                OnSelected?.Invoke(this);
        }

        private Color4 GetHoverColor() => IsHovered ? hoveredBackgroundColor : deselectedBackgroundColor;

        protected override bool OnHover(HoverEvent e)
        {
            if (!Selected)
                background.FadeColour(hoveredBackgroundColor, 200);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!Selected)
                background.FadeColour(deselectedBackgroundColor, 200);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            ToggleSelection();
            OnClicked?.Invoke(this);
            return base.OnClick(e);
        }
        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            // This is never invoked, needs to be fixed
            Selected = true;
            OnDoubleClicked?.Invoke(this);
            return base.OnDoubleClick(e);
        }

        private static IconUsage GetIcon(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.File:
                    return fileIcon;
                case ItemType.Directory:
                    return directoryIcon;
            }
            throw new ArgumentException("Invalid item type.");
        }

        public override string ToString() => ItemName;
    }

    public enum ItemType
    {
        File,
        Directory
    }
}
