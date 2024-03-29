﻿using AtDb.Extensions;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AtDb.Reader.Container
{
    [DebuggerDisplay("Name: {Name}, Type: {Type}, {StartIndex}-{EndIndex}")]
    public class AttributeDefinition
    {
        private const int INCLUSIVE_OFFSET = 1;
        private const string ENUM_FLAGGED = "flagged";

        public int StartIndex { get; private set; }
        /// <summary>
        /// The (inclusive) index of the last element.
        /// </summary>
        public int EndIndex { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }

        public bool IsSingleValue
        {
            get { return StartIndex == EndIndex; }
        }

        public int Length { get { return (EndIndex - StartIndex) + INCLUSIVE_OFFSET; } }

        public AttributeDefinition(int index, string name, string type)
        {
            StartIndex = index;
            EndIndex = index;
            Type = type;
            Name = name;
        }

        public void IncrementEndIndex()
        {
            ++EndIndex;
        }
    }
}
