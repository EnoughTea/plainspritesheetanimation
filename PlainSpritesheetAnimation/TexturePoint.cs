using System;
using System.Runtime.Serialization;

namespace PlainSpritesheetAnimation
{
    /// <summary> Represents a point coordinates on a texture. </summary>
    [DataContract(Name = "texPt", Namespace = "")]
    public struct TexturePoint : IEquatable<TexturePoint>
    {
        private static readonly char[] _Separator = { ' ' };

        /// <summary>Zero point.</summary>
        public static readonly TexturePoint Zero = new TexturePoint();

        /// <summary> Initializes a new instance of the <see cref="TexturePoint" /> struct. </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public TexturePoint(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }

        /// <summary> Gets the X coordinate. </summary>
        [DataMember(Name = "x", Order = 0)]
        public int X { get; }

        /// <summary> Gets the Y coordinate. </summary>
        [DataMember(Name = "y", Order = 1)]
        public int Y { get; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TexturePoint other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary> Returns a <see cref="String" /> that represents this instance. </summary>
        /// <returns> A <see cref="String" /> that represents this instance. </returns>
        public override string ToString()
        {
            return X + _Separator[0].ToString() + Y;
        }

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is TexturePoint && Equals((TexturePoint)obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked {
                return (X * 397) ^ Y;
            }
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>true</c> if both operands are considired equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(TexturePoint left, TexturePoint right)
        {
            return left.Equals(right);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>true</c> if both operands are considired not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(TexturePoint left, TexturePoint right)
        {
            return !left.Equals(right);
        }

        /// <summary> Parses <see cref="TexturePoint" /> from the given string in "x y" format. </summary>
        /// <param name="point">The string containg point data.</param>
        /// <param name="provider">The format provider.</param>
        /// <returns>Parsed <see cref="TextureSize" />.</returns>
        public static TexturePoint Parse(string point, IFormatProvider provider = null)
        {
            TexturePoint result = Zero;
            if (!string.IsNullOrWhiteSpace(point)) {
                string[] rawSource = point.Split(_Separator, StringSplitOptions.RemoveEmptyEntries);
                if (rawSource.Length == 2) {
                    result = new TexturePoint(Convert.ToInt32(rawSource[0], provider),
                                              Convert.ToInt32(rawSource[1], provider));
                }
            }

            return result;
        }
    }
}