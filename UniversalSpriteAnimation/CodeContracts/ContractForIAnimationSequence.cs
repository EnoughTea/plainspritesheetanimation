using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Unisa {
    [ContractClassFor(typeof(IAnimationSequence))]
    internal abstract class ContractForIAnimationSequence : IAnimationSequence {
        public string Name { get; set; }

        public List<IAnimationFrame> Frames {
            get {
                Contract.Ensures(Contract.Result<List<IAnimationFrame>>() != null);
                return null;
            }
        }

        public IAnimationFrame CurrentFrame { get; set; }

        public int CurrentFrameIndex { get; set; }

        public bool Reverse { get; set; }

        public bool Animating { get; set; }

        public bool Visible { get; set; }

        public AnimationType AnimationType { get; set; }

        public MirrorDirections Mirror { get; set; }

        public event AnimationSequenceEventHandler Stopped = delegate { };
        public event AnimationSequenceEventHandler Started = delegate { };
        public event AnimationSequenceEventHandler PlayDirectionChanged = delegate { };

        public void Start() {
            Started(this, EventArgs.Empty);
        }

        public void Stop() {
            Stopped(this, EventArgs.Empty);
        }

        public void AdvanceFrame() {
        }

        public void Update(float delta) {
            PlayDirectionChanged(this, EventArgs.Empty);
        }

        public TextureSize GetFrameBounds() {
            return TextureSize.Zero;
        }

        public void SetDuration(float newDuration) {
            Contract.Requires(newDuration >= 0);
        }

        public float GetDuration() {
            return 0;
        }

        public IAnimationSequence Clone(string name) {
            Contract.Ensures(Contract.Result<IAnimationSequence>() != null);
            return null;
        }
    }
}