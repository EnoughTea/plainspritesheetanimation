using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PlainSpritesheetAnimation {
    /// <summary> Represents a sequence of frames. </summary>
    [DataContract(Name = "animSeq", IsReference = true, Namespace = ""), KnownType(typeof(AnimationFrame))]
    public class AnimationSequence : IAnimationSequence {
        [DataMember(Name = "a", IsRequired = false, EmitDefaultValue = false, Order = 5)] private bool _animating;

        [DataMember(Name = "cfi", IsRequired = false, EmitDefaultValue = false, Order = 10)] private int
            _currentFrameIndex;

        private float _frameTime;
        [DataMember(Name = "rev", IsRequired = false, EmitDefaultValue = false, Order = 6)] private bool _reverse;

        /// <summary> Initializes a new instance of the <see cref="AnimationSequence" /> class. </summary>
        /// <param name="name">The animation sequence name.</param>
        /// <param name="animationFrames">The animation sequence frames.</param>
        public AnimationSequence(string name = null, IEnumerable<IAnimationFrame> animationFrames = null) {
            Name = name;
            Frames = (animationFrames != null)
                ? new List<IAnimationFrame>(animationFrames)
                : new List<IAnimationFrame>();
        }

        /// <summary>  Gets the name of this animation sequence. </summary>
        [DataMember(Name = "name", Order = 0)]
        public string Name { get; set; }

        /// <summary> Gets the texture ID determining which texture to use for this animation sequence. </summary>
        [DataMember(Name = "texId", Order = 1)]
        public string TextureId { get; set; }

        /// <summary> Gets the frames this sequence consist of. </summary>
        [DataMember(Name = "frames", Order = 100)]
        public List<IAnimationFrame> Frames { get; private set; }

        /// <summary>
        ///     Gets the current frame in the frame sequence. Can be null if <see cref="CurrentFrameIndex" /> is
        ///     out of range for the <see cref="Frames" /> list.
        /// </summary>
        public IAnimationFrame CurrentFrame {
            get { return !IsBeyondIndex(CurrentFrameIndex) ? Frames[CurrentFrameIndex] : null; }
        }

        /// <summary> Gets or sets index of the current frame in the frame sequence. </summary>
        public int CurrentFrameIndex {
            get { return _currentFrameIndex; }

            set {
                switch (AnimationType) {
                    case AnimationType.Once:
                        _currentFrameIndex = value;
                        if (IsBeyondIndex(_currentFrameIndex)) {
                            _currentFrameIndex = GetFirstIndex();
                            Animating = false;
                        }
                        break;

                    case AnimationType.OnceHoldLast:
                        _currentFrameIndex = value;
                        if (IsBeyondIndex(_currentFrameIndex)) {
                            _currentFrameIndex = GetLastIndex();
                            Animating = false;
                        }
                        break;

                    case AnimationType.OnceDisappear:
                        _currentFrameIndex = value;
                        if (IsBeyondIndex(_currentFrameIndex)) {
                            Animating = false;
                            Visible = false;
                            _currentFrameIndex = GetLastIndex();
                        }
                        break;

                    case AnimationType.Looping:
                        if (Frames.Count > 0) {
                            while (value < 0) { value += Frames.Count; }

                            _currentFrameIndex = value % Frames.Count;
                        } else {
                            _currentFrameIndex = value;
                        }
                        break;

                    case AnimationType.PingPong:
                        if (Frames.Count > 0) {
                            value = Math.Abs(value);
                            value %= Frames.Count * 2;
                            if (value >= Frames.Count) { value = (2 * Frames.Count) - 1 - value; }

                            _currentFrameIndex = value;
                            // Ping-pong on last index.
                            if (IsLastIndex(_currentFrameIndex)) { Reverse = !Reverse; }
                        } else {
                            _currentFrameIndex = value;
                        }
                        break;

                    default:
                        throw new NotSupportedException("This animation type is not supported: " + AnimationType);
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not this animation sequence plays in reverse.
        /// </summary>
        public bool Reverse {
            get { return _reverse; }

            set {
                if (_reverse != value) {
                    _reverse = value;
                    PlayDirectionChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the animation sequence is running.
        ///     Can be used for pausing.
        /// </summary>
        public bool Animating {
            get { return _animating; }

            set {
                if (_animating != value) {
                    _animating = value;
                    if (_animating) { Started(this, EventArgs.Empty); } else { Stopped(this, EventArgs.Empty); }
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this animation sequence is visible. Can be used for hiding.
        /// </summary>
        [DataMember(Name = "v", IsRequired = false, EmitDefaultValue = false, Order = 5)]
        public bool Visible { get; set; }

        /// <summary> Gets or sets a type of animation for this sequence. </summary>
        [DataMember(Name = "t", EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public AnimationType AnimationType { get; set; }

        /// <summary>Gets or sets the value indicating whether to mirror this sequence when rendered, or not.</summary>
        [DataMember(Name = "m", EmitDefaultValue = false, IsRequired = false, Order = 3)]
        public MirrorDirections Mirror { get; set; }

        /// <summary> Event which is raised when the animation is stopped. </summary>
        public event AnimationSequenceEventHandler Stopped = delegate { };

        /// <summary> Event which is raised when the animation is started. </summary>
        public event AnimationSequenceEventHandler Started = delegate { };

        /// <summary> Event which is raised when the play direction is changed. </summary>
        public event AnimationSequenceEventHandler PlayDirectionChanged = delegate { };

        /// <summary>
        ///     Shows the next frame in the sequence.  This pays attention to whether the animation is playing
        ///     forwards or reverse. Usually it's called automatically by <see cref="Update" />.
        /// </summary>
        public void AdvanceFrame() {
            if (Reverse) { CurrentFrameIndex--; } else { CurrentFrameIndex++; }
        }

        /// <summary> Restarts the animation sequence. </summary>
        public void Start() {
            CurrentFrameIndex = GetFirstIndex();
            Animating = true;
            Visible = true;
        }

        /// <summary>
        ///     Stops and resets the animation sequence. If you want to pause animation sequence,
        ///     use <see cref="Animating" /> property instead.
        /// </summary>
        public void Stop() {
            CurrentFrameIndex = GetFirstIndex();
            Animating = false;
            Visible = false;
        }

        /// <summary> Updates animation sequence using the specified delta time between update calls. </summary>
        /// <param name="delta">The amount of time passed between updates.</param>
        public void Update(float delta) {
            var currentFrame = CurrentFrame;
            if (currentFrame == null) { return; }
            
            if (Animating) {
                if (currentFrame.Duration > 0) {
                    _frameTime += delta;
                    while (_frameTime >= currentFrame.Duration) {
                        _frameTime -= currentFrame.Duration;
                        AdvanceFrame();
                    }
                } else {
                    AdvanceFrame();
                }
            }
        }

        /// <summary>
        ///     Calculates the maximum frame dimensions of all frames currently in the <see cref="Frames" /> list.
        /// </summary>
        /// <returns>Maximum frame dimensions; empty if no frames are present or if their sources are empty.</returns>
        public TextureSize GetFrameBounds() {
            int width = 0, height = 0;
            foreach (var frame in Frames) {
                var source = frame.Source;
                if (source.Width > width) { width = source.Width; }

                if (source.Height > height) { height = source.Height; }
            }

            return new TextureSize(width, height);
        }

        /// <summary> Creates a clone of this sequence. </summary>
        /// <param name="name">The name of the cloned sequence.</param>
        /// <returns> A clone of this sequence. </returns>
        public IAnimationSequence Clone(string name) {
            IAnimationSequence result = new AnimationSequence(name);
            foreach (var frame in Frames) { result.Frames.Add(frame.Clone()); }

            result.Animating = Animating;
            result.AnimationType = AnimationType;
            result.CurrentFrameIndex = CurrentFrameIndex;
            result.Reverse = Reverse;
            result.Visible = Visible;
            result.Mirror = Mirror;
            return result;
        }

        /// <summary>Gets the combined duration in seconds for all frames in the sequence.</summary>
        /// <returns>Combined duration in seconds for all frames in the sequence.</returns>
        public float GetDuration() {
            return Frames.Sum(frame => frame.Duration);
        }

        /// <summary>Sets the new duration in seconds for the sequence, scaling individual frame durations.</summary>
        /// <remarks>If every frame duration is 0, new duration will be distributed evenly.</remarks>
        public void SetDuration(float newDuration) {
            float oldDuration = GetDuration();
            foreach (var frame in Frames) {
                if (Math.Abs(oldDuration) > 0.001f) {   // <0.001 is considered 0, since it is too short of a duration.
                    frame.Duration = newDuration * (frame.Duration / oldDuration);
                } else {
                    frame.Duration = newDuration / Frames.Count;
                }
            }
        }

        /// <summary> Returns a <see cref="string" /> that represents this instance. </summary>
        /// <returns> A <see cref="string" /> that represents this instance. </returns>
        public override string ToString() {
            string name = !String.IsNullOrEmpty(Name) ? Name : "<nameless>";
            string visible = Visible ? "visible" : "invisible";
            string animating = Animating ? "active" : "inactive";
            return name + ": " + "frame index is " + CurrentFrameIndex + " out of " + Frames.Count + " frames (" +
                   visible + ", " + animating + ")";
        }

        private int GetLastIndex() {
            return !Reverse ? Math.Max(0, Frames.Count - 1) : 0;
        }

        private int GetFirstIndex() {
            return !Reverse ? 0 : Math.Max(0, Frames.Count - 1);
        }

        private bool IsLastIndex(int index) {
            return (Reverse && index == 0) || (!Reverse && index == Frames.Count - 1);
        }

        private bool IsBeyondIndex(int index) {
            return (index < 0 || index >= Frames.Count);
        }
    }
}