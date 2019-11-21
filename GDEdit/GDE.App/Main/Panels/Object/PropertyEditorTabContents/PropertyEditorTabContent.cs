using GDAPI.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Panels.Object.Components;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GDE.App.Main.Panels.Object.Content.PropertyEditorTabContents
{
    public abstract class PropertyEditorTabContent : FillFlowContainer<PropertyEditorGroup>
    {
        protected LevelObjectCollection SelectedObjects;

        public PropertyEditorTabContent(LevelObjectCollection objects, BindableBool deltaModeBindable)
        {
            SelectedObjects = objects;
            AddRangeInternal(CreateContent());
            var groups = GetGroups();
            foreach (var group in groups)
            {
                group.UpdateValues(objects);
                if (group is IDeltable d)
                    d.InitializeDeltaModeBindable(deltaModeBindable);
            }
        }

        protected abstract PropertyEditorGroup[] GetGroups();
        protected abstract Drawable[] CreateContent();
    }
}