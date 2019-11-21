using GDAPI.Enumerations.GeometryDash;
using GDAPI.Functions.GeometryDash;
using GDE.App.Main.UI;
using GDE.App.Main.UI.Containers;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace GDE.App.Main.Panels.Object.ValueComponents
{
    public class ZLayerInputBox : DeltransformableContainer, IHasCurrentValue<ZLayer>
    {
        private Bindable<ZLayer> current;

        private ToggleableValueButton<ZLayerPosition> positionToggle;
        private GDEIntNumberTextBox layerIndex;

        public Bindable<ZLayer> Current
        {
            get => current;
            set => current.BindTo(value);
        }

        public ZLayerInputBox()
            : base()
        {
            current = new Bindable<ZLayer>(ZLayer.B1);
            // Add positions and shit
            Children = new Drawable[]
            {
                positionToggle = new ToggleableValueButton<ZLayerPosition>(ZLayerPosition.Bottom, ZLayerPosition.Top)
                {

                },
                layerIndex = new GDEIntNumberTextBox(1),
            };

            positionToggle.ValueChanged += HandlePositionChanged;
            layerIndex.ValueChanged += HandlePositionChanged;
        }

        protected override void TransformToDeltaMode() => positionToggle.Hide();
        protected override void TransformToNormalMode() => positionToggle.Show();

        private void HandlePositionChanged(ValueChangedEvent<ZLayerPosition> value)
        {
            current.Value = ValueGenerator.GenerateZLayer(value.NewValue, layerIndex.Value);
        }
        private void HandlePositionChanged(ValueChangedEvent<int> value)
        {
            current.Value = ValueGenerator.GenerateZLayer(positionToggle.Value, value.NewValue);
        }
    }
}