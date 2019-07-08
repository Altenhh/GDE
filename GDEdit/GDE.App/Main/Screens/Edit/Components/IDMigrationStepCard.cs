﻿using System;
using System.Collections.Generic;
using System.Linq;
using GDE.App.Main.Colors;
using GDEdit.Application;
using GDEdit.Utilities.Objects.General;
using GDEdit.Utilities.Objects.GeometryDash;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using static System.Convert;

namespace GDE.App.Main.Screens.Edit.Components
{
    public class IDMigrationStepCard : ClickableContainer, IFilterable
    {
        private bool matchingFilter, filteringActive;
        private int index;

        private Box selectionBar;
        private Box hoverBox;
        private Box stepIndexContainerBackground;
        private Container stepIndexContainer;
        private DraggableCardContainer stepDragContainer;
        private SpriteText stepIndex;
        private SpriteText rightArrow, sourceText, targetText;

        public readonly Bindable<bool> Selected = new Bindable<bool>(false);

        public int Index
        {
            get => index;
            set => stepIndex.Text = (index = value).ToString();
        }

        public readonly SourceTargetRange StepRange;

        public Action<DragEvent> CardDragged;

        public bool MatchingFilter
        {
            set => Alpha = ToInt32(!filteringActive || (matchingFilter = value));
        }
        public bool FilteringActive
        {
            set => Alpha = ToInt32(!(filteringActive = value) || matchingFilter);
        }

        public IEnumerable<string> FilterTerms => new List<string>
        {
            StepRange.ToString(),
            StepRange.SourceToString(),
            StepRange.TargetToString(),
            StepRange.SourceFrom.ToString(),
            StepRange.SourceTo.ToString(),
            StepRange.TargetFrom.ToString(),
            StepRange.TargetTo.ToString()
        };
        
        public IDMigrationStepCard(SourceTargetRange range)
        {
            StepRange = range;

            CornerRadius = 5;
            Masking = true;

            RelativeSizeAxes = Axes.X;
            Height = 25;

            Children = new Drawable[]
            {
                hoverBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = GDEColors.FromHex("181818")
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Y,

                    Children = new Drawable[]
                    {
                        selectionBar = new Box
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = 5,
                            Colour = GDEColors.FromHex("808080"),
                            Alpha = 0.5f, // Transparency will allow the background to lighten the container up
                        },
                        stepIndexContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = 25,

                            Children = new Drawable[]
                            {
                                stepIndexContainerBackground = new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = new ColourInfo
                                    {
                                        BottomLeft = GDEColors.FromHex("606060"),
                                        BottomRight = GDEColors.FromHex("606060"),
                                        TopLeft = GDEColors.FromHex("606060"),
                                        TopRight = GDEColors.FromHex("606060"),
                                    },
                                    Alpha = 0.5f,
                                },
                                stepIndex = new SpriteText
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Text = Index.ToString(),
                                    Font = new FontUsage(size: 20),
                                },
                            },
                        },

                    }
                },
                // TODO: Migrate to a class called IDMigrationStepText
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,

                    Children = new Drawable[]
                    {
                        sourceText = new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            X = -100,
                            Text = range.SourceToString(),
                            Font = new FontUsage(size: 20),
                        },
                        rightArrow = new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = ">",
                            Font = new FontUsage(size: 20),
                            Alpha = 0.5f,
                        },
                        targetText = new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            X = 100,
                            Text = range.TargetToString(),
                            Font = new FontUsage(size: 20),
                        },
                    }
                },
                stepDragContainer = new DraggableCardContainer
                {
                    RelativeSizeAxes = Axes.Y,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Children = new Drawable[]
                    {
                        new SpriteIcon
                        {
                            Icon = FontAwesome.Solid.Bars,
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.7f),
                            Y = -1, // I have no fucking idea why this happens, but it's necessary to make the sprite appear centered
                            Alpha = 0.6f,
                        }
                    },
                    CardDragged = e =>
                    {
                        Position += e.Delta;
                        CardDragged?.Invoke(e);
                    },
                    Size = new Vector2(25, 1),
                },
            };

            Selected.ValueChanged += OnSelected;
            StepRange.SourceTargetRangeChanged += (a, b, c, d) =>
            {
                sourceText.Text = StepRange.SourceToString();
                targetText.Text = StepRange.TargetToString();
            };
        }

        /// <summary>Initializes the right arrow's animation. This function should be only called once.</summary>
        public void InitializeArrowAnimation()
        {
            rightArrow
                .MoveToOffset(new Vector2(20, 0), 500, Easing.InQuint).FadeTo(0, 500, Easing.InQuint)
                .Then()
                .MoveToOffset(new Vector2(-40, 0)) // Reset label position
                .Then()
                .MoveToOffset(new Vector2(20, 0), 500, Easing.OutQuint).FadeTo(0.5f, 500, Easing.OutQuint)
                .Loop(2000);
        }

        private void OnSelected(ValueChangedEvent<bool> value)
        {
            var newColor = GDEColors.FromHex(value.OldValue ? "808080" : "00ffb8");
            selectionBar.FadeColour(newColor, 200);
            stepIndexContainerBackground.FadeColour(new ColourInfo
            {
                BottomLeft = newColor.Darken(0.25f),
                TopLeft = newColor.Darken(0.25f),
                BottomRight = GDEColors.FromHex("606060"),
                TopRight = GDEColors.FromHex("606060"),
            }, 200);
        }

        protected override bool OnHover(HoverEvent e)
        {
            hoverBox.FadeColour(GDEColors.FromHex("303030"), 500);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            hoverBox.FadeColour(GDEColors.FromHex("181818"), 500);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            Selected.Value = !Selected.Value;
            return base.OnClick(e);
        }

        private class DraggableCardContainer : Container
        {
            public Action<DragEvent> CardDragged;

            protected override bool OnDrag(DragEvent e)
            {
                CardDragged?.Invoke(e);
                return base.OnDrag(e);
            }
        }
    }
}
