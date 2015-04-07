using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace PlainSpritesheetAnimation {
    /// <summary> Represents a point coordinates on a texture. </summary>
    [DataContract(Name = "texPt", Namespace = ""), DebuggerDisplay("{ToString()}")]
    public struct TexturePoint : IEquatable<TexturePoint> {
        private static readonly char[] _sep = {' '};
        public static readonly TexturePoint Zero = new TexturePoint();

        /// <summary> Initializes a new instance of the <see cref="TexturePoint" /> struct. </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public TexturePoint(int x, int y)
            : this() {
            X = x;
            Y = y;
        }

        /// <summary> Gets the X coordinate. </summary>
        [DataMember(Name = "x", Order = 0)]
        public int X { get; private set; }

        /// <summary> Gets the Y coordinate. </summary>
        [DataMember(Name = "y", Order = 1)]
        public int Y { get; private set; }

        public bool Equals(TexturePoint other) {
            return X == other.X && Y == other.Y;
        }

        /// <summary> Returns a <see cref="String" /> that represents this instance. </summary>
        /// <returns> A <see cref="String" /> that represents this instance. </returns>
        public override string ToString() {
            return X + " " + Y;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is TexturePoint && Equals((TexturePoint) obj);
        }

        public override int GetHashCode() {
            unchecked { return (X * 397) ^ Y; }
        }

        public static bool operator ==(TexturePoint left, TexturePoint right) {
            return left.Equals(right);
        }

        public static bool operator !=(TexturePoint left, TexturePoint right) {
            return !left.Equals(right);
        }

        /// <summary> Parses <see cref="TexturePoint" /> from the given string in "x y" format. </summary>
        /// <param name="point">The string containg point data.</param>
        /// <param name="provider">The format provider.</param>
        /// <returns>Parsed <see cref="TextureSize" />.</returns>
        [Pure]
        public static TexturePoint Parse(string point, IFormatProvider provider = null) {
            TexturePoint result = Zero;
            if (!String.IsNullOrWhiteSpace(point)) {
                string[] rawSource = point.Split(_sep, StringSplitOptions.RemoveEmptyEntries);
                if (rawSource.Length == 2) {
                    result = new TexturePoint(Convert.ToInt32(rawSource[0], provider),
                        Convert.ToInt32(rawSource[1], provider));
                }
            }

            return result;
        }
    }
}