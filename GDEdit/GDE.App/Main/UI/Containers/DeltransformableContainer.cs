using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using osu.Framework.Graphics.Containers;
using GDE.App.Main.Panels.Object.Components;
using osu.Framework.Bindables;

namespace GDE.App.Main.UI.Containers
{
    public abstract class DeltransformableContainer : Container, IDeltable
    {
        protected BindableBool DeltaMode;

        public BindableBool DeltaModeBindable
        {
            get => DeltaMode;
            set => DeltaMode.BindTo(value);
        }

        public DeltransformableContainer()
            : base()
        {
            DeltaMode = new BindableBool();
            DeltaMode.ValueChanged += DeltaModeAdjusted;
        }

        protected abstract void TransformToDeltaMode();
        protected abstract void TransformToNormalMode();

        private void DeltaModeAdjusted(ValueChangedEvent<bool> value)
        {
            if (value.NewValue)
                TransformToDeltaMode();
            else
                TransformToNormalMode();
        }
    }
}
