using GDE.App.Main.Graphics.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace GDE.App.Main.Panels
{
    public class CloseButton : Button
    {
        public CloseButton()
        {
            Children = new Drawable[]
            {
                new SpriteIcon
                {
                    Icon = FontAwesome.Regular.TimesCircle,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both
                }
            };
        }
    }
}
