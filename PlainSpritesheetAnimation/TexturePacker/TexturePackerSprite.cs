using System.Xml.Serialization;

namespace PlainSpritesheetAnimation
{
    /// <summary> Contains information about a single sprite. </summary>
    [XmlType(TypeName = "sprite")]
    public class TexturePackerSprite
    {
        /// <summary> Gets or sets the name of the sprite. </summary>
        [XmlAttribute("n")]
        public string Name { get; set; }

        /// <summary> Gets or sets the sprite x position in texture. </summary>
        [XmlAttribute("x")]
        public int X { get; set; }

        /// <summary> Gets or sets the sprite y position in texture. </summary>
        [XmlAttribute("y")]
        public int Y { get; set; }

        /// <summary> Gets or sets the sprite width (may be trimmed). </summary>
        [XmlAttribute("w")]
        public int Width { get; set; }

        /// <summary> Gets or sets the sprite height (may be trimmed). </summary>
        [XmlAttribute("h")]
        public int Height { get; set; }

        /// <summary> Gets or sets a value indicating whether this sprite is rotated. </summary>
        /// <value> <c>true</c> if sprite is rotated; otherwise, <c>false</c>. </value>
        public bool Rotated
        {
            get { return RotationMark == "y"; }
        }

        /// <summary> Gets or sets a value indicating whether this sprite is rotated. </summary>
        /// <value> <c>"y"</c> if sprite is rotated; otherwise, <c>false</c>. </value>
        [XmlAttribute("r")]
        public string RotationMark { get; set; }

        /// <summary> Gets or sets the sprite's x-corner offset (only available if trimmed). </summary>
        [XmlAttribute("oX")]
        public int OffsetX { get; set; }

        /// <summary> Gets or sets the sprite's y-corner offset (only available if trimmed). </summary>
        [XmlAttribute("oY")]
        public int OffsetY { get; set; }

        /// <summary> Get or sets the sprite's original width (only available if trimmed). </summary>
        [XmlAttribute("oW")]
        public int OriginalWidth { get; set; }

        /// <summary> Get or sets the sprite's original height (only available if trimmed). </summary>
        [XmlAttribute("oH")]
        public int OriginalHeight { get; set; }

        /// <summary> Gets a source rectangle: texture region where sprite is located. </summary>
        public TextureRegion Source
        {
            get { return new TextureRegion(X, Y, Width, Height); }
        }
    }
}