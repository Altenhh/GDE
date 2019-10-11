using osu.Framework.Graphics;
using osuTK;

namespace GDEdit.App.Level.Object
{
    public class GDObject : IGDObject
    {
        public int ID { get; set; } = 0;
        public Vector2 Offset { get; set; }

        public Drawable CreateDrawable() => null;
    }
}