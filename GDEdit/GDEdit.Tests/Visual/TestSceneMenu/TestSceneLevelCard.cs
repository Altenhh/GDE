using System;
using System.Collections.Generic;
using GDEdit.App.Screens.Menu;
using osu.Framework.Testing;

namespace GDEdit.Tests.Visual.TestSceneMenu
{
    public class TestSceneLevelCard : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(LevelCard)
        };

        public TestSceneLevelCard()
        {
            Add(new LevelCard());
        }
    }
}