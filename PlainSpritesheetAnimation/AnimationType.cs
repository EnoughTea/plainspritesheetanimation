using System.Runtime.Serialization;

namespace PlainSpritesheetAnimation {
    /// <summary> Different types of automatic animation that take place. </summary>
    [DataContract]
    public enum AnimationType {
        /// <summary>
        ///     Specifies that the animation should go from first frame to the end and stop,
        ///     but show first frame once the animation is finished.
        /// </summary>
        [EnumMember] Once,

        /// <summary>
        ///     Specifies that the animation should go from first frame to the end and stop there, with the last
        ///     frame shown.
        /// </summary>
        [EnumMember] OnceHoldLast,

        /// <summary> Specifies that the animation should go from first frame to the end, and then disappear. </summary>
        [EnumMember] OnceDisappear,

        /// <summary>
        ///     Specifies that the animation should go from first frame to the end, and start back at frame 0 to
        ///     repeat.
        /// </summary>
        [EnumMember] Looping,

        /// <summary>
        ///     Specifies that the animation should go from first frame to the end, and then go back down to first frame.
        ///     This cycle repeats indefinitely.
        /// </summary>
        [EnumMember] PingPong
    }
}