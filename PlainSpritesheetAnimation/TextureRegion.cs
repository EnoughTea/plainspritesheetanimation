using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PlainSpritesheetAnimation
{
    /// <summary> Represents a rectangle on a texture. Start at the top-left corner, Y axis grows upwards. </summary>
    [DataContract(Name = "texReg", Namespace = "")]
    public struct TextureRegion : IEquatable<TextureRegion>
    {
        private static readonly char[] _SpaceSeparator = { ' ' };
        public static readonly TextureRegion Empty = new TextureRegion();

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureRegion"/> struct.
        /// </summary>
        /// <param name="coordinates">Top-left corner of the region in pixel coordinates.</param>
        /// <param name="size">Region size in pixels.</param>
        public TextureRegion(TexturePoint coordinates, TextureSize size)
            : this(coordinates.X, coordinates.Y, size.Width, size.Height)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="TextureRegion"/> struct.</summary>
        /// <param name="x">X pixel coordinate of the top-left corner of the region.</param>
        /// <param name="y">Y pixel coordinate of the top-left corner of the region.</param>
        /// <param name="width">The width of the region in pixels.</param>
        /// <param name="height">The height of the region in pixels.</param>
        public TextureRegion(int x, int y, int width, int height)
            : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        [DataMember(Name = "x", Order = 0)]
        public int X { get; }

        [DataMember(Name = "y", Order = 1)]
        public int Y { get; }

        [DataMember(Name = "w", Order = 2)]
        public int Width { get; }

        [DataMember(Name = "h", Order = 3)]
        public int Height { get; }

        /// <summary> Gets the left side X coordinate. </summary>
        public int Left => X;

        /// <summary> Gets the top side Y coordinate. </summary>
        public int Top => Y;

        /// <summary> Gets the right side X coordinate. </summary>
        public int Right => X + Width;

        /// <summary> Gets the bottom side Y coordinate. </summary>
        public int Bottom => Y - Height;

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TextureRegion other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        /// <summary> Returns a <see cref="String" /> that represents this instance. </summary>
        /// <returns> A <see cref="String" /> that represents this instance. </returns>
        public override string ToString()
        {
            return X + " " + Y + " " + Width + " " + Height;
        }

        /// <summary>Determines whether the specified <see cref="Object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is TextureRegion && Equals((TextureRegion)obj);
        }

        /// <summary> Returns a hash code for this instance. </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like
        ///     a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Width;
                hashCode = (hashCode * 397) ^ Height;
                return hashCode;
            }
        }

        public static bool operator ==(TextureRegion left, TextureRegion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TextureRegion left, TextureRegion right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Calculates texture frames for given texture size.
        ///     Frames are taken from the texture from left to right.
        /// </summary>
        /// <param name="textureSize">The size of the texture containing all frames. </param>
        /// <param name="frameSize">The size of the each frame. If set to zero, will load entire texture.</param>
        /// <param name="startPoint">The starting point in pixels from which to parse frames.</param>
        /// <param name="extraSpace">How many extra pixels to insert between each frame.</param>
        public static IEnumerable<TextureRegion> MakeFrames(TextureSize textureSize, TextureSize frameSize,
                                                            TexturePoint startPoint = new TexturePoint(),
                                                            TextureSize extraSpace = new TextureSize())
        {
            int currentX = startPoint.X;
            int currentY = startPoint.Y;

            if (frameSize == TextureSize.Zero) {
                yield return new TextureRegion(TexturePoint.Zero, textureSize);
            }
            else {
                do {
                    var currentFrameSource = new TextureRegion(currentX, currentY, frameSize.Width, frameSize.Height);

                    if (currentFrameSource.Right <= textureSize.Width &&
                        currentFrameSource.Bottom <= textureSize.Height) {
                        yield return currentFrameSource;
                    }

                    currentX += frameSize.Width + extraSpace.Width;
                    if (currentX + frameSize.Width > textureSize.Width) {
                        currentX = 0;
                        currentY += frameSize.Height + extraSpace.Height;
                    }
                } while (currentY + frameSize.Height <= textureSize.Height);
            }
        }

        /// <summary> Parses <see cref="TextureRegion" /> from the given string in "x y width height" format. </summary>
        /// <param name="region">The string containg region data.</param>
        /// <param name="provider">The format provider.</param>
        /// <returns>Parsed <see cref="TextureRegion" />.</returns>
        public static TextureRegion Parse(string region, IFormatProvider provider = null)
        {
            TextureRegion result = Empty;
            if (!String.IsNullOrWhiteSpace(region)) {
                string[] rawSource = region.Split(_SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
                if (rawSource.Length == 4) {
                    result = new TextureRegion(Convert.ToInt32(rawSource[0], provider),
                                               Convert.ToInt32(rawSource[1], provider),
                                               Convert.ToInt32(rawSource[2], provider),
                                               Convert.ToInt32(rawSource[3], provider));
                }
            }

            return result;
        }
    }
}