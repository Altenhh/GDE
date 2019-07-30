﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Triggers.Interfaces;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Triggers
{
    /// <summary>Represents a Follow Player Y trigger.</summary>
    [ObjectID(TriggerType.FollowPlayerY)]
    public class FollowPlayerYTrigger : Trigger, IHasDuration, IHasTargetGroupID
    {
        private short targetGroupID;
        private float duration = 0.5f, speed = 1, delay, maxSpeed;

        /// <summary>The Object ID of the Follow Player Y trigger.</summary>
        [ObjectStringMappable(ObjectParameter.ID)]
        public override int ObjectID => (int)TriggerType.FollowPlayerY;

        /// <summary>The duration of the trigger's effect.</summary>
        [ObjectStringMappable(ObjectParameter.Duration)]
        public double Duration
        {
            get => duration;
            set => duration = (float)value;
        }
        /// <summary>The target Group ID of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.TargetGroupID)]
        public int TargetGroupID
        {
            get => targetGroupID;
            set => targetGroupID = (short)value;
        }
        /// <summary>The Speed property of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.Speed)]
        public double Speed
        {
            get => speed;
            set => speed = (float)value;
        }
        /// <summary>The Delay property of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.FollowDelay)]
        public double Delay
        {
            get => delay;
            set => delay = (float)value;
        }
        /// <summary>The Max Speed property of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.MaxSpeed)]
        public double MaxSpeed
        {
            get => maxSpeed;
            set => maxSpeed = (float)value;
        }
        /// <summary>The Offset property of the trigger.</summary>
        [ObjectStringMappable(ObjectParameter.YOffset)]
        public int Offset { get; set; }

        /// <summary>Initializes a new instance of the <seealso cref="FollowPlayerYTrigger"/> class.</summary>
        public FollowPlayerYTrigger() : base() { }
        /// <summary>Initializes a new instance of the <seealso cref="FollowPlayerYTrigger"/> class.</summary>
        /// <param name="duration">The duration of the trigger.</param>
        /// <param name="targetGroupID">The target Group ID of the trigger.</param>
        public FollowPlayerYTrigger(double duration, int targetGroupID)
            : base()
        {
            Duration = duration;
            TargetGroupID = targetGroupID;
        }
        /// <summary>Initializes a new instance of the <seealso cref="FollowPlayerYTrigger"/> class.</summary>
        /// <param name="duration">The duration of the trigger.</param>
        /// <param name="targetGroupID">The target Group ID of the trigger.</param>
        /// <param name="speed">The Speed property of the trigger.</param>
        /// <param name="delay">The Delay property of the trigger.</param>
        /// <param name="maxSpeed">The Max Speed property of the trigger.</param>
        /// <param name="offset">The Offset property of the trigger.</param>
        public FollowPlayerYTrigger(double duration, int targetGroupID, double speed, double delay, double maxSpeed, int offset)
            : this(duration, targetGroupID)
        {
            Speed = speed;
            Delay = delay;
            MaxSpeed = maxSpeed;
            Offset = offset;
        }

        /// <summary>Returns a clone of this <seealso cref="FollowPlayerYTrigger"/>.</summary>
        public override GeneralObject Clone() => AddClonedInstanceInformation(new FollowPlayerYTrigger());

        /// <summary>Adds the cloned instance information and returns the cloned instance.</summary>
        /// <param name="cloned">The cloned instance to add the information to.</param>
        protected override GeneralObject AddClonedInstanceInformation(GeneralObject cloned)
        {
            var c = cloned as FollowPlayerYTrigger;
            c.targetGroupID = targetGroupID;
            c.duration = duration;
            c.speed = speed;
            c.delay = delay;
            c.maxSpeed = maxSpeed;
            c.Offset = Offset;
            return base.AddClonedInstanceInformation(c);
        }

        /// <summary>Determines whether this object equals another object's properties; has to be <see langword="override"/>n in every object and every <see langword="override"/> should call its parent function first before determining its own <see langword="override"/>n result. That means an <see langword="override"/> should look like <see langword="return"/> <see langword="base"/>.EqualsInherited(<paramref name="other"/>) &amp;&amp; ...;.</summary>
        /// <param name="other">The other object to check whether it equals this object's properties.</param>
        protected override bool EqualsInherited(GeneralObject other)
        {
            var z = other as FollowPlayerYTrigger;
            return base.EqualsInherited(other)
                && targetGroupID == z.targetGroupID
                && duration == z.duration
                && speed == z.speed
                && delay == z.delay
                && maxSpeed == z.maxSpeed
                && Offset == z.Offset;
        }
    }
}
