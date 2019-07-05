﻿using GDEdit.Utilities.Functions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;

namespace GDEdit.Utilities.Objects.General
{
    /// <summary>Represents a source-target range of the form A-B > C-D, where A &lt;= B and C &lt;= D.</summary>
    public class SourceTargetRange
    {
        private int sourceFrom, sourceTo, targetFrom;

        /// <summary>Gets or sets the source's starting value.</summary>
        public int SourceFrom
        {
            get => sourceFrom;
            set => SourceTargetRangeChanged?.Invoke(sourceFrom = value, sourceTo, targetFrom, TargetTo);
        }
        /// <summary>Gets or sets the source's ending value.</summary>
        public int SourceTo
        {
            get => sourceTo;
            set => SourceTargetRangeChanged?.Invoke(sourceFrom, sourceTo = value, targetFrom, TargetTo);
        }
        /// <summary>Gets or sets the target's starting value.</summary>
        public int TargetFrom
        {
            get => targetFrom;
            set => SourceTargetRangeChanged?.Invoke(sourceFrom, sourceTo, targetFrom = value, TargetTo);
        }
        /// <summary>Gets or sets the target's ending value.</summary>
        public int TargetTo
        {
            get => targetFrom + Range;
            set => AdjustSourceFrom(value - TargetTo);
        }

        /// <summary>Gets the range of the source.</summary>
        public int Range => sourceTo - sourceFrom;
        /// <summary>Gets the difference of the starting value of the source and the target (target - source).</summary>
        public int Difference => targetFrom - sourceFrom;

        /// <summary>Raised whenever a property is changed.</summary>
        public event SourceTargetRangeChanged SourceTargetRangeChanged;

        /// <summary>Initializes a new instance of the <seealso cref="SourceTargetRange"/> class.</summary>
        /// <param name="sourceStart">The starting value of the source.</param>
        /// <param name="sourceEnd">The ending value of the source.</param>
        /// <param name="targetStart">The starting value of the target.</param>
        public SourceTargetRange(int sourceStart, int sourceEnd, int targetStart)
        {
            sourceFrom = sourceStart;
            sourceTo = sourceEnd;
            targetFrom = targetStart;
        }

        /// <summary>Determines whether a value is within the source's range.</summary>
        /// <param name="value">The value to determine whether it's within the source's range.</param>
        public bool IsWithinSourceRange(int value) => sourceFrom <= value && value <= sourceTo;

        /// <summary>Adjusts the source from property while also being able to maintain the source difference.</summary>
        /// <param name="adjustment">The adjustment to apply to the source from property.</param>
        /// <param name="maintainDifference">Determines whether source difference will be maintained. Defaults to <see langword="true"/>.</param>
        public void AdjustSourceFrom(int adjustment, bool maintainDifference = true)
        {
            sourceFrom += adjustment;
            if (maintainDifference)
                sourceTo += adjustment;
        }

        /// <summary>Inverts this <seealso cref="SourceTargetRange"/> by inverting the target and the source (the individual ranges remain the same).</summary>
        public SourceTargetRange Invert() => new SourceTargetRange(TargetFrom, TargetTo, SourceFrom);

        /// <summary>Parses a string of the form "A-B > C-D" into a <seealso cref="SourceTargetRange"/>.</summary>
        /// <param name="str">The string to parse into a <seealso cref="SourceTargetRange"/>. The string must be of the form "A-B > C-D", where A-B can simply be A if A = B and C-D can respectively be C if C = D.</param>
        public static SourceTargetRange Parse(string str)
        {
            string[,] split = str.Split('>').Split('-');
            int length0 = split.GetLength(0);
            int length1 = split.GetLength(1);

            for (int i = 0; i < length0; i++)
                for (int j = 0; j < length1; j++)
                {
                    while (split[i, j].First() == ' ')
                        split[i, j] = split[i, j].Remove(0, 1);
                    while (split[i, j].Last() == ' ')
                        split[i, j] = split[i, j].Remove(split[i, j].Length - 1, 1);
                }
            return new SourceTargetRange(ToInt32(split[0, 0]), ToInt32(split[0, length1 - 1]), ToInt32(split[1, 0]));
        }
        /// <summary>Loads a number of <seealso cref="SourceTargetRange"/>s from a string array.</summary>
        /// <param name="lines">The lines to load the <seealso cref="SourceTargetRange"/>s from.</param>
        /// <param name="ignoreEmptyLines">Determines whether empty lines will be ignored during parsing. There is almost no reason to set that to <see langword="false"/> unless you're a weirdo.</param>
        public static List<SourceTargetRange> LoadRangesFromStringArray(string[] lines, bool ignoreEmptyLines = true)
        {
            var list = new List<SourceTargetRange>();
            foreach (string s in lines)
                if (!ignoreEmptyLines || s != "")
                    list.Add(Parse(s));
            return list;
        }

        /// <summary>Inverts the ordering of a provided list of ranges and inverts the individual ranges and returns the resulting list.</summary>
        /// <param name="ranges">The list of ranges to invert.</param>
        public static List<SourceTargetRange> Invert(List<SourceTargetRange> ranges)
        {
            var list = new List<SourceTargetRange>();
            for (int i = ranges.Count - 1; i >= 0; i--)
                list.Add(ranges[i].Invert());
            return list;
        }

        public override string ToString() => $"{SourceToString()} > {TargetToString()}";
        /// <summary>Returns the string representation of the source.</summary>
        public string SourceToString() => ToString(SourceFrom, SourceTo);
        /// <summary>Returns the string representation of the target.</summary>
        public string TargetToString() => ToString(TargetFrom, TargetTo);

        private static string ToString(int from, int to) => $"{from}{(to - from > 0 ? $"-{to}" : "")}";
    }

    /// <summary>Represents a function that contains the new state of the <seealso cref="SourceTargetRange"/> instance that was changed.</summary>
    /// <param name="sourceFrom">The new value of the source from property.</param>
    /// <param name="sourceTo">The new value of the source to property.</param>
    /// <param name="targetFrom">The new value of the target from property.</param>
    /// <param name="targetTo">The new value of the target to property.</param>
    public delegate void SourceTargetRangeChanged(int sourceFrom, int sourceTo, int targetFrom, int targetTo);
}
