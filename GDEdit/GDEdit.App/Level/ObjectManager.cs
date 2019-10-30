using GDEdit.App.Level.Object;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace GDEdit.App.Level
{
    public static class ObjectManager
    {
        private static readonly IGDObject[] objectClasses = 
        {
            new GDObject()
        };
            
        public static Drawable GetAppropriateObject(int id, TextureStore store)
        {
            foreach (var objClass in objectClasses)
            {
                if (id == objClass.ID)
                    return objClass.CreateDrawable();
            }
            
            // Defaults to the normal 30x30 square texture
            return new Sprite
                   {
                       Texture = store.Get($"Objects/{id}.png")
                   };
        }
    }
}