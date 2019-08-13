﻿using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.SpecialObjects
{
    /// <summary>Represents a count text object.</summary>
    [ObjectID(SpecialObjectType.CountTextObject)]
    public class CountTextObject : ConstantIDSpecialObject, IHasPrimaryItemID
    {
        private short itemID;

        /// <summary>The object ID of the count text block.</summary>
        [ObjectStringMappable(ObjectParameter.ID)]
        public override int ObjectID => (int)SpecialObjectType.CountTextObject;

        /// <summary>The Item ID the count text object displays.</summary>
        [ObjectStringMappable(ObjectParameter.ItemID, 0)]
        public int ItemID
        {
            get => itemID;
            set => itemID = (short)value;
        }
        /// <summary>The Item ID the count text object displays.</summary>
        public int PrimaryItemID
        {
            get => ItemID;
            set => ItemID = value;
        }

        /// <summary>Initializes a new instance of the <seealso cref="CountTextObject"/> class.</summary>
        public CountTextObject() : base() { }

        /// <summary>Returns a clone of this <seealso cref="CountTextObject"/>.</summary>
        public override GeneralObject Clone() => AddClonedInstanceInformation(new CountTextObject());

        /// <summary>Adds the cloned instance information and returns the cloned instance.</summary>
        /// <param name="cloned">The cloned instance to add the information to.</param>
        protected override GeneralObject AddClonedInstanceInformation(GeneralObject cloned)
        {
            var c = cloned as CountTextObject;
            c.itemID = itemID;
            return base.AddClonedInstanceInformation(c);
        }

        /// <summary>Determines whether this object equals another object's properties; has to be <see langword="override"/>n in every object and every <see langword="override"/> should call its parent function first before determining its own <see langword="override"/>n result. That means an <see langword="override"/> should look like <see langword="return"/> <see langword="base"/>.EqualsInherited(<paramref name="other"/>) &amp;&amp; ...;.</summary>
        /// <param name="other">The other object to check whether it equals this object's properties.</param>
        protected override bool EqualsInherited(GeneralObject other)
        {
            var z = other as CountTextObject;
            return base.EqualsInherited(other)
                && itemID == z.itemID;
        }
    }
}
