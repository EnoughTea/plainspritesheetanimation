using System;
using System.Collections.Generic;
using System.Linq;
using Sequences = System.Collections.Generic.IEnumerable<PlainSpritesheetAnimation.IAnimationSequence>;

namespace PlainSpritesheetAnimation {
    /// <summary> Extension methods for <see cref="IEnumerable{IAnimationSequence}"/>. </summary>
    public static class EnumerableAnimationSequenceExtensions {
        /// <summary>Creates a clone of this animation.</summary>
        /// <returns> A clone of this animation. </returns>
        public static Sequences CloneSequences(this Sequences sequences) {
            return sequences.Select(sequence => sequence.Clone(sequence.Name));
        }

        /// <summary> Finds the sequence with the specified name. </summary>
        /// <param name="sequences">Animation sequences.</param>
        /// <param name="sequenceName">The name of the sequence to get.</param>
        /// <returns>Found sequence or null.</returns>
        public static IAnimationSequence FindSequenceByName(this Sequences sequences, string sequenceName) {
            return sequences.FirstOrDefault(sequence => sequence.Name == sequenceName);
        }

        /// <summary> Gets currently running animation sequences. </summary>
        /// <param name="sequences">Animation sequences.</param>
        /// <returns> Enumeration of the currently running animation sequences. </returns>
        public static Sequences GetAnimatingSequences(this Sequences sequences) {
            return sequences.Where(sequence => sequence.Animating);
        }

        /// <summary> Gets currently visible animation sequences. </summary>
        /// <param name="sequences">Animation sequences.</param>
        /// <returns> Enumeration of the currently visible animation sequences.</returns>
        public static IEnumerable<IAnimationSequence> GetVisibleSequences(this Sequences sequences) {
            return sequences.Where(sequence => sequence.Visible);
        }

        /// <summary>Gets the biggest current frame dimensions for all visible sequences.</summary>
        /// <returns>Biggest current frame dimensions for all visible sequences.</returns>
        public static TextureSize GetVisibleFrameBounds(this Sequences sequences) {
            int maxWidth = 0, maxHeight = 0;
            foreach (var sequence in sequences.GetVisibleSequences()) {
                if (sequence.CurrentFrame != null) {
                    maxWidth = Math.Max(maxWidth, sequence.CurrentFrame.Source.Width);
                    maxHeight = Math.Max(maxHeight, sequence.CurrentFrame.Source.Height);
                }
            }

            return new TextureSize(maxWidth, maxHeight);
        }

        /// <summary> Updates animation sequence using the specified delta time between update calls. </summary>
        /// <param name="sequences">Animation sequences.</param>
        /// <param name="delta">The amount of time passed between updates.</param>
        public static void UpdateSequences(this Sequences sequences, float delta) {
            foreach (var sequence in sequences) { sequence.Update(delta); }
        }

        /// <summary> Gets all texture IDs used by these animation sequences. </summary>
        /// <param name="sequences">Animation sequences.</param>
        public static IEnumerable<string> GetUsedTextureIds(this Sequences sequences) {
            return sequences.GroupBy(sequence => sequence.TextureId)
                            .Select(group => group.First().TextureId)
                            .Where(texId => !String.IsNullOrEmpty(texId));
        }
    }
}