﻿using GDEdit.Application.Editor.Delegates;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;
using System;

namespace GDEdit.Application.Editor.Actions.LevelActions
{
    /// <summary>Represents an action in the editor affecting the level which can be undone and redone containing a collection of affected objects.</summary>
    /// <typeparam name="TActionDelegate">The type of the delegate that represents the action.</typeparam>
    /// <typeparam name="TInverseDelegate">The type of the delegate that represents the inverse action.</typeparam>
    public abstract class LevelActionObjectsValueInverse<TValue, TActionDelegate, TInverseDelegate> : LevelActionObjectsInverse<TActionDelegate, TInverseDelegate>
        where TActionDelegate : Delegate
    {
        public TValue Value;

        /// <summary>Initiailizes a new instance of the <seealso cref="LevelActionObjectsInverse{TActionDelegate, TInverseDelegate}"/> class.</summary>
        /// <param name="affectedObjects">The objects that this action will affect.</param>
        /// <param name="action">The action to be performed.</param>
        /// <param name="inverse">The inverse action to be performed.</param>
        public LevelActionObjectsValueInverse(LevelObjectCollection affectedObjects, TValue value, TActionDelegate action, TInverseDelegate inverse)
            : base(affectedObjects, action, inverse)
        {
            Value = value;
        }
    }
}
