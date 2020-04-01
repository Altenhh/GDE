using System;
using System.Collections.Generic;
using GDAPI.Application;
using GDEdit.App.Graphics;
using GDEdit.App.Overlays;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;

namespace GDEdit.Tests.Visual.TestSceneMenu
{
    public class TestSceneLevelOverlay : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(LevelOverlay)
        };

        private Database collection = new DatabaseCollection()[0];

        [SetUpSteps]
        public void Setup()
        {
            Add(new Box
            {
                Colour = GDEColour.Gray20,
                RelativeSizeAxes = Axes.Both
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            collection.LevelDataSetCompleted += async () =>
            {
                var level = collection.UserLevels[0];
                await level.InitializeLoadingLevelString();

                Scheduler.Add(() =>
                {
                    var overlay = new LevelOverlay(level, collection);

                    Add(overlay);
                    AddStep("Toggle Visibility", () => overlay.ToggleVisibility());
                });
            };
        }
    }
}