using osu.Framework.Graphics;
using osuTK;

namespace GDEdit.App.Level.Object
{
    public interface IGDObject
    {
        /// <summary>ID of the object.</summary>
        int ID { get; set; }
        /// <summary>Origin offset.</summary>
        Vector2 Offset { get; set; }
        /// <summary>Create what you would see on the Level Preview.</summary>
        /// <returns>The correct interpretation of the object.</returns>
        Drawable CreateDrawable();
    }
}