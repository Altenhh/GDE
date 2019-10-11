using GDAPI.Utilities.Objects.GeometryDash.LevelObjects;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GDEdit.App.Level.Object
{
    public class GDObject : GeneralObject, IGDObject
    {
        public virtual int ID { get; set; }
        public virtual Vector2 Offset { get; set; }
        
        [Cached]
        private LargeTextureStore store { get; set; }

        public virtual Drawable CreateDrawable()
            => new Sprite
               {
                    Texture = store.Get($"Objects/{ID}"),
                    Size = new Vector2(30),
                    FillMode = FillMode.Fit
               };
    }
}