using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Unisa {
    /// <summary> Represents a sequence of frames. </summary>
    [DataContract(Name = "seq"), KnownType(typeof(AnimationFrame)), DebuggerDisplay("{ToString()}")]
    public class AnimationSequence : IAnimationSequence {
        [DataMember(Name = "a", IsRequired = false, EmitDefaultValue = false, Order = 5)]
        private bool _animating;
        [DataMember(Name = "rev", IsRequired = false, EmitDefaultValue = false, Order = 6)]
        private bool _reverse;
        [DataMember(Name = "cfi", IsRequired = false, EmitDefaultValue = false, Order = 7)]
        private int _currentFrameIndex;
        [DataMember(Name = "ft", IsRequired = false, EmitDefaultValue = false, Order = 8)]
        private float _frameTime;

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

        /// <summary> Gets the frames this sequence consist of. </summary>
        [DataMember(Name = "frames", Order = 100)]
        public List<IAnimationFrame> Frames { get; private set; }

        /// <summary>
        ///     Gets the current frame in the frame sequence. Can be null if <see cref="CurrentFrameIndex" /> is
        ///     out of range for the <see cref="Frames" /> list.
        /// </summary>
        public IAnimationFrame CurrentFrame {
            get {
                return (CurrentFrameIndex >= 0 && CurrentFrameIndex < Frames.Count) ? Frames[CurrentFrameIndex] : null;
            }
        }

        /// <summary> Gets or sets index of the current frame in the frame sequence. </summary>
        public int CurrentFrameIndex {
            get { return _currentFrameIndex; }

            set {
                switch (AnimationType) {
                    case AnimationType.Once:
                    case AnimationType.OnceHoldLast:
                    case AnimationType.OnceDisappear:
                        if (Reverse && value <= -1) {
                            _currentFrameIndex = (AnimationType == AnimationType.Once) ? Frames.Count - 1 : 0;
                            Animating = false;
                            if (AnimationType == AnimationType.OnceDisappear) { Visible = false; }
                        } else if (!Reverse && value >= Frames.Count) {
                            _currentFrameIndex = (AnimationType == AnimationType.Once) ? 0 : Frames.Count - 1;
                            Animating = false;
                            if (AnimationType == AnimationType.OnceDisappear) { Visible = false; }
                        } else {
                            _currentFrameIndex = value % Frames.Count;
                        }

                        break;

                    case AnimationType.Looping:
                        while (value < 0) { value += Frames.Count; }

                        _currentFrameIndex = value % Frames.Count;
                        break;

                    case AnimationType.PingPong:
                        value %= Frames.Count * 2;
                        if (value >= Frames.Count) { value = (2 * Frames.Count) - 1 - value; }

                        _currentFrameIndex = value;
                        // check for the ping-ponging.
                        if (Reverse && value == 0) {
                            Reverse = false;
                        } else if (Reverse == false && value == Frames.Count - 1) { Reverse = true; }

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
                bool doEvent = (value != _reverse);
                _reverse = value;
                if (doEvent) { PlayDirectionChanged(this); }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the animation sequence is running.
        ///     Can be used for pausing.
        /// </summary>
        public bool Animating {
            get { return _animating; }

            set {
                bool doEvent = (value != _animating);
                _animating = value;

                // If we're resuming 'once' animation sequences on their last frame, rewind them back, 
                // since it's most probably what user wants:
                if (_animating && (AnimationType == AnimationType.Once || AnimationType == AnimationType.OnceHoldLast ||
                                  AnimationType == AnimationType.OnceDisappear)) {
                    if (Reverse && _currentFrameIndex <= 0) {
                        _currentFrameIndex = Frames.Count - 1;
                    } else if (!Reverse && _currentFrameIndex >= Frames.Count - 1) { _currentFrameIndex = 0; }
                }

                if (doEvent) { if (_animating) { Started(this); } else { Stopped(this); } }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this animation sequence is visible. Can be used for hiding.
        /// </summary>
        [DataMember(Name = "v", IsRequired = false, EmitDefaultValue = false, Order = 5)]
        public bool Visible { get; set; }

        /// <summary> Gets or sets a type of animation for this sequence. </summary>
        [DataMember(Name = "t", EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public AnimationType AnimationType { get; set; }

        /// <summary>Gets or sets the value indicating whether to mirror this sequence when rendered, or not.</summary>
        [DataMember(Name = "m", EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public MirrorType Mirror { get; set; }

        /// <summary> Event which is raised when the animation is stopped. </summary>
        public event Action<IAnimationSequence> Stopped = delegate { };

        /// <summary> Event which is raised when the animation is started. </summary>
        public event Action<IAnimationSequence> Started = delegate { };

        /// <summary> Event which is raised when the play direction is changed. </summary>
        public event Action<IAnimationSequence> PlayDirectionChanged = delegate { };

        /// <summary>
        ///     Shows the next frame in the sequence.  This pays attention to whether the animation is playing
        ///     forwards or reverse. Usually it's called automatically by <see cref="Update" />.
        /// </summary>
        public void AdvanceFrame() {
            if (Reverse) { CurrentFrameIndex--; } else { CurrentFrameIndex++; }
        }

        /// <summary> Restarts the animation sequence. </summary>
        public void Start() {
            CurrentFrameIndex = (Reverse) ? Math.Max(0, Frames.Count - 1) : 0;
            Animating = true;
            Visible = true;
        }

        /// <summary>
        ///     Stops and resets the animation sequence. If you want to pause animation sequence,
        ///     use <see cref="Animating" /> property instead.
        /// </summary>
        public void Stop() {
            CurrentFrameIndex = (Reverse) ? Math.Max(0, Frames.Count - 1) : 0;
            Animating = false;
            Visible = false;
        }

        /// <summary> Updates animation sequence using the specified delta time between game frames. </summary>
        /// <param name="delta">The amount of time passed between game frames.</param>
        public void Update(float delta) {
            var currentFrame = CurrentFrame;
            if (currentFrame == null || currentFrame.Duration <= 0) {
                Stop();
                return;
            }

            if (Animating) {
                _frameTime += delta;
                while (_frameTime >= currentFrame.Duration) {
                    _frameTime -= currentFrame.Duration;
                    AdvanceFrame();
                }
            }
        }

        /// <summary>
        ///     Calculates the maximum frame dimensions of all frames currently in the <see cref="Frames" /> list.
        /// </summary>
        /// <returns>Maximum frame dimensions; empty if no frames are present or if their sources are empty.</returns>
        public TextureSize GetMaxFrameSize() {
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

        /// <summary> Sets the amount of time each frame should display, in seconds. </summary>
        /// <param name="time">The time for each frame to display.</param>
        public void SetFramesDuration(float time) {
            Contract.Requires(time >= 0);

            foreach (var frame in Frames) { frame.Duration = time; }
        }


        /// <summary> Returns a <see cref="String" /> that represents this instance. </summary>
        /// <returns> A <see cref="String" /> that represents this instance. </returns>
        public override string ToString() {
            string name = !String.IsNullOrEmpty(Name) ? Name : "<nameless>";
            string visible = Visible ? "visible" : "invisible";
            string animating = Animating ? "active" : "inactive";
            return name + ", " + Frames.Count + " frames (" + visible + ", " + animating +")";
        }
    }
}