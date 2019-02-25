﻿using GDEdit.Utilities.Enumerations.GeometryDash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.SpecialObjects.Portals
{
    /// <summary>Represents a blue dual portal.</summary>
    public class BlueDualPortal : Portal
    {
        /// <summary>The object ID of the blue dual portal.</summary>
        public override int ObjectID => (int)PortalType.BlueDual;

        /// <summary>Initializes a new instance of the <seealso cref="BlueDualPortal"/> class.</summary>
        public BlueDualPortal() : base() { }

        /// <summary>Returns a clone of this <seealso cref="BlueDualPortal"/>.</summary>
        public override GeneralObject Clone() => AddClonedInstanceInformation(new BlueDualPortal());
    }
}
