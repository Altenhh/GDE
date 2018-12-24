﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Enumerations.GeometryDash;
using GDEdit.Utilities.Enumerations.GeometryDash.GamesaveValues;
using GDEdit.Utilities.Objects.General;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects.Triggers;

namespace GDEdit.Utilities.Objects.GeometryDash.LevelObjects
{
    // TODO: Remove LevelObject entirely once completely migrated to this
    /// <summary>Represents a general object.</summary>
    public class GeneralObject
    {
        private double rotation;

        /// <summary>The Object ID of this object.</summary>
        [ObjectStringMappable(ObjectParameter.ID)]
        public int ObjectID { get; set; }
        /// <summary>The X position of this object.</summary>
        [ObjectStringMappable((int)ObjectParameter.X)]
        public double X { get; set; }
        /// <summary>The Y position of this object.</summary>
        [ObjectStringMappable((int)ObjectParameter.Y)]
        public double Y { get; set; }
        /// <summary>Determines whether this object is flipped horizontally or not.</summary>
        [ObjectStringMappable(ObjectParameter.FlippedHorizontally)]
        public bool FlippedHorizontally { get; set; }
        /// <summary>Determines whether this object is flipped vertically or not.</summary>
        [ObjectStringMappable(ObjectParameter.FlippedVertically)]
        public bool FlippedVertically { get; set; }
        /// <summary>The rotation of this object.</summary>
        [ObjectStringMappable((int)ObjectParameter.Rotation)]
        public double Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                if (rotation > 360 || rotation < -360)
                    rotation %= 360;
            }
        }
        /// <summary>The scaling of this object.</summary>
        [ObjectStringMappable((int)ObjectParameter.Scaling)]
        public double Scaling { get; set; }
        /// <summary>The Editor Layer 1 of this object.</summary>
        [ObjectStringMappable(ObjectParameter.EL1)]
        public int EL1 { get; set; }
        /// <summary>The Editor Layer 2 of this object.</summary>
        [ObjectStringMappable(ObjectParameter.EL2)]
        public int EL2 { get; set; }
        /// <summary>The Z Layer of this object.</summary>
        [ObjectStringMappable(ObjectParameter.ZLayer)]
        public int ZLayer { get; set; }
        /// <summary>The Z Order of this object.</summary>
        [ObjectStringMappable(ObjectParameter.ZOrder)]
        public int ZOrder { get; set; }
        /// <summary>The Color 1 ID of this object.</summary>
        [ObjectStringMappable(ObjectParameter.Color1)]
        public int Color1ID { get; set; }
        /// <summary>The Color 2 ID of this object.</summary>
        [ObjectStringMappable(ObjectParameter.Color2)]
        public int Color2ID { get; set; }
        /// <summary>The Group IDs of this object.</summary>
        [ObjectStringMappable(ObjectParameter.GroupIDs)]
        public int[] GroupIDs { get; set; }
        /// <summary>The linked group ID of this object.</summary>
        [ObjectStringMappable(ObjectParameter.LinkedGroupID)]
        public int LinkedGroupID { get; set; }
        /// <summary>Determines whether this object is the group parent or not.</summary>
        [ObjectStringMappable(ObjectParameter.GroupParent)]
        public bool GroupParent { get; set; }
        /// <summary>Determines whether this object is for high detail or not.</summary>
        [ObjectStringMappable(ObjectParameter.HighDetail)]
        public bool HighDetail { get; set; }
        /// <summary>Determines whether this object should have an entrance effect or not.</summary>
        [ObjectStringMappable(ObjectParameter.DontEnter)]
        public bool DontEnter { get; set; }
        /// <summary>Determines whether this object should have the fade in and out disabled or not.</summary>
        [ObjectStringMappable(ObjectParameter.DontFade)]
        public bool DontFade { get; set; }
        /// <summary>Determines whether this object should have its glow disabled or not.</summary>
        [ObjectStringMappable(ObjectParameter.DisableGlow)]
        public bool DisableGlow { get; set; }

        /// <summary>Gets or sets a <seealso cref="Point"/> instance with the location of the object.</summary>
        public Point Location
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        /// <summary>The rotation of this object in degrees according to math.</summary>
        public double MathRotationDegrees
        {
            get => -Rotation;
            set => Rotation = -value;
        }
        /// <summary>The rotation of this object in radians according to math.</summary>
        public double MathRotationRadians
        {
            get => MathRotationDegrees * Math.PI / 180;
            set => MathRotationDegrees = value * 180 / Math.PI;
        }

        /// <summary>Creates a new instance of the <seealso cref="GeneralObject"/> class with the object ID parameter set to 1.</summary>
        public GeneralObject()
        {
            ObjectID = 1;
        }
        /// <summary>Creates a new instance of the <seealso cref="GeneralObject"/> class.</summary>
        /// <param name="objectID">The object ID of this <seealso cref="GeneralObject"/>.</param>
        public GeneralObject(int objectID)
        {
            ObjectID = objectID;
        }
        /// <summary>Creates a new instance of the <seealso cref="GeneralObject"/> class.</summary>
        /// <param name="objectID">The object ID of this <seealso cref="GeneralObject"/>.</param>
        /// <param name="x">The X position of this <seealso cref="GeneralObject"/>.</param>
        /// <param name="y">The Y position of this <seealso cref="GeneralObject"/>.</param>
        public GeneralObject(int objectID, double x, double y)
        {
            ObjectID = objectID;
            X = x;
            Y = y;
        }
        /// <summary>Creates a new instance of the <seealso cref="GeneralObject"/> class.</summary>
        /// <param name="objectID">The object ID of this <seealso cref="GeneralObject"/>.</param>
        /// <param name="x">The X position of this <seealso cref="GeneralObject"/>.</param>
        /// <param name="y">The Y position of this <seealso cref="GeneralObject"/>.</param>
        /// <param name="rotation">The rotation of this <seealso cref="GeneralObject"/>.</param>
        public GeneralObject(int objectID, double x, double y, double rotation)
        {
            ObjectID = objectID;
            X = x;
            Y = y;
            Rotation = rotation;
        }

        /// <summary>Returns a clone of this object.</summary>
        public GeneralObject Clone() => (GeneralObject)MemberwiseClone();

        public T GetParameterWithID<T>(int ID)
        {
            var properties = typeof(GeneralObject).GetProperties();
            foreach (var p in properties)
                if (((ObjectStringMappableAttribute)p.GetCustomAttributes(typeof(ObjectStringMappableAttribute), false).First()).Key == ID)
                    return (T)p.GetValue(this);
            throw new KeyNotFoundException("The requested ID was not found.");
        }
        public void SetParameterWithID<T>(int ID, T newValue)
        {
            // Reflection is FUN
            var properties = typeof(GeneralObject).GetProperties();
            foreach (var p in properties)
                if (((ObjectStringMappableAttribute)p.GetCustomAttributes(typeof(ObjectStringMappableAttribute), false).First()).Key == ID)
                    p.SetValue(this, newValue);
        }

        /// <summary>Determines whether the object's location is within a rectangle.</summary>
        /// <param name="startingX">The starting X position of the rectangle.</param>
        /// <param name="startingY">The starting Y position of the rectangle.</param>
        /// <param name="endingX">The ending X position of the rectangle.</param>
        /// <param name="endingY">The ending Y position of the rectangle.</param>
        public bool IsWithinRange(double startingX, double startingY, double endingX, double endingY) => startingX <= X && endingX >= X && startingY <= Y && endingY >= Y;

        public static GeneralObject GetNewObjectInstance(int objectID)
        {
            switch (objectID)
            {
                case (int)Enumerations.GeometryDash.Trigger.Alpha:
                    return new AlphaTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Animate:
                    return new AnimateTrigger();
                // TODO: Create the appropriate Color trigger class
                case (int)Enumerations.GeometryDash.Trigger.BG:
                case (int)Enumerations.GeometryDash.Trigger.GRND:
                case (int)Enumerations.GeometryDash.Trigger.GRND2:
                case (int)Enumerations.GeometryDash.Trigger.ThreeDL:
                case (int)Enumerations.GeometryDash.Trigger.Obj:
                case (int)Enumerations.GeometryDash.Trigger.Line:
                case (int)Enumerations.GeometryDash.Trigger.Color:
                    return new GeneralObject();
                case (int)Enumerations.GeometryDash.Trigger.Collision:
                    return new CollisionTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Count:
                    return new GeneralObject();
                case (int)Enumerations.GeometryDash.Trigger.Follow:
                    return new FollowTrigger();
                case (int)Enumerations.GeometryDash.Trigger.FollowPlayerY:
                    return new FollowPlayerYTrigger();
                case (int)Enumerations.GeometryDash.Trigger.InstantCount:
                    return new InstantCountTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Move:
                    return new MoveTrigger();
                case (int)Enumerations.GeometryDash.Trigger.OnDeath:
                    return new OnDeathTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Pickup:
                    return new PickupTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Pulse:
                    return new PulseTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Rotate:
                    return new RotateTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Shake:
                    return new ShakeTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Spawn:
                    return new SpawnTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Stop:
                    return new StopTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Toggle:
                    return new ToggleTrigger();
                case (int)Enumerations.GeometryDash.Trigger.Touch:
                    return new TouchTrigger();
                default:
                    return new GeneralObject(objectID);
            }
        }
    }
}
