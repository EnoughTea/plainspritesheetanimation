using System;

namespace PlainSpritesheetAnimation {
    /// <summary> Defines possible ways to mirror an animation sequence. </summary>
    [Flags]
    public enum MirrorDirections {
        None = 0,
        Vertical = 1,
        Horizontal = 2,
        Both = Vertical | Horizontal
    }
}