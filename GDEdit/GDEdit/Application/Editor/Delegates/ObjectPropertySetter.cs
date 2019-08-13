﻿using GDEdit.Utilities.Objects.General;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GDEdit.Application.Editor.Delegates
{
    /// <summary>Represents a function that changes the value of an object parameter of type <typeparamref name="T"/> in the specified objects.</summary>
    /// <param name="affectedObjects">The objects that will be affected by this function.</param>
    /// <param name="newValue">The new value to set to the field.</param>
    /// <param name="registerUndoable">Determines whether this action should be registered in the undo stack after performing it.</param>
    public delegate void ObjectPropertySetter<T>(LevelObjectCollection affectedObjects, T newValue, bool registerUndoable);
}
