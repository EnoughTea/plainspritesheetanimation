namespace PlainSpritesheetAnimation
{
    /// <summary> Interface for a single animation frame. </summary>
    public interface IAnimationFrame
    {
        /// <summary> Gets the source rectangle: region of this frame in the animation's texture. </summary>
        TextureRegion Source { get; set; }

        /// <summary> Gets or sets the amount of time this frame should display, in seconds. </summary>
        float Duration { get; set; }

        /// <summary>
        ///     Drawing offset for this frame. It is often a useful property to correct frame drawing position
        ///     relative to the sequence center. Its usage depends on your workflow.
        /// </summary>
        TexturePoint Origin { get; set; }

        /// <summary> Clones this frame. </summary>
        /// <returns> Creates a clone of this frame. </returns>
        IAnimationFrame Clone();
    }
}