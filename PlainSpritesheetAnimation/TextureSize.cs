using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace PlainSpritesheetAnimation {
    /// <summary> Represents a size used to measure textures and their regions. </summary>
    [DataContract(Name = "texSz", Namespace = "")]
    public struct TextureSize : IEquatable<TextureSize> {
        private static readonly char[] _sep = {' '};
        public static readonly TextureSize Zero = new TextureSize();

        public TextureSize(int width, int height) : this() {
            Contract.Requires(width >= 0);
            Contract.Requires(height >= 0);

            Width = width;
            Height = height;
        }

        [DataMember(Name = "w", Order = 0)]
        public int Width { get; private set; }

        [DataMember(Name = "h", Order = 1)]
        public int Height { get; private set; }

        public bool Equals(TextureSize other) {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary> Returns a <see cref="String" /> that represents this instance. </summary>
        /// <returns> A <see cref="String" /> that represents this instance. </returns>
        public override string ToString() {
            return Width + " " + Height;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is TextureSize && Equals((TextureSize) obj);
        }

        public override int GetHashCode() {
            unchecked { return (Width * 397) ^ Height; }
        }

        public static bool operator ==(TextureSize left, TextureSize right) {
            return left.Equals(right);
        }

        public static bool operator !=(TextureSize left, TextureSize right) {
            return !left.Equals(right);
        }

        /// <summary> Parses <see cref="TextureSize" /> from the given string in "x y" format. </summary>
        /// <param name="size">The string containg size data.</param>
        /// <param name="provider">The format provider.</param>
        /// <returns>Parsed <see cref="TextureSize" />.</returns>
        [Pure]
        public static TextureSize Parse(string size, IFormatProvider provider = null) {
            TextureSize result = Zero;
            if (!String.IsNullOrWhiteSpace(size)) {
                string[] rawSource = size.Split(_sep, StringSplitOptions.RemoveEmptyEntries);
                if (rawSource.Length == 2) {
                    result = new TextureSize(
                        Convert.ToInt32(rawSource[0], provider),
                        Convert.ToInt32(rawSource[1], provider));
                }
            }

            return result;
        }
    }
}