﻿using GDE.App.Main.Hitboxes;
using GDEdit.Utilities.Objects.General;
using GDEdit.Utilities.Objects.GeometryDash.ObjectHitboxes;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using System;

namespace GDE.Tests.Visual
{
    public class TestCaseHitbox : TestCase
    {
        private Hitbox hitbox;
        private RectangleHitbox rectangle;
        private SpriteText angle;
        private SpriteText radius;
        private SpriteText mousePosition;

        public TestCaseHitbox()
        {
            hitbox = new Hitbox(new Point(50), new Rectangle(100, 213));

            Children = new Drawable[]
            {
                new Box
                {
                    Colour = new Color4(64, 64, 64, 255),
                    RelativeSizeAxes = Axes.Both
                },
                rectangle = new RectangleHitbox(hitbox)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new FillFlowContainer
                {
                    FillMode = FillMode.Fill,
                    Direction = FillDirection.Vertical,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Children = new[]
                    {
                        angle = new SpriteText
                        {
                            TextSize = 25,
                        },
                        radius = new SpriteText
                        {
                            TextSize = 25,
                        },
                        mousePosition = new SpriteText
                        {
                            TextSize = 25,
                        },
                    },
                },
            };
            
            AddSliderStep("Rotation", 0d, 360d, 0d, v => rectangle.HitboxRotation = v);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            // What the actual fuck is this?
            var planePosition = e.MousePosition - rectangle.ToParentSpace(rectangle.OriginPosition + rectangle.DrawPosition);
            var p = new Point(planePosition.X - 80, -planePosition.Y);
            mousePosition.Text = $"Mouse position: {p}";
            var deg = rectangle.HitboxPosition.GetAngle(p) * 180 / Math.PI;
            angle.Text = $"Angle: {deg}";
            radius.Text = $"Radius: {hitbox.GetRadiusAtRotation(deg)}";
            return base.OnMouseMove(e);
        }
    }
}
