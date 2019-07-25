﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Interfaces
{
    /// <summary>Represents a trigger which contains a definition for a primary Block ID.</summary>
    public interface IHasPrimaryBlockID
    {
        /// <summary>The primary Block ID of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.BlockAID)]
        int PrimaryBlockID { get; set; }
    }
}
