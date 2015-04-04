using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Unisa {
    /// <summary> Represents a set of animation sequences. </summary>
    [DataContract(Name = "anim"), KnownType(typeof(AnimationSequence)), DebuggerDisplay("{ToString()}")]
    public class Animation : IAnimation {
        /// <summary> Gets all animation sequences for this animation. </summary>
        [DataMember(Name = "seqs", Order = 100)]
        public HashSet<IAnimationSequence> Sequences { get; private set; }

        /// <summary> Initializes a new instance of the <see cref="Animation" /> class. </summary>
        /// <param name="textureId">The texture ID.</param>
        /// <param name="sequences">The animation sequences. Sequences with duplicate names will be ignored.</param>
        public Animation(string textureId = null, IEnumerable<IAnimationSequence> sequences = null) {
            TextureId = textureId;
            Sequences = (sequences != null) 
                ? new HashSet<IAnimationSequence>(sequences) 
                : new HashSet<IAnimationSequence>();
        }

        /// <summary> Gets the texture ID used to determine which texture to use for this animation. </summary>
        [DataMember(Name = "texId", Order = 1)]
        public string TextureId { get; set; }

        /// <summary> Gets the sequence with the specified name. </summary>
        /// <param name="sequenceName">The name of the sequence to get.</param>
        /// <returns>Found sequence or null.</returns>
        public IAnimationSequence Get(string sequenceName) {
            return Sequences.FirstOrDefault(sequence => sequence.Name == sequenceName);
        }

        /// <summary> Gets currently running animation sequences. </summary>
        /// <returns> Enumeration of the currently running animation sequences. </returns>
        public IEnumerable<IAnimationSequence> GetAnimatingSequences() {
            return Sequences.Where(sequence => sequence.Animating);
        }

        /// <summary> Gets currently visible animation sequences. </summary>
        /// <returns> Enumeration of the currently visible animation sequences.</returns>
        public IEnumerable<IAnimationSequence> GetVisibleSequences() {
            return Sequences.Where(sequence => sequence.Visible);
        }

        /// <summary> Updates animation sequence using the specified delta time between update calls. </summary>
        /// <param name="delta">The amount of time passed between updates.</param>
        public void Update(float delta) {
            foreach (var sequence in Sequences) { sequence.Update(delta); }
        }

        /// <summary>Creates a clone of this animation.</summary>
        /// <returns> A clone of this animation. </returns>
        public virtual IAnimation Clone() {
            var clonedSequences = Sequences.Select(sequence => sequence.Clone(sequence.Name));
            return new Animation(TextureId, clonedSequences);
        }

        /// <summary>Gets the biggest current frame dimensions for all visible sequences.</summary>
        /// <returns>Biggest current frame dimensions for all visible sequences.</returns>
        public TextureSize GetMaxVisibleFrameSize() {
            int maxWidth = 0, maxHeight = 0;
            foreach (var sequence in GetVisibleSequences()) {
                if (sequence.CurrentFrame != null) {
                    maxWidth = Math.Max(maxWidth, sequence.CurrentFrame.Source.Width);
                    maxHeight = Math.Max(maxHeight, sequence.CurrentFrame.Source.Height);
                }
            }

            return new TextureSize(maxWidth, maxHeight);
        }

        /// <summary> Returns a <see cref="System.String" /> that represents this instance. </summary>
        /// <returns> A <see cref="System.String" /> that represents this instance. </returns>
        public override string ToString() {
            string texture = !String.IsNullOrEmpty(TextureId) ? TextureId : "<textureless>";
            return texture + ", " + Sequences.Count + " sequences";
        }
    }
}