﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDEdit.Utilities.Enumerations.GeometryDash;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Triggers.ColorTriggers
{
    /// <summary>Represents a Color 3 Color trigger.</summary>
    public class Color3ColorTrigger : ColorTrigger
    {
        public override int ObjectID => (int)TriggerType.Color3;
        
        /// <summary>The target Color ID of the trigger.</summary>
        public new int TargetColorID => 3;

        /// <summary>Initializes a new instance of the <seealso cref="Color3ColorTrigger"/> class.</summary>
        public Color3ColorTrigger() { }
        /// <summary>Initializes a new instance of the <seealso cref="Color3ColorTrigger"/> class.</summary>
        /// <param name="duration">The duration of the trigger.</param>
        /// <param name="copyOpacity">The Copy Opacity property of the trigger.</param>
        /// <param name="tintGround">The Tint Ground property of the trigger.</param>
        public Color3ColorTrigger(float duration, bool copyOpacity = false, bool tintGround = false)
            : base(duration, (int)SpecialColorID.BG, copyOpacity, tintGround) { }
    }
}
