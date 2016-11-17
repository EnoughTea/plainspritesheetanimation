using System;

namespace PlainSpritesheetAnimation
{
    /// <summary> Defines possible ways to mirror an animation sequence. </summary>
    [Flags]
    public enum MirrorDirections
    {
        /// <summary>No mirroring should be applied.</summary>
        None = 0,
        /// <summary>Frames should be mirrored on vertical axis.</summary>
        Vertical = 1,
        /// <summary>Frames should be mirrored on horizontal axis.</summary>
        Horizontal = 2,
        /// <summary>Frames should be mirrored on both horizontal and vertical axes.</summary>
        Both = Vertical | Horizontal
    }
}