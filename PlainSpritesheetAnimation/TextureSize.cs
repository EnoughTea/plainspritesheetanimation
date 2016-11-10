using System;
using System.Runtime.Serialization;

namespace PlainSpritesheetAnimation {
    /// <summary> Represents a size used to measure textures and their regions. </summary>
    [DataContract(Name = "texSz", Namespace = "")]
    public struct TextureSize : IEquatable<TextureSize> {
        private static readonly char[] _SpaceSeparator = {' '};
        public static readonly TextureSize Zero = new TextureSize();

        /// <summary>Initializes a new instance of the <see cref="TextureSize"/> struct.</summary>
        /// <param name="width">Texture width in pixels.</param>
        /// <param name="height">Texture height in pixels.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// width - Texture width cannot be negative.
        /// or
        /// height - Texture height cannot be negative.
        /// </exception>
        public TextureSize(int width, int height) : this() {
            if (width < 0) {
                throw new ArgumentOutOfRangeException(nameof(width), width, "Texture width cannot be negative.");
            }

            if (height < 0) {
                throw new ArgumentOutOfRangeException(nameof(height), height, "Texture height cannot be negative.");
            }

            Width = width;
            Height = height;
        }

        [DataMember(Name = "w", Order = 0)]
        public int Width { get; }

        [DataMember(Name = "h", Order = 1)]
        public int Height { get; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
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
        public static TextureSize Parse(string size, IFormatProvider provider = null) {
            TextureSize result = Zero;
            if (!String.IsNullOrWhiteSpace(size)) {
                string[] rawSource = size.Split(_SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
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