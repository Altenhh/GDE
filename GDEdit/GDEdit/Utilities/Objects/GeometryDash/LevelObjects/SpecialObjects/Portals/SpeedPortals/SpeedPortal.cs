﻿using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.SpecialObjects.Portals.SpeedPortals
{
    /// <summary>Represents a speed portal in the game.</summary>
    public abstract class SpeedPortal : Portal, IHasCheckedProperty
    {
        /// <summary>The speed this speed portal sets.</summary>
        public abstract Speed Speed { get; }

        /// <summary>The checked property of the speed portal that determines whether the speed portal will be taken into account when converting X position to time.</summary>
        [ObjectStringMappable(ObjectParameter.SpecialObjectChecked, true)]
        public bool Checked
        {
            get => SpecialObjectBools[0];
            set => SpecialObjectBools[0] = value;
        }

        /// <summary>Initializes a new instance of the <seealso cref="SpeedPortal"/> class.</summary>
        public SpeedPortal()
            : base()
        {
            Checked = true;
        }
    }
}
