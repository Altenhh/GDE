﻿using GDEdit.Application.Editor.Delegates;
using GDEdit.Utilities.Objects.General;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;

namespace GDEdit.Application.Editor.Actions.LevelActions
{
    /// <summary>Represents an action where the Swipe property of the editor has been changed.</summary>
    public class ObjectsMoved : LevelActionObjectsOffset<Point>
    {
        /// <summary>Initializes a new instance of the <seealso cref="ObjectsMoved"/> class.</summary>
        /// <param name="affectedObjects">The objects that this action will affect.</param>
        /// <param name="offset">The offset to apply to the specified parameter of the affected objects.</param>
        /// <param name="action">The action to be performed.</param>
        public ObjectsMoved(LevelObjectCollection affectedObjects, Point offset, ObjectPropertyOffsetSetter<Point> action)
            : base(affectedObjects, offset, action) { }

        /// <summary>Generates the description of the action.</summary>
        protected override string GenerateDescription() => $"Move {ObjectCountStringRepresentation} by {Offset}";

        /// <summary>Gets the inverse offset based on the given offset.</summary>
        protected override Point GetInverseOffset() => -Offset;
    }
}
