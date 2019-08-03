﻿using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash;
using GDEdit.Utilities.Functions.GeometryDash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.SpecialObjects.Portals.GamemodePortals
{
    /// <summary>Represents a UFO portal.</summary>
    [ObjectID(PortalType.UFO)]
    public class UFOPortal : CheckableGamemodePortal
    {
        /// <summary>The object ID of the UFO portal.</summary>
        [ObjectStringMappable(ObjectParameter.ID)]
        public override int ObjectID => (int)PortalType.UFO;
        /// <summary>The gamemode the gamemode portal transforms the player into.</summary>
        public override Gamemode Gamemode => Gamemode.UFO;

        /// <summary>Initializes a new instance of the <seealso cref="UFOPortal"/> class.</summary>
        public UFOPortal() : base() { }

        /// <summary>Returns a clone of this <seealso cref="UFOPortal"/>.</summary>
        public override GeneralObject Clone() => AddClonedInstanceInformation(new UFOPortal());
    }
}
