using System;
using System.Collections.Generic;
using System.Linq;
using GDAPI.Objects.GeometryDash.LevelObjects;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osu.Framework.Layout;
using osu.Framework.Utils;
using osu.Framework.Threading;
using osuTK.Graphics;

namespace GDEdit.App.Overlays.Level
{
    public class ObjectDensity : CompositeDrawable
    {
        private readonly LevelObjectCollection collection;

        /// <summary>
        /// Resolution at which the graphs renders at.
        /// Set to 1 for full resolution.
        /// </summary>
        public int Resolution = 10;
        
        private Color4 defColour;

        /// <summary>
        /// The colour which low-range frequencies should be colourised with.
        /// May be null for this frequency range to not be colourised.
        /// </summary>
        public Color4 DefColour
        {
            get => defColour;
            set
            {
                if (defColour == value)
                    return;

                defColour = value;
            }
        }
        
        private Color4 lowColour;

        /// <summary>
        /// The colour which low-range frequencies should be colourised with.
        /// May be null for this frequency range to not be colourised.
        /// </summary>
        public Color4 LowColour
        {
            get => lowColour;
            set
            {
                if (lowColour == value)
                    return;

                lowColour = value;
            }
        }

        private Color4 midColour;

        /// <summary>
        /// The colour which mid-range frequencies should be colourised with.
        /// May be null for this frequency range to not be colourised.
        /// </summary>
        public Color4 MidColour
        {
            get => midColour;
            set
            {
                if (midColour == value)
                    return;

                midColour = value;
            }
        }

        private Color4 highColour;

        /// <summary>
        /// The colour which high-range frequencies should be colourised with.
        /// May be null for this frequency range to not be colourised.
        /// </summary>
        public Color4 HighColour
        {
            get => highColour;
            set
            {
                if (highColour == value)
                    return;

                highColour = value;
            }
        }

        private int[] calculatedValues;
        
        private int[] values;
        
        public int[] Values
        {
            get => values;
            set
            {
                if (value == values) return;

                values = value;
                layout.Invalidate();
            }
        }
        
        private readonly LayoutValue layout = new LayoutValue(Invalidation.DrawSize);
        private ScheduledDelegate scheduledCreate;

        public ObjectDensity(LevelObjectCollection collection)
        {
            this.collection = collection;

            BorderThickness = 3;
            Masking = true;
            RelativeSizeAxes = Axes.X;
            Height = 40;

            EdgeEffect = new EdgeEffectParameters
            {
                Colour = Color4.Black.Opacity(0.25f),
                Type = EdgeEffectType.Shadow,
                Radius = 3,
                Offset = new Vector2(0, 1)
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            CalculateValues();
            AddGraph();
        }

        protected override void Update()
        {
            base.Update();

            if (values != null && !layout.IsValid)
            {
                scheduledCreate?.Cancel();
                scheduledCreate = Scheduler.AddDelayed(AddGraph, 500);

                layout.Validate();
            }
        }

        public void CalculateValues()
        {
            if (!collection.Any())
                return;
            
            var lastObj = collection.Max(o => o.X);

            if (lastObj == 0)
                lastObj = collection.Last().X;

            int granularity = (int) (lastObj / 30);
            Values = new int[granularity];
            
            foreach (var o in collection)
                Values[Math.Clamp((int) o.X / 30 - 1, 0, int.MaxValue)]++;
        }

        public void AddGraph()
        {
            // calculate values for drawWidth.
            var newValues = new List<float>();

            if (values == null)
            {
                for (float i = 0; i < (DrawWidth / Resolution); i++)
                    newValues.Add(0);
                
                return;
            }

            var max = values.Max();

            float step = values.Length / (DrawWidth / Resolution);

            for (float i = 0; i < values.Length; i += step)
            {
                newValues.Add((float)values[(int)i] / max);
            }

            for (var i = 0; i < newValues.Count; i++)
            {
                Box box = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.X,
                    Width = 1 / (DrawWidth / Resolution),
                    X = i / (DrawWidth / Resolution)
                };

                Color4 colour = defColour;

                // colouring is applied in the order of interest to a viewer.
                colour = Interpolation.ValueAt(newValues[i], colour, lowColour, 0f, 0.3f);
                colour = Interpolation.ValueAt(newValues[i], colour, midColour, 0.3f, 0.6f);
                colour = Interpolation.ValueAt(newValues[i], colour, highColour, 0.6f, 0.9f);
                colour = Interpolation.ValueAt(newValues[i], colour, highColour, 0.9f, 1f);

                /*if (newValues[i] >= 0.1f)
                    colour = lowColour;

                if (newValues[i] >= 0.3f)
                    colour = midColour;

                if (newValues[i] >= 0.6f)
                    colour = highColour;*/

                box.Colour = colour;

                AddInternal(box);
            }
        }
    }
}