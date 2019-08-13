﻿using GDEdit.Application.Editor.Delegates;
using GDEdit.Utilities.Objects.General;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;
using System.Collections.Generic;

namespace GDEdit.Application.Editor.Actions.LevelActions
{
    /// <summary>Represents an action where the Swipe property of the editor has been changed.</summary>
    public class GroupIDsRemoved : LevelActionObjectsValueInverse<IEnumerable<int>, ObjectPropertySetter<IEnumerable<int>>, ObjectPropertySetter<IEnumerable<int>>>
    {
        /// <summary>Initializes a new instance of the <seealso cref="GroupIDsRemoved"/> class.</summary>
        /// <param name="affectedObjects">The objects that this action will affect.</param>
        /// <param name="action">The function that rotates the specified objects.</param>
        public GroupIDsRemoved(LevelObjectCollection affectedObjects, IEnumerable<int> groupIDs, ObjectPropertySetter<IEnumerable<int>> action, ObjectPropertySetter<IEnumerable<int>> inverse)
            : base(affectedObjects, groupIDs, action, inverse) { }

        /// <summary>Generates the description of the action.</summary>
        protected override string GenerateDescription() => $"Remove group IDs from {ObjectCountStringRepresentation}";

        /// <summary>Performs the action.</summary>
        public override void PerformAction() => Action(AffectedObjects, Value, false);
        /// <summary>Performs the inverse action of the editor action.</summary>
        public override void PerformInverseAction() => InverseAction(AffectedObjects, Value, false);
    }
}
