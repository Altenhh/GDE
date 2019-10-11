using GDAPI.Utilities.Objects.GeometryDash.LevelObjects;
using GDEdit.App.Level.Object;
using osu.Framework.Graphics;

namespace GDEdit.App.Level
{
    public static class ObjectManager
    {
        public static IGDObject[] ObjectClasses = 
        {
            new GDObject()
        };
            
        public static Drawable GetAppropriateObject(GeneralObject obj)
        {
            foreach (var objClass in ObjectClasses)
            {
                if (obj.ObjectID == objClass.ID)
                    return objClass.CreateDrawable();
            }
            
            // Defaults to the normal 30x30 square texture
            return new GDObject().CreateDrawable();
        }
    }
}