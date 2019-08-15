using GDAPI.Utilities.Objects.GeometryDash.LevelObjects;
using osu.Framework.Bindables;

namespace GDE.App.Main.Panels
{
    public class ObjectEditor : Panel
    {
        public Bindable<GeneralObject> ObjectBindable;

        public ObjectEditor(GeneralObject Object)
        {
            ObjectBindable = new Bindable<GeneralObject>(Object);
        }


    }
}
