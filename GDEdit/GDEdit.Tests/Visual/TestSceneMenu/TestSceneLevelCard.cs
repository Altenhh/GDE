using System;
using System.Collections.Generic;
using GDAPI.Objects.GeometryDash.General;
using GDEdit.App.Screens.Menu;
using osu.Framework.Testing;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

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
            LevelCard card;
            Container container;
            
            Add(new Box
            {
                Colour = Color4.White,
                RelativeSizeAxes = Axes.Both
            });
            Add(container = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(240, 1),
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    card = new LevelCard(new Level { Name = "Test Level", Description = "This is my own completely original level, filled with butterflies and princesses. Also..." })
                }
            });
            
            AddToggleStep("Toggle details", v => card.State.Value = v ? CarouselItemState.Selected : CarouselItemState.NotSelected);
            AddSliderStep("Scale", 1f, 5f, 2f, s => container.Scale = new Vector2(s));
        }
    }
}