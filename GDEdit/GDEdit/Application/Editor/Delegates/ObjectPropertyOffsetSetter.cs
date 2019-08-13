﻿using GDEdit.Utilities.Objects.General;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GDEdit.Application.Editor.Delegates
{
    /// <summary>Represents a function that changes the value of an object parameter of type <typeparamref name="T"/> by an offset in the specified objects.</summary>
    /// <param name="affectedObjects">The objects that will be affected by this function.</param>
    /// <param name="offset">The offset that will be applied to the field's value.</param>
    /// <param name="registerUndoable">Determines whether this action should be registered in the undo stack after performing it.</param>
    public delegate void ObjectPropertyOffsetSetter<T>(LevelObjectCollection affectedObjects, T offset, bool registerUndoable);
}
