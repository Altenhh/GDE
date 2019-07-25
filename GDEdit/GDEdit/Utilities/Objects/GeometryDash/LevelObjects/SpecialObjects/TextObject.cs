﻿using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash;
using GDEdit.Utilities.Information.GeometryDash;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.SpecialObjects
{
    /// <summary>Represents a text object.</summary>
    [ObjectID(SpecialObjectType.TextObject)]
    public class TextObject : ConstantIDSpecialObject
    {
        /// <summary>The object ID of the text object.</summary>
        public override int ObjectID => (int)SpecialObjectType.TextObject;

        /// <summary>Represents the Text property of the text object encoded in base 64.</summary>
        [ObjectStringMappable(ObjectParameter.TextObjectText)]
        public string Base64Text => Convert.ToBase64String(Encoding.UTF8.GetBytes(Text));

        /// <summary>Represents the Text property of the text object.</summary>
        public string Text { get; set; }

        /// <summary>Initializes a new instance of the <seealso cref="TextObject"/> class.</summary>
        public TextObject() : base() { }
        /// <summary>Initializes a new instance of the <seealso cref="TextObject"/> class.</summary>
        /// <param name="x">The X location of the object.</param>
        /// <param name="y">The Y location of the object.</param>
        /// <param name="text">The text of the text object.</param>
        public TextObject(double x, double y, string text = "")
            : this()
        {
            X = x;
            Y = y;
            Text = text;
        }

        /// <summary>Returns a clone of this <seealso cref="TextObject"/>.</summary>
        public override GeneralObject Clone() => AddClonedInstanceInformation(new TextObject());

        /// <summary>Adds the cloned instance information and returns the cloned instance.</summary>
        /// <param name="cloned">The cloned instance to add the information to.</param>
        protected override GeneralObject AddClonedInstanceInformation(GeneralObject cloned)
        {
            var c = cloned as TextObject;
            c.Text = Text;
            return base.AddClonedInstanceInformation(c);
        }

        /// <summary>Determines whether this object equals another object's properties; has to be <see langword="override"/>n in every object and every <see langword="override"/> should call its parent function first before determining its own <see langword="override"/>n result. That means an <see langword="override"/> should look like <see langword="return"/> <see langword="base"/>.EqualsInherited(<paramref name="other"/>) &amp;&amp; ...;.</summary>
        /// <param name="other">The other object to check whether it equals this object's properties.</param>
        protected override bool EqualsInherited(GeneralObject other)
        {
            var z = other as TextObject;
            return base.EqualsInherited(other)
                && Text == z.Text;
        }
    }
}
