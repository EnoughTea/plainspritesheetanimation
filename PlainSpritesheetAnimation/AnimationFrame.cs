using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace PlainSpritesheetAnimation {
    /// <summary> Represents a single animation frame. </summary>
    [DataContract(Name = "frame"), DebuggerDisplay("{ToString()}")]
    public class AnimationFrame : IAnimationFrame {
        private TextureRegion _source;

        /// <summary> Initializes a new instance of the <see cref="AnimationFrame" /> class. </summary>
        /// <param name="source">The source rectangle.</param>
        /// <param name="duration">The frame duration.</param>
        [CLSCompliant(false)]
        public AnimationFrame(ref TextureRegion source, float duration = 0f) {
            Contract.Requires(duration >= 0);

            Duration = duration;
            _source = source;
        }

        /// <summary> Initializes a new instance of the <see cref="AnimationFrame" /> class. </summary>
        /// <param name="source">The source rectangle.</param>
        /// <param name="duration">The frame duration.</param>
        public AnimationFrame(TextureRegion source = new TextureRegion(), float duration = 0f)
            : this(ref source, duration) {
            Contract.Requires(duration >= 0);
        }

        /// <summary> Gets the source rectangle: region of this frame in the animation's texture. </summary>
        [DataMember(Name = "src", Order = 0)]
        public TextureRegion Source { get { return _source; } set { _source = value; } }

        /// <summary> Gets or sets the amount of time this frame should display, in seconds. </summary>
        [DataMember(Name = "time", Order = 1)]
        public float Duration { get; set; }

        /// <summary>
        ///     Drawing offset for this frame. It is often a useful property to correct frame drawing position
        ///     relative to the sequence center. Its usage depends on your workflow.
        /// </summary>
        [DataMember(Name = "origin", EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public TexturePoint Origin { get; set; }

        /// <summary> Clones this frame. </summary>
        /// <returns> Creates a clone of this frame. </returns>
        public IAnimationFrame Clone() {
            return new AnimationFrame(ref _source, Duration) {Origin = Origin};
        }

        /// <summary> Returns a <see cref="string" /> that represents this instance. </summary>
        /// <returns> A <see cref="string" /> that represents this instance. </returns>
        public override string ToString() {
            string origin = (Origin != TexturePoint.Zero) ? ", origin at " + Origin : String.Empty;
            return "[" + Source + "]" + origin + " (lasts " + Duration.ToString("0.###") + " s)";
        }
    }
}