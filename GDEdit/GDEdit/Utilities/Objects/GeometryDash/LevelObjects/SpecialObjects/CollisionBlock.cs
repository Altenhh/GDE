﻿using GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Triggers.Interfaces;
using GDEdit.Utilities.Enumerations.GeometryDash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.SpecialObjects
{
    /// <summary>Represents a collision block.</summary>
    public class CollisionBlock : SpecialObject, IHasPrimaryBlockID
    {
        private short blockID;

        /// <summary>The object ID of the collision block.</summary>
        public new int ObjectID => (int)SpecialObjectType.CollisionBlock;

        /// <summary>The Block ID of the collision block.</summary>
        public int BlockID
        {
            get => blockID;
            set => blockID = (short)value;
        }
        /// <summary>The Block ID of the collision block.</summary>
        public int PrimaryBlockID
        {
            get => BlockID;
            set => BlockID = value;
        }

        /// <summary>Initializes a new instance of the <seealso cref="CollisionBlock"/> class.</summary>
        public CollisionBlock() : base() { }

        /// <summary>Returns a clone of this <seealso cref="CollisionBlock"/>.</summary>
        public override GeneralObject Clone() => AddClonedInstanceInformation(new CollisionBlock());

        /// <summary>Adds the cloned instance information and returns the cloned instance.</summary>
        /// <param name="cloned">The cloned instance to add the information to.</param>
        protected override GeneralObject AddClonedInstanceInformation(GeneralObject cloned)
        {
            var c = cloned as CollisionBlock;
            c.BlockID = BlockID;
            return base.AddClonedInstanceInformation(c);
        }
    }
}
