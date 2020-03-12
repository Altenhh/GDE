using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
namespace GDEdit.App.Screens.Menu
{
    public class MainMenu : Screen
    {
        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    Colour = Color4Extensions.FromHex(@"1A1A1A"),
                    RelativeSizeAxes = Axes.Both
                },
                new Container
                {
                    Height = 250,
                    RelativeSizeAxes = Axes.X,
                    Masking = true,
                    MaskingSmoothness = 1,
                    Children = new Drawable[]
                    {
                        new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            FillMode = FillMode.Fill,
                            Texture = store.Get("https://i.imgur.com/SM58hh7.jpg"),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        },
                        new Box
                        {
                            Colour = ColourInfo.GradientVertical(Color4Extensions.FromHex("1A1A1A").Opacity(0),
                                                                           Color4Extensions.FromHex("1A1A1A")),
                            RelativeSizeAxes = Axes.Both
                        },
                    }
                }
            });
        }
    }
}