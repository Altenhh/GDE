using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using osu.Framework.Bindables;

namespace GDE.App.Main.UI
{
    public interface ISupportsMixedValue
    {
        bool IsMixedCommonValue { get; set; }

        void FadeToMixedColors(double duration = 500);
        void FadeToNormalColors(double duration = 500);

        void UpdateColors(ValueChangedEvent<bool> value)
        {
            if (value.NewValue)
                FadeToMixedColors();
            else
                FadeToNormalColors();
        }
    }
}
