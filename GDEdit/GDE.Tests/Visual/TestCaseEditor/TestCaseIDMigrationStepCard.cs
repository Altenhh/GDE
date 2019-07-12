﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GDE.App.Main.Screens.Edit;
using GDE.App.Main.Screens.Edit.Components;
using GDEdit.Application.Editor;
using GDEdit.Utilities.Objects.General;
using GDEdit.Utilities.Objects.GeometryDash;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using static GDE.App.Main.Screens.Edit.Components.IDMigrationStepList;

namespace GDE.Tests.Visual.TestCaseEditor
{
    public class TestCaseIDMigrationStepCard : TestCase
    {
        private IDMigrationStepCard card;

        public override IReadOnlyList<Type> RequiredTypes => new Type[] { typeof(IDMigrationStepCard) };

        public TestCaseIDMigrationStepCard()
        {
            card = new IDMigrationStepCard(new SourceTargetRange(1, 2, 3))
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Width = 0.5f,
                Index = 0,
            };
            card.CardClicked += (c, e) => c.ToggleSelection();
            Add(card);

            AddStep("Set index to 0", () => card.Index = 0);
            AddAssert("Check index is 0", () => card.Index == 0);
            AddStep("Set index to 3", () => card.Index = 3);
            AddAssert("Check index is 3", () => card.Index == 3);
            AddStep("Set index to 9999", () => card.Index = 9999); // Something needs to be done so that text does not overflow out of the index box
            AddAssert("Check index is 9999", () => card.Index == 9999);
            AddStep("Set index to 69", () => card.Index = 69);
            AddAssert("Check index is 69", () => card.Index == 69);

            AddStep("Set SourceTo to 14", () => card.StepRange.SourceTo = 14);
            AddAssert("Check SourceTo is 14", () => card.StepRange.SourceTo == 14);
            AddAssert("Check TargetTo is 16", () => card.StepRange.TargetTo == 16);
            AddStep("Set SourceFrom to 10", () => card.StepRange.SourceFrom = 10);
            AddAssert("Check SourceFrom is 10", () => card.StepRange.SourceFrom == 10);
            AddAssert("Check TargetTo is 7", () => card.StepRange.TargetTo == 7);
            AddStep("Set SourceFrom to 3", () => card.StepRange.SourceFrom = 3);
            AddStep("Set SourceTo to 3", () => card.StepRange.SourceTo = 3);
            AddAssert("Check SourceFrom is 3", () => card.StepRange.SourceFrom == 3);
            AddAssert("Check SourceTo is 3", () => card.StepRange.SourceTo == 3);
            AddAssert("Check TargetFrom is 3", () => card.StepRange.TargetFrom == 3);
            AddAssert("Check TargetTo is 3", () => card.StepRange.TargetTo == 3);

            AddStep("Select", () => card.Selected.Value = true);
            AddWaitStep("Wait for animation to be completed", 1);
            AddStep("Deselect", () => card.Selected.Value = false);
            AddWaitStep("Wait for animation to be completed", 1);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            card.InitializeArrowAnimation();
        }
    }
}
