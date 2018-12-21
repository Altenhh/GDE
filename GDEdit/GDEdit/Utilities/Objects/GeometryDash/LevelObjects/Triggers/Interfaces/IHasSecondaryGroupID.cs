﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash.GamesaveValues;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Triggers.Interfaces
{
    /// <summary>Represents a trigger which contains a definition for a secondary Group ID.</summary>
    public interface IHasSecondaryGroupID
    {
        [ObjectStringMappable(ObjectParameter.SecondaryGroupID)]
        int SecondaryGroupID { get; }
    }
}