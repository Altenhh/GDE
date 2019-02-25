﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash.GamesaveValues;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Triggers.Interfaces;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Triggers
{
    /// <summary>Represents an Instant Count trigger.</summary>
    public class InstantCountTrigger : Trigger, IHasTargetGroupID, IHasPrimaryItemID
    {
        private short targetGroupID, itemID;

        public override int ObjectID => (int)Enumerations.GeometryDash.TriggerType.InstantCount;

        /// <summary>The target Group ID of the trigger.</summary>
        public int TargetGroupID
        {
            get => targetGroupID;
            set => targetGroupID = (short)value;
        }
        /// <summary>The Item ID of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.ItemID)]
        public int ItemID
        {
            get => itemID;
            set => itemID = (short)value;
        }
        /// <summary>The primary Item ID of the trigger.</summary>
        public int PrimaryItemID
        {
            get => ItemID;
            set => ItemID = value;
        }
        /// <summary>The Target Count property of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.Count)]
        public int TargetCount { get; set; }
        /// <summary>The Activate Group property of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.ActivateGroup)]
        public bool ActivateGroup
        {
            get => TriggerBools[3];
            set => TriggerBools[3] = value;
        }
        /// <summary>The Comparison property of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.Comparison)]
        public InstantCountComparison Comparison { get; set; }

        /// <summary>Initializes a new instance of the <seealso cref="InstantCountTrigger"/> class.</summary>
        public InstantCountTrigger() { }
        /// <summary>Initializes a new instance of the <seealso cref="InstantCountTrigger"/> class.</summary>
        /// <param name="targetGroupID">The target Group ID of the trigger.</param>
        /// <param name="itemID">The Item ID of the trigger.</param>
        /// <param name="count">The Target Count property of the trigger.</param>
        /// <param name="activateGroup">The Activate Group property of the trigger.</param>
        /// <param name="comparison">The Comparison property of the trigger.</param>
        public InstantCountTrigger(int targetGroupID, int itemID, int targetCount, bool activateGroup = false, InstantCountComparison comparison = InstantCountComparison.Equals)
        {
            TargetGroupID = targetGroupID;
            ItemID = itemID;
            TargetCount = targetCount;
            ActivateGroup = activateGroup;
            Comparison = comparison;
        }

        /// <summary>Returns a clone of this <seealso cref="InstantCountTrigger"/>.</summary>
        public override GeneralObject Clone() => AddClonedInstanceInformation(new InstantCountTrigger());

        /// <summary>Adds the cloned instance information and returns the cloned instance.</summary>
        /// <param name="cloned">The cloned instance to add the information to.</param>
        protected override GeneralObject AddClonedInstanceInformation(GeneralObject cloned)
        {
            var c = cloned as InstantCountTrigger;
            c.TargetGroupID = TargetGroupID;
            c.ItemID = ItemID;
            c.TargetCount = TargetCount;
            c.ActivateGroup = ActivateGroup;
            c.Comparison = Comparison;
            return base.AddClonedInstanceInformation(c);
        }
    }
}
