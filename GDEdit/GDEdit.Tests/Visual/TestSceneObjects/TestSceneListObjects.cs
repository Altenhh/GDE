using System;
using System.Collections.Generic;
using GDEdit.App.Level;
using GDEdit.App.Level.Object;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;

namespace GDEdit.Tests.Visual.TestSceneObjects
{
    public class TestSceneListObjects : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(GDObject),
            typeof(ObjectManager),
            typeof(Sawblade)
        };

        [Resolved]
        private TextureStore Textures { get; set; }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            FillFlowContainer container;

            Add(new BasicScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = container = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y
                }
            });

            for (var i = 0; i < 1911; i++)
            {
                var obj = ObjectManager.GetAppropriateObject(i, Textures);

                if (obj != null)
                    container.Add(obj);
            }
        }
    }
}