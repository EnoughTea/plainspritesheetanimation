using System;
using System.Collections.Generic;

namespace PlainSpritesheetAnimation
{
    /// <summary> Event handler for an animation sequence. </summary>
    /// <param name="sender">Animation sequence which fired the event.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void AnimationSequenceEventHandler(IAnimationSequence sender, EventArgs e);

    /// <summary> Interface for a sequence of frames. </summary>
    public interface IAnimationSequence
    {
        /// <summary>  Gets the name of this animation sequence. </summary>
        string Name { get; set; }

        /// <summary> Gets the texture ID determining which texture to use for this animation sequence. </summary>
        string TextureId { get; set; }

        /// <summary> Gets the frames this sequence consist of. </summary>
        List<IAnimationFrame> Frames { get; }

        /// <summary>
        ///     Gets the current frame in the frame sequence. Can be null if <see cref="CurrentFrameIndex" /> is
        ///     out of range for the <see cref="Frames" /> list.
        /// </summary>
        IAnimationFrame CurrentFrame { get; }

        /// <summary> Gets or sets index of the current frame in the frame sequence. </summary>
        int CurrentFrameIndex { get; set; }

        /// <summary>Gets or sets a value indicating whether or not this animation sequence plays in reverse.</summary>
        bool Reverse { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the animation sequence is running. Can be used for pausing.
        /// </summary>
        bool Animating { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this animation sequence is visible.
        ///     Can be used for hiding.
        /// </summary>
        bool Visible { get; set; }

        /// <summary> Gets or sets a type of animation for this sequence. </summary>
        AnimationType AnimationType { get; set; }

        /// <summary>Gets or sets the value indicating whether to mirror this sequence when rendered, or not.</summary>
        MirrorDirections Mirror { get; set; }

        /// <summary> Event which is raised when the animation is stopped. </summary>
        event AnimationSequenceEventHandler Stopped;

        /// <summary> Event which is raised when the animation is started. </summary>
        event AnimationSequenceEventHandler Started;

        /// <summary> Event which is raised when the play direction is changed. </summary>
        event AnimationSequenceEventHandler PlayDirectionChanged;

        /// <summary> Restarts the animation sequence. </summary>
        void Start();

        /// <summary>
        ///     Stops and resets the animation sequence. If you want to pause animation sequence,
        ///     use <see cref="Animating" /> property instead.
        /// </summary>
        void Stop();

        /// <summary>
        ///     Shows the next frame (previous if the animation is playing in reverse) in the sequence.
        ///     Usually it's called automatically by <see cref="Update" />.
        /// </summary>
        void AdvanceFrame();

        /// <summary> Updates animation sequence using the specified delta time between update calls. </summary>
        /// <param name="delta">The amount of time passed between updates.</param>
        void Update(float delta);

        /// <summary>
        ///     Calculates the maximum frame dimensions of all frames currently in the <see cref="Frames" /> list.
        /// </summary>
        /// <returns>Maximum frame dimensions; empty if no frames are present or if their sources are empty.</returns>
        TextureSize GetFrameBounds();

        /// <summary>Sets the new duration for the sequence, scaling individual frame durations.</summary>
        void SetDuration(float newDuration);

        /// <summary>Gets the combined duration for all frames in the sequence.</summary>
        /// <returns>Combined duration for all frames in the sequence.</returns>
        float GetDuration();

        /// <summary> Creates a clone of this sequence. </summary>
        /// <param name="name">The name of the cloned sequence.</param>
        /// <returns> A clone of this sequence. </returns>
        IAnimationSequence Clone(string name);
    }
}