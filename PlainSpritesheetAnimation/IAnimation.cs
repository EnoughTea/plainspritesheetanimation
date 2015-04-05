using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PlainSpritesheetAnimation {
    /// <summary> Interface for a managing class for animation sequences. </summary>
    [ContractClass(typeof(ContractForIAnimation))]
    public interface IAnimation {
        /// <summary> Gets or sets a texture ID used to determine which texture to use for this animation. </summary>
        string TextureId { get; set; }

        /// <summary> Gets all animation sequences for this animation. </summary>
        HashSet<IAnimationSequence> Sequences { get; }

        /// <summary> Searches for the first sequence with the specified name. </summary>
        /// <param name="sequenceName">Name of the sequence to look for.</param>
        /// <returns>Found sequence or null.</returns>
        IAnimationSequence FindSequence(string sequenceName);

        /// <summary> Gets currently running animation sequences. </summary>
        /// <returns> Enumeration of the currently running animation sequences. </returns>
        IEnumerable<IAnimationSequence> GetAnimatingSequences();

        /// <summary> Gets currently visible animation sequences. </summary>
        /// <returns> Enumeration of the currently visible animation sequences.</returns>
        IEnumerable<IAnimationSequence> GetVisibleSequences();

        /// <summary>Gets the biggest current frame dimensions for all visible sequences.</summary>
        /// <returns>Biggest current frame dimensions for all visible sequences.</returns>
        TextureSize GetMaxVisibleFrameSize();

        /// <summary> Updates animation sequence using the specified delta time between update calls. </summary>
        /// <param name="delta">The amount of time passed between updates.</param>
        void Update(float delta);

        /// <summary> Creates a clone of this animation. </summary>
        /// <returns> A clone of this animation. </returns>
        IAnimation Clone();
    }
}