﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Interfaces
{
    /// <summary>Represents a trigger which contains a definition for movement easing.</summary>
    public interface IHasEasing
    {
        /// <summary>The easing of the trigger's effect.</summary>
        [ObjectStringMappable(ObjectParameter.Easing)]
        Easing Easing { get; set; }
        /// <summary>The easing rate of the trigger's effect.</summary>
        [ObjectStringMappable(ObjectParameter.EasingRate)]
        double EasingRate { get; set; }
    }
}