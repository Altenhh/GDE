using osu.Framework.Graphics;
using osuTK;

namespace GDEdit.App.Level.Object
{
    public interface IGDObject
    {
        int ID { get; set; }
        Vector2 Offset { get; set; }
        Drawable[] CreateDrawable();
    }
}