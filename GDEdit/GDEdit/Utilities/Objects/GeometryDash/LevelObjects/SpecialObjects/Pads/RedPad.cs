﻿using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash;
using GDEdit.Utilities.Enumerations.GeometryDash.GamesaveValues;
using GDEdit.Utilities.Information.GeometryDash;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Triggers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.SpecialObjects.Pads
{
    /// <summary>Represents a red pad.</summary>
    public class RedPad : Pad
    {
        /// <summary>The object ID of the red pad.</summary>
        public override int ObjectID => (int)PadType.RedPad;

        /// <summary>Initializes a new instance of the <seealso cref="RedPad"/> class.</summary>
        public RedPad() : base() { }

        /// <summary>Returns a clone of this <seealso cref="RedPad"/>.</summary>
        public override GeneralObject Clone() => AddClonedInstanceInformation(new RedPad());
    }
}
