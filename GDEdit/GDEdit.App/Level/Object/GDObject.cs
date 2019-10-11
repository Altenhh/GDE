using GDAPI.Utilities.Objects.GeometryDash.LevelObjects;
using osu.Framework.Graphics;
using osuTK;

namespace GDEdit.App.Level.Object
{
    public class GDObject : GeneralObject, IGDObject
    {
        public Vector2 Offset { get; set; } = Vector2.Zero;

        public Drawable[] CreateDrawable()
        {
            return null;
        }
    }
}