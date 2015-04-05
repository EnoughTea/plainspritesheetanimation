using System;

namespace PlainSpritesheetAnimation {
    /// <summary> Defines possible ways to mirror an animation sequence. </summary>
    [Flags]
    public enum MirrorDirections {
        None = 0,
        Vertically = 1,
        Horizontally = 2,
        Both = Vertically | Horizontally
    }
}