using osu.Framework.Bindables;

namespace GDE.App.Main.Panels.Object.Components
{
    public interface IDeltable
    {
        BindableBool DeltaModeBindable { get; set; }

        void InitializeDeltaModeBindable(BindableBool deltaModeBindable) => DeltaModeBindable = new BindableBool { BindTarget = deltaModeBindable };
    }
}
