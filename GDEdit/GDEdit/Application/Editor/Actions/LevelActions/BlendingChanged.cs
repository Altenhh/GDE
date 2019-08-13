﻿using GDEdit.Application.Editor.Delegates;
using GDEdit.Utilities.Objects.General;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;

namespace GDEdit.Application.Editor.Actions.LevelActions
{
    /// <summary>Represents an action where the Swipe property of the editor has been changed.</summary>
    public class BlendingChanged : LevelActionWithValue<int, FieldValueSetter<int>>
    {
        /// <summary>Initializes a new instance of the <seealso cref="BlendingChanged"/> class.</summary>
        /// <param name="affectedObjects">The objects that this action will affect.</param>
        /// <param name="centralPoint">The central point that was taken into account while flipping the objects horizontally.</param>
        /// <param name="action">The function that rotates the specified objects.</param>
        public BlendingChanged(int value, FieldValueSetter<int> action)
            : base(value, action) { }

        /// <summary>Generates the description of the action.</summary>
        protected override string GenerateDescription() => $"Toggle Blending color ID {NewValue}";

        /// <summary>Performs the action.</summary>
        public override void PerformAction() => Action(NewValue, false);
        /// <summary>Performs the inverse action of the editor action.</summary>
        public override void PerformInverseAction() => Action(NewValue, false);
    }
}
