﻿using GDE.App.Main.Colors;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace GDE.App.Main.UI.Containers
{
    public class GDEScrollContainer : ScrollContainer<Drawable>
    {
        /// <summary>
        /// Allows controlling the scroll bar from any position in the container using the right mouse button.
        /// Uses the value of <see cref="DistanceDecayOnRightMouseScrollbar"/> to smoothly scroll to the dragged location.
        /// </summary>
        public bool RightMouseScrollbar = false;

        /// <summary>
        /// Controls the rate with which the target position is approached when performing a relative drag. Default is 0.02.
        /// </summary>
        public double DistanceDecayOnRightMouseScrollbar = 0.02;

        private bool shouldPerformRightMouseScroll(MouseButtonEvent e) => RightMouseScrollbar && e.Button == MouseButton.Right;

        private void scrollToRelative(float value) => ScrollTo(Clamp((value - Scrollbar.DrawSize[ScrollDim] / 2) / Scrollbar.Size[ScrollDim]), true, DistanceDecayOnRightMouseScrollbar);

        private bool mouseScrollBarDragging;

        protected override bool IsDragging => base.IsDragging || mouseScrollBarDragging;

        public GDEScrollContainer(Direction scrollDirection = Direction.Vertical)
            : base(scrollDirection) { }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (shouldPerformRightMouseScroll(e))
            {
                scrollToRelative(e.MousePosition[ScrollDim]);
                return true;
            }

            return base.OnMouseDown(e);
        }

        protected override bool OnDrag(DragEvent e)
        {
            if (mouseScrollBarDragging)
            {
                scrollToRelative(e.MousePosition[ScrollDim]);
                return true;
            }

            return base.OnDrag(e);
        }

        protected override bool OnDragStart(DragStartEvent e)
        {
            if (shouldPerformRightMouseScroll(e))
            {
                mouseScrollBarDragging = true;
                return true;
            }

            return base.OnDragStart(e);
        }

        protected override bool OnDragEnd(DragEndEvent e)
        {
            if (mouseScrollBarDragging)
            {
                mouseScrollBarDragging = false;
                return true;
            }

            return base.OnDragEnd(e);
        }

        protected override ScrollbarContainer CreateScrollbar(Direction direction) => new GDEScrollbar(direction);

        protected class GDEScrollbar : ScrollbarContainer
        {
            private const float dim_size = 10;

            private Color4 hoverColour;
            private Color4 defaultColour;
            private Color4 highlightColour;

            private readonly Box box;

            public GDEScrollbar(Direction scrollDir)
                : base(scrollDir)
            {
                Blending = BlendingMode.Additive;

                CornerRadius = 5;

                const float margin = 3;

                Margin = new MarginPadding
                {
                    Left = scrollDir == Direction.Vertical ? margin : 0,
                    Right = scrollDir == Direction.Vertical ? margin : 0,
                    Top = scrollDir == Direction.Horizontal ? margin : 0,
                    Bottom = scrollDir == Direction.Horizontal ? margin : 0,
                };

                Masking = true;
                Child = box = new Box { RelativeSizeAxes = Axes.Both };

                ResizeTo(1);
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Colour = defaultColour = GDEColors.FromHex("888");
                hoverColour = GDEColors.FromHex("FFF");
                highlightColour = GDEColors.FromHex("88b300");
            }

            public override void ResizeTo(float val, int duration = 0, Easing easing = Easing.None)
            {
                Vector2 size = new Vector2(dim_size)
                {
                    [(int)ScrollDirection] = val
                };
                this.ResizeTo(size, duration, easing);
            }

            protected override bool OnHover(HoverEvent e)
            {
                this.FadeColour(hoverColour, 100);
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                this.FadeColour(defaultColour, 100);
            }

            protected override bool OnMouseDown(MouseDownEvent e)
            {
                if (!base.OnMouseDown(e)) return false;

                //note that we are changing the colour of the box here as to not interfere with the hover effect.
                box.FadeColour(highlightColour, 100);
                return true;
            }

            protected override bool OnMouseUp(MouseUpEvent e)
            {
                if (e.Button != MouseButton.Left) return false;

                box.FadeColour(Color4.White, 100);

                return base.OnMouseUp(e);
            }
        }
    }
}