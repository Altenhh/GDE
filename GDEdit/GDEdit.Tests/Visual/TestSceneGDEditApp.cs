using System;
using System.Collections.Generic;
using GDEdit.App;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace GDEdit.Tests.Visual
{
    public class TestSceneGDEditApp : TestScene
    {
        private GDEMain app;

        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(GDEMain)
        };

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            app = new GDEMain();
            app.SetHost(host);

            Add(app);
        }
    }
}