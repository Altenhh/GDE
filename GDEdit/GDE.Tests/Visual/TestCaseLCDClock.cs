﻿using GDE.App.Main.UI.LCDComponents;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using System;
using System.Collections.Generic;

namespace GDE.Tests.Visual
{
    public class TestCaseLCDClock : TestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] { typeof(LCDClock), typeof(LCDCharacterBar), typeof(LCDDigit), typeof(LCDNumber) };

        private Random r = new Random();
        private LCDNumber number;
        private LCDClock clock;
        private LCDFPSDisplay fps;

        public TestCaseLCDClock()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
            CreateDrawables();

            AddSliderStep("Value", 0, 9999999, 0, v => number.Value = v);
            AddStep("Increase number", () => number.Value += number.Value < 9999999 ? 1 : 0);
            AddStep("Decrease number", () => number.Value -= number.Value > 0 ? 1 : 0);
            AddStep("Set random value", () => number.TransformTo("Value", r.Next(0, 10000000), 1250, Easing.OutQuint));
            AddToggleStep("Include time milliseconds", v => CreateDrawables(v));
        }

        private void CreateDrawables(bool includeMS = false)
        {
            Children = new Drawable[]
            {
                number = new LCDNumber(0, 7, true)
                {
                    Value = number?.Value ?? 0,
                    Y = -100,
                },
                clock = new LCDClock(includeMS)
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Y = 100,
                },
                fps = new LCDFPSDisplay
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.33f),
                },
            };
        }
    }
}
